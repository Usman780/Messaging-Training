
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using TrainingTracker.DAL;
//using TrainingTracker.Models;

//namespace TrainingTracker.BL
//{     
// public class TaskManagerBL
//    {
       

// #region TaskManagers
//        public List<TaskManager> getTaskManagersList(DatabaseEntities de= null)
//        {
//            return new TaskManagerDL().getTaskManagersList(de);
//        }
//        public List<TaskManager> getTaskManagersListWithoutAdmin(DatabaseEntities de = null)
//        {
//            return new TaskManagerDL().getTaskManagersListWithoutAdmin(de);
//        }
//        public List<TaskManager> getAllTaskManagersList()
//        {
//            return new TaskManagerDL().getAllTaskManagersList();
//        }
//        public TaskManager getTaskManagersById(int _id)
//        {
//            return new TaskManagerDL().getTaskManagerById(_id);
//        }
//        public TaskManager getTaskManagersByIdWrapper(int _id, DatabaseEntities de = null)
//        {
//            return new TaskManagerDL().getTaskManagerByIdWrapper(_id,de);
//        }

//        public TaskManager AddTaskManagers(TaskManager _TaskManagers, DatabaseEntities de = null)
//        {
//            //if (_TaskManagers.Name == null || _TaskManagers.Email == null || _TaskManagers.Password == null || _TaskManagers.Website_Address == null || _TaskManagers.Phone == null)
//            //    return false;
//            return new TaskManagerDL().AddTaskManager(_TaskManagers,de);
//        }

//        public bool UpdateTaskManagers(TaskManager _TaskManagers,DatabaseEntities de=null)
//        {
//            //if (_TaskManagers.Name == null || _TaskManagers.Email == null || _TaskManagers.Password == null || _TaskManagers.Website_Address == null || _TaskManagers.Phone == null)
//            //    return false;

//            return new TaskManagerDL().UpdateTaskManager(_TaskManagers,de);
//        }

//        public void DeleteTaskManagers(int _id)
//        {
//            new TaskManagerDL().DeleteTaskManager(_id);
//        }
//        #endregion

//}
//}