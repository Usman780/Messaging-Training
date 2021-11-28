using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;
using TrainingTracker.BL;
using TrainingTracker.DataHub;
using TrainingTracker.Helper_Classes;
using TrainingTracker.Models;

namespace TrainingTracker.HelpingClasses
{
    public class MainMailClass
    {      

        public bool sendForgetMail(string email, int Id)
        {
            string temp = "<html><head></head><body> <div><p >Dear User,<br> <p > In order to reset your password, please click on the this <a target='_blank' href='LINKFORFORGOTPASSWORD' style='text-decoration: none;'> <b>link</b></a>. This link will expire in 6 hrs from now.</p></div> Thanks and Regards, <br> Team Zuptu";

            temp += "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";
            //string link = "https://zuptu.nodlays.com/Auth/resetpassword?val=" + StringCipher.Base64Encode(email) + "&t=" + StringCipher.Base64Encode(DateTime.Now.AddHours(6).Ticks.ToString())+ "&i=" + StringCipher.Base64Encode(Id.ToString());
            string link = ProjectVaraiables.ResetPasswordUrl + StringCipher.Base64Encode(email) + "&t=" + StringCipher.Base64Encode(DateTime.Now.AddHours(6).Ticks.ToString())+ "&i=" + HttpUtility.UrlEncode(StringCipher.Base64Encode(Id.ToString()));
           // string link =  + StringCipher.Base64Encode(email) + "&t=" + StringCipher.Base64Encode(DateTime.Now.AddHours(6).Ticks.ToString())+ "&i=" + StringCipher.Base64Encode(Id.ToString());
            link = link.Replace("+", "%20");
            temp = temp.Replace("LINKFORFORGOTPASSWORD", link);

            sendForgetMail(email, temp, "Zuptu Forget Mail");
            return true;
        }
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

        public bool DownloadReport(List<string> path, string v, string name="Zuptu File")
        {

            //Add DownloadFileLink 

            // CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
            int sss = Convert.ToInt32(path[0]);
            User u = new UserBL().getUsersById(sss);

            FileDownloadLink fdl = new FileDownloadLink()
            {
                UserId = u.Id,
                CreatedAt = DateTime.Now,
                IsActive = 1,
                CompanyId = Convert.ToInt32(logedinuser.Company),
                IsDownloaded = 0,
                ReportLink = "",
                ReportTitle = name

            };
           int fid= new FileDownloadLinkBL().AddFileDownloadLink(fdl);


            string text = "<link href='https://fonts.googleapis.com/css?family=Bree+Serif' rel='stylesheet'><style>  * {";
            text += "  font-family: 'Bree Serif', serif; }";
            text += " .list-group-item {       border: none;  }    .hor {      border-bottom: 5px solid black;   }";
            text += " .line {       margin-bottom: 20px; }";
            string temp = "<html><head></head><body> <div><p >Dear User,<br> <p >  Please click on following <a target='_blank' href='LINKFORFORGOTPASSWORD' style='text-decoration: none;'><b> link</b></a> to download your file.</p></div></b> <br><br> Thanks and Regards, </br> Team Zuptu";
            temp += "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";
            //string link = "https://zuptu.nodlays.com/Utilities/UpdateCanvasLoginIdAction?val=" + StringCipher.Base64Encode(CanvasMail) + "&t=" + StringCipher.Base64Encode(DateTime.Now.AddHours(72).Ticks.ToString()) + "&IsUser=" + Id ;
            // string link = "http://localhost:60126/Utilities/DownloadPDFReport?path=" + path + "&v=" + v + "&name=" + name;
            //string link = path;
            string link = ProjectVaraiables.SYSTEM_URL + "/Trainee/DownloadBlobFile?filePath=" + path[1] + "&name=" + name+"&u="+General_Purpose.EncryptId((int)u.CompanyID) + "&fid=" + General_Purpose.EncryptId((int)fid);
            DatabaseEntities db = new DatabaseEntities();
            FileDownloadLink Fdlink = new FileDownloadLinkBL().getFileDownloadLinkById(fid, db);
            Fdlink.ReportLink = link;
            new FileDownloadLinkBL().UpdateFileDownloadLink(Fdlink, db);

            temp = temp.Replace("LINKFORFORGOTPASSWORD", link);
            //sendForgetMail(u.Email, temp, "Download PDF Report");
            List<string> email = new List<string>();
            List<string> smscontent = new List<string>();
            List<string> emailcontent = new List<string>();
            List<string> numbers = new List<string>();
            email.Add(u.Email);
            emailcontent.Add(temp);

            smscontent.Add("Download link has been sent to your email address '" + u.Email + "' , please check your mail");
            numbers.Add(u.PhoneNumber);
            MainMailClass mailingClass = new MainMailClass();
            //mailingClass.mail(emails, EmailText, title);
            HostingEnvironment.QueueBackgroundWorkItem(ctx => mailingClass.sendForgetMail(u.Email, temp, "Download File"));
            HostingEnvironment.QueueBackgroundWorkItem(ctx => SMS_Service.sendSMSWrapper(smscontent, numbers));

            //  Communication.sendMessage(content, email);



            //"broadcastNotification" client function used in all layouts to update notifications
            // var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            //context.Clients.All.broadcastReportNotification("1", u.Id, link);
            string datetime = Fdlink.CreatedAt.Value.ToString("MM/dd/yyy hh:mm tt");
            General_Purpose.SendReportDownloadNotification(u.Id, name, link,datetime);//Temporary Commented
            //"broadcastNotification" client function used in all layouts to update notifications



            return true;
        }
        public bool ContactMail(string email, int id,string File="",string FileName="")
        {
            if (File == null)
                File = "";
            if (FileName == null)
                FileName = "";
            Contact u = new ContactBAL().getContactUsById(id) ;
            User user = new UserBL().getUsersById((int)u.UserId);
            string temp = "";
            if (user.Email == email)
            {
                if (File != "" && FileName != "")
                {
                    temp = "<html><head></head><body> <div><p >Dear " + user.FirstName + " " + user.LastName + " ,<br> <p > Thank you for contacting us. We will review your issue and route it to the most appropriate agent. Our agent will get back to you in 24 to 48 hours. The details of the ticket are listed below. When replying please include the ticket ID in the subject line so all of your replies are kept together.</p></div><br/><br/><b>Ticket ID:: </b>" + u.TicketNo + "<br/>" +
                   "<b>Company Name:: </b>" + user.Company.Name + "<br/>" +
                   "<b>Subject:: </b>" + u.Subject + "<br/>" +
                   "<b>Message:: </b>" + u.Message + "<br/>" +
                   
                   "<p>Click this  <a target='_blank' href='LINKFORFORGOTPASSWORD' style='text-decoration: none;'> <b>link</b></a> to download your attached file </p><br/>" +
                   " Thanks and Regards, <br> Team Zuptu";
                }
                else if (File == "" && FileName == "")
                {
                    temp = "<html><head></head><body> <div><p >Dear " + user.FirstName + " " + user.LastName + " ,<br> <p > Thank you for contacting us. We will review your issue and route it to the most appropriate agent. The details of the ticket are listed below. When replying please include the ticket ID in the subject line so all of your replies are kept together.</p></div><br/><b>Ticket ID:: </b>" + u.TicketNo + "<br/>" +
                   "<b>Company Name:: </b>" + user.Company.Name + "<br/>" +
                   "<b>Subject:: </b>" + u.Subject + "<br/>" +
                   "<b>Message:: </b>" + u.Message + "<br/><br/>" +

                   "Thanks and Regards, <br> Team Zuptu";
                }
            }
            else
            {
                if (File != "" && FileName != "")
                {
                    temp = "<html><head></head><body> <div><p><b>" + user.FirstName + " " + user.LastName + "</b> have some issues, the details of the ticket are listed below.</p></div>" +
                        
                        "<br/><b>Company Name:: </b>" + user.Company.Name +
                        "<br/><b>User Email:: </b>" + user.Email + "<br/>" +
                        "<br/><b>Priority:: </b>" + u.Priority + "<br/>" +
                        "<b>Ticket ID:: </b>" + u.TicketNo + "<br/>" +
                   "<b>Subject:: </b>" + u.Subject + "<br/>" +
                   "<b>Message:: </b>"+u.Message+"<br/>" +
                   
                   "<p>Click this  <a target='_blank' href='LINKFORFORGOTPASSWORD' style='text-decoration: none;'> <b>link</b></a> to download your attached file </p><br/>" +
                   " Thanks and Regards, <br> Team Zuptu";
                }
                else if (File == "" && FileName == "")
                {
                    temp = "<html><head></head><body> <div><p><b>" + user.FirstName + " " + user.LastName + "</b> have some issues, the details of the ticket are listed below.</p></div>" +
                         "<br/><b>Company Name:: </b>" + user.Company.Name +
                        "<br/><b>User Email:: </b>" + user.Email + "<br/>" +

                         "<br/><b>Priority:: </b>" + u.Priority + "<br/>" +
                         "<b>Ticket ID:: </b>" + u.TicketNo + "<br/>" +
                    "<b>Subject:: </b>" + u.Subject + "<br/>" +
                    "<b>Message:: </b>" + u.Message + "<br/><br/>" +
                    
                    " Thanks and Regards, <br> Team Zuptu";
                }
            }

           
            

            temp += "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";
            //string link = "https://zuptu.nodlays.com/Auth/resetpassword?val=" + StringCipher.Base64Encode(email) + "&t=" + StringCipher.Base64Encode(DateTime.Now.AddHours(6).Ticks.ToString())+ "&i=" + StringCipher.Base64Encode(Id.ToString());
            if(File!="" && FileName != "")
            {
                string link = ProjectVaraiables.SYSTEM_URL + "/Trainee/downloadmailfile?filepath=" + File+"&name="+File+ "&companyid=" + user.CompanyID;
                // string link =  + StringCipher.Base64Encode(email) + "&t=" + StringCipher.Base64Encode(DateTime.Now.AddHours(6).Ticks.ToString())+ "&i=" + StringCipher.Base64Encode(Id.ToString());
                link = link.Replace("+", "%20");
                temp = temp.Replace("LINKFORFORGOTPASSWORD", link);
            }


            string Sub = "["+u.Priority+"]"+ " "+u.Subject;

            sendForgetMail(email, temp,Sub);
            return true;
        }


        public bool inviteUser(string email, int Id,int newcompanyid=-1)
        {
            CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
            Company compnay = new Company();
            if (newcompanyid == -1)
            {
                 compnay = new CompanyBL().getCompanyById(Convert.ToInt32(logedinuser.Company));

            }
            else
            {
                 compnay = new CompanyBL().getCompanyById(Convert.ToInt32(newcompanyid));

            }

            string text = "<link href='https://fonts.googleapis.com/css?family=Bree+Serif' rel='stylesheet'><style>  * {";
            text += "  font-family: 'Bree Serif', serif; }";
            text += " .list-group-item {       border: none;  }    .hor {      border-bottom: 5px solid black;   }";
            text += " .line {       margin-bottom: 20px; }";

            string temp = "<html><head></head><body> <div><p>Welcome to Zuptu - your team and task management solution for <b>"+compnay.Name+"!</b> </p>" +
                "<p>Please click this <a target='_blank' href='LINKFORFORGOTPASSWORD' style='text-decoration: none;'><b> link</b></a> to join our system.  You have 72 hours to complete the registration process.  During your initial log-in, you will need to use <b>" +compnay.Name+ "</b> as the Company Name, your email, and then create a password to register.</p></div></b> We look forward to working with you! <br><br> <b>Team Zuptu</b>";

            temp += "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";
          //string link = "https://zuptu.nodlays.com/Auth/isotaskingUpdate?val=" + StringCipher.Base64Encode(email) + "&t=" + StringCipher.Base64Encode(DateTime.Now.AddHours(72).Ticks.ToString()) + "&IsUser=" + "yes" + "&i=" + StringCipher.Base64Encode(Id.ToString());
          string link = ProjectVaraiables.InviteUser + StringCipher.Base64Encode(email) + "&t=" + StringCipher.Base64Encode(DateTime.Now.AddHours(72).Ticks.ToString()) + "&IsUser=" + "yes" + "&i=" + StringCipher.Base64Encode(Id.ToString());
            //   string link = "http://localhost:60126/Auth/isotaskingUpdate?val=" + StringCipher.Base64Encode(email) + "&t=" + StringCipher.Base64Encode(DateTime.Now.AddHours(72).Ticks.ToString()) + "&IsUser=" + "yes" + "&i=" + StringCipher.Base64Encode(Id.ToString());
            //link = link.Replace("+", "%20");
            temp = temp.Replace("LINKFORFORGOTPASSWORD", link);

            sendForgetMail(email, temp, "Zuptu Invitation");
            return true;
        }


        public bool UpdtaeCanvasMail(string Id, string CanvasMail)
        {
            CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

            Company compnay = new CompanyBL().getCompanyById(Convert.ToInt32(logedinuser.Company));

            string text = "<link href='https://fonts.googleapis.com/css?family=Bree+Serif' rel='stylesheet'><style>  * {";
            text += "  font-family: 'Bree Serif', serif; }";
            text += " .list-group-item {       border: none;  }    .hor {      border-bottom: 5px solid black;   }";
            text += " .line {       margin-bottom: 20px; }";

            string temp = "<html><head></head><body> <div><p >Dear User,<br> <p >  Please click on following <a target='_blank' href='LINKFORFORGOTPASSWORD' style='text-decoration: none;'><b> link</b></a> to update your Canvas Email. This link will expire in 72 hrs from now.</p></div></b> <br><br> Thanks and Regards, </br> Team Zuptu";

            temp += "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";
            //string link = "https://zuptu.nodlays.com/Utilities/UpdateCanvasLoginIdAction?val=" + StringCipher.Base64Encode(CanvasMail) + "&t=" + StringCipher.Base64Encode(DateTime.Now.AddHours(72).Ticks.ToString()) + "&IsUser=" + Id ;
            string link = ProjectVaraiables.InviteCanvasMail + StringCipher.Base64Encode(CanvasMail) + "&t=" + StringCipher.Base64Encode(DateTime.Now.AddHours(72).Ticks.ToString()) + "&User=" + Id ;
             //string link = "http://localhost:60126/Utilities/UpdateCanvasLoginIdAction?val=" + StringCipher.Base64Encode(CanvasMail) + "&t=" + StringCipher.Base64Encode(DateTime.Now.AddHours(72).Ticks.ToString()) + "&User=" + Id;
            //link = link.Replace("+", "%20");
            temp = temp.Replace("LINKFORFORGOTPASSWORD", link);

            sendForgetMail(CanvasMail, temp, "Confirmation for Canvas Email");
            return true;
        }
        public string getEmployeeTasks(User item)
        {
            string content = "";
            foreach (var t in item.User_Task1.Where(x => x.IsActive == 1))
            {
                if (t.CompletionDate == null)
                    content = content + System.Environment.NewLine + "Task Name : " + t.Task.Name + " Due Date : " + t.EndDate;
            }
            foreach (var t in item.GroupTask_User.Where(x => x.IsActive == 1).ToList())
            {
                if (t.GroupTasks_Details.CompletionDate == null)
                    content = content + System.Environment.NewLine + "Group Task Name : " + t.GroupTasks_Details.GroupTask.Name + " Due Date : " + t.GroupTasks_Details.EndDate;

            }

            return content;

        }

        public void mail(List<string> emails, List<string> details, string title = "Zuptu Update")
        {

            var fromAddress = new MailAddress(ProjectVaraiables.EMAIL, title);

             string fromPassword = ProjectVaraiables.PASSWORD;
            string subject = title;


            var smtp = new SmtpClient
            {
                Host = ProjectVaraiables.WEBHOST,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            
            if (details.Count == emails.Count)
            {
                for (int i = 0; i < emails.Count; i++)
                {


                    using (var message = new MailMessage(fromAddress, new MailAddress(emails[i]))
                    {
                        Subject = subject,
                        Body = details[i]
                    })
                    {
                        smtp.Send(message);
                    }
                }
            }
            else
            {
                if (details.Count > 0)
                {
                    for (int i = 0; i < emails.Count; i++)
                    {


                        using (var message = new MailMessage(fromAddress, new MailAddress(emails[i]))
                        {
                            Subject = subject,
                            Body = details[0]
                        })
                        {
                            smtp.Send(message);
                        }
                    }

                }
            }
        }


        public void Remindermail(string email, string details, string title = "Zuptu Task Reminder")
        {
            if (email.Contains("/"))
            {
                string[] star = email.Split('/');
                for (int i = 0; i < star.Length; i++)
                {

                    var fromAddress = new MailAddress(ProjectVaraiables.EMAIL, "Zuptu System");

                    string fromPassword = ProjectVaraiables.PASSWORD;
                    string subject = title;


                    var smtp = new SmtpClient
                    {
                        Host = ProjectVaraiables.WEBHOST,
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    if (email != null)
                    {
                        using (var message = new MailMessage(fromAddress, new MailAddress(star[i]))
                        {
                            Subject = subject,
                            Body = details
                        })
                        {
                            smtp.Send(message);
                        }

                    }
                }
            }
            else
            {
                var fromAddress = new MailAddress(ProjectVaraiables.EMAIL, "Zuptu System");

                string fromPassword = ProjectVaraiables.PASSWORD;
                string subject = title;


                var smtp = new SmtpClient
                {
                    Host = ProjectVaraiables.WEBHOST,
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                if (email != null)
                {
                    using (var message = new MailMessage(fromAddress, new MailAddress(email))
                    {
                        Subject = subject,
                        Body = details
                    })
                    {
                        smtp.Send(message);
                    }

                }
            }


        
           
        }

        public void CanvasCourseAssingingMail(string email, string details, string title)
        {

            var fromAddress = new MailAddress(ProjectVaraiables.EMAIL, "Zuptu System");

             string fromPassword = ProjectVaraiables.PASSWORD;
            string subject = title;


            var smtp = new SmtpClient
            {
                Host = ProjectVaraiables.WEBHOST,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            if (email != null)
            {
                using (var message = new MailMessage(fromAddress, new MailAddress(email))
                {
                    Subject = subject,
                    Body = details
                })
                {
                    smtp.Send(message);
                }
            }

        }

        
        public void sendForgetMail(string email, string content, string title)
        {
            try
            {
                var fromAddress = new MailAddress(ProjectVaraiables.EMAIL, "Zuptu");

                 string fromPassword = ProjectVaraiables.PASSWORD;
                string subject = title;
                string body = content;

                var smtp = new SmtpClient
                {

                    Host = ProjectVaraiables.WEBHOST,
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, new MailAddress(email))
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch(Exception e)
            {

            }

        }
    }
}