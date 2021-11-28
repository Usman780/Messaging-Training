using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;

namespace TrainingTracker.BL
{
    public class FileVersionBL
    {
        #region FileVersion
        public List<FileVersion> getFileVersionListByLogedinUser()
        {
            return new FileVersionDAL().getFileVersionListByLogedinUser();
        }
        public List<FileVersion> getAllFileVersionList()
        {
            return new FileVersionDAL().getAllFileVersionList();
        }


        public FileVersion getFileVersionById(int _id, DatabaseEntities de = null)
        {
            return new FileVersionDAL().getFileVersionById(_id, de);
        }

        public Models.FileVersion AddFileVersion(Models.FileVersion file)
        {
            return new FileVersionDAL().AddFileVersion(file);
        }

        public Models.FileVersion UpdateFileVersion(Models.FileVersion file)
        {
            return new FileVersionDAL().UpdateFileVersion(file);
        }

        #endregion
    }
}