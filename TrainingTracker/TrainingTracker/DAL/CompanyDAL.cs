
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;



namespace TrainingTracker.DAL
{

  public class CompanyDL
    {
 #region Company
        public List<Company> getCompanyList()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<Company> Company = db.Companies.Where(x=>x.isActive==1).ToList();

            return Company;
        }
        public List<Company> getAllCompanyList()
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                
                List<Company> Company = db.Companies.ToList();
            
            return Company;
                }
        }
        
        public Company getCompanyById(int _Id, DatabaseEntities de=null)
        {
            Company _Company;
            if (de == null)
            {
         
                DatabaseEntities db = new DatabaseEntities();

                _Company = db.Companies.FirstOrDefault(x => x.Id == _Id && x.isActive == 1);
            }
            else
            {
                _Company = de.Companies.FirstOrDefault(x => x.Id == _Id && x.isActive == 1);

            }


            return _Company;
        }

        public int AddCompany(Company _Company)
        {
            try
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Companies.Add(_Company);
                    db.SaveChanges();
                    return _Company.Id;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
               
            }
            return -1;
        }

        public bool UpdateCompany(Company _Company, DatabaseEntities de= null)
        {
            if(de==null)
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Entry(_Company).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                de.Entry(_Company).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            return true;
        }

        public void DeleteCompany(int _id)
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Companies.Remove(db.Companies.FirstOrDefault(x => x.Id == _id));
                db.SaveChanges();
            }
        }
#endregion

}
}