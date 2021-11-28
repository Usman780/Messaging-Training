using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class TaskCommentDTO2
    {
        public string Name { get; set; }
        public string File { get; set; }
        public string FilePath { get; set; }
        public DateTime CommentTime { get; set; }
        public string FileSize { get; set; }
        public string Comment { get; set; }
        public int prime { get; set; }
        public int isManager { get; set; }
        public int Id { get; set; }
        public int userId { get; set; }
        public string ROLE { get; set; }
        public string Image { get; set; }

        //by wajeeh for update comment
        public int? IsDocMFile { get; set; }

    }
    
    public class TaskCommentDTO
    {
        
        public string Name { get; set; }
        public string File { get; set; }
        public string FilePath { get; set; }
        public DateTime CommentTime { get; set; }
        public string FileSize { get; set; }
        public string Comment { get; set; }
        public int prime { get; set; }
        public int isManager { get; set; }
        public int Id { get; set; }
        public int userId { get; set; }
        public string ROLE { get; set; }
        public string Image { get; set; }

        public List<TaskCommentDTO2> obj = new List<TaskCommentDTO2>();

        //by wajeeh for update comment
        public int? IsDocMFile { get; set; }


    }
   
}