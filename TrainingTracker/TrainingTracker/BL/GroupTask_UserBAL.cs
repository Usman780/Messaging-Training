
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{
    public class GroupTask_UserBL
    {


        #region GroupTask_Users
        public List<GroupTask_User> getGroupTask_UsersList(DatabaseEntities de=null)
        {
            return new GroupTask_UserDL().getGroupTask_UsersList(de);
        }

        public List<GroupTask_User> getInActiveGroupTask_UsersList(DatabaseEntities de = null)
        {
            return new GroupTask_UserDL().getInActiveGroupTask_UsersList(de);
        }
        public List<GroupTask_User> getAllGroupTask_UsersList()
        {
            return new GroupTask_UserDL().getAllGroupTask_UsersList();
        }
        public GroupTask_User getGroupTask_UsersById(int _id)
        {
            return new GroupTask_UserDL().getGroupTask_UserById(_id);
        }

        public GroupTask_User getTrainee_TasksByIdWrapper(int _id, DatabaseEntities de = null)
        {
            return new GroupTask_UserDL().getTrainee_TaskByIdWrapper(_id, de);
        }

        public bool UpdateGroupTask_UserWrapper(GroupTask_User _Trainee_Tasks, DatabaseEntities de = null)
        {
            //if (_Trainee_Tasks.Name == null || _Trainee_Tasks.Email == null || _Trainee_Tasks.Password == null || _Trainee_Tasks.Website_Address == null || _Trainee_Tasks.Phone == null)
            //    return false;

            return new GroupTask_UserDL().UpdateTrainee_Task(_Trainee_Tasks, de);
        }
        public bool AddGroupTask_Users(GroupTask_User _GroupTask_Users, DatabaseEntities dewrapper = null)
        {
            //if (_GroupTask_Users.Name == null || _GroupTask_Users.Email == null || _GroupTask_Users.Password == null || _GroupTask_Users.Website_Address == null || _GroupTask_Users.Phone == null)
            //    return false;
            return new GroupTask_UserDL().AddGroupTask_User(_GroupTask_Users, dewrapper);
        }

        public bool UpdateGroupTask_Users(GroupTask_User _GroupTask_Users, DatabaseEntities de = null)
        {
            //if (_GroupTask_Users.Name == null || _GroupTask_Users.Email == null || _GroupTask_Users.Password == null || _GroupTask_Users.Website_Address == null || _GroupTask_Users.Phone == null)
            //    return false;

            return new GroupTask_UserDL().UpdateGroupTask_User(_GroupTask_Users,de);
        }

        public void DeleteGroupTask_Users(int _id,DatabaseEntities de=null)
        {
            new GroupTask_UserDL().DeleteGroupTask_User(_id,de);
        }
        #endregion

    }
}