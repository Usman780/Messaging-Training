using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Auth.OAuth2.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainingTracker.HelpingClasses;
using TrainingTracker.HelpingClasses.GoogleCalendar;

namespace TrainingTracker.Controllers
{
    public class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        public ActionResult Error()
        {
            return View();
        }
        protected override FlowMetadata FlowData
        {
            get { return flowMetaData; }
        }

        private FlowMetadata flowMetaData { get; }

        public AuthCallbackController()
        {
            var dataStore = new DataStore();
            var clientID =ProjectVaraiables.GOOGLE_CLIENT_ID;
            var clientSecret = ProjectVaraiables.GOOGLE_SECRET_KEY;
            flowMetaData = new GoogleAppFlowMetaData(dataStore, clientID, clientSecret);
        }

        public AuthCallbackController(FlowMetadata flow)
        {
            flowMetaData = flow;
        }

        public override async Task<ActionResult> IndexAsync(AuthorizationCodeResponseUrl authorizationCode, CancellationToken taskCancellationToken)
        {
            if (string.IsNullOrEmpty(authorizationCode.Code))
            {
                var errorResponse = new TokenErrorResponse(authorizationCode);

                return OnTokenError(errorResponse);
            }

            var returnUrl = Request.Url.ToString();
            returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("?"));

            await Flow.ExchangeCodeForTokenAsync(UserId, authorizationCode.Code, returnUrl,
                taskCancellationToken);

            var success = GoogleCalendarAsyncer.SyncToGoogleCalendar(this);
            

            return Redirect(Url.Content("~/Auth/Index"));
        }

        protected override ActionResult OnTokenError(TokenErrorResponse errorResponse)
        {
            return Redirect(Url.Content("~/"));
        }
    }
} 