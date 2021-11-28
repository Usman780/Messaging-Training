using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;

namespace TrainingTracker.BL
{
    public class FavoriteReportBL
    {

        #region FavoriteReport
        public List<FavoriteReport> getFavoriteReportList()
        {
            return new FavoriteReportDAL().getFavoriteReportList();
        }
        public List<FavoriteReport> getSharedReportList()
        {
            return new FavoriteReportDAL().getSharedReportList();
        }
        public List<FavoriteReport> getAllFavoriteReportList()
        {
            return new FavoriteReportDAL().getAllFavoriteReportList();
        }
        public FavoriteReport getFavoriteReportById(int _id, DatabaseEntities de = null)
        {
            return new FavoriteReportDAL().getFavoriteReportById(_id, de);
        }

        public int AddFavoriteReport(FavoriteReport _FavoriteReport,DatabaseEntities de=null)
        {
            //if (_Company.Name == null || _Company.Email == null || _Company.Password == null || _Company.Website_Address == null || _Company.Phone == null)
            //    return false;
            return new FavoriteReportDAL().AddFavoriteReport(_FavoriteReport,de);
        }

        public bool UpdateFavoriteReport(FavoriteReport _FavoriteReport, DatabaseEntities de = null)
        {
            return new FavoriteReportDAL().UpdateFavoriteReport(_FavoriteReport, de);
        }

        public void DeleteFavoriteReport(int _id)
        {
            new FavoriteReportDAL().DeleteFavoriteReport(_id);
        }
        #endregion
    }
}