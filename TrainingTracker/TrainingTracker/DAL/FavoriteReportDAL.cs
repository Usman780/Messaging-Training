using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class FavoriteReportDAL
    {
        #region FavoriteReport

        CheckAuthenticationDTO loggedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);

        public List<FavoriteReport> getFavoriteReportList()
        {
            DatabaseEntities db = new DatabaseEntities();
           // List<FavoriteReport> FavoriteReport = db.FavoriteReports.Where(x => x.IsActive == 1 && x.IsShared!=1).ToList();
            List<FavoriteReport> FavoriteReport = db.FavoriteReports.Where(x => x.IsActive == 1 && x.IsShared!=1 && x.CompanyId == CompanyId).ToList();

            return FavoriteReport;
        } 
        public List<FavoriteReport> getSharedReportList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<FavoriteReport> FavoriteReport = db.FavoriteReports.Where(x => x.IsActive == 1 && x.IsShared==1).ToList();
            List<FavoriteReport> FavoriteReport = db.FavoriteReports.Where(x => x.IsActive == 1 && x.IsShared==1 && x.CompanyId == CompanyId).ToList();

            return FavoriteReport;
        }
        public List<FavoriteReport> getAllFavoriteReportList()
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {

                List<FavoriteReport> FavoriteReport = db.FavoriteReports.Where(x=>x.CompanyId == CompanyId).ToList();

                return FavoriteReport;
            }
        }

        public FavoriteReport getFavoriteReportById(int _Id, DatabaseEntities de = null)
        {
            FavoriteReport _FavoriteReport;
            if (de == null)
            {

                DatabaseEntities db = new DatabaseEntities();

                _FavoriteReport = db.FavoriteReports.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            }
            else
            {
                _FavoriteReport = de.FavoriteReports.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);

            }

            return _FavoriteReport;
        }

        public int AddFavoriteReport(FavoriteReport _FavoriteReport, DatabaseEntities de = null)
        {
            try
            {
                _FavoriteReport.CompanyId = CompanyId;
                if (de != null)
                {
                    de.FavoriteReports.Add(_FavoriteReport);
                    de.SaveChanges();
                    return _FavoriteReport.Id;
                }
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.FavoriteReports.Add(_FavoriteReport);
                    db.SaveChanges();
                    return _FavoriteReport.Id;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

            }
            return -1;
        }

        public bool UpdateFavoriteReport(FavoriteReport _FavoriteReport, DatabaseEntities de = null)
        {
            if (_FavoriteReport.CompanyId == 0 || _FavoriteReport.CompanyId == null)
                _FavoriteReport.CompanyId = CompanyId;

            if (de == null)
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_FavoriteReport).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            else
            {
                de.Entry(_FavoriteReport).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            return true;
        }

        public void DeleteFavoriteReport(int _id)
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.FavoriteReports.Remove(db.FavoriteReports.FirstOrDefault(x => x.Id == _id));
                db.SaveChanges();
            }
        }
        #endregion
    }
}