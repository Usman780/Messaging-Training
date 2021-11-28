using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public int IsActive { get; set; }
      
        public string Name { get; set; }
        public string Task { get; set; }
        public string StartDate { get; set; }
        public string EndtDate { get; set; }
        public string WorkStatus { get; set; }
        public string Priority { get; set; }
        public string EncryptedId { get; set; }
        public string Temp { get; set; }
        public string AssignedBy { get; set; }
        public int LogedInId { get; set; }
        public int Role { get; set; }

    }
}