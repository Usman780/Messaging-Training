
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;


namespace TrainingTracker.DAL
{

  public class GroupTaskDL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        #region GroupTask
        public List<GroupTask> getGroupTasksList(DatabaseEntities de=null)
        {
            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            List<GroupTask> GroupTasks = new List<GroupTask>();
            if(de!=null)
            //GroupTasks = de.GroupTasks.Where(x=>x.IsActive==1 && x.User.CompanyID==Companyid).ToList();
            GroupTasks = de.GroupTasks.Where(x=>x.IsActive==1 && x.CompanyId==Companyid).ToList();
            else
            //GroupTasks = db.GroupTasks.Where(x=>x.IsActive==1 && x.User.CompanyID==Companyid).ToList();
            GroupTasks = db.GroupTasks.Where(x=>x.IsActive==1 && x.CompanyId==Companyid).ToList();

            return GroupTasks;
        }
        public List<GroupTask> getAllGroupTasksList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            //List<GroupTask> GroupTasks = db.GroupTasks.Where(x =>  x.User.CompanyID == Companyid).ToList();
            List<GroupTask> GroupTasks = db.GroupTasks.Where(x =>  x.CompanyId == Companyid).ToList();

            return GroupTasks;
        }
        
        public GroupTask getGroupTaskById(int _Id)
        {
            GroupTask _GroupTask;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                _GroupTask = db.GroupTasks.FirstOrDefault(x => x.Id == _Id);
            }

            return _GroupTask;
        }

        public int AddGroupTask(GroupTask _GroupTask)
        {
            _GroupTask.CompanyId = Convert.ToInt32(logedinuser.Company);
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.GroupTasks.Add(_GroupTask);
                db.SaveChanges();
                return _GroupTask.Id;
            }
         
        }

        public bool UpdateGroupTask(GroupTask _GroupTask, DatabaseEntities de=null)
        {
            if (_GroupTask.CompanyId == 0 || _GroupTask.CompanyId == null)
                _GroupTask.CompanyId = Convert.ToInt32(logedinuser.Company);
            if (de != null)
            {
                de.Entry(_GroupTask).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            else
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_GroupTask).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
           
            return true;
        }

        public void DeleteGroupTask(int _id)
        {
            GroupTask gt = getGroupTaskById(_id);
            if (gt != null)
            {
                gt.IsActive = 0;
                UpdateGroupTask(gt);
            }
        }
#endregion

}
}