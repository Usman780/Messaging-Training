using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public Nullable<int> IsActive { get; set; }
        public string Name { get; set; }
        public Nullable<int> TaskTypeID { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public string Description { get; set; }
        public Nullable<double> Cost_ { get; set; }
        public Nullable<double> Hours { get; set; }
        public Nullable<double> CEU { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string File { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> IsPrivate { get; set; }
        public Nullable<int> SessionRole { get; set; }
        public int CourseId { get; set; }
    }
}