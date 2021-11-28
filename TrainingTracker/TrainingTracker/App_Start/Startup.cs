using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Identity.Client;
using System.Web;
using System.IdentityModel.Claims;
using System.Configuration;
using System;
using TrainingTracker.HelpingClasses.Outlook;
using TrainingTracker.HelpingClasses;

using TrainingTracker.BL;
using TrainingTracker.Models;

namespace TrainingTracker.App_Start
{
    public class Startup
    {
       // CheckAuthenticationDTO logedinuser =  General_Purpose.CheckAuthentication();

        public static string appId = ProjectVaraiables.APPID;
        public static string appPassword = ProjectVaraiables.AppPassword;
        public static string redirectUri = ProjectVaraiables.RedirectUri;
        public static string[] scopes = ProjectVaraiables.AppScopes
          .Replace(' ', ',').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Auth/Login"),
                ExpireTimeSpan = TimeSpan.FromDays(1)

            });

            app.MapSignalR(); //required to referencing SignalR classes

            app.UseOpenIdConnectAuthentication(
              new OpenIdConnectAuthenticationOptions
              {
                  ClientId = appId,
                  Authority = "https://login.microsoftonline.com/common/v2.0",
                  Scope = "openid offline_access profile email " + string.Join(" ", scopes),
                  RedirectUri = redirectUri,
                  PostLogoutRedirectUri = "/",
                  TokenValidationParameters = new TokenValidationParameters
                  {
                      // For demo purposes only, see below
                      ValidateIssuer = false

                      // In a real multitenant app, you would add logic to determine whether the
                      // issuer was from an authorized tenant
                      //ValidateIssuer = true,
                      //IssuerValidator = (issuer, token, tvp) =>
                      //{
                      //  if (MyCustomTenantValidation(issuer))
                      //  {
                      //    return issuer;
                      //  }
                      //  else
                      //  {
                      //    throw new SecurityTokenInvalidIssuerException("Invalid issuer");
                      //  }
                      //}
                  },
                  Notifications = new OpenIdConnectAuthenticationNotifications
                  {
                      AuthenticationFailed = OnAuthenticationFailed,
                      AuthorizationCodeReceived = OnAuthorizationCodeReceived
                  }
              }
            );
        }
        private System.Threading.Tasks.Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage,
            OpenIdConnectAuthenticationOptions> notification)
        {
            notification.HandleResponse();
            string redirect = "/Home/Error?message=" + notification.Exception.Message;
            if (notification.ProtocolMessage != null && !string.IsNullOrEmpty(notification.ProtocolMessage.ErrorDescription))
            {
                redirect += "&debug=" + notification.ProtocolMessage.ErrorDescription;
            }
            notification.Response.Redirect(redirect);
            return System.Threading.Tasks.Task.FromResult(0);
        }

        private async System.Threading.Tasks.Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedNotification notification)
        {

            // Get the signed in user's id and create a token cache
            string signedInUserId = notification.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            SessionTokenCache tokenCache = new SessionTokenCache(signedInUserId,
                notification.OwinContext.Environment["System.Web.HttpContextBase"] as string);
            HttpContext.Current.Session["UserId"] = signedInUserId;
            ConfidentialClientApplication cca = new ConfidentialClientApplication(
                appId, redirectUri, new ClientCredential(appPassword), tokenCache.GetMsalCacheInstance(), null);

            try
            {
                var result = await cca.AcquireTokenByAuthorizationCodeAsync(notification.Code, scopes);
                //TrainingTracker.Models.DatabaseEntities de = new TrainingTracker.Models.DatabaseEntities();
                //UserBL ubl = new UserBL();
                //TrainingTracker.Models.User user = ubl.getUsersById(logedinuser.Id, de);
                //user.OutlookToken = result.AccessToken;
                //ubl.UpdateUsers(user, de);
            }
            catch (MsalException ex)
            {
                string message = "AcquireTokenByAuthorizationCodeAsync threw an exception";
                string debug = ex.Message;
                notification.HandleResponse();
                notification.Response.Redirect("/Home/Error?message=" + message + "&debug=" + debug);
            }
        }





    }
}