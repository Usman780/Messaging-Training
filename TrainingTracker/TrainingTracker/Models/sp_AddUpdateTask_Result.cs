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
    
    public partial class sp_AddUpdateTask_Result
    {
        public int Id { get; set; }
        public Nullable<int> IsActive { get; set; }
        public string Name { get; set; }
        public Nullable<int> TaskTypeID { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public string Description { get; set; }
        public Nullable<double> Cost_ { get; set; }
        public Nullable<double> Hours { get; set; }
        public Nullable<double> CEU { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string File { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> IsPrivate { get; set; }
        public Nullable<int> CourseId { get; set; }
        public Nullable<int> IsResultAnnounced { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}
