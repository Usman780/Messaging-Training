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
    using System.Collections.Generic;
    
    public partial class GroupTask
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GroupTask()
        {
            this.GroupTask_Task = new HashSet<GroupTask_Task>();
            this.GroupTasks_Details = new HashSet<GroupTasks_Details>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> IsPrivate { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> DivisionId { get; set; }
    
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupTask_Task> GroupTask_Task { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupTasks_Details> GroupTasks_Details { get; set; }
        public virtual Division Division { get; set; }
    }
}
