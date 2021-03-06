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
    
    public partial class Quiz_Result
    {
        public int Id { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> Course_UserTaskID { get; set; }
        public Nullable<int> QuestionID { get; set; }
        public Nullable<int> OptionID { get; set; }
        public Nullable<System.DateTime> SubmittedAt { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        public Nullable<int> Status { get; set; }
    
        public virtual Course_UserTask Course_UserTask { get; set; }
        public virtual Option Option { get; set; }
        public virtual Question Question { get; set; }
        public virtual User User { get; set; }
    }
}
