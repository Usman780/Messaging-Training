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
    
    public partial class sp_GetOldPasswords_Result
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}
