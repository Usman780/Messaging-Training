
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;



namespace TrainingTracker.DAL
{

  public class TaskFileDL
    {
        #region TaskFile
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);
        public List<TaskFile> getTaskFileList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<TaskFile> TaskFile = db.TaskFiles.Where(x=>x.IsActive==1).ToList();
            List<TaskFile> TaskFile = db.TaskFiles.Where(x=>x.IsActive==1&&x.CompanyId==CompanyId).ToList();

            return TaskFile;
        }
        public List<TaskFile> getAllTaskFileList()
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                //List<TaskFile> TaskFile = db.TaskFiles.ToList();
                List<TaskFile> TaskFile = db.TaskFiles.Where(x=>x.CompanyId == CompanyId).ToList();
            
            return TaskFile;
                }
        }
        
        public TaskFile getTaskFileById(int _Id)
        {
            TaskFile _TaskFile;
            DatabaseEntities db = new DatabaseEntities();
            
                _TaskFile = db.TaskFiles.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            

            return _TaskFile;
        }

        public bool AddTaskFile(TaskFile _TaskFile)
        {
            _TaskFile.CompanyId = CompanyId;
            try
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.TaskFiles.Add(_TaskFile);
                    db.SaveChanges();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return true;
        }

        public bool UpdateTaskFile(TaskFile _TaskFile, DatabaseEntities de = null)
        {
            if (_TaskFile.CompanyId == 0 || _TaskFile.CompanyId == null)
                _TaskFile.CompanyId = CompanyId;
            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_TaskFile).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                de.Entry(_TaskFile).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            return true;
        }

        public void DeleteTaskFile(int _id)
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.TaskFiles.Remove(db.TaskFiles.FirstOrDefault(x => x.Id == _id));
                db.SaveChanges();
            }
        }
#endregion

}
}