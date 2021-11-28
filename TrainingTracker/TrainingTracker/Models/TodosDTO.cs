using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class TodosDTO
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string TodoCreatedBy { get; set; }
        public string TicketNotes { get; set; }
        public int Edited { get; set; }
        public string CreationDatetime { get; set; }
        public Nullable<System.DateTime> CompletionDatetime { get; set; }
        public string File { get; set; }
        public string FileName { get; set; }





    }
}