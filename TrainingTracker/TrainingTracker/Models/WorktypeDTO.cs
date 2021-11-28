using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class WorktypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IsActive { get; set; }
        public int CompanyID { get; set; }
        public string DivisionName { get; set; }
        public int DivisionID { get; set; }
    }
}