
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class GroupTaskBL
    {
       

 #region GroupTasks
        public List<GroupTask> getGroupTasksList(DatabaseEntities de=null)
        {
            return new GroupTaskDL().getGroupTasksList(de);
        }
        public List<GroupTask> getAllGroupTasksList()
        {
            return new GroupTaskDL().getAllGroupTasksList();
        }
        public GroupTask getGroupTasksById(int _id)
        {
            return new GroupTaskDL().getGroupTaskById(_id);
        }

        public int AddGroupTasks(GroupTask _GroupTasks)
        {
            //if (_GroupTasks.Name == null || _GroupTasks.Email == null || _GroupTasks.Password == null || _GroupTasks.Website_Address == null || _GroupTasks.Phone == null)
            //    return false;
            return new GroupTaskDL().AddGroupTask(_GroupTasks);
        }

        public bool UpdateGroupTasks(GroupTask _GroupTasks, DatabaseEntities de=null)
        {
            //if (_GroupTasks.Name == null || _GroupTasks.Email == null || _GroupTasks.Password == null || _GroupTasks.Website_Address == null || _GroupTasks.Phone == null)
            //    return false;

            return new GroupTaskDL().UpdateGroupTask(_GroupTasks,de);
        }

        public void DeleteGroupTasks(int _id)
        {
            new GroupTaskDL().DeleteGroupTask(_id);
        }
        #endregion

}
}