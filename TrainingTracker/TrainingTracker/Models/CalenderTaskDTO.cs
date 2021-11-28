using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class CalenderTaskDTO
    {
        //User_Task Model
        public int Id { get; set; }
        public string EncryptedTaskId { get; set; }        
        public Nullable<int> IsActive { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Notes { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<double> CEU { get; set; }
        public Nullable<int> Grad { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> TaskID { get; set; }
        public string TaskName { get; set; }
        public Nullable<int> CreatedID { get; set; }
        public Nullable<double> Hours { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public string File { get; set; }
        public Nullable<int> RepeatTime { get; set; }
        public Nullable<int> RepeatDeadline { get; set; }
        public Nullable<int> IsPrivate { get; set; }
        public Nullable<int> Priority { get; set; }

        public ICollection<ExtensionRequest> ExtensionRequests { get; set; }
        public ICollection<Task_Ticket> completedTickets { get; set; }
        public ICollection<Task_Ticket> uncompletedTickets { get; set; }
        
        public int completedTicketscount { get; set; }
        public int uncompletedTicketscount { get; set; }
       
        public ICollection<TaskCommentDTO> comments { get; set; }

        public Nullable<int> IsGroupTask { get; set; }
        public Nullable<int> SessionRole { get; set; }
        public Nullable<int> CurrentStatus { get; set; }
        public int StatusValue { get; set; }
        public IEnumerable<int> status { get; set; }
        public int Role { get; set; }
        public int Employee { get; set; } // to check the task (employee or manager)

    }
}