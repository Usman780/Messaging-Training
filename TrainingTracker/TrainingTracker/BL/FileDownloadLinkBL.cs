using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;

namespace TrainingTracker.BL
{
    public class FileDownloadLinkBL
    {
        #region FileDownloadLink
        public List<FileDownloadLink> getFileDownloadLinkList()
        {
            return new FileDownloadLinkDAL().getFileDownloadLinksList();
        }
        public List<FileDownloadLink> getAllFileDownloadLinkList()
        {
            return new FileDownloadLinkDAL().getAllFileDownloadLinksList();
        }
        public FileDownloadLink getFileDownloadLinkById(int _id, DatabaseEntities db=null)
        {
            return new FileDownloadLinkDAL().getFileDownloadLinkById(_id,db);
        }

        public int AddFileDownloadLink(FileDownloadLink _FileDownloadLink)
        {
            //if (_FileDownloadLink.Name == null || _FileDownloadLink.Email == null || _FileDownloadLink.Password == null || _FileDownloadLink.Website_Address == null || _FileDownloadLink.Phone == null)
            //    return false;
            return new FileDownloadLinkDAL().AddFileDownloadLink(_FileDownloadLink);
        }

        public bool UpdateFileDownloadLink(FileDownloadLink _FileDownloadLink,DatabaseEntities db=null)
        {
            //if (_FileDownloadLink.Name == null || _FileDownloadLink.Email == null || _FileDownloadLink.Password == null || _FileDownloadLink.Website_Address == null || _FileDownloadLink.Phone == null)
            //    return false;

            return new FileDownloadLinkDAL().UpdateFileDownloadLink(_FileDownloadLink,db);
        }

        public void DeleteFileDownloadLink(int _id)
        {
            new FileDownloadLinkDAL().DeleteFileDownloadLink(_id);
        }
        #endregion
    }
}