using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;

namespace TrainingTracker.DAL
{
    public class GroupTaskCommentDAL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);
        public GroupTaskComment getGroupTaskCommentById(int _Id, DatabaseEntities de=null)
        {
            GroupTaskComment _GroupTaskComment;
            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    _GroupTaskComment = db.GroupTaskComments.FirstOrDefault(x => x.Id == _Id);
                  }
            }
            else
            {
                _GroupTaskComment = de.GroupTaskComments.FirstOrDefault(x => x.Id == _Id);
            }

            return _GroupTaskComment;
        }

        public List<GroupTaskComment> getAllGroupTaskCommentsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //int CompanyId = CompanyId;
            //List<GroupTaskComment> GroupTaskComments = db.GroupTaskComments.ToList();
            List<GroupTaskComment> GroupTaskComments = db.GroupTaskComments.Where(x=>x.CompanyId == CompanyId).ToList();

            return GroupTaskComments;
        }

        public List<GroupTaskComment> getGroupTaskCommentsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //int CompanyId = CompanyId;
            //List<GroupTaskComment> GroupTaskComments = db.GroupTaskComments.Where(x=> x.GroupTasks_Details.GroupTask.User.CompanyID == CompanyId && x.IsActive==1).ToList();
            List<GroupTaskComment> GroupTaskComments = db.GroupTaskComments.Where(x=> x.CompanyId == CompanyId && x.IsActive==1).ToList();

            return GroupTaskComments;
        }

     public List<GroupTaskComment> getInActiveGroupTaskCommentsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //int CompanyId = CompanyId;
            //List<GroupTaskComment> GroupTaskComments = db.GroupTaskComments.Where(x=> x.GroupTasks_Details.GroupTask.User.CompanyID == CompanyId && x.IsActive==0).ToList();
            List<GroupTaskComment> GroupTaskComments = db.GroupTaskComments.Where(x=> x.CompanyId == CompanyId && x.IsActive==0).ToList();

            return GroupTaskComments;
        }

        public bool AddGroupTaskComment(GroupTaskComment _GroupTaskComment)
        {
            _GroupTaskComment.CompanyId = CompanyId;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.GroupTaskComments.Add(_GroupTaskComment);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateGroupTaskComment(GroupTaskComment _GroupTaskComment, DatabaseEntities de= null)
        {
            _GroupTaskComment.CompanyId = CompanyId;
            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_GroupTaskComment).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }else
            {
                de.Entry(_GroupTaskComment).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }

            return true;
        }

        public void DeleteGroupTaskComment(int _id, DatabaseEntities db = null)
        {
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }
            GroupTaskComment _GroupTaskComment = db.GroupTaskComments.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
            if (_GroupTaskComment != null)
            {
                _GroupTaskComment.IsActive = 0;
                db.Entry(_GroupTaskComment).State = System.Data.Entity.EntityState.Modified;
            }

            if (!varialble)
            {
                db.SaveChanges();
            }
        }


    }
}