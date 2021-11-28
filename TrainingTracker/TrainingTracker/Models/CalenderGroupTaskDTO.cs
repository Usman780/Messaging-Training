using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class CalenderGroupTaskDTO
    {
        //GroupTasks_Details
        public int Id { get; set; }
        public string EncryptedTaskId { get; set; }
        public Nullable<double> Hours { get; set; }
        public Nullable<double> CEU { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Grade { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public Nullable<int> IsActive { get; set; }
        public string Notes { get; set; }
        public Nullable<int> GroupTasks_DetailsId { get; set; }
        public Nullable<int> GroupTaskId { get; set; }
        public string GroupTaskName { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<int> Priority { get; set; }
        public string SlackChannel { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> grouptaskuserId { get; set; }

        public virtual ICollection<ExtensionRequest> ExtensionRequests { get; set; }
        public virtual GroupTask GroupTask { get; set; }
        public virtual ICollection<GroupTask_Ticket> completedgroupTickets { get; set; }
        public virtual ICollection<GroupTask_Ticket> uncompletedgroupTickets { get; set; }
        //public string completedgroupTicketName { get; set; }
        //public DateTime completedgroupTicketCreationDatetime { get; set; }
        public int completedgroupTicketscount { get; set; }
        //public string uncompletedgroupTicketName { get; set; }
        //public DateTime uncompletedgroupTicketCreationDatetime { get; set; }        
        public int uncompletedgroupTicketscount { get; set; }
        public virtual ICollection<GroupTask_User> GroupTask_User { get; set; }
        public virtual ICollection<TaskCommentDTO> GroupTaskComments { get; set; }
        public virtual ICollection<User> emp { get; set; }
        public virtual ICollection<User> mang { get; set; }
        public virtual User User { get; set; }

        public Nullable<int> SessionRole { get; set; }
        public Nullable<int> IsGroupTask { get; set; }
        public Nullable<int> CurrentStatus { get; set; }
        public string StatusValue { get; set; }
        public IEnumerable<int> status { get; set; }


    }
}