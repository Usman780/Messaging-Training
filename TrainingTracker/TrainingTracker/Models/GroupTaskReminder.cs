//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingTracker.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GroupTaskReminder
    {
        public int Id { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> ReminderTime { get; set; }
        public Nullable<int> IsSent { get; set; }
        public Nullable<int> GroupTasks_DetailsID { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> CompanyId { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual GroupTasks_Details GroupTasks_Details { get; set; }
        public virtual User User { get; set; }
    }
}