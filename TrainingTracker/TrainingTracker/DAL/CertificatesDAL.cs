
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;



namespace TrainingTracker.DAL
{

  public class CertificateDL
    {
        #region Certificate
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);
        public List<Certificate> getCertificateList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //List<Certificate> Certificate = db.sp_GetCertificates().Where(x => x.IsActive == 1).ToList();
            List<Certificate> Certificate = db.sp_GetCertificates().Where(x => x.IsActive == 1&&x.CompanyId == CompanyId).ToList();

            return Certificate;
        }
        public List<Certificate> getAllCertificateList()
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                //List<Certificate> Certificate = db.sp_GetCertificates().ToList();
                List<Certificate> Certificate = db.sp_GetCertificates().Where(x=>x.CompanyId == CompanyId).ToList();

                return Certificate;
            }
        }

        public Certificate getCertificateById(int _Id)
        {
            Certificate _Certificate;
            DatabaseEntities db = new DatabaseEntities();

            _Certificate = db.sp_GetCertificateById(_Id).FirstOrDefault(x => x.IsActive == 1);


            return _Certificate;
        }

        public bool AddCertificate(Certificate _Certificate)
        {
            try
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    //db.Certificates.Add(_Certificate);
                    //db.sp_AddUpdateCertificate("Insert", _Certificate.Id, _Certificate.IsActive, _Certificate.Name, _Certificate.Path, _Certificate.UserID, _Certificate.CreatedAt);
                    db.sp_AddUpdateCertificate("Insert", _Certificate.Id, _Certificate.IsActive, _Certificate.Name, _Certificate.Path, _Certificate.UserID, _Certificate.CreatedAt, _Certificate.CompanyId);

                    db.SaveChanges();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return true;
        }

        public bool UpdateCertificate(Certificate _Certificate, DatabaseEntities de = null)
        {
            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_Certificate).State = System.Data.Entity.EntityState.Modified;
                   // db.sp_AddUpdateCertificate("Update", _Certificate.Id, _Certificate.IsActive, _Certificate.Name, _Certificate.Path, _Certificate.UserID, _Certificate.CreatedAt);
                    //db.sp_AddUpdateCertificate("Update", _Certificate.Id, _Certificate.IsActive, _Certificate.Name, _Certificate.Path, _Certificate.UserID, _Certificate.CreatedAt, _Certificate.CompanyId);
                    db.SaveChanges();
                }
                return true;
            }
            else
            {
                de.Entry(_Certificate).State = System.Data.Entity.EntityState.Modified;
                //de.sp_AddUpdateCertificate("Update", _Certificate.Id, _Certificate.IsActive, _Certificate.Name, _Certificate.Path, _Certificate.UserID, _Certificate.CreatedAt);
                //de.sp_AddUpdateCertificate("Update", _Certificate.Id, _Certificate.IsActive, _Certificate.Name, _Certificate.Path, _Certificate.UserID, _Certificate.CreatedAt, _Certificate.CompanyId);
                de.SaveChanges();

                return true;
            }
        }

        public void DeleteCertificate(int _id, DatabaseEntities db = null)
        {
            Certificate _Certificate = db.sp_GetCertificateById(_id).FirstOrDefault(x => x.IsActive == 1);
            //_Certificate.IsActive = 0;

            //db.Entry(_Certificate).State = System.Data.Entity.EntityState.Modified;

            db.sp_AddUpdateCertificate("Update", _Certificate.Id, 0, _Certificate.Name, _Certificate.Path, _Certificate.UserID, _Certificate.CreatedAt, _Certificate.CompanyId);
            db.SaveChanges();

        }
        #endregion

    }
}