
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;



namespace TrainingTracker.DAL
{

  public class GroupTask_UserDL
    {
        #region GroupTask_User
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);
        public List<GroupTask_User> getGroupTask_UsersList(DatabaseEntities de = null)
        {
            if (de != null)
            {
              //  DatabaseEntities db = new DatabaseEntities();
                //List<GroupTask_User> GroupTask_Uses = de.GroupTask_User.Where(x => x.IsActive == 1).ToList();
                List<GroupTask_User> GroupTask_Uses = de.GroupTask_User.Where(x => x.IsActive == 1 && x.CompanyId == CompanyId).ToList();

                return GroupTask_Uses;
            }
            DatabaseEntities db = new DatabaseEntities();
            //List<GroupTask_User> GroupTask_Users = db.GroupTask_User.Where(x=>x.IsActive==1).ToList();
            List<GroupTask_User> GroupTask_Users = db.GroupTask_User.Where(x=>x.IsActive==1 && x.CompanyId == CompanyId).ToList();

            return GroupTask_Users;
        }
        public List<GroupTask_User> getInActiveGroupTask_UsersList(DatabaseEntities de = null)
        {
            if (de != null)
            {
              //  DatabaseEntities db = new DatabaseEntities();
                //List<GroupTask_User> GroupTask_Uses = de.GroupTask_User.Where(x => x.IsActive == 0).ToList();
                List<GroupTask_User> GroupTask_Uses = de.GroupTask_User.Where(x => x.IsActive == 0 && x.CompanyId == CompanyId).ToList();

                return GroupTask_Uses;
            }
            DatabaseEntities db = new DatabaseEntities();
           // List<GroupTask_User> GroupTask_Users = db.GroupTask_User.Where(x=>x.IsActive==0).ToList();
            List<GroupTask_User> GroupTask_Users = db.GroupTask_User.Where(x=>x.IsActive==0 && x.CompanyId == CompanyId).ToList();

            return GroupTask_Users;
        }
        public List<GroupTask_User> getAllGroupTask_UsersList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<GroupTask_User> GroupTask_Users = db.GroupTask_User.ToList();
            List<GroupTask_User> GroupTask_Users = db.GroupTask_User.Where(x=>x.CompanyId == CompanyId).ToList();

            return GroupTask_Users;
        }
        
        public GroupTask_User getGroupTask_UserById(int _Id)
        {
            GroupTask_User _GroupTask_User;
            DatabaseEntities db = new DatabaseEntities();
            
                _GroupTask_User = db.GroupTask_User.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
           

            return _GroupTask_User;
        }

        public bool AddGroupTask_User(GroupTask_User _GroupTask_User, DatabaseEntities dewrapper=null)
        {
            _GroupTask_User.CompanyId = CompanyId;
            if(dewrapper != null)
            {
                dewrapper.GroupTask_User.Add(_GroupTask_User);
                return true;

            }
            else
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.GroupTask_User.Add(_GroupTask_User);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateGroupTask_User(GroupTask_User _GroupTask_User, DatabaseEntities de=null)
        {
            if (_GroupTask_User.CompanyId == 0 || _GroupTask_User.CompanyId == null)
                _GroupTask_User.CompanyId = CompanyId;

            if (de != null)
            {
                de.Entry(_GroupTask_User).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                return true;

            }
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Entry(_GroupTask_User).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }

        public void DeleteGroupTask_User(int _id, DatabaseEntities db = null)
        {
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }




            GroupTask_User _GroupTask_User = db.GroupTask_User.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);

            if (_GroupTask_User != null)
            {
                foreach (var item in _GroupTask_User.GroupTaskComments)
                {
                    new GroupTaskCommentDAL().DeleteGroupTaskComment(item.Id, db);
                }


                _GroupTask_User.IsActive = 0;
                db.Entry(_GroupTask_User).State = System.Data.Entity.EntityState.Modified;

            }
            if (!varialble)
            {
                db.SaveChanges();   
            }
        }


        public bool UpdateTrainee_Task(GroupTask_User _Trainee_Task, DatabaseEntities de = null)
        {
            if (_Trainee_Task.CompanyId == 0 || _Trainee_Task.CompanyId == null)
                _Trainee_Task.CompanyId = CompanyId;
            if (de == null)
            {
                using (DatabaseEntities db1 = new DatabaseEntities())
                {

                    db1.Entry(_Trainee_Task).State = System.Data.Entity.EntityState.Modified;
                    db1.SaveChanges();
                }
                return true;
            }
            de.Entry(_Trainee_Task).State = System.Data.Entity.EntityState.Modified;
            return true;
        }

        public GroupTask_User getTrainee_TaskByIdWrapper(int _Id, DatabaseEntities de = null)
        {
            GroupTask_User _Trainee_Task;
            using (DatabaseEntities db1 = new DatabaseEntities())
            {
                _Trainee_Task = de.GroupTask_User.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);

            }
            return _Trainee_Task;
        }
        #endregion

    }
}