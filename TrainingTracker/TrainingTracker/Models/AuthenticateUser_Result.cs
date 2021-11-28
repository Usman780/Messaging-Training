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
    
    public partial class AuthenticateUser_Result
    {
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
    }
}
