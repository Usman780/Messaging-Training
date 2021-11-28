
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class TaskTypeBL
    {
       

 #region TaskTypes
        public List<TaskType> getTaskTypesList()
        {
            return new TaskTypeDL().getTaskTypesList();
        }
        public List<TaskType> getAllTaskTypesList()
        {
            return new TaskTypeDL().getAllTaskTypesList();
        }
        public TaskType getTaskTypesById(int _id)
        {
            return new TaskTypeDL().getTaskTypeById(_id);
        }

        public bool AddTaskTypes(TaskType _TaskTypes)
        {
            //if (_TaskTypes.Name == null || _TaskTypes.Email == null || _TaskTypes.Password == null || _TaskTypes.Website_Address == null || _TaskTypes.Phone == null)
            //    return false;
            return new TaskTypeDL().AddTaskType(_TaskTypes);
        }

        public bool UpdateTaskTypes(TaskType _TaskTypes)
        {
            //if (_TaskTypes.Name == null || _TaskTypes.Email == null || _TaskTypes.Password == null || _TaskTypes.Website_Address == null || _TaskTypes.Phone == null)
            //    return false;

            return new TaskTypeDL().UpdateTaskType(_TaskTypes);
        }

        public void DeleteTaskTypes(int _id)
        {
            new TaskTypeDL().DeleteTaskType(_id);
        }
        #endregion

}
}