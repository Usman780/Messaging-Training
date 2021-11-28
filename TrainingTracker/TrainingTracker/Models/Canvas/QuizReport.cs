using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class QuizReport
    {
        public int id { get; set; }
        public int quiz_id { get; set; }
        public string report_type { get; set; }
        public string readable_type { get; set; }
        public bool includes_all_versions { get; set; }
        public bool anonymous { get; set; }
        public bool generatable { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string url { get; set; }
        public object file { get; set; }
        public object progress_url { get; set; }
        public object progress { get; set; }
    }
}