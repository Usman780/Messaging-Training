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
    
    public partial class sp_GetUserTask_QuizResults_Result
    {
        public int Id { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> Course_UserTaskID { get; set; }
        public Nullable<int> QuizID { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> StartedAt { get; set; }
        public string Obtainedmarks { get; set; }
        public Nullable<System.DateTime> CompletedAt { get; set; }
        public Nullable<int> PublishedBy { get; set; }
        public Nullable<int> IsPublished { get; set; }
        public Nullable<int> ResultStatus { get; set; }
    }
}
