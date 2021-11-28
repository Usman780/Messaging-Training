using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class ManagerDTO
    {
        public int Id { get; set; }
        public int IsActive { get; set; }
        public int Temp { get; set; }
        public string Priority { get; set; }
        public int Role2 { get; set; }
        public int SerialNo { get; set; }
        public int deptId { get; set; }
        public int DivisionId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EncryptedId { get; set; }
        public string EncryptedEmpId { get; set; }
        public string EncryptedDeptId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string DivisionName { get; set; }
        public string DepartmentName { get; set; }
        public string WorkStatus { get; set; }
        public string CreatedBy { get; set; }
        //Task 
        public double Cost { get; set; }
        public double Hours { get; set; }
        public double CEU { get; set; }
        public string Description { get; set; }
    }
}