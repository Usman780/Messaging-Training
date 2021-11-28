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
    public class WorktypeDL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        #region Worktype
        public List<Worktype> getWorktypesList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            List<Worktype> Worktypes = db.Worktypes.Where(x => x.IsActive == 1 && x.CompanyID == Companyid).ToList();

            return Worktypes;
        }
        public List<Worktype> getAllWorktypesList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            List<Worktype> Worktypes = db.Worktypes.Where(x => x.CompanyID == Companyid).ToList();

            return Worktypes;
        }

        public Worktype getWorktypeById(int _Id)
        {
            Worktype _Worktype;
            DatabaseEntities db = new DatabaseEntities();

            _Worktype = db.Worktypes.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);


            return _Worktype;
        }

        public bool AddWorktype(Worktype _Worktype)
        {
            _Worktype.CompanyID = Convert.ToInt32(logedinuser.Company);
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Worktypes.Add(_Worktype);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateWorktype(Worktype _Worktype)
        {
            _Worktype.CompanyID = Convert.ToInt32(logedinuser.Company);
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Entry(_Worktype).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }

        public Worktype getWorktypeByIdUpdate(int _Id)
        {
            Worktype _Worktype;
            using (DatabaseEntities db = new DatabaseEntities())
            {

                _Worktype = db.Worktypes.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            }

            return _Worktype;
        }


        public void DeleteWorktype(int _id)
        {
            DatabaseEntities db = new DatabaseEntities();

            Worktype Worktype = db.Worktypes.Where(x => x.Id == _id && x.IsActive == 1).FirstOrDefault();
            if (Worktype != null)
            {
                DepartmentBL dbl = new DepartmentBL();
                Worktype.IsActive = 0;

                db.Entry(Worktype).State = System.Data.Entity.EntityState.Modified;

            }
            db.SaveChanges();

        }
        #endregion

    }
}