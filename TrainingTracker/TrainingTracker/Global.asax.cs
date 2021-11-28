using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TrainingTracker
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //private const string jobCacheAction = "http://localhost:60126/Task/checkTasks";
        //private const string jobCacheAction = "https://isotasking.com/Task/checkTasks";
        private const string jobCacheAction = "https://dev.zuptu.systems/Task/checkTasks";

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //logging
            log4net.Config.XmlConfigurator.Configure();
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{

        //    Exception ex = Server.GetLastError();
        //    Server.ClearError();
        //    Response.Redirect("Error");
        //}

        protected void Application_BeginRequest(object sender, EventArgs e) 
        {
            //if (!Context.Request.IsSecureConnection)
            //    Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));

            if (HttpContext.Current.Cache["jobkey"] == null)
            {
                HttpContext.Current.Cache.Add("jobkey", "jobvalue", null, DateTime.MaxValue, TimeSpan.FromMinutes(60), System.Web.Caching.CacheItemPriority.Default, JobCacheRemoved);
            }
        }

        private void JobCacheRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            var client = new WebClient();
            client.DownloadData(jobCacheAction);
        }
    }
}
