using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class CheckAuthenticationDTO
    {
       
        public int Role { get; set; }
        public string SlackAddress { get; set; }
        public string Company { get; set; }
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int isGoogle { get; set; }
        public int IsMasterAdmin { get; set; }
        public double PicSignatureTime { get; set; }
      
    }
}