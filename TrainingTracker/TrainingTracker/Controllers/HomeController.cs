using System.Threading;
using System.Web.Mvc;
using System.Web.UI;
using TrainingTracker.HelpingClasses.GoogleCalendar;
using TrainingTracker.Models;
using TrainingTracker.HelpingClasses;
using System.Configuration;
using System.Linq;
using TrainingTracker.BL;
using System.Web;
using System;
using RestSharp;
using TrainingTracker.Helper_Classes;

namespace TrainingTracker.Controllers
{
    public class HomeController : Controller
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        MainMailClass errormail = new MainMailClass();

        public void clearCache()
        {
            var oldCtx = Request.GetOwinContext();
            var oldAuthManager = oldCtx.Authentication;
            oldAuthManager.SignOut("ApplicationCookie");

        }
        public bool AuthenticateUser()
        {

            if (logedinuser.Name == null)
            {
                clearCache();
                return false;
            }

            Models.User user = new UserBL().getUsersById(logedinuser.Id);

            if (user == null)
            {
                clearCache();
                return false;
            }

            if (logedinuser.Role != user.Role)
            {
                clearCache();
                return false;
            }

            return true;
        }

        public bool testPuch()
        {
            PushDataDTO obj = new PushDataDTO();
            obj.TaskId = "1";
            obj.GroupTaskDetailsId = "2";
            bool x = General_Purpose.SendPushNotification("sddsdggg", "Message from web APP", "Message from web", obj, "28");

            return x;
        }
        ////public string EncryptID(int id)
        ////{
        ////    return StringCipher.Encrypt(id.ToString(), "zuptu");
        ////}
        public string DecryptID(string id)
        {
            if (id.Contains(' '))
            {
                id = id.Replace(' ', '+');
            }
            return StringCipher.Decrypt(id.ToString(), "zuptu");
        }
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult ConnectionString()
        {
            try
            {
                ViewBag.ConnectionStrings =
                ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>();
                ViewBag.AppSettings = ConfigurationManager.AppSettings;
                DatabaseEntities dd = new DatabaseEntities();

                var x = dd.Worktypes.FirstOrDefault().Name;
                ViewBag.worktype = x;
                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult PrivacyPolicy()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult Landing()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }
         
        public ActionResult ErrorPage()
        {
            return   View();
        }

        public  ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult About()
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                ViewBag.Message = "Your application description page.";

                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult Contact(string message="",string error="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                ViewBag.message = message;
                ViewBag.error = error;

                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult PostContact(Contact obj)
        {
            try
            {
                if (obj.Subject == "" || obj.Subject == null)
                    return RedirectToAction("Contact", "Home", new { error = "Subject must have value" });
                if (obj.Message == "" || obj.Message == null)
                    return RedirectToAction("Contact", "Home", new { error = "Message must have value" });
                if (obj.Priority == "" || obj.Priority == null)
                    return RedirectToAction("Contact", "Home", new { error = "Priority must be selected" });

                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                obj.UserId = logedinuser.Id;
                int count = Request.Files.Count;

                var file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    BlobManager BlobManagerObj = new BlobManager(ProjectVaraiables.COMPANY_ALPHABET + Convert.ToInt32(logedinuser.Company));
                    obj.FileName = file.FileName;
                    string fileName = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                    string FileAbsoluteUri = BlobManagerObj.UploadFile(file, fileName);

                    obj.File = fileName;

                }
                obj.SendingTime = DateTime.Now;
                obj.CreatedAt = DateTime.Now;
                obj.IsActive = 1;
                int c = new ContactBAL().getAllContactUsList().Count();
                if (c == 0)
                    obj.TicketNo = "ZP_" + 900;
                else
                {
                    Contact cc = new ContactBAL().getAllContactUsList().LastOrDefault();

                    obj.TicketNo = "ZP_" + (cc.Id + 900);

                }


                string mails = "";
                if (obj.Priority == "0")
                {
                    obj.Priority = "Normal";
                    mails = ProjectVaraiables.NormalContactMails;
                }
                else if (obj.Priority == "1")
                {
                    obj.Priority = "Moderate";
                    mails = ProjectVaraiables.ModerateContactMails;
                }
                else
                {
                    obj.Priority = "Severe";
                    mails = ProjectVaraiables.SevereContactMails;
                }



                new ContactBAL().AddContactUs(obj);



                MainMailClass mail = new MainMailClass();

                User user = new UserBL().getUsersById(logedinuser.Id);

                mail.ContactMail(user.Email, obj.Id, obj.File, obj.FileName);



                string[] sendingmails;

                if (mails != null && mails != "")
                {

                    if (mails.Contains("/"))
                    {
                        sendingmails = mails.Split('/');
                        for (int i = 0; i < sendingmails.Length; i++)
                        {
                            mail.ContactMail(sendingmails[i], obj.Id, obj.File, obj.FileName);
                        }
                    }
                    else
                    {
                        mail.ContactMail(mails, obj.Id, obj.File, obj.FileName);
                    }
                }




                return RedirectToAction("Contact", "Home", new { message = "Thank you for contacting us" });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }
        public ActionResult GroupTaskDetails()
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult BatteryTraining()
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult TaskManager()
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        #region partialViews
        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public ActionResult cssCache()
        {
            return PartialView("CoreCss_Files","Shared");
        }


       
        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public ActionResult jsCache()
        {
           
            return PartialView("Corejs_Files", "Shared");
        }
        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public ActionResult jsAuthCache()
        {
            return PartialView("Corejs_Files_AuthIndex", "Shared");
        }
        #endregion
    }
}