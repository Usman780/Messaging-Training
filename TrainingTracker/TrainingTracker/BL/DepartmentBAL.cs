
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class DepartmentBL
    {
       

 #region Departments
        public List<Department> getDepartmentsList()
        {
            return new DepartmentDL().getDepartmentsList();
        }
        public List<Department> getAllDepartmentsList()
        {
            return new DepartmentDL().getAllDepartmentsList();
        }
        public Department getDepartmentsById(int _id)
        {
            return new DepartmentDL().getDepartmentById(_id);
        }

        public bool AddDepartments(Department _Departments)
        {
            //if (_Departments.Name == null || _Departments.Email == null || _Departments.Password == null || _Departments.Website_Address == null || _Departments.Phone == null)
            //    return false;
            return new DepartmentDL().AddDepartment(_Departments);
        }

        public bool UpdateDepartments(Department _Departments)
        {
            //if (_Departments.Name == null || _Departments.Email == null || _Departments.Password == null || _Departments.Website_Address == null || _Departments.Phone == null)
            //    return false;

            return new DepartmentDL().UpdateDepartment(_Departments);
        }

        public void DeleteDepartments(int _id)
        {
            new DepartmentDL().DeleteDepartment(_id);
        }
        #endregion

}
}