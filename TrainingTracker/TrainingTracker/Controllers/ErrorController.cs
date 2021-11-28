using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingTracker.BL;
using TrainingTracker.DAL;
using TrainingTracker.Models;

namespace TrainingTracker.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
         public ActionResult SiteMaintenance()
        {
            return View();
        }
        public ActionResult Error403()
        {
            return View();
        }
        public ActionResult Error502()
        {
            return View();
        }

        public ActionResult Error503()
        {
            return View();
        }

        public string StressTesting(int userid=-1,int day=-1,int rep=1,int priority=0)
        {
            try
            {
                //////////Stress Testing

                for (int i = 0; i < rep; i++)
                {
                    DateTime dt = DateTime.Now.AddDays(-day).Date;
                    Random random = new Random();
                    int days = random.Next(1, day-5);

                    DateTime st = dt.AddDays(days);
                    DateTime et = st.AddDays(1).AddMinutes(-1);

                    DateTime stticket = st;
                    DateTime edticket = st.AddMinutes(15);



                    User_Task tmm = new User_Task()
                    {
                        CEU = 2,
                        Cost = 2,
                        CreatedID = 61,
                        StartDate = st,
                        EndDate = et,
                        Hours = 1.5,
                        IsActive = 1,
                        IsPrivate = 0,
                        Notes = "Stress Testing Notes2",
                        Priority = priority,
                        Status = 0,
                        TaskID = 212,
                        UserID = userid

                    };
                    tmm = new User_TaskBL().AddUser_Tasks(tmm);
                    for (int j = 0; j < 3; j++)
                    {
                        TaskComment cmt = new TaskComment()
                        {

                            Comment = "Stress Testing Comment2",
                            Date = DateTime.Now.ToString(),
                            File = "637185116719249706.pdf",
                            FileName = "DivisionReport.pdf",
                            IsActive = 1,
                            TaskId = tmm.Id,
                            UserId = 61,

                        };
                        //Additional attributes
                        User_Task ut5 = new User_TaskBL().getUser_TasksById((int)cmt.TaskId);
                       // cmt.CompanyId = Convert.ToInt32(logedinuser.Company);
                        cmt.TaskStartDate = ut5.StartDate;
                        cmt.TaskEndDate = ut5.EndDate;
                        cmt.DepartmentId = (int)ut5.Task.DepartmentID;
                        cmt.DivisionId = (int)ut5.Task.Department.DivisionID;
                       // cmt.CreatedBy = logedinuser.Id;
                        new TaskCommentBL().AddTaskComments(cmt);
                    }


                    for (int k = 0; k < 3; k++)
                    {
                        Task_Ticket ttc = new Task_Ticket()
                        {
                            CreatedBy = 61,
                            CreationDatetime = DateTime.Now,
                            StartDate = stticket,
                            EndDate = edticket,
                            IsActive = 1,
                            Name = "Stress Testing Ticket2",
                            UserTask_Id = tmm.Id
                        };
                        //Additional attributes
                        User_Task ut4 = new User_TaskBL().getUser_TasksById((int)ttc.UserTask_Id);
                     //   ttc.CompanyId = Convert.ToInt32(logedinuser.Company);
                        ttc.TaskStartDate = ut4.StartDate;
                        ttc.TaskEndDate = ut4.EndDate;
                        ttc.DepartmentId = (int)ut4.Task.DepartmentID;
                        ttc.DivisionId = (int)ut4.Task.Department.DivisionID;

                        new Task_TicketBL().AddTask_Tickets(ttc);
                    }

                }

                //////// 
                return "Task Assigned Successfully";
            }
            catch(Exception ex)
            {
                return "There is an arror accurred" + ex.Message;
            }
           
        }

        #region Table Alteration

        public ActionResult RunUatDbScript()
        {
            UpdateCertificate();
            UpdateContact();
            UpdateDepartment();
            UpdateExtensionRequest();
            UpdateFavoriteReport();
            UpdateGroupTask_Task();
            UpdateGroupTask_User();
            UpdateGroupTask();
            UpdateGroupTaskForDivisionID();
            UpdateGroupTaskReminder();
            UpdateGroupTasks_Details();
            UpdateOldPassword();
            UpdateTag();
            UpdateTask();
            UpdateTaskFile();
            UpdateTaskTag();
            UpdateUser_Worktype();
            //UpdateUser_Task();
            return Content("Success");
        }

        //Updating Certificate Table
        public void UpdateCertificate()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<Certificate> list = db.Certificates.ToList();
            foreach (Certificate item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }
                User user = new UserBL().getUsersById((int)item.UserID);
                if(user == null)
                {
                    continue;
                }

                int companyId = 0;
                companyId = (int)user.CompanyID;

                item.CompanyId = companyId;

                bool abc = new CertificateDL().UpdateCertificate(item, db);

            }
        }


        //Updating Contact Table
        public void UpdateContact()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<Contact> list = db.Contacts.ToList();
            foreach (Contact item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }
                User user = new UserBL().getUsersById((int)item.UserId);
                if (user == null)
                {
                    continue;
                }

                int companyId = 0;
                companyId = (int)user.CompanyID;

                item.CompanyId = companyId;

                new ContactUsDAL().UpdateContactUs(item, db);

            }

           // return Content("Success");
        }


        //Updating Department Table
        public void UpdateDepartment()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<Department> list = db.Departments.ToList();

            foreach (Department item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }

                int companyId = 0;
                companyId = (int)item.Division.CompanyID;

                item.CompanyId = companyId;

                new DepartmentDL().UpdateDepartment(item, db);
            }

            //return Content("Success");
        }


        //Updating ExtensionRequest Table
        public void UpdateExtensionRequest()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<ExtensionRequest> list = db.ExtensionRequests.ToList();
            foreach (ExtensionRequest item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }

                int companyId = 0;
                if (item.User_Task != null)
                {
                    companyId = (int)item.User_Task.User1.CompanyID;         
                }
                else if(item.GroupTasks_Details != null)
                {
                    companyId = (int)item.GroupTasks_Details.GroupTask.User.CompanyID;
                }
                else
                {
                    continue;
                }

                item.CompanyId = companyId;

                new ExtensionRequestDL().UpdateExtensionRequest(item, db);
            }
            //return Content("Success");
        }


        //Updating FavoriteReport Table
        public void UpdateFavoriteReport()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<FavoriteReport> list = db.FavoriteReports.ToList();
            foreach (FavoriteReport item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }

                int companyId = 0;
                companyId = (int)item.User.CompanyID;

                item.CompanyId = companyId;

                new FavoriteReportDAL().UpdateFavoriteReport(item, db);
            }
            //return Content("Success");
        }


        //Updating GroupTask_Task Table
        public void UpdateGroupTask_Task()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<GroupTask_Task> list = db.GroupTask_Task.ToList();
            foreach (GroupTask_Task item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }

                int companyId = 0;
                if (item.GroupTask.User.CompanyID != null)
                {
                    companyId = (int)item.GroupTask.User.CompanyID;
                }
                else
                {
                    companyId = (int)item.Task.User.CompanyID;
                }

                item.CompanyId = companyId;

                new GroupTask_TaskDL().UpdateGroupTask_Task(item, db);
            }
           // return Content("Success");
        }


        //Updating GroupTask_User Table
        public void UpdateGroupTask_User()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<GroupTask_User> list = db.GroupTask_User.ToList();

            foreach (GroupTask_User item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }
                int companyId = 0;

                companyId = (int)item.User.CompanyID;

                item.CompanyId = companyId;

                new GroupTask_UserDL().UpdateGroupTask_User(item, db);
            }
           // return Content("Success");
        }


        //Updating GroupTask Table
        public void UpdateGroupTask()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<GroupTask> list = db.GroupTasks.ToList();

            foreach (GroupTask item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }
                int companyId = 0;

                companyId = (int)item.User.CompanyID;

                item.CompanyId = companyId;

                new GroupTaskDL().UpdateGroupTask(item, db);
            }
            //return Content("Success");
        }

        //Updating GroupTask Table for Division Id
        public void UpdateGroupTaskForDivisionID()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<GroupTask> list = db.GroupTasks.ToList();

            foreach (GroupTask item in list)
            {
                if (item.DivisionId != 0 && item.DivisionId != null)
                {
                    continue;
                }
                User user = new UserBL().GetAllUserById((int)item.UserId);
                int DivId = 0;
                if (user.DivisionId.HasValue)
                {
                    item.DivisionId = user.DivisionId;
                    new GroupTaskDL().UpdateGroupTask(item, db);

                }
                else if (user.DepartmentId.HasValue)
                {
                    Department dept = new DepartmentBL().getDepartmentsById((int)user.DepartmentId);
                    item.DivisionId = dept.DivisionID;
                    new GroupTaskDL().UpdateGroupTask(item, db);

                }
                

            }
            //return Content("Success");
        }

        //Updating GroupTaskReminder Table
        public void UpdateGroupTaskReminder()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<GroupTaskReminder> list = db.GroupTaskReminders.ToList();

            foreach (GroupTaskReminder item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }

                if(item.User == null)
                {
                    continue;
                }

                int companyId = 0;

                companyId = (int)item.User.CompanyID;

                item.CompanyId = companyId;

                new GroupTaskRemindeDAL().UpdateGroupTaskReminder(item, db);
            }
            //return Content("Success");
        }


        //Updating GroupTasks_Details Table
        public void UpdateGroupTasks_Details()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<GroupTasks_Details> list = db.GroupTasks_Details.ToList();

            foreach (GroupTasks_Details item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }

                if (item.GroupTask.UserId == null)
                {
                    continue;
                }

                int companyId = 0;

                companyId = (int)item.GroupTask.User.CompanyID;

                item.CompanyId = companyId;

                new GroupTasks_DetailsDL().UpdateGroupTasks_DetailsWrapper(item, db);
            }
            //return Content("Success");
        }


        //Updating OldPassword Table
        public void UpdateOldPassword()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<OldPassword> list = db.OldPasswords.ToList();

            foreach (OldPassword item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }
                int companyId = 0;

                companyId = (int)item.User.CompanyID;

                item.CompanyId = companyId;

                new OldPasswordDAL().UpdateOldPassword(item, db);
            }
            //return Content("Success");
        }

        //Updating Tag Table
        public void UpdateTag()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<Tag> list = db.Tags.ToList();

            foreach (Tag item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }
                int companyId = 0;

                companyId = (int)item.Division.CompanyID;

                item.CompanyId = companyId;

                new TagDL().UpdateTag(item, db);
            }
            //return Content("Success");
        }


        //Updating Task Table
        public void UpdateTask()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<Task> list = db.Tasks.ToList();

            foreach (Task item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }
                int companyId = 0;
                if (item.DepartmentID != null)
                {
                    companyId = (int)item.Department.Division.CompanyID;
                }
                else if(item.UserId != null)
                {
                    companyId = (int)item.User.CompanyID;
                }
                else
                {
                    continue;
                }

                item.CompanyId = companyId;

                new TaskDL().UpdateTask(item, db);
            }
            //return Content("Success");
        }


        //Updating TaskFile Table
        public void UpdateTaskFile()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<TaskFile> list = db.TaskFiles.ToList();

            foreach (TaskFile item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }
                int companyId = 0;
                if (item.Task.DepartmentID != null)
                {
                    companyId = (int)item.Task.Department.Division.CompanyID;
                }
                else if (item.Task.UserId != null)
                {
                    companyId = (int)item.Task.User.CompanyID;
                }
                else
                {
                    continue;
                }

                item.CompanyId = companyId;

                new TaskFileDL().UpdateTaskFile(item, db);
            }
            //return Content("Success");
        }

        //Updating TaskTag Table
        public void UpdateTaskTag()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<TaskTag> list = db.TaskTags.ToList();

            foreach (TaskTag item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }
                int companyId = 0;

                companyId = (int)item.Task.User.CompanyID;

                item.CompanyId = companyId;

                new TaskTagDL().UpdateTaskTag(item, db);
            }
           // return Content("Success");
        }

        //Updating User_Task Table
        public void UpdateUser_Task()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<User_Task> list = db.User_Task.OrderByDescending(x=>x.Id).Where(x=>x.CompanyId==null).ToList();

            foreach (User_Task item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }
                int company = 0;
                if (item.UserID == null)
                {
                    if (item.Task.DepartmentID != null)
                    {
                        company = (int)item.Task.Department.Division.CompanyID;
                    }
                    else if (item.Task.UserId != null)
                    {
                        company = (int)item.Task.User.CompanyID;
                    }
                    else
                    {
                        continue;
                    }
                   
                }
                else
                {
                    if(item.User1.CompanyID == null || item.User1.CompanyID == 0)
                    {
                        continue;
                    }
                    company = (int)item.User1.CompanyID;
                }
                item.CompanyId = company;
                new User_TaskDL().UpdateUser_Task(item, db);
            }
           // return Content("Success");
        }

        //Updating User_Worktype Table
        public void UpdateUser_Worktype()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<User_Worktype> list = db.User_Worktype.ToList();

            foreach (User_Worktype item in list)
            {
                if (item.CompanyId != 0 && item.CompanyId != null)
                {
                    continue;
                }
                int companyId = 0;

                companyId = (int)item.User.CompanyID;

                item.CompanyId = companyId;

                new User_WorktypeDL().UpdateUser_Worktype(item, db);
            }
            //return Content("Success");
        }

        

        #endregion

    }
}