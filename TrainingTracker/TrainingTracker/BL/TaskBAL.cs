
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class TaskBL
    {
       

 #region Tasks
        public List<Task> getTasksList()
        {
            return new TaskDL().getTasksList();
        }
        public List<Task> getAllTasksList()
        {
            return new TaskDL().getAllTasksList();
        }
        public Task getTasksById(int _id)
        {
            return new TaskDL().getTaskById(_id);
        }

        public int AddTasks(Task _Tasks)
        {
            //if (_Tasks.Name == null || _Tasks.Email == null || _Tasks.Password == null || _Tasks.Website_Address == null || _Tasks.Phone == null)
            //    return false;
            return new TaskDL().AddTask(_Tasks);
        }

        public bool UpdateTasks(Task _Tasks)
        {
            //if (_Tasks.Name == null || _Tasks.Email == null || _Tasks.Password == null || _Tasks.Website_Address == null || _Tasks.Phone == null)
            //    return false;

            return new TaskDL().UpdateTask(_Tasks);
        }

        public void DeleteTasks(int _id, DatabaseEntities de= null)
        {
            new TaskDL().DeleteTask(_id,de);
        }
        #endregion

}
}