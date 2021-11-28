
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class GroupTask_TaskBL
    {
       

 #region GroupTask_Tasks
        public List<GroupTask_Task> getGroupTask_TasksList()
        {
            return new GroupTask_TaskDL().getGroupTask_TasksList();
        }
        public List<GroupTask_Task> getAllGroupTask_TasksList()
        {
            return new GroupTask_TaskDL().getAllGroupTask_TasksList();
        }
        public GroupTask_Task getGroupTask_TasksById(int _id)
        {
            return new GroupTask_TaskDL().getGroupTask_TaskById(_id);
        }

        public bool AddGroupTask_Tasks(GroupTask_Task _GroupTask_Tasks)
        {
            //if (_GroupTask_Tasks.Name == null || _GroupTask_Tasks.Email == null || _GroupTask_Tasks.Password == null || _GroupTask_Tasks.Website_Address == null || _GroupTask_Tasks.Phone == null)
            //    return false;
            return new GroupTask_TaskDL().AddGroupTask_Task(_GroupTask_Tasks);
        }

        public bool UpdateGroupTask_Tasks(GroupTask_Task _GroupTask_Tasks)
        {
            //if (_GroupTask_Tasks.Name == null || _GroupTask_Tasks.Email == null || _GroupTask_Tasks.Password == null || _GroupTask_Tasks.Website_Address == null || _GroupTask_Tasks.Phone == null)
            //    return false;

            return new GroupTask_TaskDL().UpdateGroupTask_Task(_GroupTask_Tasks);
        }

        public void DeleteGroupTask_Tasks(int _id)
        {
            new GroupTask_TaskDL().DeleteGroupTask_Task(_id);
        }
       
        #endregion

    }
}