using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.DAL;
namespace TrainingTracker.BL
{
    public class OldPasswordBL
    {
        #region Oldpasswords
        public List<OldPassword> getOldPasswordsList()
        {
            return new OldPasswordDAL().getOldPasswordsList();
        }
        public List<OldPassword> getAllOldPasswordList()
        {
            return new OldPasswordDAL().getAllOldPasswordList();
        }
        public OldPassword getOldPasswordById(int _id)
        {
            return new OldPasswordDAL().getOldPasswordById(_id);
        }

        public bool AddOldPassword(OldPassword oldpass)
        {
            //if (_TaskTags.Name == null || _TaskTags.Email == null || _TaskTags.Password == null || _TaskTags.Website_Address == null || _TaskTags.Phone == null)
            //    return false;
            return new OldPasswordDAL().AddOldPassword(oldpass);
        }

        public bool UpdateOldPassword(OldPassword oldpass)
        {
            //if (_TaskTags.Name == null || _TaskTags.Email == null || _TaskTags.Password == null || _TaskTags.Website_Address == null || _TaskTags.Phone == null)
            //    return false;

            return new OldPasswordDAL().UpdateOldPassword(oldpass);
        }

        public void DeleteOldPassword(int _id)
        {
            new OldPasswordDAL().DeleteOldPassword(_id);
        }
        #endregion
    }
}