using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;
using TrainingTracker.BL;


namespace TrainingTracker.DAL
{

  public class UserDL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        #region User
        public List<User> getActiveandInvitedUser()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<User> Users = db.Users.Where(x => x.IsZuptuSuperAdminUser != 1 && x.IsActive == 1 || x.IsActive == 2).ToList();

            return Users;
        }

        public List<User> getUsersList()
        {
            int sessionId = Convert.ToInt32(logedinuser.Company);
            DatabaseEntities db = new DatabaseEntities();
            List<User> Users = db.Users.Where(x=> x.IsZuptuSuperAdminUser != 1 && x.IsActive==1 && x.CompanyID==sessionId).ToList();

            return Users;
        }

        public List<User> getUsersListActiveandInactiveofCompany()
        {
            int sessionId = Convert.ToInt32(logedinuser.Company);
            DatabaseEntities db = new DatabaseEntities();
            List<User> Users = db.Users.Where(x => x.IsZuptuSuperAdminUser != 1 && (x.IsActive == 1 || x.IsActive == 2) && x.CompanyID == sessionId).ToList();

            return Users;
        }
        public List<User> getInActiveUsersofCompany()
        {
            int sessionId = Convert.ToInt32(logedinuser.Company);
            DatabaseEntities db = new DatabaseEntities();
            List<User> Users = db.Users.Where(x => x.IsZuptuSuperAdminUser != 1 && (x.IsActive == 0 ) && x.CompanyID == sessionId).ToList();

            return Users;
        }


        public List<User> getFogetPwdUsersList()
        {
           // int sessionId = (int)(int)HttpContext.Current.Session["Company"];
            DatabaseEntities db = new DatabaseEntities();
            List<User> Users = db.Users.Where(x => x.IsZuptuSuperAdminUser != 1 && x.IsActive == 1).ToList();

            return Users;
        }

        public List<User> getTraineesList()
        {
            int sessionId = Convert.ToInt32(logedinuser.Company);

            DatabaseEntities db = new DatabaseEntities();
            List<User> Users = db.Users.Where(x => x.IsZuptuSuperAdminUser != 1 && x.IsActive == 1 && x.Role==3 && x.CompanyID==sessionId).ToList();

            return Users;
        }
        public List<User> getManagerList()
        {
            int sessionId = Convert.ToInt32(logedinuser.Company);
            DatabaseEntities db = new DatabaseEntities();
            List<User> Users = db.Users.Where(x => x.IsZuptuSuperAdminUser != 1 && x.IsActive == 1 && (x.Role == 2 || x.Role==4) && x.CompanyID==sessionId).ToList();

            return Users;
        }
        public List<User> getAdminList()
        {
            int sessionId = Convert.ToInt32(logedinuser.Company);
            DatabaseEntities db = new DatabaseEntities();
            List<User> Users = db.Users.Where(x => x.IsZuptuSuperAdminUser != 1 && x.IsActive == 1 && x.Role == 1 && x.CompanyID==sessionId).ToList();

            return Users;
        }
        public List<User> getAllUsersList()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<User> Users = db.Users.Where(x=> x.IsZuptuSuperAdminUser != 1).ToList();

            return Users;
        }
        public List<User> getAllUsersForLogin()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<User> Users = db.Users.ToList();

            return Users;
        }
        public List<User> getAllActiveUsersList()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<User> Users = db.Users.Where(x=> x.IsZuptuSuperAdminUser != 1 && x.IsActive==1).ToList();

            return Users;
        }


        public User getUserById(int _Id, DatabaseEntities de=null)
        {
           
            User _User;
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();

                _User = db.Users.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
                //_User = db.Users.FirstOrDefault(x => x.Id == _Id);

            }
            else
            {
                _User = de.Users.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
                //_User = de.Users.FirstOrDefault(x => x.Id == _Id);

            }
            return _User;
        }
         
        public User GetAllUserById(int _Id, DatabaseEntities de = null)
        {
            User _User;
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();
                _User = db.Users.FirstOrDefault(x => x.Id == _Id);
            }
            else
            {
                _User = de.Users.FirstOrDefault(x => x.Id == _Id);
            }
            return _User;
        }

        public User getDeletedUserById(int _Id, DatabaseEntities de=null)
        {
           
            User _User;
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();
                
                    _User = db.Users.FirstOrDefault(x => x.Id == _Id && x.IsActive == 0);
                
            }
            else
            {
                _User = de.Users.FirstOrDefault(x => x.Id == _Id && x.IsActive == 0);

            }
            return _User;
        }
        public User getInActiveUserById(int _Id, DatabaseEntities de = null)
        {

            User _User;
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();

                _User = db.Users.FirstOrDefault(x => x.Id == _Id && x.IsActive == 0);

            }
            else
            {
                _User = de.Users.FirstOrDefault(x => x.Id == _Id && x.IsActive == 0);

            }
            return _User;
        }
        public User getUserByIdInactive(int _Id, DatabaseEntities de = null)
        {

            User _User;
            if (de == null)
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    _User = db.Users.FirstOrDefault(x => x.Id == _Id && x.IsActive == 2);
                }
            else
            {
                _User = de.Users.FirstOrDefault(x => x.Id == _Id && x.IsActive == 2);

            }
            return _User;
        }

        public bool AddUser(User _User)
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Users.Add(_User);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateUser(User _User, DatabaseEntities de= null)
        {
            if (de == null) { 
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_User).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                de.Entry(_User).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            return true;
        }

        public void DeleteUser(int _id, DatabaseEntities de=null)
        {
            if(de ==null)
            using (DatabaseEntities db = new DatabaseEntities())
            {
                    List<User_Task> uTask = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == _id).ToList();
                    List<GroupTask_User> GrpUser = new GroupTask_UserBL().getGroupTask_UsersList().Where(x => x.UserId == _id).ToList();
                    List<TaskComment> taskcomment = new TaskCommentBL().getTaskCommentsList().Where(x => x.UserId == _id).ToList();
                    List<GroupTaskComment> grpcomment = new GroupTaskCommentBL().getGroupTaskCommentsList().Where(x => x.GroupTaskUserId == _id || x.UserId == _id).ToList();
                    List<User_Worktype> userWorkertypes = new User_WorktypeDL().getUser_WorktypesList().Where(x => x.UserId == _id && x.IsActive == 1).ToList();

                    foreach (TaskComment tas in taskcomment)
                    {
                        new TaskCommentBL().DeleteTaskComments(tas.Id);
                    }
                    foreach (User_Task us in uTask)
                    {
                        new User_TaskBL().DeleteUser_Tasks(us.Id);
                    }
                    foreach (GroupTaskComment grpcom in grpcomment)
                    {
                        new GroupTaskCommentBL().DeleteGroupTaskComments(grpcom.Id);
                    }
                    foreach (GroupTask_User grpt in GrpUser)
                    {
                        new GroupTask_UserBL().DeleteGroupTask_Users(grpt.Id);
                    }
                    
                    foreach(User_Worktype item in userWorkertypes)
                    {
                        //User_Worktype Uwt = new User_WorktypeDL().getUser_WorktypeById(item.Id);
                        new User_WorktypeDL().DeleteUser_Worktype(item.Id);
                    }
                    


                    User u = db.Users.FirstOrDefault(x => x.Id == _id && ( x.IsActive == 1 || x.IsActive==2));
                    if (u != null)
                    {
                      
                        u.IsActive = 0;
                        UpdateUser(u, db);
                    }
                }
            else
            {
                User u = getUserById(_id, de);
                if (u != null)
                {
                    u.IsActive = 0;
                    UpdateUser(u, de);
                }
                

            }
        }

        public void ActiveDeletedUser(int _id, DatabaseEntities de = null)
        {
            if (de == null)
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    List<User_Task> uTask = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == _id).ToList();
                    List<GroupTask_User> GrpUser = new GroupTask_UserBL().getGroupTask_UsersList().Where(x => x.UserId == _id).ToList();
                    List<TaskComment> taskcomment = new TaskCommentBL().getTaskCommentsList().Where(x => x.UserId == _id).ToList();
                    List<GroupTaskComment> grpcomment = new GroupTaskCommentBL().getGroupTaskCommentsList().Where(x => x.GroupTaskUserId == _id || x.UserId == _id).ToList();


                    foreach (TaskComment tas in taskcomment)
                    {
                        new TaskCommentBL().DeleteTaskComments(tas.Id);
                    }
                    foreach (User_Task us in uTask)
                    {
                        new User_TaskBL().DeleteUser_Tasks(us.Id);
                    }
                    foreach (GroupTaskComment grpcom in grpcomment)
                    {
                        new GroupTaskCommentBL().DeleteGroupTaskComments(grpcom.Id);
                    }
                    foreach (GroupTask_User grpt in GrpUser)
                    {
                        new GroupTask_UserBL().DeleteGroupTask_Users(grpt.Id);
                    }




                    User u = db.Users.FirstOrDefault(x => x.Id == _id && (x.IsActive == 1 || x.IsActive == 2));
                    if (u != null)
                    {

                        u.IsActive = 0;
                        UpdateUser(u, db);
                    }
                }
            else
            {
                User u = getUserById(_id, de);
                u.IsActive = 0;
                UpdateUser(u, de);
            }
        }
        #endregion

    }
}