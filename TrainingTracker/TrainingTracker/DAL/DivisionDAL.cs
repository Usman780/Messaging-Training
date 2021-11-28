
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

  public class DivisionDL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        
        #region Division
        public List<Division> getDivisionsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int company =Convert.ToInt32(logedinuser.Company);
            List<Division> Divisions = db.Divisions.Where(x=>x.IsActive==1 && x.CompanyID==company).ToList();

            return Divisions;
        }
        public List<Division> getAllDivisionsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            List<Division> Divisions = db.Divisions.Where(x=>x.CompanyID == Companyid).ToList();

            return Divisions;
        }
        
        public Division getDivisionById(int _Id)
        {
            Division _Division;
            DatabaseEntities db = new DatabaseEntities();
            
                _Division = db.Divisions.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            

            return _Division;
        }

        public bool AddDivision(Division _Division)
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {

                db.Divisions.Add(_Division);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateDivision(Division _Division)
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Entry(_Division).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }

        public Division getDivisionByIdUpdate(int _Id)
        {
            Division _Division;
            using (DatabaseEntities db = new DatabaseEntities())
            {

                _Division = db.Divisions.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            }

            return _Division;
        }


        public void DeleteDivision(int _id)
        {
            DatabaseEntities db = new DatabaseEntities();

            Division divion = db.Divisions.Where(x=>x.Id==_id && x.IsActive==1).FirstOrDefault();
            if (divion != null)
            {
                DepartmentBL dbl = new DepartmentBL();
                divion.IsActive = 0;


                foreach (Department item in divion.Departments.Where(x => x.IsActive == 1).ToList())
                {

                    new DAL.DepartmentDL().DeleteDepartment(item.Id, db);
                }
                
                foreach (Tag tag in divion.Tags)
                {
                    tag.IsActive = 0;
                    db.Entry(tag).State = System.Data.Entity.EntityState.Modified;


                }



                db.Entry(divion).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
#endregion

}
}