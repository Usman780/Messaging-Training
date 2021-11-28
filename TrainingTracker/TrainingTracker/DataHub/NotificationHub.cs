using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TrainingTracker.DataHub
{
    public class NotificationHub : Hub
    {
        public void Hello(int a)
        {
            Clients.All.hello(a);
        }

        public void ExtensionNotification(string response, int companyId)
        {
            Clients.All.broadcastExtensionNotification(response, companyId);
        }
    }
}