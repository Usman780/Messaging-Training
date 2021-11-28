using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class LateReportDTO

    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PhoneNumber { get; set; }
        public int LateDays { get; set; }
    }
}