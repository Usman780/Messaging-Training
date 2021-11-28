using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class CalendarEvents
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime start_at { get; set; }
        public DateTime end_at { get; set; }
        public string description { get; set; }
        public string location_name { get; set; }
        public string location_address { get; set; }
        public string context_code { get; set; }
        public object effective_context_code { get; set; }
        public string all_context_codes { get; set; }
        public string workflow_state { get; set; }
        public bool hidden { get; set; }
        public object parent_event_id { get; set; }
        public int child_events_count { get; set; }
        public object child_events { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string all_day_date { get; set; }
        public bool all_day { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public object appointment_group_id { get; set; }
        public object appointment_group_url { get; set; }
        public bool own_reservation { get; set; }
        public object reserve_url { get; set; }
        public bool reserved { get; set; }
        public string participant_type { get; set; }
        public object participants_per_appointment { get; set; }
        public object available_slots { get; set; }
        public object user { get; set; }
        public object group { get; set; }
    }
}