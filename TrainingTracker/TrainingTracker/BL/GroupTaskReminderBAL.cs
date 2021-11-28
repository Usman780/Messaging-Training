using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.DAL;

namespace TrainingTracker.BL
{
    public class GroupTaskReminderBAL
    {
        #region GroupTaskReminders
        public List<GroupTaskReminder> getGroupTaskRemindersList(DatabaseEntities de=null)
        {
            return new GroupTaskRemindeDAL().getGroupTaskRemindersList(de);
        }
        public List<GroupTaskReminder> getGroupTaskRemindersListwithoutCompany(DatabaseEntities de = null)
        {
            return new GroupTaskRemindeDAL().getGroupTaskRemindersListwithoutCompany(de);
        }
        

        public GroupTaskReminder getGroupTaskRemindersById(int _id, DatabaseEntities de = null)
        {
            return new GroupTaskRemindeDAL().getGroupTaskReminderById(_id,de);
        }

        public bool AddGroupTaskReminders(GroupTaskReminder _GroupTaskReminders)
        {
            //if (_GroupTaskReminders.Name == null || _GroupTaskReminders.Email == null || _GroupTaskReminders.Password == null || _GroupTaskReminders.Website_Address == null || _GroupTaskReminders.Phone == null)
            //    return false;
            return new GroupTaskRemindeDAL().AddGroupTaskReminder(_GroupTaskReminders);
        }

        public bool UpdateGroupTaskReminders(GroupTaskReminder _GroupTaskReminders,DatabaseEntities de=null)
        {
            //if (_GroupTaskReminders.Name == null || _GroupTaskReminders.Email == null || _GroupTaskReminders.Password == null || _GroupTaskReminders.Website_Address == null || _GroupTaskReminders.Phone == null)
            //    return false;

            return new GroupTaskRemindeDAL().UpdateGroupTaskReminder(_GroupTaskReminders,de);
        }
        public bool UpdateGroupTaskReminderwithoutCompany(GroupTaskReminder _GroupTaskReminders, DatabaseEntities de = null)
        {
            //if (_GroupTaskReminders.Name == null || _GroupTaskReminders.Email == null || _GroupTaskReminders.Password == null || _GroupTaskReminders.Website_Address == null || _GroupTaskReminders.Phone == null)
            //    return false;

            return new GroupTaskRemindeDAL().UpdateGroupTaskReminderwithoutCompany(_GroupTaskReminders, de);
        }


        #endregion
    }
}