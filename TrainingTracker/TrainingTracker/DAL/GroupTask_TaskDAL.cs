
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;



namespace TrainingTracker.DAL
{

  public class GroupTask_TaskDL
    {
        #region GroupTask_Task
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);
        public List<GroupTask_Task> getGroupTask_TasksList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<GroupTask_Task> GroupTask_Tasks = db.GroupTask_Task.Where(x=>x.IsActive==1).ToList();
            List<GroupTask_Task> GroupTask_Tasks = db.GroupTask_Task.Where(x=>x.IsActive==1 && x.CompanyId == CompanyId).ToList();

            return GroupTask_Tasks;
        }
        public List<GroupTask_Task> getAllGroupTask_TasksList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<GroupTask_Task> GroupTask_Tasks = db.GroupTask_Task.ToList();
            List<GroupTask_Task> GroupTask_Tasks = db.GroupTask_Task.Where(x=>x.CompanyId == CompanyId).ToList();

            return GroupTask_Tasks;
        }
        
        public GroupTask_Task getGroupTask_TaskById(int _Id)
        {
            GroupTask_Task _GroupTask_Task;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                _GroupTask_Task = db.GroupTask_Task.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            }

            return _GroupTask_Task;
        }

        public bool AddGroupTask_Task(GroupTask_Task _GroupTask_Task)
        {
            
                _GroupTask_Task.CompanyId = CompanyId;

            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.GroupTask_Task.Add(_GroupTask_Task);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateGroupTask_Task(GroupTask_Task _GroupTask_Task, DatabaseEntities de = null)
        {
            if (_GroupTask_Task.CompanyId == 0 || _GroupTask_Task.CompanyId == null)
                _GroupTask_Task.CompanyId = CompanyId;

            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_GroupTask_Task).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                de.Entry(_GroupTask_Task).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            return true;
        }

        public void DeleteGroupTask_Task(int _id, DatabaseEntities db=null)
        {
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }
            GroupTask_Task _GroupTask_Task = db.GroupTask_Task.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
            if (_GroupTask_Task != null)
            {
                _GroupTask_Task.IsActive = 0;
                db.Entry(_GroupTask_Task).State = System.Data.Entity.EntityState.Modified;
            }

            if (!varialble)
            {
                db.SaveChanges();
            }
        }
#endregion

}
}