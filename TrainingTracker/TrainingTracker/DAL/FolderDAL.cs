using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class FolderDAL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

        #region Folder
        public List<Folder> getFolderListByLogedinUser()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<Folder> Folder = db.sp_GetFolders().Where(x => x.UserId == logedinuser.Id 
            && x.CompanyId == null
            && x.DivisionId == null
            && x.DepartmentId == null
            && x.IsActive != 0).ToList();

            return Folder;
        }


        public List<Folder> getAllFolderList()
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                List<Folder> Folder = db.sp_GetFolders().Where(x => 
                x.CompanyId == null
                && x.DivisionId == null
                && x.DepartmentId == null
                && x.IsActive != 0).ToList();
                return Folder;
            }
        }

        public Folder getFolderById(int _Id, DatabaseEntities de = null)
        {
            Folder Folder;
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();

                Folder = db.sp_GetFolderById(_Id).FirstOrDefault();
            }
            else
            {
                Folder = de.sp_GetFolderById(_Id).FirstOrDefault();
            }

            return Folder;
        }

        public Folder AddFolderWithReturnValues(Folder folder)
        {
            DatabaseEntities db = new DatabaseEntities();

            try
            {
                Folder _folder = db.sp_FolderAddUpdate("Insert", folder.Id, folder.Name, folder.IsActive, folder.CreatedAt,
                folder.UserId, folder.FolderPath, folder.ArchiveDate, folder.Privacy,
                folder.IsSharedRoot, folder.ParentId, folder.LastModified, folder.ArchivedDate, folder.ArchivedBy,
                folder.CompanyId, folder.DivisionId, folder.DepartmentId).FirstOrDefault();

                db.SaveChangesAsync();

                folder.Id = _folder.Id;

                return folder;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Folder UpdateFolderWithReturnValues(Folder folder)
        {
            DatabaseEntities db = new DatabaseEntities();

            try
            {
                Folder _folder = db.sp_FolderAddUpdate("Update", folder.Id, folder.Name, folder.IsActive, folder.CreatedAt,
                folder.UserId, folder.FolderPath, folder.ArchiveDate, folder.Privacy,
                folder.IsSharedRoot, folder.ParentId, folder.LastModified, folder.ArchivedDate, folder.ArchivedBy,
                folder.CompanyId, folder.DivisionId, folder.DepartmentId).FirstOrDefault();

                db.SaveChangesAsync();

                folder.Id = _folder.Id;

                return folder;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}