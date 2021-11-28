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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Certificates = new HashSet<Certificate>();
            this.Contacts = new HashSet<Contact>();
            this.Courses = new HashSet<Course>();
            this.Course_File = new HashSet<Course_File>();
            this.DelegateFiles = new HashSet<DelegateFile>();
            this.DelegateFiles1 = new HashSet<DelegateFile>();
            this.FavoriteReports = new HashSet<FavoriteReport>();
            this.Files = new HashSet<File>();
            this.Files1 = new HashSet<File>();
            this.Files2 = new HashSet<File>();
            this.Files3 = new HashSet<File>();
            this.FileDownloadLinks = new HashSet<FileDownloadLink>();
            this.FileLogs = new HashSet<FileLog>();
            this.FileVersions = new HashSet<FileVersion>();
            this.FileVersions1 = new HashSet<FileVersion>();
            this.FileVersions2 = new HashSet<FileVersion>();
            this.FileVersions3 = new HashSet<FileVersion>();
            this.Folders = new HashSet<Folder>();
            this.Folders1 = new HashSet<Folder>();
            this.FolderAccesses = new HashSet<FolderAccess>();
            this.FolderAccesses1 = new HashSet<FolderAccess>();
            this.GroupTasks = new HashSet<GroupTask>();
            this.GroupTask_Ticket = new HashSet<GroupTask_Ticket>();
            this.GroupTask_Ticket1 = new HashSet<GroupTask_Ticket>();
            this.GroupTask_User = new HashSet<GroupTask_User>();
            this.GroupTaskComments = new HashSet<GroupTaskComment>();
            this.GroupTaskComments1 = new HashSet<GroupTaskComment>();
            this.GroupTaskReminders = new HashSet<GroupTaskReminder>();
            this.GroupTasks_Details = new HashSet<GroupTasks_Details>();
            this.Meetings = new HashSet<Meeting>();
            this.OldPasswords = new HashSet<OldPassword>();
            this.Quizs = new HashSet<Quiz>();
            this.Quizs1 = new HashSet<Quiz>();
            this.Quiz_Result = new HashSet<Quiz_Result>();
            this.Tasks = new HashSet<Task>();
            this.Task_Ticket = new HashSet<Task_Ticket>();
            this.Task_Ticket1 = new HashSet<Task_Ticket>();
            this.Task_Ticket2 = new HashSet<Task_Ticket>();
            this.TaskComments = new HashSet<TaskComment>();
            this.TaskComments1 = new HashSet<TaskComment>();
            this.User_Worktype = new HashSet<User_Worktype>();
            this.UserTask_QuizResult = new HashSet<UserTask_QuizResult>();
            this.User_Task = new HashSet<User_Task>();
            this.User_Task1 = new HashSet<User_Task>();
            this.User_Meeting = new HashSet<User_Meeting>();
            this.UserSecurities = new HashSet<UserSecurity>();
            this.CourseGroupStudies = new HashSet<CourseGroupStudy>();
            this.CourseGroupStudies1 = new HashSet<CourseGroupStudy>();
        }
    
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string HomeNumber { get; set; }
        public string Notes { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> AcessLevel { get; set; }
        public string Image { get; set; }
        public string Password { get; set; }
        public Nullable<int> isSlack { get; set; }
        public Nullable<int> isSMS { get; set; }
        public Nullable<int> isMail { get; set; }
        public string SlackAddress { get; set; }
        public string GoogleKeyLength { get; set; }
        public string GoggleTaskColor { get; set; }
        public Nullable<int> ShowTasks { get; set; }
        public Nullable<int> ShowUrgent { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> Role { get; set; }
        public Nullable<int> IsMasterAdmin { get; set; }
        public string OutlookToken { get; set; }
        public string SearchByPriority { get; set; }
        public string SearchByDivision { get; set; }
        public string SearchByDepartment { get; set; }
        public string SearchByPrivate { get; set; }
        public string SearchByUserType { get; set; }
        public string LowPriorityColor { get; set; }
        public string MediumPriorityColor { get; set; }
        public string HighPriorityColor { get; set; }
        public string CanvasLoginId { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<System.DateTime> DeletionDate { get; set; }
        public string Player_Id { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> IsDelegate { get; set; }
        public Nullable<int> IsZuptuSuperAdminUser { get; set; }
        public Nullable<int> IsPrimary { get; set; }
        public string SignatureImage { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contacts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course_File> Course_File { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DelegateFile> DelegateFiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DelegateFile> DelegateFiles1 { get; set; }
        public virtual Department Department { get; set; }
        public virtual Division Division { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FavoriteReport> FavoriteReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> Files { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> Files1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> Files2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> Files3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileDownloadLink> FileDownloadLinks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileLog> FileLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileVersion> FileVersions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileVersion> FileVersions1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileVersion> FileVersions2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileVersion> FileVersions3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Folder> Folders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Folder> Folders1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FolderAccess> FolderAccesses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FolderAccess> FolderAccesses1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupTask> GroupTasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupTask_Ticket> GroupTask_Ticket { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupTask_Ticket> GroupTask_Ticket1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupTask_User> GroupTask_User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupTaskComment> GroupTaskComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupTaskComment> GroupTaskComments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupTaskReminder> GroupTaskReminders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupTasks_Details> GroupTasks_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Meeting> Meetings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OldPassword> OldPasswords { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quiz> Quizs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quiz> Quizs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quiz_Result> Quiz_Result { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Tasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_Ticket> Task_Ticket { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_Ticket> Task_Ticket1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_Ticket> Task_Ticket2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskComment> TaskComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskComment> TaskComments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Worktype> User_Worktype { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserTask_QuizResult> UserTask_QuizResult { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Task> User_Task { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Task> User_Task1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Meeting> User_Meeting { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSecurity> UserSecurities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseGroupStudy> CourseGroupStudies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseGroupStudy> CourseGroupStudies1 { get; set; }
    }
}