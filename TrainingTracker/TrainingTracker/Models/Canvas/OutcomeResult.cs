using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class Links
    {
        public string user { get; set; }
        public string learning_outcome { get; set; }
        public string alignment { get; set; }
    }

    public class OutcomeResult
    {
        public int id { get; set; }
        public int score { get; set; }
        public DateTime submitted_or_assessed_at { get; set; }
        public Links links { get; set; }
        public double percent { get; set; }
    }

}