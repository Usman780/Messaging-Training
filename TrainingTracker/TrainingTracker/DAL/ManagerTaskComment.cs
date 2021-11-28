//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using TrainingTracker.Models;

//namespace TrainingTracker.DAL
//{
//    public class ManagerTaskCommentDAL
//    {
//        public ManagerTaskComment getManagerTaskCommentById(int _Id, DatabaseEntities de=null)
//        {
//            ManagerTaskComment _ManagerTaskComment;
//            if(de==null)
//            using (DatabaseEntities db = new DatabaseEntities())
//            {
//                _ManagerTaskComment = db.ManagerTaskComments.FirstOrDefault(x => x.Id == _Id);
//            }else
//                _ManagerTaskComment = de.ManagerTaskComments.FirstOrDefault(x => x.Id == _Id);


//            return _ManagerTaskComment;
//        }

//        public List<ManagerTaskComment> getAllManagerTaskCommentsList()
//        {
//            DatabaseEntities db = new DatabaseEntities();
           
//            List<ManagerTaskComment> ManagerTaskComments = db.ManagerTaskComments.ToList();

//            return ManagerTaskComments;


//        }


//        public List<ManagerTaskComment> getManagerTaskCommentsList()
//        {
//            DatabaseEntities db = new DatabaseEntities();
//            int adminId = (int)HttpContext.Current.Session["Admin"];
//            List<ManagerTaskComment> ManagerTaskComments = db.ManagerTaskComments.Where(x => x.TaskManager.Manager.AdminID == adminId).ToList(); ;

//            return ManagerTaskComments;
//        }


//        public bool AddManagerTaskComment(ManagerTaskComment _ManagerTaskComment)
//        {
//            using (DatabaseEntities db = new DatabaseEntities())
//            {
//                db.ManagerTaskComments.Add(_ManagerTaskComment);
//                db.SaveChanges();
//            }
//            return true;
//        }

//        public bool UpdateManagerTaskComment(ManagerTaskComment _ManagerTaskComment, DatabaseEntities de=null)
//        {
//            if(de==null)
//            using (DatabaseEntities db = new DatabaseEntities())
//            {
//                db.Entry(_ManagerTaskComment).State = System.Data.Entity.EntityState.Modified;
//                db.SaveChanges();
//            }
//            else
//            {
//                de.Entry(_ManagerTaskComment).State = System.Data.Entity.EntityState.Modified;
//                de.SaveChanges();

//            }
//            return true;
//        }

//        public void DeleteManagerTaskComment(int _id,DatabaseEntities db=null)
//        {
//            bool varialble = true;
//            if (db == null)
//            {
//                db = new DatabaseEntities();
//                varialble = false;
//            }

//            db.ManagerTaskComments.Remove(db.ManagerTaskComments.FirstOrDefault(x => x.Id == _id));
//            if (!varialble)
//            {
//                db.SaveChanges();
//            }

//        }


//    }
//}