using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class DivisionReportDTO
    {
        public string DepartmentName { get; set; }

        public string DivisionName { get; set; }
        public string TaskType { get; set; }
        public string TaskName { get; set; }
        public string Priority { get; set; }
        public string StartDate { get; set; }
        public string DueDate { get; set; }
        public string CompletionDate { get; set; }
        public string CompletionStatus { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedTo { get; set; }

    }
}