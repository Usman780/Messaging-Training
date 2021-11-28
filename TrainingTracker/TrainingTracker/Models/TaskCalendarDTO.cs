using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class TaskCalendarDTO
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string color { get; set; }
        public string url { get; set; }
        public Boolean editable { get; set; }
        public string DivisionID { get; set; }
        public string DepartmentID { get; set; }
        public string description { get; set; }
        public string EDate { get; set; }
        public int TaskPriority { get; set; }
        public string PrimaryLead { get; set; }
        public string SecondaryLead { get; set; }
        public string GtReminder { get; set; }
        public string TReminder { get; set; }
        public string DReminder { get; set; }
        public int TaskId { get; set; }
        public int TaskType { get; set; }
        public int TaskStatus { get; set; }
        public string IsOutlook { get; set; }
        public string IsGoogle { get; set; }
        public string OldURL { get; set; }

    }
}