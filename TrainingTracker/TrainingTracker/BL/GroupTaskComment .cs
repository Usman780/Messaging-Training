
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class GroupTaskCommentBL
    {
       

 #region GroupTaskComments
        public List<GroupTaskComment> getGroupTaskCommentsList()
        {
            return new GroupTaskCommentDAL().getGroupTaskCommentsList();
        }
        public List<GroupTaskComment> getInActiveGroupTaskCommentsList()
        {
            return new GroupTaskCommentDAL().getInActiveGroupTaskCommentsList();
        }
        public List<GroupTaskComment> getAllGroupTaskCommentsList()
        {
            return new GroupTaskCommentDAL().getAllGroupTaskCommentsList();
        }
        public GroupTaskComment getGroupTaskCommentsById(int _id, DatabaseEntities de=null)
        {
            return new GroupTaskCommentDAL().getGroupTaskCommentById(_id,de);
        }

        public bool AddGroupTaskComments(GroupTaskComment _GroupTaskComments)
        {
            //if (_GroupTaskComments.Name == null || _GroupTaskComments.Email == null || _GroupTaskComments.Password == null || _GroupTaskComments.Website_Address == null || _GroupTaskComments.Phone == null)
            //    return false;
            return new GroupTaskCommentDAL().AddGroupTaskComment(_GroupTaskComments);
        }

        public bool UpdateGroupTaskComments(GroupTaskComment _GroupTaskComments, DatabaseEntities de=null)
        {
            //if (_GroupTaskComments.Name == null || _GroupTaskComments.Email == null || _GroupTaskComments.Password == null || _GroupTaskComments.Website_Address == null || _GroupTaskComments.Phone == null)
            //    return false;

            return new GroupTaskCommentDAL().UpdateGroupTaskComment(_GroupTaskComments,de);
        }

        public void DeleteGroupTaskComments(int _id)
        {
            new GroupTaskCommentDAL().DeleteGroupTaskComment(_id);
        }
        #endregion

}
}