//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using TrainingTracker.Models;

//namespace TrainingTracker.DAL
//{
//    public class GroupTaskManager_CommentDAL
//    {
//        public GroupTaskManager_Comment getGroupTaskManager_CommentById(int _Id, DatabaseEntities de = null)
//        {

//            GroupTaskManager_Comment _GroupTaskManager_Comment;
//            if (de == null)
//            {
//                using (DatabaseEntities db = new DatabaseEntities())
//                {
//                    _GroupTaskManager_Comment = db.GroupTaskManager_Comment.FirstOrDefault(x => x.Id == _Id);
//                }
//            }
//            else
//            {
//                _GroupTaskManager_Comment = de.GroupTaskManager_Comment.FirstOrDefault(x => x.Id == _Id);
//            }

//            return _GroupTaskManager_Comment;
//        }

//        public List<GroupTaskManager_Comment> getAllGroupTaskManager_CommentsList()
//        {
//            DatabaseEntities db = new DatabaseEntities();
//            List<GroupTaskManager_Comment> GroupTaskManager_Comments = db.GroupTaskManager_Comment.ToList();

//            return GroupTaskManager_Comments;
//        }
//        public List<GroupTaskManager_Comment> getGroupTaskManager_CommentsList()
//        {
//            DatabaseEntities db = new DatabaseEntities();
//            int adminId = (int)HttpContext.Current.Session["Admin"];
//            List<GroupTaskManager_Comment> GroupTaskManager_Comments = db.GroupTaskManager_Comment.Where(x => x.GroupTask_Manager.Manager.AdminID == adminId).ToList();

//            return GroupTaskManager_Comments;
//        }

//        public bool AddGroupTaskManager_Comment(GroupTaskManager_Comment _GroupTaskManager_Comment)
//        {
//            using (DatabaseEntities db = new DatabaseEntities())
//            {
//                db.GroupTaskManager_Comment.Add(_GroupTaskManager_Comment);
//                db.SaveChanges();
//            }
//            return true;
//        }

//        public bool UpdateGroupTaskManager_Comment(GroupTaskManager_Comment _GroupTaskManager_Comment, DatabaseEntities de = null)
//        {
//            if (de == null)
//            {
//                using (DatabaseEntities db = new DatabaseEntities())
//                {
//                    db.Entry(_GroupTaskManager_Comment).State = System.Data.Entity.EntityState.Modified;
//                    db.SaveChanges();
//                }
//            }
//            else
//            {
//                de.Entry(_GroupTaskManager_Comment).State = System.Data.Entity.EntityState.Modified;
//                de.SaveChanges();
//            }
//            return true;
//        }

//        public void DeleteGroupTaskManager_Comment(int _id, DatabaseEntities db = null)
//        {
//            bool varialble = true;
//            if (db == null)
//            {
//                db = new DatabaseEntities();
//                varialble = false;
//            }
//            GroupTaskManager_Comment _GroupTaskManager_Comment = db.GroupTaskManager_Comment.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
//            if (_GroupTaskManager_Comment != null)
//            {
//                _GroupTaskManager_Comment.IsActive = 0;
//                db.Entry(_GroupTaskManager_Comment).State = System.Data.Entity.EntityState.Modified;

//            }
//            if (!varialble)
//            {
//                db.SaveChanges();
//            }
//        }


//    }
//}