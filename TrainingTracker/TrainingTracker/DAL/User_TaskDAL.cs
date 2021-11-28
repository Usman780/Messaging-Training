
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;
using TrainingTracker.BL;
using RusticiSoftware.HostedEngine.Client;

namespace TrainingTracker.DAL
{

    public class User_TaskDL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        public User_TaskDL()
        {

        }
        #region User_Task

        //Functions with Store Procedures
        public List<User_Task> spGetUserTasksByCompany(int CompanyId)
        {
            DatabaseEntities db = new DatabaseEntities();

            List<User_Task> User_Tasks = db.spGetUserTasks(CompanyId).Where(x=>x.GroupTaskDetailId==null).ToList();

            return User_Tasks;
        }

        public List<User_Task> GetGroupStudyUser_Tasks(int CompanyId,DatabaseEntities de=null)
        {
            DatabaseEntities db = new DatabaseEntities();

            List<User_Task> User_Tasks = new List<User_Task>();
            if(de!=null)
            User_Tasks = de.spGetUserTasks(CompanyId).Where(x => x.GroupTaskDetailId != null).ToList();
            else
            User_Tasks = db.spGetUserTasks(CompanyId).Where(x => x.GroupTaskDetailId != null).ToList();

            return User_Tasks;
        }

        public List<User_Task> spGetUserTasksByRole(int Role, int CompanyId)
        {
            DatabaseEntities db = new DatabaseEntities();
            List<User_Task> User_Tasks = new List<User_Task>();
            
            try
            {
                User_Tasks = db.spGetUser_Tasks(Role, CompanyId).Where(x => x.GroupTaskDetailId == null).ToList();

            }
            catch (Exception ex)
            {

            }

            return User_Tasks;
        }

   
        public List<User_Task> spGetUserTasksByDate(int CompanyId, DateTime startdate, DateTime enddate)
        {
            DatabaseEntities db = new DatabaseEntities();

            List<User_Task> User_Tasks = db.spGetUserTasksByDate(CompanyId, startdate, enddate).Where(x => x.GroupTaskDetailId == null).ToList();

            return User_Tasks;
        }
        public List<User_Task> spGetUserTasksByDateAndRole(int Role, int CompanyId, DateTime startdate, DateTime enddate)
        {
            DatabaseEntities db = new DatabaseEntities();

            List<User_Task> User_Tasks = db.spGetUserTasksByRoleAndDate(Role, CompanyId, startdate, enddate).Where(x => x.GroupTaskDetailId == null).ToList();

            return User_Tasks;
        }
        //Ends
        public List<User_Task> getUser_TasksList(DatabaseEntities de = null, int template = -1)
        {
            int Companyid = Convert.ToInt32(logedinuser.Company);
            int id = logedinuser.Id;
            if (de != null)
            {
                if (template != -1)
                {
                    //return de.User_Task.AsNoTracking().Where(x => x.IsActive == 1 && x.Task.Department.Division.CompanyID == Companyid).ToList();
                    return de.User_Task.AsNoTracking().Where(x => x.IsActive == 1 && x.CompanyId == Companyid).Where(x => x.GroupTaskDetailId == null).ToList();

                }
                else
                {

                    //return de.User_Task.AsNoTracking().Where(x => x.IsActive == 1 && x.User1.CompanyID == Companyid && x.UserID != null).ToList();
                    return de.User_Task.AsNoTracking().Where(x => x.IsActive == 1 && x.CompanyId == Companyid && x.UserID != null).Where(x => x.GroupTaskDetailId == null).ToList();

                }
            }
            DatabaseEntities db = new DatabaseEntities();
            List<User_Task> User_Tasks;
            if (template != -1)
            {
                //User_Tasks = db.User_Task.Where(x => x.IsActive == 1 && x.Task.Department.Division.CompanyID == Companyid).ToList();
                User_Tasks = db.User_Task.Where(x => x.IsActive == 1 && x.CompanyId == Companyid).Where(x => x.GroupTaskDetailId == null).ToList();

            }
            else
            {
                //var q = db.User_Task.Where(x => x.IsActive == 1 && x.User1.CompanyID == Companyid && x.UserID != null).ToList();
                //var q = db.User_Task.Where(x => x.IsActive == 1 && x.CompanyId == Companyid && x.UserID != null).ToList();


                //User_Tasks = db.User_Task.Where(x => x.IsActive == 1 && x.User1.CompanyID == Companyid && x.UserID != null).ToList();
                User_Tasks = db.User_Task.Where(x => x.IsActive == 1 && x.CompanyId == Companyid && x.UserID != null).Where(x => x.GroupTaskDetailId == null).ToList();
            }
            //User_Tasks =User_Tasks.Where(x=>x.User1.CompanyID  == Companyid).ToList();
            return User_Tasks;
        }
        public List<User_Task> getInAactiveUser_TasksList(DatabaseEntities de = null, int template = -1)
        {
            int Companyid = Convert.ToInt32(logedinuser.Company);
            int id = logedinuser.Id;
            if (de != null)
            {
                if (template != -1)
                {
                    //return de.User_Task.AsNoTracking().Where(x => x.IsActive == 0 && x.Task.Department.Division.CompanyID == Companyid).ToList();
                    return de.User_Task.AsNoTracking().Where(x => x.IsActive == 0 && x.CompanyId == Companyid).Where(x => x.GroupTaskDetailId == null).ToList();

                }
                else
                {

                    //return de.User_Task.AsNoTracking().Where(x => x.IsActive == 0 && x.User1.CompanyID == Companyid && x.UserID != null).ToList();
                    return de.User_Task.AsNoTracking().Where(x => x.IsActive == 0 && x.CompanyId == Companyid && x.UserID != null).Where(x => x.GroupTaskDetailId == null).ToList();

                }
            }
            DatabaseEntities db = new DatabaseEntities();
            List<User_Task> User_Tasks;
            if (template != -1)
            {
                //User_Tasks = db.User_Task.Where(x => x.IsActive == 0 && x.Task.Department.Division.CompanyID == Companyid).ToList();
                User_Tasks = db.User_Task.Where(x => x.IsActive == 0 && x.CompanyId == Companyid).Where(x => x.GroupTaskDetailId == null).ToList();

            }
            else
            {
                //var q = db.User_Task.Where(x => x.IsActive == 0 && x.User1.CompanyID == Companyid && x.UserID != null).ToList();


                //User_Tasks = db.User_Task.Where(x => x.IsActive == 0 && x.User1.CompanyID == Companyid && x.UserID != null).ToList();
                User_Tasks = db.User_Task.Where(x => x.IsActive == 0 && x.CompanyId == Companyid && x.UserID != null).Where(x => x.GroupTaskDetailId == null).ToList();
            }
            //User_Tasks =User_Tasks.Where(x=>x.User1.CompanyID  == Companyid).ToList();
            return User_Tasks;
        }

        public List<User_Task> GetUser_TasksListWithoutCompany(DatabaseEntities de = null)
        {
            if (de != null)
            {
                return de.User_Task.AsNoTracking().Where(x => x.IsActive == 1).ToList();
            }
            DatabaseEntities db = new DatabaseEntities();
            List<User_Task> User_Tasks = db.User_Task.Where(x => x.IsActive == 1).Where(x => x.GroupTaskDetailId == null).ToList();

            return User_Tasks;
        }

        public List<User_Task> getAllUser_TasksList()
        {
            int Companyid = Convert.ToInt32(logedinuser.Company);
            DatabaseEntities db = new DatabaseEntities();
           // List<User_Task> User_Tasks = db.User_Task.Where(x => x.User1.CompanyID == Companyid).ToList();
            List<User_Task> User_Tasks = db.User_Task.Where(x => x.CompanyId == Companyid).Where(x => x.GroupTaskDetailId == null).ToList();

            return User_Tasks;
        }


        public User_Task getUser_TaskById(int _Id)
        {
            DatabaseEntities db = new DatabaseEntities();
            User_Task _User_Task;


            _User_Task = db.User_Task.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);


            return _User_Task;
        }


        public User_Task getUser_TaskByIdWrapper(int _Id, DatabaseEntities de = null)
        {
            User_Task _User_Task;
            using (DatabaseEntities db1 = new DatabaseEntities())
            {
                _User_Task = de.User_Task.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);

            }
            return _User_Task;
        }

        public User_Task AddUser_Task(User_Task _User_Task, DatabaseEntities de = null)
        {
            _User_Task.CompanyId = Convert.ToInt32(logedinuser.Company);
            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.User_Task.Add(_User_Task);
                    db.SaveChanges();
                }
                return _User_Task;
            }
            else
            {
                de.User_Task.Add(_User_Task);
                de.SaveChanges();
                return _User_Task;
            }
        }

        public bool UpdateUser_Task(User_Task _User_Task, DatabaseEntities de = null)
        {
            if (_User_Task.CompanyId == 0 || _User_Task.CompanyId == null)
                _User_Task.CompanyId = Convert.ToInt32(logedinuser.Company);
            if (de == null)
            {
                using (DatabaseEntities db1 = new DatabaseEntities())
                {

                    db1.Entry(_User_Task).State = System.Data.Entity.EntityState.Modified;
                    db1.SaveChanges();
                }
                return true;
            }
            de.Entry(_User_Task).State = System.Data.Entity.EntityState.Modified;
            de.SaveChanges();
            return true;
        }

        public void DeleteUser_Task(int _id, DatabaseEntities db = null)
        {

            bool varialble = true;
            ///////////////////////////////////////Delete PArent Frequency task 
            DatabaseEntities de = new DatabaseEntities();
            List<Course_UserTask> Cutlist = new Course_UserTaskDAL().getCourse_UserTasksList(de).Where(x=>x.User_TaskID==_id).ToList();
            foreach(Course_UserTask item in Cutlist)
            {
                Course course = new CourseDAL().getCourseById((int)item.CourseID);
                if (course != null && course.IsScorm == 1)
                {
                    ScormCloud.Configuration = new RusticiSoftware.HostedEngine.Client.Configuration(
    "https://cloud.scorm.com/EngineWebServices",
    ProjectVaraiables.ScormAppId,
    ProjectVaraiables.ScormSecretKey,
       ProjectVaraiables.ScormOrigin);
                    try
                    {
                        ScormCloud.RegistrationService.DeleteRegistration(item.Id.ToString(), false);

                    }
                    catch (Exception ex)
                    {

                    }
                }
                item.IsActive = 0;
                new Course_UserTaskDAL().UpdateCourse_UserTask(item, de);
            }
            User_Task utask = new User_TaskDL().getUser_TaskByIdWrapper(_id, de);

            User_Task childutask = new User_TaskDL().getUser_TasksList(de).Where(x => x.ParentID == utask.Id).FirstOrDefault();
            if (utask.ParentID == null && childutask != null)
            {
                List<User_Task> list = new User_TaskDL().getUser_TasksList(de).Where(x => x.ParentID == utask.Id).ToList();
                foreach (User_Task i in list)
                {
                    i.ParentID = childutask.Id;
                    new User_TaskDL().UpdateUser_Task(i, de);
                    de.SaveChanges();
                }
                User_Task child1 = new User_TaskDL().getUser_TaskByIdWrapper(childutask.Id, de);
                child1.ParentID = null;
                new User_TaskDL().UpdateUser_Task(child1, de);
                de.SaveChanges();


            }

            ///////////////////////////////////////////

            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }
            User_Task _User_Task = db.User_Task.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
            if (_User_Task != null)
            {
                _User_Task.IsActive = 0;
                foreach (var item in _User_Task.TaskComments.Where(x => x.IsActive == 1).ToList())
                {
                    new TaskCommentDAL().DeleteTaskComment(item.Id, db);

                }

                db.Entry(_User_Task).State = System.Data.Entity.EntityState.Modified;

            }
            if (!varialble)
            {
                db.SaveChanges();
            }
        }
        #endregion

    }
}