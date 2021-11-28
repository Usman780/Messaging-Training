using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using TrainingTracker.Models;
using static Google.Apis.Auth.OAuth2.Web.AuthorizationCodeWebApp;

namespace TrainingTracker.HelpingClasses.GoogleCalendar
{
    public class GoogleCalendars
    {
        public async System.Threading.Tasks.Task<List<TaskCalendarDTO>> googleTasksAsync(AuthResult ar, User user)
        {


           
           
            string[] Scopes = { Google.Apis.Calendar.v3.CalendarService.Scope.Calendar };

            var result = ar;

             if (result.Credential != null)
            {

                var service = new Google.Apis.Calendar.v3.CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = ProjectVaraiables.GOOGLE_APP_Name,
                });

                EventsResource.ListRequest request = service.Events.List("primary");
                request.TimeMin = new DateTime(2018, 1, 1);
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 100;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                Events events = request.Execute();
                List<TaskCalendarDTO> data = new List<TaskCalendarDTO>();
                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items)
                    {

                        DateTime dates;

                        if(eventItem.Start.DateTime!=null)
                        dates = Convert.ToDateTime(eventItem.Start.DateTime);
                        else
                            dates = Convert.ToDateTime(eventItem.Start.Date);

                        DateTime datee ;
                        if(eventItem.End.DateTime!=null)
                         datee = Convert.ToDateTime(eventItem.End.DateTime);
                        else
                            datee = Convert.ToDateTime(eventItem.End.Date);
                        string color = string.Empty;
                        if (user.GoggleTaskColor == null)
                            color = "#4285F4";

                        TaskCalendarDTO x = new TaskCalendarDTO()
                        {
                            title = eventItem.Summary,
                            start = dates.ToString("yyyy-MM-dd"),
                            end = datee.ToString("yyyy-MM-dd"),
                            color = user.GoggleTaskColor,
                            IsGoogle = "1"


                        };
                        data.Add(x);
                    }
                }
                else
                {

                 }
                return data;
            }
            else
                return null;
        }
    }
}