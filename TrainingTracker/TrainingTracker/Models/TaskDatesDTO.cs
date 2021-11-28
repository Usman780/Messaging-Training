using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class TaskDatesDTO
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CreationDate { get; set; }
        public string CompletionDate { get; set; }
    }
}