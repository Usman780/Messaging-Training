
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class CompanyBL
    {
       

 #region Company
        public List<Company> getCompanyList()
        {
            return new CompanyDL().getCompanyList();
        }
        public List<Company> getAllCompanyList()
        {
            return new CompanyDL().getAllCompanyList();
        }
        public Company getCompanyById(int _id, DatabaseEntities de= null)
        {
            return new CompanyDL().getCompanyById(_id, de);
        }

        public int AddCompany(Company _Company)
        {
            //if (_Company.Name == null || _Company.Email == null || _Company.Password == null || _Company.Website_Address == null || _Company.Phone == null)
            //    return false;
            return new CompanyDL().AddCompany(_Company);
        }

        public bool UpdateCompany(Company _Company, DatabaseEntities de= null)
        {
            //if (_Company.Name == null || _Company.Email == null || _Company.Password == null || _Company.Website_Address == null || _Company.Phone == null)
            //    return false;

            return new CompanyDL().UpdateCompany(_Company);
        }

        public void DeleteCompany(int _id)
        {
            new CompanyDL().DeleteCompany(_id);
        }
        #endregion

}
}