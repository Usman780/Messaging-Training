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
    
    public partial class sp_AddUpdateGroupTasks_Detail_Result
    {
        public int Id { get; set; }
        public Nullable<double> Hours { get; set; }
        public Nullable<double> CEU { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Grade { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public Nullable<int> IsActive { get; set; }
        public string Notes { get; set; }
        public Nullable<int> GroupTaskId { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<int> Priority { get; set; }
        public string SlackChannel { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> TicketSortBy { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<int> RepeatTime { get; set; }
        public Nullable<int> RepeatDeadline { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}
