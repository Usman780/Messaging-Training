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
    
    public partial class sp_AddUpdateGroupTask_Ticket_Result
    {
        public int Id { get; set; }
        public Nullable<int> GroupTaskDetails_Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> CreationDatetime { get; set; }
        public Nullable<System.DateTime> CompletionDatetime { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> CompletedByUser { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<System.DateTime> FileUploadDate { get; set; }
        public string Ticket_FileName { get; set; }
        public string Ticket_File { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Position { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<System.DateTime> GTaskStartDate { get; set; }
        public Nullable<System.DateTime> GTaskEndDate { get; set; }
        public Nullable<int> IsDocMFile { get; set; }
    }
}
