using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class CertificateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedAt { get; set; }
        public string Path { get; set; }
        public int Sr { get; set; }
        //public Nullable<int> IsActive { get; set; }
    }
}