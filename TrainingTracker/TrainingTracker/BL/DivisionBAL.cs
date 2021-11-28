
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class DivisionBL
    {
       

 #region Divisions
        public List<Division> getDivisionsList()
        {
            return new DivisionDL().getDivisionsList();
        }
        public List<Division> getAllDivisionsList()
        {
            return new DivisionDL().getAllDivisionsList();
        }
        public Division getDivisionsById(int _id)
        {
            return new DivisionDL().getDivisionById(_id);
        }

        public bool AddDivisions(Division _Divisions)
        {
            //if (_Divisions.Name == null || _Divisions.Email == null || _Divisions.Password == null || _Divisions.Website_Address == null || _Divisions.Phone == null)
            //    return false;
            return new DivisionDL().AddDivision(_Divisions);
        }

        public bool UpdateDivisions(Division _Divisions)
        {
            //if (_Divisions.Name == null || _Divisions.Email == null || _Divisions.Password == null || _Divisions.Website_Address == null || _Divisions.Phone == null)
            //    return false;

            return new DivisionDL().UpdateDivision(_Divisions);
        }

        public void DeleteDivisions(int _id)
        {
            new DivisionDL().DeleteDivision(_id);
        }
        #endregion

}
}