using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public Nullable<int> DivisionID { get; set; }
        public string Name { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> SessionRole { get; set; }
    }
}