using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class UserTaskDTO
    {
        public UserDTO user { get; set; }
        public TaskDTO task { get; set; }


    }
}