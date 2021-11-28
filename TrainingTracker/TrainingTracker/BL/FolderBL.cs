using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;

namespace TrainingTracker.BL
{
    public class FolderBL
    {
        #region Folder
        public List<Folder> getFolderListByLogedinUser()
        {
            return new FolderDAL().getFolderListByLogedinUser();
        }
        public List<Folder> getAllFolderList()
        {
            return new FolderDAL().getAllFolderList();
        }

        public Folder getFolderById(int _id, DatabaseEntities de = null)
        {
            return new FolderDAL().getFolderById(_id, de);
        }

        public Folder AddFolderWithReturnValues(Folder folder)
        {
            return new FolderDAL().AddFolderWithReturnValues(folder);
        }

        public Folder UpdateFolderWithReturnValues(Folder folder)
        {
            return new FolderDAL().UpdateFolderWithReturnValues(folder);
        }

        #endregion
    }
}