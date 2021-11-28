using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Calendar.v3;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.HelpingClasses.GoogleCalendar
{
    public class GoogleAppFlowMetaData : FlowMetadata
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        private IAuthorizationCodeFlow flow { get; set; }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }

        public GoogleAppFlowMetaData(IDataStore dataStore, string clientID, string clientSecret)
        {
            var flowInitializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientID,
                    ClientSecret = clientSecret
                },
                Scopes = new[] { CalendarService.Scope.Calendar },
                DataStore = dataStore
            };
            flow = new GoogleAuthorizationCodeFlow(flowInitializer);
        }

        public override string GetUserId(Controller controller)
        {
            // return "2";
            string v = Convert.ToString(logedinuser.Id)+ ">"+ Convert.ToString(logedinuser.Role);
            return v;
        }

        public override string AuthCallback
        {
            get
            {
                return @"/AuthCallback/IndexAsync";
            }
        }
    }
}