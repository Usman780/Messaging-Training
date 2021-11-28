using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class FileDownloadLinkDAL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

        #region FileDownloadLink
        public List<FileDownloadLink> getFileDownloadLinksList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int company = Convert.ToInt32(logedinuser.Company);
            List<FileDownloadLink> FileDownloadLinks = db.FileDownloadLinks.Where(x => x.IsActive == 1 && x.CompanyId == company).ToList();

            return FileDownloadLinks;
        }
        public List<FileDownloadLink> getAllFileDownloadLinksList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            List<FileDownloadLink> FileDownloadLinks = db.FileDownloadLinks.Where(x => x.CompanyId == Companyid).ToList();

            return FileDownloadLinks;
        }

        public FileDownloadLink getFileDownloadLinkById(int _Id, DatabaseEntities de = null)
        {
            FileDownloadLink _FileDownloadLink;
            DatabaseEntities db = new DatabaseEntities();
            if (de != null)
            {
                _FileDownloadLink = de.FileDownloadLinks.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);

            }
            else
            {
                _FileDownloadLink = db.FileDownloadLinks.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);

            }


            return _FileDownloadLink;
        }

        public int AddFileDownloadLink(FileDownloadLink _FileDownloadLink)
        {
            _FileDownloadLink.CompanyId = Convert.ToInt32(logedinuser.Company);
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.FileDownloadLinks.Add(_FileDownloadLink);
                db.SaveChanges();
            }
            return _FileDownloadLink.Id;
        }

        public bool UpdateFileDownloadLink(FileDownloadLink _FileDownloadLink, DatabaseEntities de = null)
        {
            _FileDownloadLink.CompanyId = Convert.ToInt32(logedinuser.Company);
            if (de != null)
            {
                de.Entry(_FileDownloadLink).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            else
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_FileDownloadLink).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            
            return true;
        }

        public FileDownloadLink getFileDownloadLinkByIdUpdate(int _Id)
        {
            FileDownloadLink _FileDownloadLink;
            using (DatabaseEntities db = new DatabaseEntities())
            {

                _FileDownloadLink = db.FileDownloadLinks.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            }

            return _FileDownloadLink;
        }


        public void DeleteFileDownloadLink(int _id, DatabaseEntities db = null)
        {
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }
            FileDownloadLink t = db.FileDownloadLinks.Where(x => x.Id == _id).FirstOrDefault();
            if (t != null)
            {
                t.IsActive = 0;
                
                db.Entry(t).State = System.Data.Entity.EntityState.Modified;

                if (!varialble)
                {
                    db.SaveChanges();
                }
            }

        }
        #endregion
    }
}