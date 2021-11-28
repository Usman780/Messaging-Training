using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string EncryptedId { get; set; }
        public int IsActive { get; set; }

        public string CourseName { get; set; }
        public string TaskName { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public string CompletedAt { get; set; }
        public string ResultStatus { get; set; }
        public int SerialNumber { get; set; }
    }
}