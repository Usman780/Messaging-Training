using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class DueDateReminderDTO
    {
        public string Id { get; set; }
        public string Task { get; set; }
        public DateTime DueDate { get; set; }
        public int Late { get; set; }
        public int Role { get; set; }

    }
}