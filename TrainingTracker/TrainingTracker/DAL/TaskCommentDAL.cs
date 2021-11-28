using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;

namespace TrainingTracker.DAL
{
    public class TaskCommentDAL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);
        public TaskComment getTaskCommentById(int _Id, DatabaseEntities de=null)
        {
            TaskComment _TaskComment;
            if (de == null)
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    _TaskComment = db.TaskComments.FirstOrDefault(x => x.Id == _Id);
                }
            else

            {
                _TaskComment = de.TaskComments.FirstOrDefault(x => x.Id == _Id);
            }

            return _TaskComment;
        }

        public List<TaskComment> getAllTaskCommentsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<TaskComment> TaskComments = db.TaskComments.ToList();
            List<TaskComment> TaskComments = db.TaskComments.Where(x=>x.CompanyId == CompanyId).ToList();

            return TaskComments;
        }

        public List<TaskComment> getTaskCommentsList(int template=-1)
        {
            DatabaseEntities db = new DatabaseEntities();
            int adminId =CompanyId;
            List<TaskComment> TaskComments = new List<TaskComment>();
            if (template != -1)
            {
                //TaskComments = db.TaskComments.Where(x => x.User_Task.Task.Department.Division.CompanyID == adminId).ToList();
                TaskComments = db.TaskComments.Where(x => x.CompanyId == adminId).ToList();

            }
            else
            {
                //TaskComments = db.TaskComments.Where(x => x.User_Task.User1.CompanyID == adminId).ToList();
                TaskComments = db.TaskComments.Where(x => x.CompanyId == adminId).ToList();

            }

            return TaskComments;
        }

          public List<TaskComment> getInActiveTaskCommentsList(int template=-1)
        {
            DatabaseEntities db = new DatabaseEntities();
            int adminId =CompanyId;
            List<TaskComment> TaskComments = new List<TaskComment>();
            if (template != -1)
            {
                //TaskComments = db.TaskComments.Where(x => x.User_Task.Task.Department.Division.CompanyID == adminId && x.IsActive==0).ToList();
                TaskComments = db.TaskComments.Where(x => x.CompanyId == adminId && x.IsActive==0).ToList();

            }
            else
            {
                //TaskComments = db.TaskComments.Where(x => x.User_Task.User1.CompanyID == adminId && x.IsActive == 0).ToList();
                TaskComments = db.TaskComments.Where(x => x.CompanyId == adminId && x.IsActive == 0).ToList();

            }

            return TaskComments;
        }


        public int AddTaskComment(TaskComment _TaskComment)
        {
            int i;
            _TaskComment.CompanyId = CompanyId;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.TaskComments.Add(_TaskComment);
                db.SaveChanges();
            }
            return _TaskComment.Id;
        }

        public bool UpdateTaskComment(TaskComment _TaskComment, DatabaseEntities de=null)
        {
            _TaskComment.CompanyId = CompanyId;
            if (de==null)
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Entry(_TaskComment).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                de.Entry(_TaskComment).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();

            }
            return true;
        }

        public void DeleteTaskComment(int _id,DatabaseEntities db=null)
        {
            DatabaseEntities de = new DatabaseEntities();
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }
            List<TaskComment> listcomnts = new TaskCommentDAL().getTaskCommentsList().Where(x => x.ParentId == _id).ToList();
            if (listcomnts.Count() > 0)
            {
                foreach(TaskComment t in listcomnts)
                {
                    new TaskCommentDAL().DeleteTaskComment(t.Id);
                }
            }
            TaskComment tcomnt = new TaskCommentDAL().getTaskCommentById(_id,de);
            if (tcomnt != null)
            {
                tcomnt.IsActive = 0;
                new TaskCommentDAL().UpdateTaskComment(tcomnt,de);
               
            }
         
            // db.TaskComments.Remove(db.TaskComments.FirstOrDefault(x => x.Id == _id));
            //if (!varialble)
            //{
            //    db.SaveChanges();
            //}

        }

 public void ActivateDeletedTaskComments(int _id,DatabaseEntities db=null)
        {
            DatabaseEntities de = new DatabaseEntities();
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }
            List<TaskComment> listcomnts = new TaskCommentDAL().getTaskCommentsList().Where(x => x.ParentId == _id).ToList();
            if (listcomnts.Count() > 0)
            {
                foreach(TaskComment t in listcomnts)
                {
                    new TaskCommentDAL().ActivateDeletedTaskComments(t.Id);
                }
            }
            TaskComment tcomnt = new TaskCommentDAL().getTaskCommentById(_id,de);
            if (tcomnt != null)
            {
                tcomnt.IsActive = 1;
                new TaskCommentDAL().UpdateTaskComment(tcomnt,de);
               
            }
         
            // db.TaskComments.Remove(db.TaskComments.FirstOrDefault(x => x.Id == _id));
            //if (!varialble)
            //{
            //    db.SaveChanges();
            //}

        }


    }
}