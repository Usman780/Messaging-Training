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
    
    public partial class sp_GetArchivedFolders_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> UserId { get; set; }
        public string FolderPath { get; set; }
        public Nullable<System.DateTime> ArchiveDate { get; set; }
        public string Privacy { get; set; }
        public Nullable<int> IsSharedRoot { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public Nullable<System.DateTime> ArchivedDate { get; set; }
        public Nullable<int> ArchivedBy { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
    }
}
