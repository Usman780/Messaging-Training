using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;

namespace TrainingTracker.DAL
{
    public class User_WorktypeDL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
      
        #region User_Worktype
        public List<User_Worktype> getUser_WorktypesList(DatabaseEntities de = null)
        {
            int Companyid = Convert.ToInt32(logedinuser.Company);
            int id = logedinuser.Id;
            if (de != null)
            {
                //return de.User_Worktype.AsNoTracking().Where(x => x.IsActive == 1 && x.User.CompanyID== Companyid).ToList();
                return de.User_Worktype.AsNoTracking().Where(x => x.IsActive == 1 && x.CompanyId== Companyid).ToList();
            }
            DatabaseEntities db = new DatabaseEntities();
            List<User_Worktype> User_Worktypes;
            //User_Worktypes = db.User_Worktype.Where(x => x.IsActive == 1 && x.User.CompanyID == Companyid).ToList();
            User_Worktypes = db.User_Worktype.Where(x => x.IsActive == 1 && x.CompanyId == Companyid).ToList();
            //User_Worktypes =User_Worktypes.Where(x=>x.User1.CompanyID  == Companyid).ToList();
            return User_Worktypes;
        }

        public List<User_Worktype> GetUser_WorktypesListWithoutCompany(DatabaseEntities de = null)
        {
            int Companyid = Convert.ToInt32(logedinuser.Company);
            if (de != null)
            {
                //return de.User_Worktype.AsNoTracking().Where(x => x.IsActive == 1).ToList();
                return de.User_Worktype.AsNoTracking().Where(x => x.IsActive == 1 && x.CompanyId == Companyid).ToList();
            }
            DatabaseEntities db = new DatabaseEntities();
            //List<User_Worktype> User_Worktypes = db.User_Worktype.Where(x => x.IsActive == 1).ToList();
            List<User_Worktype> User_Worktypes = db.User_Worktype.Where(x => x.IsActive == 1 && x.CompanyId == Companyid).ToList();

            return User_Worktypes;
        }

        public List<User_Worktype> getAllUser_WorktypesList()
        {
            int Companyid = Convert.ToInt32(logedinuser.Company);
            DatabaseEntities db = new DatabaseEntities();
           // List<User_Worktype> User_Worktypes = db.User_Worktype.Where(x => x.User.CompanyID == Companyid).ToList();
            List<User_Worktype> User_Worktypes = db.User_Worktype.Where(x => x.CompanyId == Companyid).ToList();

            return User_Worktypes;
        }

        public User_Worktype getUser_WorktypeById(int _Id)
        {
            DatabaseEntities db = new DatabaseEntities();
            User_Worktype _User_Worktype;


            _User_Worktype = db.User_Worktype.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);


            return _User_Worktype;
        }


        public User_Worktype getUser_WorktypeByIdWrapper(int _Id, DatabaseEntities de = null)
        {
            User_Worktype _User_Worktype;
            using (DatabaseEntities db1 = new DatabaseEntities())
            {
                _User_Worktype = de.User_Worktype.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);

            }
            return _User_Worktype;
        }

        public User_Worktype AddUser_Worktype(User_Worktype _User_Worktype, DatabaseEntities de = null)
        {
            _User_Worktype.CompanyId = Convert.ToInt32(logedinuser.Company);
            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.User_Worktype.Add(_User_Worktype);
                    db.SaveChanges();
                }
                return _User_Worktype;
            }
            else
            {
                de.User_Worktype.Add(_User_Worktype);
                return _User_Worktype;
            }
        }

        public bool UpdateUser_Worktype(User_Worktype _User_Worktype, DatabaseEntities de = null)
        {
            if (_User_Worktype.CompanyId == 0 || _User_Worktype.CompanyId == null)
                _User_Worktype.CompanyId = Convert.ToInt32(logedinuser.Company);
            if (de == null)
            {
                using (DatabaseEntities db1 = new DatabaseEntities())
                {

                    db1.Entry(_User_Worktype).State = System.Data.Entity.EntityState.Modified;
                    db1.SaveChanges();
                }
                return true;
            }
            de.Entry(_User_Worktype).State = System.Data.Entity.EntityState.Modified;
            de.SaveChanges();
            return true;
        }

        public void DeleteUser_Worktype(int _id, DatabaseEntities db = null)
        {
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }
            User_Worktype _User_Worktype = db.User_Worktype.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
            if (_User_Worktype != null)
            {
                _User_Worktype.IsActive = 0;
               

                db.Entry(_User_Worktype).State = System.Data.Entity.EntityState.Modified;

            }
            if (!varialble)
            {
                db.SaveChanges();
            }
        }
        #endregion

    }
}