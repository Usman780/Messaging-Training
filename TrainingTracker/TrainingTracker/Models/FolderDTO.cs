using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class FolderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }
        public string FolderPath { get; set; }
        public int? ParentId { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? ArchivedDate { get; set; }
        public int? ArchivedBy { get; set; }
        public int? IsActive { get; set; }
        public string PathText { get; set; }
    }
}