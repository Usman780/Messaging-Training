using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.HelpingClasses
{
    public  static  class Enums
    {
        public enum Role
        {
            Admin=1,
            Manager=2,
            Trainee=3,
            Cordinator=4
        }

        public enum GroupTaskLead
        {
            Primary=1,
            Secondary=2
        }
       
        public enum TaskPrivacy
        {
            Public=0,
            Private=1
        }

        public enum TaskPriority
        {
            Low=0,
            Medium=1,
            High=2
        }


        public enum ReportType
        {
            StatusReportDivision = 1,
            StatusReportDepartment = 2,
            StatusReportTask = 3,
            StatusReportEmp = 4
        }
    }
}