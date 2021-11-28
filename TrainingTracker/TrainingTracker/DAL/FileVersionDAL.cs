using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class FileVersionDAL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

        #region FileVersion
        public List<FileVersion> getFileVersionListByLogedinUser()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<FileVersion> File = db.sp_GetFileVersions().Where(x => x.UploadedBy == logedinuser.Id).ToList();

            return File;
        }


        public List<FileVersion> getAllFileVersionList()
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                List<FileVersion> File = db.sp_GetFileVersions().ToList();
                return File;
            }
        }

        public FileVersion getFileVersionById(int _Id, DatabaseEntities de = null)
        {
            FileVersion _File;
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();

                _File = db.sp_GetFileVersionById(_Id).FirstOrDefault();
            }
            else
            {
                _File = de.sp_GetFileVersionById(_Id).FirstOrDefault();
            }

            return _File;
        }

        public Models.FileVersion AddFileVersion(Models.FileVersion file)
        {
            DatabaseEntities db = new DatabaseEntities();

            try
            {
                Models.FileVersion _file = db.sp_FileVersionAddUpdate("Insert", file.Id, file.Name, file.IsActive, file.CreatedAt, file.CheckInTime,
              file.CheckOutTime, file.FolderId, file.FilePath, file.Privacy, file.IsSigned,
              file.SignedImage, file.SignedBy, file.ArchiveDate, file.UploadingDate, file.LastModified,
              file.CheckIn, file.CheckInMessage, file.CheckOut, file.CheckOutBy, file.UploadedBy, file.ArchivedDate,
              file.ArchivedBy, file.VersionNo, file.FileId, file.CurrentVersion,
                    file.CompanyId, file.DivisionId, file.DepartmentId).FirstOrDefault();

                db.SaveChangesAsync();

                file.Id = _file.Id;

                return file;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public Models.FileVersion UpdateFileVersion(Models.FileVersion file)
        {
            DatabaseEntities db = new DatabaseEntities();

            try
            {
                Models.FileVersion _file = db.sp_FileVersionAddUpdate("Update", file.Id, file.Name, file.IsActive, file.CreatedAt, file.CheckInTime,
                   file.CheckOutTime, file.FolderId, file.FilePath, file.Privacy, file.IsSigned,
                   file.SignedImage, file.SignedBy, file.ArchiveDate, file.UploadingDate, file.LastModified,
                   file.CheckIn, file.CheckInMessage, file.CheckOut, file.CheckOutBy, file.UploadedBy, file.ArchivedDate,
                   file.ArchivedBy, file.VersionNo, file.FileId, file.CurrentVersion,
                   file.CompanyId, file.DivisionId, file.DepartmentId).FirstOrDefault();

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