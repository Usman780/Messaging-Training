
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;



namespace TrainingTracker.DAL
{

  public class TagDL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

 #region Tag
        public List<Tag> getTagsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            //List<Tag> Tags = db.Tags.Where(x => x.IsActive == 1 && x.Division.CompanyID ==Companyid).ToList();
            List<Tag> Tags = db.Tags.Where(x => x.IsActive == 1 && x.CompanyId ==Companyid).ToList();

            return Tags;
        }
        public List<Tag> getAllTagsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            //List<Tag> Tags = db.Tags.Where(x=> x.Division.CompanyID == Companyid).ToList();
            List<Tag> Tags = db.Tags.Where(x=> x.CompanyId == Companyid).ToList();

            return Tags;
        }
        
        public Tag getTagById(int _Id)
        {
            Tag _Tag;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                _Tag = db.Tags.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            }

            return _Tag;
        }

        public bool AddTag(Tag _Tag)
        {
            _Tag.CompanyId = Convert.ToInt32(logedinuser.Company);
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Tags.Add(_Tag);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateTag(Tag _Tag, DatabaseEntities de=null)
        {
            if (_Tag.CompanyId == 0 || _Tag.CompanyId == null)
                _Tag.CompanyId = Convert.ToInt32(logedinuser.Company);
            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_Tag).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                de.Entry(_Tag).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            return true;
        }

        public void DeleteTag(int _id, DatabaseEntities db = null)
        {
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }
            Tag t = db.Tags.Where(x => x.Id == _id).FirstOrDefault();
            if (t != null)
            {
                t.IsActive = 0;
                TaskTagDL tsdl = new TaskTagDL();
                foreach (var item in t.TaskTags)
                {
                    item.IsActive = 0;
                    tsdl.UpdateTaskTag(item, db);
                }
                db.Entry(t).State = System.Data.Entity.EntityState.Modified;

                if (!varialble)
                {
                    db.SaveChanges();
                }
            }
        }
        #endregion

    }
}