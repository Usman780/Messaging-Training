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
    
    public partial class sp_GetFileLogById_Result
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> FileId { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> LogTime { get; set; }
        public Nullable<int> NoOfVersions { get; set; }
        public Nullable<int> Role { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}