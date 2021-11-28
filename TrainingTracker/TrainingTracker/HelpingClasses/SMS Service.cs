using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TrainingTracker.HelpingClasses
{
    public class SMS_Service
    {
        public static void sendMessage(string content, string phoneNumber)
        {
            try
            {
                 string accountSid = ProjectVaraiables.TWILIO_ACCOUNTSID;
                 string authToken = ProjectVaraiables.TWILIO_AUTHENTTOKEN;
                TwilioClient.Init(accountSid, authToken);

                var to = new PhoneNumber(phoneNumber);
                var message = MessageResource.Create(
                    to,
                    from: new PhoneNumber(ProjectVaraiables.TWILIO_SENDERNUMBER),
                    body: content);
            }catch(Exception e)
            {

            }
          
        }

        public static void sendSMSWrapper(List<string> content, List<string> numbers)
        {
            if(content.Count==numbers.Count)
            {
                for(int i=0;i<content.Count;i++)
                {
                    sendMessage(content[i], numbers[i]);
                }
            }
            else if (content.Count>0)
            {
                foreach (var item in numbers)
                {
                    sendMessage(content[0], item);
                }
            }

        }



    }
}