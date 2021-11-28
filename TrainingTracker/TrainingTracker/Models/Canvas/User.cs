using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class CanvasUser
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime created_at { get; set; }
        public string sortable_name { get; set; }
        public string short_name { get; set; }
        public object sis_user_id { get; set; }
        public object integration_id { get; set; }
        public string login_id { get; set; }
        public string email { get; set; }
    }
}