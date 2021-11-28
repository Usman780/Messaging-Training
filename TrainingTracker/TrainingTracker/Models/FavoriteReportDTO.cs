using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class FavoriteReportDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public string TaskName { get; set; }
        public string TaskType { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string StartingDate { get; set; }
        public string EndingDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CompleteDate { get; set; }
        public string UserType { get; set; }
        public string Employee { get; set; }
        public string ReportType { get; set; }
        public string IsStatic { get; set; }
        public int User_Id { get; set; }
        public int IsActive { get; set; }
        public DateTime Created_At { get; set; }
        public string WorkerType { get; set; }
        public string FilterDate { get; set; }
        public string IsShared { get; set; }
        public string SharedBy { get; set; }
        public string SharedDescription { get; set; }

    }
}