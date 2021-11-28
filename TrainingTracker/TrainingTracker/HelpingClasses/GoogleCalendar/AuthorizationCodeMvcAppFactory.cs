using Google.Apis.Auth.OAuth2.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingTracker.HelpingClasses.GoogleCalendar
{
    public class AuthorizationCodeMvcAppFactory
    {
        public FlowMetadata FlowMetadata { get; }
        public Controller CurrentController { get; }

        public AuthorizationCodeMvcAppFactory(FlowMetadata flowMetadata, Controller currentController)
        {
            FlowMetadata = flowMetadata;
            CurrentController = currentController;
        }

        public AuthorizationCodeMvcApp Create()
        {
            var result = new AuthorizationCodeMvcApp(CurrentController, FlowMetadata);
            return result;
        }   
    }
}