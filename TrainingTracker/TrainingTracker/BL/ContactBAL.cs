using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;



namespace TrainingTracker.BL
{
    public class ContactBAL
    {
        #region Contact
        public List<Contact> getContactUsList()
        {
            return new ContactUsDAL().getContactUssList();
        }
        public List<Contact> getAllContactUsList()
        {
            return new ContactUsDAL().getAllContactUsList();
        }
        public Contact getContactUsById(int _id)
        {
            return new ContactUsDAL().getContactUsById(_id);
        }

        public bool AddContactUs(Contact oldpass)
        {
            //if (_TaskTags.Name == null || _TaskTags.Email == null || _TaskTags.Password == null || _TaskTags.Website_Address == null || _TaskTags.Phone == null)
            //    return false;
            return new ContactUsDAL().AddContactUs(oldpass);
        }

        public bool UpdateContactUs(Contact oldpass)
        {
            //if (_TaskTags.Name == null || _TaskTags.Email == null || _TaskTags.Password == null || _TaskTags.Website_Address == null || _TaskTags.Phone == null)
            //    return false;

            return new ContactUsDAL().UpdateContactUs(oldpass);
        }

        public void DeleteContactUs(int _id)
        {
            new ContactUsDAL().DeleteContactUs(_id);
        }
        #endregion
    }
}