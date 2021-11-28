using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class DivisionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> SessionRole { get; set; }
    }
}