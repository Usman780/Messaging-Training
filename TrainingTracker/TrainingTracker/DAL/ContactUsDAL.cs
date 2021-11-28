using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class ContactUsDAL
    {
        #region Olpasswords

        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);
        public List<Contact> getContactUssList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<Contact> pass = db.Contacts.Where(x => x.IsActive == 1).ToList();
            List<Contact> pass = db.Contacts.Where(x => x.IsActive == 1 && x.CompanyId == CompanyId).ToList();

            return pass;
        }
        public List<Contact> getAllContactUsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<Contact> pass = db.Contacts.ToList();
            List<Contact> pass = db.Contacts.Where(x=>x.CompanyId == CompanyId).ToList();

            return pass;
        }
        public Contact getContactUsById(int _id)
        {
            Contact pass;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                pass = db.Contacts.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
            }

            return pass;
        }

        public bool AddContactUs(Contact oldpass)
        {
            oldpass.CompanyId = CompanyId;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                
                db.Contacts.Add(oldpass);
                db.SaveChanges();
            }
            return true;

        }

        public bool UpdateContactUs(Contact oldpass, DatabaseEntities de = null)
        {
            if(oldpass.CompanyId == 0)
            {
                oldpass.CompanyId = CompanyId;
            }
           
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

        public void DeleteContactUs(int _id)
        {
            DatabaseEntities db = new DatabaseEntities();

            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }

            db.Contacts.Remove(db.Contacts.FirstOrDefault(x => x.Id == _id));
            if (!varialble)
            {
                db.SaveChanges();
            }

        }
        #endregion
    }
}