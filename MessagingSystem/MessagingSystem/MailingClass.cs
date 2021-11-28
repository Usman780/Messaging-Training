using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MessagingSystem
{
    public class MailingClass
    {
       
        public void mail(List<string> emails, List<string> details, string title = "ISO Training Update")
        {

            var fromAddress = new MailAddress(MessagingVariables.EMAIL, "Iso Training System");

            const string fromPassword = MessagingVariables.PASSWORD;
            string subject = title;


            var smtp = new SmtpClient
            {
                Host = MessagingVariables.WEBHOST,
                Port = 25,
                EnableSsl = false,
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
        public void sendForgetMail(string email, string content, string title)
        {

            var fromAddress = new MailAddress(MessagingVariables.EMAIL, title);

            const string fromPassword = MessagingVariables.PASSWORD;
            string subject = title;
            string body = content;

            var smtp = new SmtpClient
            {
                
                Host = MessagingVariables.WEBHOST,
                Port = 25,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, new MailAddress(email))
            {
                IsBodyHtml=true,
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

        }
    }
}
