using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class FileDAL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

        #region File
        public List<File> getFileListByLogedinUser()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<File> File = db.sp_GetFiles().Where(x => x.UploadedBy == logedinuser.Id).ToList();

            return File;
        }

        
        public List<File> getAllFileList()
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                List<File> File = db.sp_GetFiles().ToList();
                return File;
            }
        }

        public File getFileById(int _Id, DatabaseEntities de = null)
        {
            File _File;
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();

                _File = db.sp_GetFileById(_Id).FirstOrDefault();
            }
            else
            {
                _File = de.sp_GetFileById(_Id).FirstOrDefault();
            }

            return _File;
        }

        public Models.File AddFile(Models.File file)
        {
            DatabaseEntities db = new DatabaseEntities();

            try
            {
                Models.File _file = db.sp_FileAddUpdate("Insert", file.Id, file.Name, file.IsActive, file.CreatedAt, file.CheckInTime,
                file.CheckOutTime, file.FolderId, file.FilePath, file.Privacy, file.IsSigned,
                file.SignedImage, file.SignedBy, file.ArchiveDate, file.UploadingDate, file.LastModified,
                file.CheckIn, file.CheckInMessage, file.CheckOut, file.CheckOutBy, file.UploadedBy, file.ArchivedDate,
                file.ArchivedBy, file.CompanyId, file.DivisionId, file.DepartmentId).FirstOrDefault();

                db.SaveChangesAsync();

                file.Id = _file.Id;

                return file;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public Models.File UpdateFile(Models.File file)
        {
            DatabaseEntities db = new DatabaseEntities();

            try
            {
                Models.File _file = db.sp_FileAddUpdate("Update", file.Id, file.Name, file.IsActive, file.CreatedAt, file.CheckInTime,
                     file.CheckOutTime, file.FolderId, file.FilePath, file.Privacy, file.IsSigned,
                     file.SignedImage, file.SignedBy, file.ArchiveDate, file.UploadingDate, file.LastModified,
                     file.CheckIn, file.CheckInMessage, file.CheckOut, file.CheckOutBy, file.UploadedBy, file.ArchivedDate,
                     file.ArchivedBy, file.CompanyId, file.DivisionId, file.DepartmentId).FirstOrDefault();

                db.SaveChangesAsync();

                file.Id = _file.Id;

                return file;

            }
            catch (Exception)
            {
                return null;
            }
        }


        #endregion
    }
}