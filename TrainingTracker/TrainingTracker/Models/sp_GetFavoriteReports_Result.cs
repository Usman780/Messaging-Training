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
    
    public partial class sp_GetFavoriteReports_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public string TaskName { get; set; }
        public string TaskType { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string StartingDate { get; set; }
        public string EndingDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CompleteDate { get; set; }
        public string UserType { get; set; }
        public string Employee { get; set; }
        public Nullable<int> ReportType { get; set; }
        public Nullable<int> IsStatic { get; set; }
        public Nullable<int> User_Id { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<System.DateTime> Created_At { get; set; }
        public string WorkerType { get; set; }
        public string FilterDate { get; set; }
        public Nullable<int> IsShared { get; set; }
        public Nullable<int> SharedBy { get; set; }
        public string SharedDescription { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}
