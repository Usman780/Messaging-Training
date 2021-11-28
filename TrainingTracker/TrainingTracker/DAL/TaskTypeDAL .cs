
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.BL;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;



namespace TrainingTracker.DAL
{

  public class TaskTypeDL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
 #region TaskType
        public List<TaskType> getTaskTypesList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            List<TaskType> TaskTypes = db.TaskTypes.Where(x => x.IsActive == 1 && x.CompanyID == Companyid).ToList();

            return TaskTypes;
        }
        public List<TaskType> getAllTaskTypesList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            List<TaskType> TaskTypes = db.TaskTypes.Where(x => x.CompanyID == Companyid).ToList();

            return TaskTypes;
        }
        
        public TaskType getTaskTypeById(int _Id)
        {
            TaskType _TaskType;
            DatabaseEntities db = new DatabaseEntities();
            
                _TaskType = db.TaskTypes.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            

            return _TaskType;
        }

        public bool AddTaskType(TaskType _TaskType)
        {
            _TaskType.CompanyID = Convert.ToInt32(logedinuser.Company);
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.TaskTypes.Add(_TaskType);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateTaskType(TaskType _TaskType)
        {
            _TaskType.CompanyID = Convert.ToInt32(logedinuser.Company);
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Entry(_TaskType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }

        public TaskType getTaskTypeByIdUpdate(int _Id)
        {
            TaskType _TaskType;
            using (DatabaseEntities db = new DatabaseEntities())
            {

                _TaskType = db.TaskTypes.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            }

            return _TaskType;
        }


        public void DeleteTaskType(int _id)
        {
            DatabaseEntities db = new DatabaseEntities();

            TaskType taskType = db.TaskTypes.Where(x=>x.Id==_id && x.IsActive==1).FirstOrDefault();
            if (taskType != null)
            {
                DepartmentBL dbl = new DepartmentBL();
                taskType.IsActive = 0;


                foreach (Task item in taskType.Tasks.Where(x => x.IsActive == 1).ToList())
                {

                    new DAL.TaskDL().DeleteTask(item.Id, db);
                }




                db.Entry(taskType).State = System.Data.Entity.EntityState.Modified;

            }
            db.SaveChanges();
            
        }
#endregion

}
}