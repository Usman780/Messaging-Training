using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class OldPasswordDAL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);
        #region Olpasswords
        public List<OldPassword> getOldPasswordsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<OldPassword> pass = db.OldPasswords.Where(x => x.IsActive == 1).ToList();
            List<OldPassword> pass = db.OldPasswords.Where(x => x.IsActive == 1 && x.CompanyId == CompanyId).ToList();

            return pass;
        }
        public List<OldPassword> getAllOldPasswordList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<OldPassword> pass = db.OldPasswords.ToList();
            List<OldPassword> pass = db.OldPasswords.Where(x=>x.CompanyId == CompanyId).ToList();

            return pass;
        }
        public OldPassword getOldPasswordById(int _id)
        {
            OldPassword pass;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                pass = db.OldPasswords.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
            }

            return pass;
        }

        public bool AddOldPassword(OldPassword oldpass)
        {
            if (oldpass.CompanyId == 0 || oldpass.CompanyId == null)
                oldpass.CompanyId = CompanyId;

            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.OldPasswords.Add(oldpass);
                db.SaveChanges();
            }
            return true;

        }

        public bool UpdateOldPassword(OldPassword oldpass, DatabaseEntities de = null)
        {
            if (oldpass.CompanyId == 0 || oldpass.CompanyId == null)
                oldpass.CompanyId = CompanyId;
            //DatabaseEntities de = new DatabaseEntities();
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();

                db.Entry(oldpass).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                de.Entry(oldpass).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            return true;
        }

        public void DeleteOldPassword(int _id)
        {
            DatabaseEntities db = new DatabaseEntities();

            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }

            db.OldPasswords.Remove(db.OldPasswords.FirstOrDefault(x => x.Id == _id));
            if (!varialble)
            {
                db.SaveChanges();
            }

        }
        #endregion
    }
}