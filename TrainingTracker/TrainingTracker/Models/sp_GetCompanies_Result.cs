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
    
    public partial class sp_GetCompanies_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone_Number { get; set; }
        public Nullable<int> isActive { get; set; }
        public string SlackWebhook { get; set; }
        public Nullable<int> ManagerNumber { get; set; }
        public Nullable<int> EmployeeNumber { get; set; }
        public Nullable<int> IsFavReport { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> IsLMS { get; set; }
        public Nullable<int> IsDocManager { get; set; }
        public string LmsBanner { get; set; }
        public Nullable<int> AdminNumber { get; set; }
    }
}
