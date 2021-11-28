
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class UserBL
    {
       

 #region Users
        public List<User> getUsersList()
        {
            return new UserDL().getUsersList();
        }
        public List<User> getFogetPwdUsersList()
        {
            return new UserDL().getFogetPwdUsersList();
        }
        public List<User> getTraineesList()
        {
            return new UserDL().getTraineesList();
        }
        public List<User> getManagerList()
        {
            return new UserDL().getManagerList();
        }
        public List<User> getAdminList()
        {
            return new UserDL().getAdminList();
        }
        public List<User> getActiveandInvitedUser()
        {
            return new UserDL().getActiveandInvitedUser();
        } 
        public List<User> getInActiveUsersofCompany()
        {
            return new UserDL().getInActiveUsersofCompany();
        }
        
        public List<User> getAllUsersList()
        {
            return new UserDL().getAllUsersList();
        }
        public List<User> getAllUsersForLogin()
        {
            return new UserDL().getAllUsersForLogin();
        }
        public User getDeletedUserById(int id,DatabaseEntities de=null)
        {

            return new UserDL().getDeletedUserById(id,de);
        }

        public List<User> getAllActiveUsersList()
        {
            return new UserDL().getAllActiveUsersList();
        }
        public User getUsersById(int _id, DatabaseEntities de=null)
        {
            return new UserDL().getUserById(_id, de);
        }

        public User GetAllUserById(int _id, DatabaseEntities de = null)
        {
            return new UserDL().GetAllUserById(_id, de);
        }

        public User getInActiveUserById(int _id, DatabaseEntities de=null)
        {
            return new UserDL().getInActiveUserById(_id, de);
        }
        public User getUserByIdInactive(int _id, DatabaseEntities de = null)
        {
            return new UserDL().getUserByIdInactive(_id, de);
        }
        public List<User>  getUsersListActiveandInactiveofCompany()
        {
            return new UserDL().getUsersListActiveandInactiveofCompany();
        }
        public bool AddUsers(User _Users)
        {
            //if (_Users.Name == null || _Users.Email == null || _Users.Password == null || _Users.Website_Address == null || _Users.Phone == null)
            //    return false;
            return new UserDL().AddUser(_Users);
        }

        public bool UpdateUsers(User _Users, DatabaseEntities de=null)
        {
            //if (_Users.Name == null || _Users.Email == null || _Users.Password == null || _Users.Website_Address == null || _Users.Phone == null)
            //    return false;

            return new UserDL().UpdateUser(_Users,de);
        }

        public void DeleteUsers(int _id)
        {
            new UserDL().DeleteUser(_id);
        }
        #endregion

}
}