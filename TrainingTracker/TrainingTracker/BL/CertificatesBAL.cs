
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class CertificateBL
    {
       

 #region Certificate
        public List<Certificate> getCertificateList()
        {
            return new CertificateDL().getCertificateList();
        }
        public List<Certificate> getAllCertificateList()
        {
            return new CertificateDL().getAllCertificateList();
        }
        public Certificate getCertificateById(int _id)
        {
            return new CertificateDL().getCertificateById(_id);
        }

        public bool AddCertificate(Certificate _Certificate)
        {
            //if (_Certificate.Name == null || _Certificate.Email == null || _Certificate.Password == null || _Certificate.Website_Address == null || _Certificate.Phone == null)
            //    return false;
            return new CertificateDL().AddCertificate(_Certificate);
        }

        public bool UpdateCertificate(Certificate _Certificate)
        {
            //if (_Certificate.Name == null || _Certificate.Email == null || _Certificate.Password == null || _Certificate.Website_Address == null || _Certificate.Phone == null)
            //    return false;

            return new CertificateDL().UpdateCertificate(_Certificate);
        }

        public void DeleteCertificate(int _id,DatabaseEntities de=null)
        {
            new CertificateDL().DeleteCertificate(_id,de);
        }
        #endregion

    }
}