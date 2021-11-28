using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace MessagingSystem
{
    class SMSService
    {
        public void sendMessage(string content,string phoneNumber)
        {
            const string accountSid = MessagingVariables.TWILIO_ACCOUNTSID;
            const string authToken = MessagingVariables.TWILIO_AUTHENTTOKEN;
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber(phoneNumber);
            var message = MessageResource.Create(
                to,
                from: new PhoneNumber(MessagingVariables.TWILIO_SENDERNUMBER),
                body: content);

            Console.WriteLine(message.Sid);
        }


    }
}
