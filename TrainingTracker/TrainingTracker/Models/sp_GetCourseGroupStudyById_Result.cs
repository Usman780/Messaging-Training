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
    
    public partial class sp_GetCourseGroupStudyById_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> CourseID { get; set; }
        public Nullable<int> User_TaskId { get; set; }
        public Nullable<int> LeadBy { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}
