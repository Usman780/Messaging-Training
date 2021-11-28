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
    
    public partial class sp_FileVersionAddUpdate_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> VersionNo { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> CheckInTime { get; set; }
        public Nullable<System.DateTime> CheckOutTime { get; set; }
        public Nullable<int> FolderId { get; set; }
        public string FilePath { get; set; }
        public string Privacy { get; set; }
        public Nullable<int> IsSigned { get; set; }
        public string SignedImage { get; set; }
        public Nullable<int> SignedBy { get; set; }
        public Nullable<System.DateTime> ArchiveDate { get; set; }
        public Nullable<System.DateTime> UploadingDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public Nullable<int> CheckIn { get; set; }
        public string CheckInMessage { get; set; }
        public Nullable<int> CheckOut { get; set; }
        public Nullable<int> CheckOutBy { get; set; }
        public Nullable<int> UploadedBy { get; set; }
        public Nullable<int> FileId { get; set; }
        public Nullable<int> CurrentVersion { get; set; }
        public Nullable<System.DateTime> ArchivedDate { get; set; }
        public Nullable<int> ArchivedBy { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
    }
}
