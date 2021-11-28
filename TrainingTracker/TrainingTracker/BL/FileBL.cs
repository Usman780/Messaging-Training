using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;

namespace TrainingTracker.BL
{
    public class FileBL
    {
        #region File
        public List<File> getFileListByLogedinUser()
        {
            return new FileDAL().getFileListByLogedinUser();
        }
        public List<File> getAllFileList()
        {
            return new FileDAL().getAllFileList();
        }

        
        public File getFileById(int _id, DatabaseEntities de = null)
        {
            return new FileDAL().getFileById(_id, de);
        }

        public Models.File AddFile(Models.File file)
        {
            return new FileDAL().AddFile(file);
        }

        public Models.File UpdateFile(Models.File file)
        {
            return new FileDAL().UpdateFile(file);
        }

        #endregion
    }
}