using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using MessagingSystem;
using TrainingTracker.HelpingClasses.Slack;
using TrainingTracker.Models;
using TrainingTracker.BL;

namespace TrainingTracker.HelpingClasses
{
    public class Communication
    {
       static CheckAuthenticationDTO  logedinuser = General_Purpose.CheckAuthentication();

        public static void sendMessage(List<string> text,List<User> users, string groupTaskTitle=null, string channel=null, string title = "Zuptu Update")
        {
            try
            {
                List<string> emails = new List<string>();
                List<string> smsTexts = new List<string>(text);
                List<string> numbers = new List<string>();
                List<string> slackAddress = new List<string>();
                List<string> slackContent = new List<string>(text);
                List<string> slackCcontent = new List<string>();

               
                List<string> EmailText = new List<string>();
                List<string> SMSText = new List<string>();
                List<string> MainText = new List<string>(text);
                
               
                for (int i = 0; i < users.Count; i++)
                {

                    if (users[i].isMail == 1)
                    {
                        emails.Add(users[i].Email);
                        EmailText.Add(MainText[i]);

                    }
                    else
                    {
                        if (text.Count > i)
                            text.RemoveAt(i);

                    }
                    if (users[i].isSMS == 1)
                    {
                        numbers.Add(users[i].PhoneNumber);
                        SMSText.Add(MainText[i]);

                    }
                    else
                    {
                        if (smsTexts.Count > i)
                            smsTexts.RemoveAt(i);
                    }
                    
                        if (users[i].isSlack == 1)
                        {
                            if (users[i].SlackAddress != null)
                        {
                            slackAddress.Add(users[i].SlackAddress);
                            slackCcontent.Add(MainText[i]);
                        }
                                

                        }
                        else
                        {
                            if (slackContent.Count > i)
                                slackContent.RemoveAt(i);
                        }
                    




                }

                if (slackCcontent.Count > 0 && slackAddress.Count>0)
                {
                    Company comp = new CompanyBL().getCompanyById(Convert.ToInt32(logedinuser.Company));
                    if (comp.SlackWebhook != null)
                    {
                       
                        string slackWebhook = comp.SlackWebhook;
                        SlackClient slackClient = new SlackClient(slackWebhook);
                        if (channel != null)
                        {
                            slackClient.slackAdapter(new List<string>() { groupTaskTitle }, null, channel);
                        }
                     
                        slackClient.slackAdapter(slackCcontent, slackAddress);
                    }
                }
                MainMailClass mailingClass = new MainMailClass();
                //mailingClass.mail(emails, EmailText, title);
                HostingEnvironment.QueueBackgroundWorkItem(ctx => mailingClass.mail(emails, EmailText, title));
                HostingEnvironment.QueueBackgroundWorkItem(ctx => SMS_Service.sendSMSWrapper(SMSText, numbers));
            }catch(Exception e)
            {
                Console.WriteLine("exception");
            }
        }



    }
}