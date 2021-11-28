using TrainingTracker.HelpingClasses.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using TrainingTracker.BL;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Api2PdfLibrary;
using System.Threading;

namespace TrainingTracker.Controllers
{
    public class ExportReportController : Controller
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        MainMailClass errormail = new MainMailClass();

        public void clearCache()
        {
            var oldCtx = Request.GetOwinContext();
            var oldAuthManager = oldCtx.Authentication;
            oldAuthManager.SignOut("ApplicationCookie");

        }
        public bool AuthenticateUser()
        {

            if (logedinuser.Name == null)
            {
                clearCache();
                return false;
            }

            Models.User user = new UserBL().getUsersById(logedinuser.Id);

            if (user == null)
            {
                clearCache();
                return false;
            }

            if (logedinuser.Role != user.Role)
            {
                clearCache();
                return false;
            }

            return true;
        }

        public ActionResult Error()
        {
            return View();
        }
        // GET: ExportReport
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }
        #region Excel
        /// <summary>
        /// This method is to download the excel sheet with all the employees of the given query.
        /// The reason why these queries are implemented here and not in the separate class is because
        /// they have specific 
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="departname"></param>
        /// <param name="divisionName"></param>
        /// <returns></returns>
        public ActionResult ExportEmployees(string fname = "", string lname = "", string departname = "", string divisionName = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                List<Department> departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();
                List<User> trainee = new List<User>();
                int adminid = logedinuser.Role;

                if (adminid == (int)Enums.Role.Admin)
                {
                    trainee = new UserBL().getTraineesList().OrderBy(s => s.FirstName).ToList();
                }
                else if (adminid == (int)Enums.Role.Manager || adminid == (int)Enums.Role.Cordinator)
                {
                    int id = logedinuser.Id;
                    User manager = new UserBL().getUsersById(id);
                    if (manager.DivisionId.HasValue)
                    {
                        int managerDivisionId = manager.DivisionId.Value;
                        trainee = new UserBL().getTraineesList().Where(x => x.Department.DivisionID == manager.DivisionId).OrderBy(s => s.FirstName).ToList();
                    }
                    if (manager.DivisionId != null)
                        departments = departments.Where(x => x.DivisionID == manager.DivisionId).OrderBy(s => s.Name).ToList();
                }

                if (fname != "")
                {
                    trainee = trainee.Where(x => x.FirstName.ToUpper().Contains(fname.ToUpper())).OrderBy(s => s.FirstName).ToList();
                }
                if (lname != "")
                {
                    trainee = trainee.Where(x => x.LastName.ToUpper().Contains(lname.ToUpper())).OrderBy(s => s.FirstName).ToList();
                }
                if (divisionName != "")
                {
                    trainee = trainee.Where(x => x.Department != null && (x.Department.Division.Name.ToUpper().Contains(divisionName.ToUpper()))).OrderBy(s => s.FirstName).ToList();
                }
                if (departname != "AllEmpkck")
                    if (departname != "")
                    {
                        trainee = trainee.Where(x => x.Department != null && (x.Department.Name.ToUpper().Contains(departname.ToUpper()))).OrderBy(s => s.FirstName).ToList();
                    }

                string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Employee" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
                ExcelManagement.generateGenericExcelFile(path, trainee);
                return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employee" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }

        }
        public ActionResult ExportManagers()
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                int adminid = logedinuser.Role;
                if (adminid == (int)Enums.Role.Admin)
                {
                    string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Manager" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
                    ExcelManagement.generateGenericExcelFile(path, null, new UserBL().getManagerList());
                    return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Manager" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");
                }
                return null;
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }
        public List<string> ExportEmployeesTask(string startDate = "", string endDate = "", int tag = -1, int department = -1, int division = 0, string taskName = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int priority = -1, int completionStatus = -1, int sd=0,int ed=0,int cd=0,string DivDepID="",string empselftask="", int isCompleted = -1)
        {

            if (AuthenticateUser() == false)
            {
              //  return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
            }
            List<string> objlist = new List<string>();
            // objlist.Add(logedinuser.Company);
            objlist.Add(logedinuser.Id.ToString());
            User manager = new UserBL().getUsersById(logedinuser.Id);
            List<User_Task> tasks = new List<User_Task>();
            if (isCompleted != -1)
            {
                tasks = new User_TaskBL().getUser_TasksList().Where(x => x.CompletionDate != null).ToList();
            }
            else
            {
                tasks = new User_TaskBL().getUser_TasksList();
            }

            if (DivDepID != "")
            {
                // tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Role == 3 && (x.IsPrivate == 0 || x.UserID == manager.Id)).ToList();
                tasks=tasks.Where(x => x.DepartmentID == DivDepID || x.DivisionID == DivDepID && x.IsPrivate == 0 && x.User1!=null).ToList();
                tasks = tasks.Where(x => x.User1.Role != 1).ToList();
                // tasks = new User_TaskBL().getUser_TasksList().Where(x => (x.DepartmentID == DivDepID || x.DivisionID == DivDepID) && x.IsPrivate == 0).ToList();

                //tasks = new User_TaskBL().getUser_TasksList().Where(x => (x.DepartmentID == DivDepID || x.DivisionID == DivDepID) && x.IsPrivate == 0).ToList();

            }
            else if (empselftask!="")
            {
                //tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == logedinuser.Id).ToList();
                tasks = tasks.Where(x => x.User1.Id == logedinuser.Id).ToList();

            }
            else
            {
                //tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Role == 3 && (x.IsPrivate == 0 || x.UserID == manager.Id)).ToList();
                tasks = tasks.Where(x => x.User1.Role == 3 && (x.IsPrivate == 0 || x.UserID == manager.Id)).ToList();
            }


            string complettionName = string.Empty;
            if (logedinuser.Role  == 2 || logedinuser.Role ==4)
            {
                tasks = tasks.Where(x => x.Task.Department.DivisionID == manager.DivisionId).ToList();
            }
            else if(logedinuser.Role == 3)
            {
                //tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Role == 3 && (x.IsPrivate == 0 && x.UserID == manager.Id)).ToList();
                tasks = tasks.Where(x => x.User1.Role == 3 && (x.IsPrivate == 0 && x.UserID == manager.Id)).ToList();

            }
            if (sd == 1)
            {
                if (startDate != null && startDate != "")
                {
                    tasks = tasks.Where(x => x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))).ToList();
                 }

                if (endDate != null && endDate != "")
                {
                    tasks = tasks.Where(x => x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))).ToList();
                     }

                ViewBag.sd = 1;
            }
            if (ed==1)
            {
                if (startDate != "" && startDate != "")
                {
                    tasks = tasks.Where(x => x.EndDate != null && (x.EndDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    tasks = tasks.Where(x => x.EndDate != null && (x.EndDate < DateTime.Parse(endDate))).ToList();
                 }
                ViewBag.ed = 1;
            }
            if (cd==1)
            {
                if (startDate != null && startDate != "")
                {
                    tasks = tasks.Where(x => x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    tasks = tasks.Where(x => x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.cd = 1;
            }
            if (taskType != 0)
            {
                tasks = tasks.Where(x => x.Task.TaskTypeID == taskType).ToList();
             }
            if (department != -1)
            {
                tasks = tasks.Where(x => x.User1.DepartmentId != null && x.User1.DepartmentId == department).ToList();
            }
            if (tag != -1)
            {
                tasks = tasks.Where(x => x.Task.TaskTags.Where(z => z.TagId == tag).Count() > 0).ToList();
           }

            if (taskName != "")
            {
                tasks = tasks.Where(x => x.Task.Name.ToUpper().Contains(taskName.ToUpper())).ToList();
             }
            if (fname != "")
            {
                tasks = tasks.Where(x => x.User1.FirstName.ToUpper().Contains(fname.ToUpper())).ToList();
             }
            if (lname != "")
            {
                tasks = tasks.Where(x => x.User1.LastName.ToUpper().Contains(lname.ToUpper())).ToList();
            }
            if (status != -1)
            {
                tasks = tasks.Where(x => x.Status == status).ToList();
             }
            if (priority != -1)
            {
                tasks = tasks.Where(x => x.Priority == priority).ToList();
            }
            if (completionStatus != -1)
            {
                if (completionStatus == 2)
                {
                    tasks = tasks.Where(x => x.CompletionDate != null).ToList();
                    complettionName = "Completed";
                }
                else if (completionStatus == 3)
                {
                    tasks = tasks.Where(x => x.EndDate < DateTime.Now && x.CompletionDate == null).ToList();
                    complettionName = "Late";
                }
                else if (completionStatus == 1)
                {
                    tasks = tasks.Where(x => x.CompletionDate == null).ToList();
                    complettionName = "In Process";
                }
            }

            string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Employee Task" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
            ExcelManagement.generateGenericExcelFile(path, null,null,null,null,tasks);

            List<string> objlist2 = new List<string>();

            objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
            objlist.AddRange(objlist2);

            return objlist;
            // return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employee Task" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");


        }
        public List<string> ExportManagerTask(string startDate = "", string endDate = "", int tag = -1, string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, string fname = "", string lname = "", int division = -1, int isCompleted = -1)
        {
            if (AuthenticateUser() == false)
            {
              //  return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
            }
            int role = logedinuser.Role;
            List<string> objlist = new List<string>();
            // objlist.Add(logedinuser.Company);
            objlist.Add(logedinuser.Id.ToString());
            User manager = new UserBL().getUsersById(logedinuser.Id);
            List<User_Task> tasks = new List<User_Task>();
            

            if (isCompleted != -1)
            {
                tasks = new User_TaskBL().getUser_TasksList().Where(x => x.IsActive == 1 && x.CompletionDate != null && (x.User1.Role == 2 || x.User1.Role == 4)).ToList();
            }
            else
            {
                tasks = new User_TaskBL().getUser_TasksList().Where(x => x.IsActive == 1 && (x.User1.Role == 2 || x.User1.Role == 4)).ToList();
            }
            ViewBag.sd = 0;
            ViewBag.ed = 0;
            ViewBag.cd = 0;
            string complettionName = string.Empty;
            int id = logedinuser.Id;
            if (role == 3)
            {
              //  return Content("Unauthorised Access");
            }
            else if (role == 2 || role == 4)
            {
                tasks = tasks.Where(x => x.UserID == id).ToList();
            }
            else
            {
                tasks = tasks.Where(x => x.IsPrivate == 0).ToList();
            }

            if (sd != 0)
            {
                if (startDate != "")
                {
                    tasks = tasks.Where(x => x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != "")
                {
                    tasks = tasks.Where(x => x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))).ToList();
                }

                ViewBag.sd = 1;
            }
            if (ed != 0)
            {
                if (startDate != "")
                {
                    tasks = tasks.Where(x => x.EndDate != null && (x.EndDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != "")
                {
                    tasks = tasks.Where(x => x.EndDate != null && (x.EndDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.ed = 1;
            }
            if (cd != 0)
            {
                if (startDate != "")
                {
                    tasks = tasks.Where(x => x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != "")
                {
                    tasks = tasks.Where(x => x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.cd = 1;
            }

            if (fname != "")
            {
                tasks = tasks.Where(x => x.User1.FirstName.ToUpper().Contains(fname.ToUpper())).ToList();
            }
            if (lname != "")
            {
                tasks = tasks.Where(x => x.User1.LastName.ToUpper().Contains(lname.ToUpper())).ToList();
            }
            if (division != -1)
            {
                tasks = tasks.Where(x => x.User1.DivisionId != null && x.User1.DivisionId == division).ToList();
            }


            if (taskType != 0)
            {
                tasks = tasks.Where(x => x.Task.TaskTypeID == taskType).ToList();
            }

            if (name != "")
            {
                tasks = tasks.Where(x => x.Task.Name.ToUpper().Contains(name.ToUpper())).ToList();
            }

            if (tag != -1)
            {
                tasks = tasks.Where(x => x.Task.TaskTags.Where(z => z.TagId == tag).Count() > 0).ToList();
            }

            if (status != -1)
            {
                tasks = tasks.Where(x => x.Status == status).ToList();
            }
            if (priority != -1)
            {
                tasks = tasks.Where(x => x.Priority == priority).ToList();
            }
            if (completionStatus != -1)
            {
                if (completionStatus == 2)
                {
                    tasks = tasks.Where(x => x.CompletionDate != null).ToList();
                    complettionName = "Completed";
                }
                else if (completionStatus == 3)
                {
                    tasks = tasks.Where(x => x.EndDate < DateTime.Now && x.CompletionDate == null).ToList();
                    complettionName = "Late";
                }
                else if (completionStatus == 1)
                {
                    tasks = tasks.Where(x => x.CompletionDate == null && x.EndDate > DateTime.Now).ToList();
                    complettionName = "In Process";
                }
            }
            string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Manager Task" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
            ExcelManagement.generateGenericExcelFile(path, null, null, null, null,null,tasks,null);

            List<string> objlist2 = new List<string>();

            objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
            objlist.AddRange(objlist2);

            return objlist;
            // return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Manager Task" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");


        }
        public List<string> ExportAdminTask(string v="", string startDate = "", string endDate = "", int tag = -1, string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, int isCompleted = -1)
        {
            if (AuthenticateUser() == false)
            {
               // return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
            }
            int role = logedinuser.Role;
            List<string> objlist = new List<string>();
            // objlist.Add(logedinuser.Company);
            objlist.Add(logedinuser.Id.ToString());
            User admin = new UserBL().getUsersById(logedinuser.Id);
            List<User_Task> tasks = new List<User_Task>();

            if(isCompleted != -1)
            {
                tasks = new User_TaskBL().getUser_TasksList().OrderByDescending(c => c.Id).Where(x => x.User1.Role == 1 && x.DivisionID == null && x.DepartmentID == null && x.CompletionDate != null).ToList();
            }
            else
            {
                tasks = new User_TaskBL().getUser_TasksList().OrderByDescending(c => c.Id).Where(x => x.User1.Role == 1 && x.DivisionID == null && x.DepartmentID == null).ToList();
            }

            ViewBag.sd = 0;
            ViewBag.ed = 0;
            ViewBag.cd = 0;
            string complettionName = string.Empty;
            int id = logedinuser.Id;
            if (role == 3)
            {
              //  return Content("Unauthorised Access");
            }
            else if (role == 2 || role == 4)
            {
               // return Content("Unauthorised Access");
            }
            else if (v != "")
            {
                if (logedinuser.IsMasterAdmin == 1)
                    tasks = tasks.Where(x => x.UserID != id).ToList();
                else
                    tasks = tasks.Where(x => x.UserID != id && x.User1.IsMasterAdmin == 0).ToList();

            }
            else
            {
                tasks = tasks.Where(x => x.UserID == id).ToList();
            }

            if (sd != 0)
            {
                if (startDate != "")
                {
                    tasks = tasks.Where(x => x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != "")
                {
                    tasks = tasks.Where(x => x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))).ToList();
                }

                ViewBag.sd = 1;
            }
            if (ed != 0)
            {
                if (startDate != "")
                {
                    tasks = tasks.Where(x => x.EndDate != null && (x.EndDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != "")
                {
                    tasks = tasks.Where(x => x.EndDate != null && (x.EndDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.ed = 1;
            }
            if (cd != 0)
            {
                if (startDate != "")
                {
                    tasks = tasks.Where(x => x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != "")
                {
                    tasks = tasks.Where(x => x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.cd = 1;
            }
            if (taskType != 0)
            {
                tasks = tasks.Where(x => x.Task.TaskTypeID == taskType).ToList();
            }

            if (name != "")
            {
                tasks = tasks.Where(x => x.Task.Name.ToUpper().Contains(name.ToUpper())).ToList();
            }

            if (tag != -1)
            {
                tasks = tasks.Where(x => x.Task.TaskTags.Where(z => z.TagId == tag).Count() > 0).ToList();
            }

            if (status != -1)
            {
                tasks = tasks.Where(x => x.Status == status).ToList();
            }
            if (priority != -1)
            {
                tasks = tasks.Where(x => x.Priority == priority).ToList();
            }
            if (completionStatus != -1)
            {
                if (completionStatus == 2)
                {
                    tasks = tasks.Where(x => x.CompletionDate != null).ToList();
                    complettionName = "Completed";
                }
                else if (completionStatus == 3)
                {
                    tasks = tasks.Where(x => x.EndDate < DateTime.Now && x.CompletionDate == null).ToList();
                    complettionName = "Late";
                }
                else if (completionStatus == 1)
                {
                    tasks = tasks.Where(x => x.CompletionDate == null && x.EndDate > DateTime.Now).ToList();
                    complettionName = "In Process";
                }
            }
            string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Admin Task" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
            ExcelManagement.generateGenericExcelFile(path, null, null, null, null, null,null, tasks);

            List<string> objlist2 = new List<string>();

            objlist2 = General_Purpose.GetBlobFilePath(path, admin.Id);
            objlist.AddRange(objlist2);

            return objlist;
            // return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Admin Task" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");


        }
        public List<string> ExportGroupTask(string startDate = "", string endDate = "", int tag = -1, int department = -1, int division = 0,int priority=-1, string taskName = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, int isCompleted = -1, int isGroupStudy=-1)
        {
            if (AuthenticateUser() == false)
            {
               // return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
            }
            List<string> objlist = new List<string>();
            // objlist.Add(logedinuser.Company);
            objlist.Add(logedinuser.Id.ToString());
            User admin = new UserBL().getUsersById(logedinuser.Id);

            List<GroupTasks_Details> gTasks = null;
            if (isCompleted == 1)
            {
                if(isGroupStudy!=-1)
                gTasks = new GroupTasks_DetailsBL().getGroupTasks_DetailssList().Where(x=>x.CompletionDate!= null && x.CourseId!=null).ToList();
                else
                gTasks = new GroupTasks_DetailsBL().getGroupTasks_DetailssList().Where(x=>x.CompletionDate!= null && x.CourseId == null).ToList();
            }
            else
            {
                if(isGroupStudy!=-1)
                gTasks = new GroupTasks_DetailsBL().getGroupTasks_DetailssList().Where(x=> x.CourseId != null && x.CompletionDate == null).ToList();
                else
                gTasks = new GroupTasks_DetailsBL().getGroupTasks_DetailssList().Where(x => x.CourseId == null && x.CompletionDate == null).ToList();
            }
            string complettionName = string.Empty;
            int id = logedinuser.Id;
            int role = logedinuser.Role;
            if (role == 3)
            {
                gTasks = gTasks.Where(x => x.GroupTask_User.Where(y => y.IsActive == 1 && y.UserId == id && y.User.Role==3).Count() > 0).ToList();
            }
            else if (role == 2 || role == 4)
            {
                gTasks = gTasks.Where(x => x.GroupTask_User.Where(y => y.IsActive == 1 && y.UserId == id && (y.User.Role==2 || y.User.Role==4)).Count() > 0 || x.CreatedBy == id).ToList();
            }

          

            if (sd==1)
            {
                if (startDate != null && startDate != "")
                {
                    gTasks = gTasks.Where(x => x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    gTasks = gTasks.Where(x => x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))).ToList();
                }

                ViewBag.sd = 1;
            }
            if (ed==1)
            {
                if (startDate != "" && startDate != "")
                {
                    gTasks = gTasks.Where(x => x.EndDate != null && (x.EndDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    gTasks = gTasks.Where(x => x.EndDate != null && (x.EndDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.ed = 1;
            }
            if (cd==1)
            {
                if (startDate != null && startDate != "")
                {
                    gTasks = gTasks.Where(x => x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    gTasks = gTasks.Where(x => x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.cd = 1;
            }

            if (fname != "")
            {
                gTasks = gTasks.Where(x => x.GroupTask_User.Where(y => y.LeadRole == 1 && y.User.FirstName.ToUpper().Contains(fname.ToUpper())).Count() > 0).ToList();
            }
            if (lname != "")
            {
                gTasks = gTasks.Where(x => x.GroupTask_User.Where(y => y.LeadRole == 1 && y.User.LastName.ToUpper().Contains(lname.ToUpper())).Count() > 0).ToList();
            }
            if (taskName != "")
            {
                gTasks = gTasks.Where(x => x.GroupTask.Name.ToUpper().Contains(taskName.ToUpper())).ToList();
            }

            if (priority != -1)
            {
                gTasks = gTasks.Where(x => x.Priority == priority).ToList();
            }


            if (tag != -1)
            {
                gTasks = gTasks.Where(x => x.GroupTask.GroupTask_Task.Where(z => z.Task.TaskTags.Where(y => y.TagId == tag).Count() > 0).Count() > 0).ToList();
            }


            if (completionStatus != -1)
            {
                if (completionStatus == 2)
                {
                    gTasks = gTasks.Where(x => x.CompletionDate != null).ToList();
                    complettionName = "Completed";
                }
                else if (completionStatus == 3)
                {
                    gTasks = gTasks.Where(x => x.EndDate < DateTime.Now && x.CompletionDate == null).ToList();
                    complettionName = "Late";
                }
                else if (completionStatus == 1)
                {
                    gTasks = gTasks.Where(x => x.CompletionDate == null).ToList();
                    complettionName = "In Process";
                }
            }

            if (status != -1)
            {
                gTasks = gTasks.Where(x => x.Status == status).ToList();
            }
        
            string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Group Task" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
            ExcelManagement.generateGenericExcelFile(path, null, null, null, gTasks, null, null,null);

            List<string> objlist2 = new List<string>();

            objlist2 = General_Purpose.GetBlobFilePath(path, admin.Id);
            objlist.AddRange(objlist2);

            return objlist;
            //return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Group Task" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");

        }
        
        public ActionResult ExportAddUserErrorMsg(List<string> ErrorMsg)
        {
            try
            {
                string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Group Task" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";

                ExcelManagement.GenerateAddUserErrorExcel(path, ErrorMsg);

                return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Error_Report(" + DateTime.Now.ToString("MM-dd-yyyy") + ").xlsx");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }


        #endregion

        #region pdf
        public List<string> ExportEmployeesPdf(string startDate = "", string endDate = "", int tag = -1, int department = -1, int division = 0, string taskName = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0,string DivDepID="",List<int> list=null,string empselftask="", int isCompleted = -1)
        {

            Log.Info("Pdf generation method called");
            if (AuthenticateUser() == false)
            {
            }
            List<string> objlist = new List<string>();
           // objlist.Add(logedinuser.Company);
            objlist.Add(logedinuser.Id.ToString());
            User manager = new UserBL().getUsersById(logedinuser.Id);


            List<User_Task> tasks = new List<User_Task>();
            if(isCompleted != -1)
            {
                tasks = new User_TaskBL().getUser_TasksList().Where(x => x.CompletionDate != null).ToList();
            }
            else
            {
                tasks = new User_TaskBL().getUser_TasksList().ToList();
            }

            if (DivDepID != "")
            {
                //tasks = new User_TaskBL().getUser_TasksList().Where(x => x.DepartmentID == DivDepID || x.DivisionID == DivDepID && x.IsPrivate == 0 && x.User1!=null).ToList();
                tasks = tasks.Where(x => x.DepartmentID == DivDepID || x.DivisionID == DivDepID && x.IsPrivate == 0 && x.User1!=null).ToList();
                tasks = tasks.Where(x => x.User1.Role != 1).ToList();
            }
            else if (empselftask != "")
            {
                //tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == logedinuser.Id).ToList();
                tasks = tasks.Where(x => x.User1.Id == logedinuser.Id).ToList();

            }
            else
            {
                //tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Role == 3 && (x.IsPrivate == 0)).ToList();
                tasks = tasks.Where(x => x.User1.Role == 3 && (x.IsPrivate == 0)).ToList();

            }
            string complettionName = string.Empty;
            if (logedinuser.Role == 2 || logedinuser.Role == 4)
            {
             
                tasks = tasks.Where(x => x.Task.Department.DivisionID == manager.DivisionId).ToList();
            }
            else if(logedinuser.Role == 3)
            {
              
                //tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Role == 3 && (x.IsPrivate == 0 && x.UserID == manager.Id)).ToList();
                tasks = tasks.Where(x => x.User1.Role == 3 && (x.IsPrivate == 0 && x.UserID == manager.Id)).ToList();

            }
            if (sd == 1)
            {
                if (startDate != null && startDate != "")
                {
                    tasks = tasks.Where(x => x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    tasks = tasks.Where(x => x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))).ToList();
                }

                ViewBag.sd = 1;
            }
            if (ed == 1)
            {
                if (startDate != "" && startDate != "")
                {
                    tasks = tasks.Where(x => x.EndDate != null && (x.EndDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    tasks = tasks.Where(x => x.EndDate != null && (x.EndDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.ed = 1;
            }
            if (cd == 1)
            {
                if (startDate != null && startDate != "")
                {
                    tasks = tasks.Where(x => x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    tasks = tasks.Where(x => x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.cd = 1;
            }
            if (taskType != 0)
            {
                tasks = tasks.Where(x => x.Task.TaskTypeID == taskType).ToList();
            }
            if (department != -1)
            {
                tasks = tasks.Where(x => x.User1.Department != null && x.User1.DepartmentId == department).ToList();
            }
            if (tag != -1)
            {
                tasks = tasks.Where(x => x.Task.TaskTags.Where(z => z.TagId == tag).Count() > 0).ToList();
            }

            if (taskName != "")
            {
                tasks = tasks.Where(x => x.Task.Name.ToUpper().Contains(taskName.ToUpper())).ToList();
            }
            if (fname != "")
            {
                tasks = tasks.Where(x => x.User1.FirstName.ToUpper().Contains(fname.ToUpper())).ToList();
            }
            if (lname != "")
            {
                tasks = tasks.Where(x => x.User1.FirstName.ToUpper().Contains(lname.ToUpper())).ToList();
            }
            if (status != -1)
            {
                tasks = tasks.Where(x => x.Status == status).ToList();
            }
            if (priority != -1)
            {
                tasks = tasks.Where(x => x.Priority == priority).ToList();
            }
            if (completionStatus != -1)
            {
                if (completionStatus == 2)
                {
                    tasks = tasks.Where(x => x.CompletionDate != null).ToList();
                    complettionName = "Completed";
                }
                else if (completionStatus == 3)
                {
                    tasks = tasks.Where(x => x.EndDate < DateTime.Now && x.CompletionDate == null).ToList();
                    complettionName = "Late";
                }
                else if (completionStatus == 1)
                {
                    tasks = tasks.Where(x => x.CompletionDate == null).ToList();
                    complettionName = "In Process";
                }
            }

            Log.Info("Pdf generation method search completed");
            string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Employee" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".pdf";
            //string t= new PDF_Export().generate_employee_assignments(tasks, path);
            // string time = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToString("MMMM dd,yyyy");

            // string htmlHeader = "<!DOCTYPE html><html><body><table style=\"width: 100%; border: 2px solid black;\"><tr><td><h2 style=\" text-align: center;\">                 Employee Assignment Report </h2></td></tr></table></body></html>";
            // string HtmlFragment = "<footer style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\"><table style=\"width:100%;\"><tr><td style=\"text-align:left;width:20%;\">" + time + "</td><td style=\"text-align:center;width:60%;\"><img src=\"\\Content\\landing_page\\images\\logo.png\" width=\"50px\" height=\"50px\" /></td><td style=\"text-align:right;width:20%;\"></td></tr></table></footer>";

            // using (MemoryStream stream = new System.IO.MemoryStream())
            // {


            //     StringReader sr = new StringReader(t);
            //     Document pdfDoc = new Document(PageSize.A4.Rotate());
            //     PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
            //     writer.PageEvent = new HtmlPageEventHelper(htmlHeader);
            //     writer.PageEvent = new HtmlPageEventHelperFooter(HtmlFragment);
            //     pdfDoc.Open();
            //     XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
            //     pdfDoc.Close();
            //     return File(stream.ToArray(), "application/pdf", "Employee.pdf");

            // }
            new PDF_Export().generate_manager_assignments(tasks, path, "Employee", 3);

            // return File(path, MediaTypeNames.Text.Plain, "Employee  Assignments.pdf");
           
            List<string> objlist2 = new List<string>();
            
              objlist2=  General_Purpose.GetBlobFilePath(path,manager.Id);
            objlist.AddRange(objlist2);

            return objlist;
        }
        public List<string> ExportManagerPdf(string startDate = "", string endDate = "", int tag = -1, string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, string fname = "", string lname = "", int division = -1, int isCompleted = -1)
        {
            if (AuthenticateUser() == false)
            {
               // return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
            }
            int role = logedinuser.Role;
            List<string> objlist = new List<string>();
            // objlist.Add(logedinuser.Company);
            objlist.Add(logedinuser.Id.ToString());
            User manager = new UserBL().getUsersById(logedinuser.Id);
            List<User_Task> tasks = new List<User_Task>();

            if (isCompleted != -1)
            {
                tasks = new User_TaskBL().getUser_TasksList().Where(x => x.IsActive == 1 && x.CompletionDate != null && (x.User1.Role == 2 || x.User1.Role == 4)).ToList();
            }
            else
            {
                tasks = new User_TaskBL().getUser_TasksList().Where(x => x.IsActive == 1 && (x.User1.Role == 2 || x.User1.Role == 4)).ToList();
            }
            ViewBag.sd = 0;
            ViewBag.ed = 0;
            ViewBag.cd = 0;
            string complettionName = string.Empty;
            int id = logedinuser.Id;
            if (role == 3)
            {
               // return Content("Unauthorised Access");
            }
            else if (role == 2 || role == 4)
            {
                tasks = tasks.Where(x => x.UserID == id).ToList();
            }
            else
            {
                tasks = tasks.Where(x => x.IsPrivate == 0).ToList();
            }

            if (sd != 0)
            {
                if (startDate != "")
                {
                    tasks = tasks.Where(x => x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != "")
                {
                    tasks = tasks.Where(x => x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))).ToList();
                }

                ViewBag.sd = 1;
            }
            if (ed != 0)
            {
                if (startDate != "")
                {
                    tasks = tasks.Where(x => x.EndDate != null && (x.EndDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != "")
                {
                    tasks = tasks.Where(x => x.EndDate != null && (x.EndDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.ed = 1;
            }
            if (cd != 0)
            {
                if (startDate != "")
                {
                    tasks = tasks.Where(x => x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != "")
                {
                    tasks = tasks.Where(x => x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.cd = 1;
            }

            if (fname != "")
            {
                tasks = tasks.Where(x => x.User1.FirstName.ToUpper().Contains(fname.ToUpper())).ToList();
            }
            if (lname != "")
            {
                tasks = tasks.Where(x => x.User1.LastName.ToUpper().Contains(lname.ToUpper())).ToList();
            }
            if (division != -1)
            {
                tasks = tasks.Where(x => x.User1.DivisionId != null && x.User1.DivisionId == division).ToList();
            }

            if (taskType != 0)
            {
                tasks = tasks.Where(x => x.Task.TaskTypeID == taskType).ToList();
            }

            if (name != "")
            {
                tasks = tasks.Where(x => x.Task.Name.ToUpper().Contains(name.ToUpper())).ToList();
            }

            if (tag != -1)
            {
                tasks = tasks.Where(x => x.Task.TaskTags.Where(z => z.TagId == tag).Count() > 0).ToList();
            }

            if (status != -1)
            {
                tasks = tasks.Where(x => x.Status == status).ToList();
            }
            if (priority != -1)
            {
                tasks = tasks.Where(x => x.Priority == priority).ToList();
            }
            if (completionStatus != -1)
            {
                if (completionStatus == 2)
                {
                    tasks = tasks.Where(x => x.CompletionDate != null).ToList();
                    complettionName = "Completed";
                }
                else if (completionStatus == 3)
                {
                    tasks = tasks.Where(x => x.EndDate < DateTime.Now && x.CompletionDate == null).ToList();
                    complettionName = "Late";
                }
                else if (completionStatus == 1)
                {
                    tasks = tasks.Where(x => x.CompletionDate == null && x.EndDate > DateTime.Now).ToList();
                    complettionName = "In Process";
                }
            }

            string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Manager" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".pdf";
            new PDF_Export().generate_manager_assignments(tasks, path);

            List<string> objlist2 = new List<string>();

            objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
            objlist.AddRange(objlist2);

            return objlist;
            // return File(path, MediaTypeNames.Text.Plain, "Manager Assignment.pdf");
        }
        public List<string> ExportAdminPdf(string v="", string startDate = "", string endDate = "", int tag = -1, string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, int isCompleted = -1)
        {
            if (AuthenticateUser() == false)
            {
                //return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
            }
            int role = logedinuser.Role;
            List<string> objlist = new List<string>();
            // objlist.Add(logedinuser.Company);
            objlist.Add(logedinuser.Id.ToString());
            User admin = new UserBL().getUsersById(logedinuser.Id);
            List<User_Task> tasks = new List<User_Task>();

            if(isCompleted != -1)
            {
                tasks = new User_TaskBL().getUser_TasksList().OrderByDescending(c => c.Id).Where(x => x.User1.Role == 1 && x.DivisionID == null && x.DepartmentID == null && x.CompletionDate != null).ToList();
            }
            else
            {
                tasks = new User_TaskBL().getUser_TasksList().OrderByDescending(c => c.Id).Where(x => x.User1.Role == 1 && x.DivisionID == null && x.DepartmentID == null).ToList();
            }

            ViewBag.sd = 0;
            ViewBag.ed = 0;
            ViewBag.cd = 0;
            string complettionName = string.Empty;
            int id = logedinuser.Id;
            if (role == 3)
            {
               // return Content("Unauthorised Access");
            }
            else if (role == 2 || role == 4)
            {
                //return Content("Unauthorised Access");
            }
            else if (v != "")
            {
                if (logedinuser.IsMasterAdmin == 1)
                    tasks = tasks.Where(x => x.UserID != id).ToList();
                else
                    tasks = tasks.Where(x => x.UserID != id && x.User1.IsMasterAdmin == 0).ToList();

            }
            else
            {
                tasks = tasks.Where(x => x.UserID==id).ToList();
            }

            if (sd != 0)
            {
                if (startDate != "")
                {
                    tasks = tasks.Where(x => x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != "")
                {
                    tasks = tasks.Where(x => x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))).ToList();
                }

                ViewBag.sd = 1;
            }
            if (ed != 0)
            {
                if (startDate != "")
                {
                    tasks = tasks.Where(x => x.EndDate != null && (x.EndDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != "")
                {
                    tasks = tasks.Where(x => x.EndDate != null && (x.EndDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.ed = 1;
            }
            if (cd != 0)
            {
                if (startDate != "")
                {
                    tasks = tasks.Where(x => x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != "")
                {
                    tasks = tasks.Where(x => x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.cd = 1;
            }
            if (taskType != 0)
            {
                tasks = tasks.Where(x => x.Task.TaskTypeID == taskType).ToList();
            }

            if (name != "")
            {
                tasks = tasks.Where(x => x.Task.Name.ToUpper().Contains(name.ToUpper())).ToList();
            }

            if (tag != -1)
            {
                tasks = tasks.Where(x => x.Task.TaskTags.Where(z => z.TagId == tag).Count() > 0).ToList();
            }

            if (status != -1)
            {
                tasks = tasks.Where(x => x.Status == status).ToList();
            }
            if (priority != -1)
            {
                tasks = tasks.Where(x => x.Priority == priority).ToList();
            }
            if (completionStatus != -1)
            {
                if (completionStatus == 2)
                {
                    tasks = tasks.Where(x => x.CompletionDate != null).ToList();
                    complettionName = "Completed";
                }
                else if (completionStatus == 3)
                {
                    tasks = tasks.Where(x => x.EndDate < DateTime.Now && x.CompletionDate == null).ToList();
                    complettionName = "Late";
                }
                else if (completionStatus == 1)
                {
                    tasks = tasks.Where(x => x.CompletionDate == null && x.EndDate > DateTime.Now).ToList();
                    complettionName = "In Process";
                }
            }

            string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Manager" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".pdf";
            new PDF_Export().generate_manager_assignments(tasks, path,"Admin",1);

            List<string> objlist2 = new List<string>();

            objlist2 = General_Purpose.GetBlobFilePath(path, admin.Id);
            objlist.AddRange(objlist2);

            return objlist;
            // return File(path, MediaTypeNames.Text.Plain, "Admin  Assignment.pdf");
        }
        public List<string> ExportGroupTaskPdf(string startDate = "", string endDate = "", int tag = -1, int department = -1,int priority=-1, int division = 0, string taskName = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, int isCompleted = -1, int isGroupStudy=-1)
        {
            if (AuthenticateUser() == false)
            {
              //  return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
            }
            List<string> objlist = new List<string>();
            // objlist.Add(logedinuser.Company);
            objlist.Add(logedinuser.Id.ToString());
            User admin = new UserBL().getUsersById(logedinuser.Id);

            List<GroupTasks_Details> gTasks = null;

            if (isCompleted == 1) // this condition will trigger when we open Completed Groupt Tasks
            {
                if(isGroupStudy!=-1)
                gTasks = new GroupTasks_DetailsBL().getGroupTasks_DetailssList().Where(x => x.CompletionDate != null && x.CourseId!=null).ToList();
                else
                gTasks = new GroupTasks_DetailsBL().getGroupTasks_DetailssList().Where(x => x.CompletionDate != null && x.CourseId == null).ToList();
            }
            else
            {
                if(isGroupStudy!=-1)
                gTasks= new GroupTasks_DetailsBL().getGroupTasks_DetailssList().Where(x=> x.CompletionDate == null && x.CourseId != null).ToList();
                else
                gTasks= new GroupTasks_DetailsBL().getGroupTasks_DetailssList().Where(x=> x.CompletionDate == null && x.CourseId == null).ToList();
            }

            string complettionName = string.Empty;
            int id = logedinuser.Id;
            int role = logedinuser.Role;
            if (role == 3)
            {
                gTasks = gTasks.Where(x => x.GroupTask_User.Where(y => y.IsActive == 1 && y.UserId == id).Count() > 0).ToList();
            }
            else if (role == 2 || role == 4)
            {
                gTasks = gTasks.Where(x => x.GroupTask_User.Where(y => y.IsActive == 1 && y.UserId == id && (y.User.Role == 2 || y.User.Role == 4)).Count() > 0 || x.CreatedBy == id).ToList();
            }




            if (sd == 1)
            {
                if (startDate != null && startDate != "")
                {
                    gTasks = gTasks.Where(x => x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    gTasks = gTasks.Where(x => x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))).ToList();
                }

                ViewBag.sd = 1;
            }
            if (ed == 1)
            {
                if (startDate != "" && startDate != "")
                {
                    gTasks = gTasks.Where(x => x.EndDate != null && (x.EndDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    gTasks = gTasks.Where(x => x.EndDate != null && (x.EndDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.ed = 1;
            }
            if (cd == 1)
            {
                if (startDate != null && startDate != "")
                {
                    gTasks = gTasks.Where(x => x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate))).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    gTasks = gTasks.Where(x => x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate))).ToList();
                }
                ViewBag.cd = 1;
            }

            if (fname != "")
            {
                gTasks = gTasks.Where(x => x.GroupTask_User.Where(y => y.LeadRole == 1 && y.User.FirstName.ToUpper().Contains(fname.ToUpper())).Count() > 0).ToList();
            }
            if (lname != "")
            {
                gTasks = gTasks.Where(x => x.GroupTask_User.Where(y => y.LeadRole == 1 && y.User.LastName.ToUpper().Contains(lname.ToUpper())).Count() > 0).ToList();
            }
            if (taskName != "")
            {
                gTasks = gTasks.Where(x => x.GroupTask.Name.ToUpper().Contains(taskName.ToUpper())).ToList();
            }
            if (priority != -1)
            {
                gTasks = gTasks.Where(x => x.Priority == priority).ToList();
            }
            if (tag != -1)
            {
                gTasks = gTasks.Where(x => x.GroupTask.GroupTask_Task.Where(z => z.Task.TaskTags.Where(y => y.TagId == tag).Count() > 0).Count() > 0).ToList();
            }

            if (completionStatus != -1)
            {
                if (completionStatus == 2)
                {
                    gTasks = gTasks.Where(x => x.CompletionDate != null).ToList();
                    complettionName = "Completed";
                }
                else if (completionStatus == 3)
                {
                    gTasks = gTasks.Where(x => x.EndDate < DateTime.Now && x.CompletionDate == null).ToList();
                    complettionName = "Late";
                }
                else if (completionStatus == 1)
                {
                    gTasks = gTasks.Where(x => x.CompletionDate == null).ToList();
                    complettionName = "In Process";
                }
            }


            if (status != -1)
            {
                gTasks = gTasks.Where(x => x.Status == status).ToList();
            }
          
            string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "GT" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".pdf";
            new PDF_Export().generate_groupTaskDetailsassignment_pdf(gTasks, path, isGroupStudy);

            List<string> objlist2 = new List<string>();

            objlist2 = General_Purpose.GetBlobFilePath(path, admin.Id);
            objlist.AddRange(objlist2);

            return objlist;
            //  return File(path, MediaTypeNames.Text.Plain, "Group Assignment.pdf");


        }
        public ActionResult downloadGroupTaskDetails(string sId)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                int id = General_Purpose.DecryptId(sId);
                GroupTasks_Details gtd = new GroupTasks_DetailsBL().getGroupTasks_DetailssByIdWrapepr(id);
                string path = Server.MapPath("~") + ProjectVaraiables.PDFPATH + DateTime.Now.Ticks.ToString() + ".pdf";
                new PDF_Export().generate_groupTaskDetails_pdf(gtd, path);
                return File(path, MediaTypeNames.Text.Plain, "Group Task.pdf");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        #region Async PDF & Excel Generations
        [HttpPost]
        public FileResult AsyncEmpPDF(string startDate = "", string endDate = "", int tag = -1, int department = -1, int division = 0, string taskName = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, string DivDepID = "", List<int> list = null, string empselftask = "", int isCompleted = -1)
        {
            // var local = new Byte[1000000];
            string reportName = "Employee Assignments";
            if(isCompleted == 1)
            {
                reportName = "Completed Employee Assignments";
            }
            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = ExportEmployeesPdf(startDate, endDate, tag, department, division, taskName, taskType, status, fname, lname, priority, completionStatus, sd, ed, cd, DivDepID, list, empselftask, isCompleted);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }

        [HttpPost]
        public FileResult AsyncEmpExcel(string startDate = "", string endDate = "", int tag = -1, int department = -1, int division = 0, string taskName = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, string DivDepID = "", string empselftask = "", int isCompleted = -1)
        {
            // var local = new Byte[1000000];
            string reportName = "Employee Assignments";
            if (isCompleted == 1)
            {
                reportName = "Completed Employee Assignments";
            }

            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = ExportEmployeesTask(startDate,endDate,tag,department,division,taskName,taskType,status,fname,lname,priority,completionStatus,sd,ed,cd,DivDepID,empselftask,isCompleted);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }

        [HttpPost]
        public FileResult AsyncMangPDF(string startDate = "", string endDate = "", int tag = -1, string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, string fname = "", string lname = "", int division = -1, int isCompleted = -1)
        {
            // var local = new Byte[1000000];

            string reportName = "Manager Assignments";
            if (isCompleted == 1)
            {
                reportName = "Completed Manager Assignments";
            }

            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = ExportManagerPdf(startDate,endDate,tag,name,status,startcompletionDate,taskType,priority,completionStatus,sd,ed,cd,fname,lname,division,isCompleted);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }

        [HttpPost]
        public FileResult AsyncMangExcel(string startDate = "", string endDate = "", int tag = -1, string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, string fname = "", string lname = "", int division = -1, int isCompleted = -1)
        {
            // var local = new Byte[1000000];
            string reportName = "Manager Assignments";
            if(isCompleted == 1)
            {
                reportName = "Completed Manager Assignments";
            }
            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = ExportManagerTask(startDate,endDate,tag,name,status,startcompletionDate,taskType,priority,completionStatus,sd,ed,cd,fname,lname,division,isCompleted);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }

        [HttpPost]
        public FileResult AsyncAdminPDF(string v = "", string startDate = "", string endDate = "", int tag = -1, string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, int isCompleted = -1)
        {
            // var local = new Byte[1000000];
            string reportName = "Admin Assignments";
            if (isCompleted == 1)
            {
                reportName = "Completed Admin Assignments";
            }
            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = ExportAdminPdf(v,startDate,endDate,tag,name,status,startcompletionDate,taskType,priority,completionStatus,sd,ed,cd,isCompleted);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="tag"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="startcompletionDate"></param>
        /// <param name="taskType"></param>
        /// <param name="priority"></param>
        /// <param name="completionStatus"></param>
        /// <param name="sd"></param>
        /// <param name="ed"></param>
        /// <param name="cd"></param>
        /// <param name="isCompleted"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult AsyncAdminExcel(string v = "", string startDate = "", string endDate = "", int tag = -1, string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, int isCompleted = -1)
        {
            // var local = new Byte[1000000];
            string reportName = "Admin Assignments";
            if (isCompleted == 1)
            {
                reportName = "Completed Admin Assignments";
            }
            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = ExportAdminTask(v, startDate, endDate, tag, name, status, startcompletionDate, taskType, priority, completionStatus, sd, ed, cd, isCompleted);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }

        [HttpPost]
        public FileResult AsyncGroupTaskPDF(string startDate = "", string endDate = "", int tag = -1, int department = -1, int priority = -1, int division = 0, string taskName = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, int isCompleted = -1)
        {
            // var local = new Byte[1000000];
            string reportName = "Group Assignments";
            if (isCompleted == 1)
            {
                reportName = "Completed Group Assignments";
            }
            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = ExportGroupTaskPdf(startDate, endDate, tag,department,priority,division,taskName,taskType,status,fname,lname,completionStatus,sd,ed,cd,isCompleted);
                //mail.DownloadPDFReport(local, MediaTypeNames.Text.Plain, "Admin Assignments");
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }

         [HttpPost]
        public FileResult AsyncGroupStudyPDF(string startDate = "", string endDate = "", int tag = -1, int department = -1, int priority = -1, int division = 0, string taskName = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, int isCompleted = -1)
        {
            // var local = new Byte[1000000];
            string reportName = "Group Study Assignments";
            if (isCompleted == 1)
            {
                reportName = "Completed Group Study Assignments";
            }
            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = ExportGroupTaskPdf(startDate, endDate, tag,department,priority,division,taskName,taskType,status,fname,lname,completionStatus,sd,ed,cd,isCompleted,1);
                //mail.DownloadPDFReport(local, MediaTypeNames.Text.Plain, "Admin Assignments");
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }

        
        [HttpPost]
        public FileResult AsyncGroupTaskExcel(string startDate = "", string endDate = "", int tag = -1, int department = -1, int priority = -1, int division = 0, string taskName = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, int isCompleted = -1)
        {
            // var local = new Byte[1000000];

            string reportName = "Group Assignments";
            if (isCompleted == 1)
            {
                reportName = "Completed Group Assignments";
            }
            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = ExportGroupTask(startDate, endDate, tag, department, division, priority, taskName, taskType, status, fname, lname, completionStatus, sd, ed, cd, isCompleted);
                //mail.DownloadPDFReport(local, MediaTypeNames.Text.Plain, "Admin Assignments");
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }

        [HttpPost]
        public FileResult AsyncGroupStudyExcel(string startDate = "", string endDate = "", int tag = -1, int department = -1, int priority = -1, int division = 0, string taskName = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, int isCompleted = -1)
        {
            // var local = new Byte[1000000];

            string reportName = "Group Study Assignments";
            if (isCompleted == 1)
            {
                reportName = "Completed Group Study Assignments";
            }
            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = ExportGroupTask(startDate, endDate, tag, department, division, priority,taskName, taskType, status, fname, lname, completionStatus, sd, ed, cd, isCompleted,1);
                //mail.DownloadPDFReport(local, MediaTypeNames.Text.Plain, "Admin Assignments");
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }


        [HttpPost]
        public FileResult AsyncTraineePDF(string startDate = "", string endDate = "", int tag = -1, int department = -1, int division = 0, string taskName = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, string DivDepID = "", List<int> list = null, string empselftask = "", int isCompleted = -1)
        {
            // var local = new Byte[1000000];
            string reportName = "Employee Assignments";
            if (isCompleted == 1)
            {
                reportName = "Completed Employee Assignments";
            }
            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = ExportEmployeesPdf(startDate, endDate, tag, department, division, taskName, taskType, status, fname, lname, priority, completionStatus, sd, ed, cd, DivDepID, list, empselftask, isCompleted);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }

        [HttpPost]
        public FileResult AsyncTraineeExcel(string startDate = "", string endDate = "", int tag = -1, int department = -1, int division = 0, string taskName = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int priority = -1, int completionStatus = -1, int sd = 0, int ed = 0, int cd = 0, string DivDepID = "", string empselftask = "", int isCompleted = -1)
        {
            // var local = new Byte[1000000];
            string reportName = "Employee Assignments";
            if (isCompleted == 1)
            {
                reportName = "Completed Employee Assignments";
            }

            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = ExportEmployeesTask(startDate, endDate, tag, department, division, taskName, taskType, status, fname, lname, priority, completionStatus, sd, ed, cd, DivDepID, empselftask, isCompleted);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }


        #endregion

        public ActionResult generateTraineeTaskReport(string sId)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                int id = General_Purpose.DecryptId(sId);
                User_Task gtd = new User_TaskBL().getUser_TasksById(id);
                string path = Server.MapPath("~") + ProjectVaraiables.PDFPATH + DateTime.Now.Ticks.ToString() + ".pdf";
                new PDF_Export().generate_employeeTask_pdf(new List<User_Task>() { gtd }, path);
                return File(path, "Group_Task.pdf");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }
        public ActionResult generateManagerTaskReport(string sId)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                int id = General_Purpose.DecryptId(sId);
                User_Task gtd = new User_TaskBL().getUser_TasksById(id);
                string path = Server.MapPath("~") + ProjectVaraiables.PDFPATH + DateTime.Now.Ticks.ToString() + ".pdf";
                new PDF_Export().generate_ManagerTask_pdf(new List<User_Task>() { gtd }, path);
                return File(path, MediaTypeNames.Text.Plain, "Task Manager.pdf");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }
        public ActionResult generateAdminTaskReport(string sId)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                int id = General_Purpose.DecryptId(sId);
                User_Task gtd = new User_TaskBL().getUser_TasksById(id);
                string path = Server.MapPath("~") + ProjectVaraiables.PDFPATH + DateTime.Now.Ticks.ToString() + ".pdf";
                new PDF_Export().generate_ManagerTask_pdf(new List<User_Task>() { gtd }, path);
                return File(path, MediaTypeNames.Text.Plain, "Task Admin.pdf");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }
        public ActionResult generateManagerIndividualReport(int sid)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                List<User_Task> tasks = new User_TaskBL().getUser_TasksList().Where(x => x.IsActive == 1 && x.UserID == sid).ToList();
                string path = Server.MapPath("~") + ProjectVaraiables.PDFPATH + DateTime.Now.Ticks.ToString() + ".pdf";
                new PDF_Export().generate_ManagerTask_pdf(tasks, path);
                return File(path, MediaTypeNames.Text.Plain, "Task Manager.pdf");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }
        public ActionResult generateEmployeeIndividualReport(int sid)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                List<User_Task> tasks = new User_TaskBL().getUser_TasksList().Where(x => x.IsActive == 1 && x.UserID == sid).ToList();
                string path = Server.MapPath("~") + ProjectVaraiables.PDFPATH + DateTime.Now.Ticks.ToString() + ".pdf";
                new PDF_Export().generate_employeeTask_pdf(tasks, path);
                return File(path, MediaTypeNames.Text.Plain, "Employee Task.pdf");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }
        public ActionResult generateGroupTask(int sid)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                List<User_Task> tasks = new User_TaskBL().getUser_TasksList().Where(x => x.IsActive == 1 && x.User1.Id == sid).ToList();
                string path = Server.MapPath("~") + ProjectVaraiables.PDFPATH + DateTime.Now.Ticks.ToString() + ".pdf";
                new PDF_Export().generate_employeeTask_pdf(tasks, path);
                return File(path, MediaTypeNames.Text.Plain, "GroupTaskList.pdf");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }


        #endregion

        public ActionResult CourseCompletionCertificatePdf(string CutId)
        {
            try
            {
                // Log.Info("Pdf generation method called");
                if (AuthenticateUser() == false)
                {
                }
                int id = General_Purpose.DecryptId(CutId);
                Course_UserTask cut = new Course_UserTaskBL().getCourse_UserTaskById(id);

                List<Course_UserTask> list = new List<Course_UserTask>();
                list.Add(cut);
                // Log.Info("Pdf generation method search completed");
                string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Course Completion Certificate PDF" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".pdf";

                new PDF_Export().generate_CourseCertificate_PDF(list, path, "Course Completion Certificate");


                return File(path, MediaTypeNames.Text.Plain, "Course Completion Certificate.pdf");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }

        }

    }
}