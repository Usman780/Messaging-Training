using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace MessagingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            new SMSService().sendMessage("Hi how are you?", "+15876024203");
        }
    }
}
