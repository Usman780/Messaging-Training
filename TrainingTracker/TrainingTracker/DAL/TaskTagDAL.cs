
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;



namespace TrainingTracker.DAL
{
    public class TaskTagDL
    {
        #region TaskTag
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);
        public List<TaskTag> getTaskTagsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<TaskTag> TaskTags = db.TaskTags.Where(x=>x.IsActive==1).ToList();
            List<TaskTag> TaskTags = db.TaskTags.Where(x=>x.IsActive==1 && x.CompanyId == CompanyId).ToList();

            return TaskTags;
        }
        public List<TaskTag> getAllTaskTagsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<TaskTag> TaskTags = db.TaskTags.ToList();
            List<TaskTag> TaskTags = db.TaskTags.Where(x=> x.CompanyId == CompanyId).ToList();

            return TaskTags;
        }
        
        public TaskTag getTaskTagById(int _Id)
        {
            TaskTag _TaskTag;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                _TaskTag = db.TaskTags.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            }

            return _TaskTag;
        }

        public bool AddTaskTag(TaskTag _TaskTag)
        {
            _TaskTag.CompanyId = CompanyId;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.TaskTags.Add(_TaskTag);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateTaskTag(TaskTag _TaskTag, DatabaseEntities de=null)
        {
            if (_TaskTag.CompanyId == 0 || _TaskTag.CompanyId == null)
                _TaskTag.CompanyId = CompanyId;
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();

                db.Entry(_TaskTag).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                de.Entry(_TaskTag).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            return true;
        }

        public void DeleteTaskTag(int _id, DatabaseEntities db = null)
        {
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }

            db.TaskTags.Remove(db.TaskTags.FirstOrDefault(x => x.Id == _id));
            if (!varialble)
            {
                db.SaveChanges();
            }

        }
        #endregion

    }
}