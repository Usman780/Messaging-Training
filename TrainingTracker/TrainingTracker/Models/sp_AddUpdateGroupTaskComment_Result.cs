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
    
    public partial class sp_AddUpdateGroupTaskComment_Result
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public Nullable<int> GroupTaskUserId { get; set; }
        public string Date { get; set; }
        public Nullable<int> IsActive { get; set; }
        public string File { get; set; }
        public string FileName { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> GroupTaskDetailsId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> GTaskStartDate { get; set; }
        public Nullable<System.DateTime> GTaskEndDate { get; set; }
        public Nullable<int> IsDocMFile { get; set; }
    }
}
