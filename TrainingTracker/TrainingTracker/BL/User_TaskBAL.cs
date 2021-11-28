
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class User_TaskBL
    {


        #region User_Tasks


        //Functions with store procedures

        public List<User_Task> spGetUserTasksByCompany(int CompanyId)
        {
            return new User_TaskDL().spGetUserTasksByCompany(CompanyId);
        }
          public List<User_Task> GetGroupStudyUser_Tasks(int CompanyId, DatabaseEntities de = null)
        {
            return new User_TaskDL().GetGroupStudyUser_Tasks(CompanyId,de);
        }

        public List<User_Task> spGetUserTasksByRole(int Role, int CompanyId)
        {
            return new User_TaskDL().spGetUserTasksByRole(Role, CompanyId);
        }

        public List<User_Task> spGetUserTasksByDate(int CompanyId, DateTime startdate, DateTime enddate)
        {
            return new User_TaskDL().spGetUserTasksByDate(CompanyId, startdate, enddate);
        }

        public List<User_Task> spGetUserTasksByDateAndRole(int Role, int CompanyId, DateTime startdate, DateTime enddate)
        {
            return new User_TaskDL().spGetUserTasksByDateAndRole(Role, CompanyId, startdate, enddate);
        }

        //Ends
        public List<User_Task> getUser_TasksList(DatabaseEntities de=null,int template=-1)
        {
            return new User_TaskDL().getUser_TasksList(de, template);
        }

        public List<User_Task> getInAactiveUser_TasksList(DatabaseEntities de = null, int template = -1)
        {
            return new User_TaskDL().getInAactiveUser_TasksList(de, template);
        }
        public List<User_Task> getUser_TasksListWithoutAdmin(DatabaseEntities de = null)
        {
            return new User_TaskDL().GetUser_TasksListWithoutCompany(de);
        }
        public List<User_Task> getAllUser_TasksList()
        {
            return new User_TaskDL().getAllUser_TasksList();
        }
        public User_Task getUser_TasksById(int _id)
        {
            return new User_TaskDL().getUser_TaskById(_id);
        }
        public User_Task getUser_TasksByIdWrapper(int _id, DatabaseEntities de = null)
        {
            return new User_TaskDL().getUser_TaskByIdWrapper(_id,de);
        }

        public User_Task AddUser_Tasks(User_Task _User_Tasks, DatabaseEntities de=null)
        {
            //if (_User_Tasks.Name == null || _User_Tasks.Email == null || _User_Tasks.Password == null || _User_Tasks.Website_Address == null || _User_Tasks.Phone == null)
            //    return false;
            return new User_TaskDL().AddUser_Task(_User_Tasks,de);
        }

        public bool UpdateUser_Tasks(User_Task _User_Tasks,DatabaseEntities de=null)
        {
            //if (_User_Tasks.Name == null || _User_Tasks.Email == null || _User_Tasks.Password == null || _User_Tasks.Website_Address == null || _User_Tasks.Phone == null)
            //    return false;

            return new User_TaskDL().UpdateUser_Task(_User_Tasks,de);
        }

        public void DeleteUser_Tasks(int _id)
        {
            new User_TaskDL().DeleteUser_Task(_id);
        }
        #endregion

}
}