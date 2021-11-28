//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using TrainingTracker.Models;

//namespace TrainingTracker.DAL
//{
//    public class TaskManagerDL
//    {
//        public TaskManagerDL()
//        {
//        }

//        #region TaskManager

//        public List<TaskManager> getTaskManagersList(DatabaseEntities de = null)
//        {
//            int adminid = (int)(int)HttpContext.Current.Session["Admin"];
//            if (de != null)
//            {
//                return de.TaskManagers.Where(x => x.IsActive == 1 && x.Manager.AdminID == adminid).ToList();
//            }
//            DatabaseEntities db = new DatabaseEntities();
//            List<TaskManager> TaskManagers = db.TaskManagers.Where(x => x.IsActive == 1 && x.Manager.AdminID == adminid).ToList();

//            return TaskManagers;
//        }


//        public List<TaskManager> getTaskManagersListWithoutAdmin(DatabaseEntities de = null)
//        {
           
//            if (de != null)
//            {
//                return de.TaskManagers.Where(x => x.IsActive == 1).ToList();
//            }
//            DatabaseEntities db = new DatabaseEntities();
//            List<TaskManager> TaskManagers = db.TaskManagers.Where(x => x.IsActive == 1).ToList();

//            return TaskManagers;
//        }

//        public List<TaskManager> getAllTaskManagersList()
//        {
//            int adminid = (int)(int)HttpContext.Current.Session["Admin"];
//            DatabaseEntities db = new DatabaseEntities();
//            List<TaskManager> TaskManagers = db.TaskManagers.Where(x => x.Manager.AdminID == adminid).ToList();

//            return TaskManagers;
//        }

//        public TaskManager getTaskManagerById(int _Id, DatabaseEntities de = null)
//        {
//            TaskManager _TaskManager;

//            if (de == null)
//            {
//                DatabaseEntities db = new DatabaseEntities();

//                _TaskManager = db.TaskManagers.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);

//                return _TaskManager;
//            }

//            _TaskManager = de.TaskManagers.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);

//            return _TaskManager;
//        }

//        public TaskManager getTaskManagerByIdWrapper(int _Id, DatabaseEntities de = null)
//        {
//            TaskManager _TaskManager;
//            using (DatabaseEntities db1 = new DatabaseEntities())
//            {
//                _TaskManager = db1.TaskManagers.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
//            }
//            return _TaskManager;
//        }

//        public TaskManager AddTaskManager(TaskManager _TaskManager, DatabaseEntities de = null)
//        {
//            if (de != null)
//            {
//                de.TaskManagers.Add(_TaskManager);
//            }
//            else
//                using (DatabaseEntities db = new DatabaseEntities())
//                {
//                    db.TaskManagers.Add(_TaskManager);
//                    db.SaveChanges();
//                }
//            return _TaskManager;
//        }

//        public bool UpdateTaskManager(TaskManager _TaskManager, DatabaseEntities de = null)
//        {
//            if (de == null)
//            {
//                using (DatabaseEntities db1 = new DatabaseEntities())
//                {
//                    db1.Entry(_TaskManager).State = System.Data.Entity.EntityState.Modified;
//                    db1.SaveChanges();
//                }
//                return true;
//            }
//            de.Entry(_TaskManager).State = System.Data.Entity.EntityState.Modified;
//            return true;
//        }

//        public void DeleteTaskManager(int _id, DatabaseEntities db = null)
//        {
//            bool varialble = true;
//            if (db == null)
//            {
//                db = new DatabaseEntities();
//                varialble = false;
//            }
//            TaskManager _TaskManager = db.TaskManagers.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
//            if (_TaskManager != null)
//            {
//                _TaskManager.IsActive = 0;
//                foreach (var item in _TaskManager.ManagerTaskComments.ToList())
//                {
//                    new ManagerTaskCommentDAL().DeleteManagerTaskComment(item.Id, db);
//                }

//                db.Entry(_TaskManager).State = System.Data.Entity.EntityState.Modified;

//                if (!varialble)
//                {
//                    db.SaveChanges();
//                }
//            }
//        }

//        #endregion TaskManager
//    }
//}