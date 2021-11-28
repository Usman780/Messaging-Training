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
    
    public partial class TaskComment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskComment()
        {
            this.TaskComment1 = new HashSet<TaskComment>();
        }
    
        public int Id { get; set; }
        public string Comment { get; set; }
        public Nullable<int> TaskId { get; set; }
        public string Date { get; set; }
        public string File { get; set; }
        public string FileName { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> TaskStartDate { get; set; }
        public Nullable<System.DateTime> TaskEndDate { get; set; }
        public Nullable<int> IsDocMFile { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Department Department { get; set; }
        public virtual Division Division { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskComment> TaskComment1 { get; set; }
        public virtual TaskComment TaskComment2 { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual User_Task User_Task { get; set; }
    }
}
