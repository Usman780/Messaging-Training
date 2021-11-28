using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class GroupTaskRemindeDAL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);

        #region GroupTaskReminder
        public List<GroupTaskReminder> getGroupTaskRemindersListwithoutCompany(DatabaseEntities de = null)
        {
            DatabaseEntities db = new DatabaseEntities();
            //     int adminid = CompanyId;
            List<GroupTaskReminder> GroupTaskReminders = new List<GroupTaskReminder>();
            if (de != null)
                //GroupTaskReminders = de.GroupTaskReminders.Where(x => x.IsActive == 1).ToList();
                GroupTaskReminders = de.GroupTaskReminders.Where(x => x.IsActive == 1).ToList();
            else
                //GroupTaskReminders = db.GroupTaskReminders.Where(x => x.IsActive == 1).ToList();
                GroupTaskReminders = db.GroupTaskReminders.Where(x => x.IsActive == 1).ToList();

            return GroupTaskReminders;
        }

        public List<GroupTaskReminder> getGroupTaskRemindersList(DatabaseEntities de = null)
        {
            DatabaseEntities db = new DatabaseEntities();
            //     int adminid = CompanyId;
            List<GroupTaskReminder> GroupTaskReminders = new List<GroupTaskReminder>();
            if (de != null)
                //GroupTaskReminders = de.GroupTaskReminders.Where(x => x.IsActive == 1).ToList();
                GroupTaskReminders = de.GroupTaskReminders.Where(x => x.IsActive == 1 && x.CompanyId == CompanyId).ToList();
            else
                //GroupTaskReminders = db.GroupTaskReminders.Where(x => x.IsActive == 1).ToList();
                GroupTaskReminders = db.GroupTaskReminders.Where(x => x.IsActive == 1 && x.CompanyId == CompanyId).ToList();

            return GroupTaskReminders;
        }
        public GroupTaskReminder getGroupTaskReminderById(int _Id, DatabaseEntities de = null)
        {

            GroupTaskReminder _GroupTaskReminder;
            DatabaseEntities db = new DatabaseEntities();
            if (de != null)
                _GroupTaskReminder = de.GroupTaskReminders.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            else
                _GroupTaskReminder = db.GroupTaskReminders.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);


            return _GroupTaskReminder;
        }

        public bool AddGroupTaskReminder(GroupTaskReminder _GroupTaskReminder)
        {
            _GroupTaskReminder.CompanyId = CompanyId;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.GroupTaskReminders.Add(_GroupTaskReminder);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateGroupTaskReminderwithoutCompany(GroupTaskReminder _GroupTaskReminder, DatabaseEntities de = null)
        {
            

            if (de != null)
            {
                de.Entry(_GroupTaskReminder).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                return true;
            }

            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Entry(_GroupTaskReminder).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateGroupTaskReminder(GroupTaskReminder _GroupTaskReminder, DatabaseEntities de = null)
        {
            if (_GroupTaskReminder.CompanyId == 0 || _GroupTaskReminder.CompanyId == null)
                _GroupTaskReminder.CompanyId = CompanyId;

            if (de != null)
            {
                de.Entry(_GroupTaskReminder).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                return true;
            }

            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Entry(_GroupTaskReminder).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }
        public GroupTaskReminder getGroupTaskReminderByIdUpdated(int _Id)
        {
            GroupTaskReminder _GroupTaskReminder;
            using (DatabaseEntities db = new DatabaseEntities())
            {

                _GroupTaskReminder = db.GroupTaskReminders.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            }

            return _GroupTaskReminder;
        }


        #endregion


    }
}
