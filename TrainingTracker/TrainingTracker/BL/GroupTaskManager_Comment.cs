
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using TrainingTracker.DAL;
//using TrainingTracker.Models;
//namespace TrainingTracker.BL
//{     
// public class GroupTaskManager_CommentBL
//    {
       

// #region GroupTaskManager_Comments
//        public List<GroupTaskManager_Comment> getGroupTaskManager_CommentsList()
//        {
//            return new GroupTaskManager_CommentDAL().getAllGroupTaskManager_CommentsList();
//        }
//        public List<GroupTaskManager_Comment> getAllGroupTaskManager_CommentsList()
//        {
//            return new GroupTaskManager_CommentDAL().getAllGroupTaskManager_CommentsList();
//        }
//        public GroupTaskManager_Comment getGroupTaskManager_CommentsById(int _id, DatabaseEntities de=null)
//        {
//            return new GroupTaskManager_CommentDAL().getGroupTaskManager_CommentById(_id,de);
//        }

//        public bool AddGroupTaskManager_Comments(GroupTaskManager_Comment _GroupTaskManager_Comments)
//        {
//            //if (_GroupTaskManager_Comments.Name == null || _GroupTaskManager_Comments.Email == null || _GroupTaskManager_Comments.Password == null || _GroupTaskManager_Comments.Website_Address == null || _GroupTaskManager_Comments.Phone == null)
//            //    return false;
//            return new GroupTaskManager_CommentDAL().AddGroupTaskManager_Comment(_GroupTaskManager_Comments);
//        }

//        public bool UpdateGroupTaskManager_Comments(GroupTaskManager_Comment _GroupTaskManager_Comments, DatabaseEntities de=null)
//        {
//            //if (_GroupTaskManager_Comments.Name == null || _GroupTaskManager_Comments.Email == null || _GroupTaskManager_Comments.Password == null || _GroupTaskManager_Comments.Website_Address == null || _GroupTaskManager_Comments.Phone == null)
//            //    return false;

//            return new GroupTaskManager_CommentDAL().UpdateGroupTaskManager_Comment(_GroupTaskManager_Comments,de);
//        }

//        public void DeleteGroupTaskManager_Comments(int _id)
//        {
//            new GroupTaskManager_CommentDAL().DeleteGroupTaskManager_Comment(_id);
//        }
//        #endregion

//}
//}