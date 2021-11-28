
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class TaskCommentBL
    {
       

 #region TaskComments
        public List<TaskComment> getTaskCommentsList(int template=-1)
        {
            return new TaskCommentDAL().getTaskCommentsList(template);
        }

        public List<TaskComment> getInActiveTaskCommentsList(int template = -1)
        {
            return new TaskCommentDAL().getInActiveTaskCommentsList(template);
        }
        public List<TaskComment> getAllTaskCommentsList()
        {
            return new TaskCommentDAL().getAllTaskCommentsList();
        }
        public TaskComment getTaskCommentsById(int _id, DatabaseEntities de=null)
        {
            return new TaskCommentDAL().getTaskCommentById(_id,de);
        }

        public int AddTaskComments(TaskComment _TaskComments)
        {
            //if (_TaskComments.Name == null || _TaskComments.Email == null || _TaskComments.Password == null || _TaskComments.Website_Address == null || _TaskComments.Phone == null)
            //    return false;
            return new TaskCommentDAL().AddTaskComment(_TaskComments);
        }

        public bool UpdateTaskComments(TaskComment _TaskComments, DatabaseEntities de=null)
        {
            //if (_TaskComments.Name == null || _TaskComments.Email == null || _TaskComments.Password == null || _TaskComments.Website_Address == null || _TaskComments.Phone == null)
            //    return false;

            return new TaskCommentDAL().UpdateTaskComment(_TaskComments,de);
        }

        public void DeleteTaskComments(int _id)
        {
            new TaskCommentDAL().DeleteTaskComment(_id);
        }
        public void ActivateDeletedTaskComments(int _id)
        {
            new TaskCommentDAL().ActivateDeletedTaskComments(_id);
        }
        #endregion

    }
}