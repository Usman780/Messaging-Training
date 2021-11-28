
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;




namespace TrainingTracker.DAL
{

  public class TaskDL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

 #region Task
        public List<Task> getTasksList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int sessionId = Convert.ToInt32(logedinuser.Company);
           //List<Task> Tasks = db.Tasks.Where(x => x.IsActive == 1 && x.Department.Division.CompanyID == sessionId).ToList();
           List<Task> Tasks = db.Tasks.Where(x => x.IsActive == 1 && x.CompanyId == sessionId).ToList();

            return Tasks;
        }

       


        public List<Task> getAllTasksList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            //List<Task> Tasks = db.Tasks.Where(x =>  x.Department.Division.CompanyID == Companyid).ToList();
            List<Task> Tasks = db.Tasks.Where(x =>  x.CompanyId == Companyid).ToList();

            return Tasks;
        }
        
        public Task getTaskById(int _Id)
        {

            DatabaseEntities db = new DatabaseEntities();


              Task _Task = db.Tasks.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            

            return _Task;
        }

        public int AddTask(Task _Task)
        {
            _Task.CompanyId = Convert.ToInt32(logedinuser.Company);
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Tasks.Add(_Task);
                db.SaveChanges();
            }
            return _Task.Id;
        }

        public bool UpdateTask(Task _Task, DatabaseEntities de = null)
        {
            if (_Task.CompanyId == 0 || _Task.CompanyId == null)
                _Task.CompanyId = Convert.ToInt32(logedinuser.Company);
            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_Task).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                de.Entry(_Task).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            return true;
        }

        public void DeleteTask(int _id, DatabaseEntities db = null)
        {
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }



            Task task = db.Tasks.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
            if (task != null)
            {
                task.IsActive = 0;


                foreach (var item in task.GroupTask_Task.Where(x => x.IsActive == 1).ToList())
                {
                    new DAL.GroupTask_TaskDL().DeleteGroupTask_Task(item.Id, db);

                }

                foreach (var item in task.User_Task.Where(x => x.IsActive == 1).ToList())
                {
                    new DAL.User_TaskDL().DeleteUser_Task(item.Id, db);
                }


                if (!varialble)
                {
                    db.SaveChanges();
                }
            }
        }
        #endregion

    }
}