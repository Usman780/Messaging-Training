using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class Result
    {
        public string id { get; set; }
        public string userId { get; set; }
        public int resultScore { get; set; }
        public int resultMaximum { get; set; }
        public object comment { get; set; }
        public string scoreOf { get; set; }
    }
}