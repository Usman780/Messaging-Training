
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

  public class DepartmentDL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);
        #region Department
        public List<Department> getDepartmentsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //int adminid = CompanyId;
            //List<Department> Departments = db.Departments.Where(x=>x.IsActive==1 &&  x.Division.CompanyID == adminid).ToList();
            List<Department> Departments = db.Departments.Where(x=>x.IsActive==1 && x.CompanyId == CompanyId).ToList();

            return Departments;
        }
        public List<Department> getAllDepartmentsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //int adminid = CompanyId;
            //List<Department> Departments = db.Departments.Where(x => x.Division.CompanyID == adminid).ToList();
            List<Department> Departments = db.Departments.Where(x => x.CompanyId == CompanyId).ToList();

            return Departments;
        }
        
        public Department getDepartmentById(int _Id)
        {
            Department _Department;
            DatabaseEntities db = new DatabaseEntities();
            
                _Department = db.Departments.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            

            return _Department;
        }

        public bool AddDepartment(Department _Department)
        {
            _Department.CompanyId = CompanyId;
            using (DatabaseEntities db = new DatabaseEntities())
            {
               
                db.Departments.Add(_Department);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateDepartment(Department _Department, DatabaseEntities de = null)
        {
            if(_Department.CompanyId == 0)
                _Department.CompanyId = CompanyId;

            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_Department).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                de.Entry(_Department).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            return true;
        }

        public Department getDepartmentByIdUpdated(int _Id)
        {
            Department _Department;
            using (DatabaseEntities db = new DatabaseEntities())
            {

                _Department = db.Departments.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            }

            return _Department;
        }


        public void DeleteDepartment(int _id,DatabaseEntities db=null)
        {
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false; 
            }
            Department _Department = db.Departments.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);

            if (_Department != null)
            {
                foreach (User trainee in _Department.Users)
                {

                    new UserDL().DeleteUser(trainee.Id, db);

                }
                _Department.IsActive = 0;
                db.Entry(_Department).State = System.Data.Entity.EntityState.Modified;
            }

         

            if(!varialble)
            {
                db.SaveChanges();
            }

        }

    }
#endregion

}
