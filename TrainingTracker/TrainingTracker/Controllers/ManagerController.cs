using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using TrainingTracker.BL;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;
using File = TrainingTracker.Models.File;

namespace TrainingTracker.Controllers
{
    public class ManagerController : Controller
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        PushDataDTO data = new PushDataDTO();
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
        // GET: Manager
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

        public ActionResult displayManagerTask()
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                if (logedinuser.Role == 1)
                {
                    ViewBag.role = 2;

                }
                int role = logedinuser.Role;
                ViewBag.sd = 0;
                ViewBag.ed = 0;
                ViewBag.cd = 0;
                string complettionName = string.Empty;
                int id = logedinuser.Id;
                if (role == 3)
                {
                    return Content("Unauthorised Access");
                }
                else if (role == 2 || role == 4)
                {
                    ViewBag.way = "MyTask";
                }
                else
                {
                }



                ViewBag.tags = new TagBL().getTagsList();

                ViewBag.taskTypes = new TaskTypeBL().getTaskTypesList();
                ViewBag.statuses = new List<int>() { 0, 1, 2, 3, 4 };
                ViewBag.completionListId = new List<int>() { 1, 2, 3 };
                ViewBag.completionList = new List<string>() { "In Process", "Completed", "Late" };

                ViewBag.priorities = new List<int>() { 0, 1, 2 }.ToList();
                List<Task> taskslist = new TaskBL().getTasksList().Where(x => x.IsActive == 1).ToList();
                ViewBag.TaskName = taskslist;


                List<User> userslist = new UserBL().getUsersList().Where(x => x.IsActive == 1 && (x.Role == 2 || x.Role == 4)).ToList();
                ViewBag.FirstLastName = userslist;
                List<Division> divlist = new DivisionBL().getDivisionsList().Where(x => x.IsActive == 1).ToList();
                ViewBag.DivList = divlist;

                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        public ActionResult GetdisplayManagerTask(string startDate = "", string endDate = "", int tag = -1, string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int completionStatus = -1, int startD = 0, int end = 0, int complete = 0, string fname = "", string lname = "", int division = -1, int isCompleted = -1)
        {
            try
            {
                int role = logedinuser.Role;
                //  List<User_Task> tasks = new User_TaskBL().getUser_TasksList().OrderByDescending(c => c.Id).Where(x =>x.User1.Role == 2 || x.User1.Role == 4).ToList();
                List<User_Task> tasks = new List<User_Task>();

                if(isCompleted != -1)
                {
                    tasks = new User_TaskBL().spGetUserTasksByRole(2, Convert.ToInt32(logedinuser.Company)).OrderByDescending(c => c.Id).Where(x => x.User1 != null && x.CompletionDate != null).ToList();
                }
                else
                {
                    tasks = new User_TaskBL().spGetUserTasksByRole(2, Convert.ToInt32(logedinuser.Company)).OrderByDescending(c => c.Id).Where(x => x.User1 != null && x.CompletionDate==null).ToList();
                }


                tasks = tasks.Where(x => x.User1.Role == 2 || x.User1.Role == 4).ToList();
                ViewBag.sd = 0;
                ViewBag.ed = 0;
                ViewBag.cd = 0;
                string complettionName = string.Empty;
                int id = logedinuser.Id;
                if (role == 3)
                {
                    return Content("Unauthorised Access");
                }
                else if (role == 2 || role == 4)
                {
                    tasks = tasks.Where(x => x.UserID == id).ToList();
                }
                else
                {
                    tasks = tasks.Where(x => x.IsPrivate == 0).ToList();
                }
                ///////////////////////////////////////////////////////////////
                if (startD == 1)
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
                if (end == 1)
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
                if (complete == 1)
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



                /////////////////////////////////////////////////////////

                int start2 = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];


                int totalrows = tasks.Count();
                //filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    tasks = tasks.Where(x => x.User1.FirstName.ToLower().Contains(searchValue.ToLower()) || x.Task.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                int totalrowsafterfilterinig = tasks.Count();
                //sorting

                // pagination
                tasks = tasks.Skip(start2).Take(length).ToList();

                //List<User> mnglist = new List<User>();
                List<EmployeeDTO> mnglist = new List<EmployeeDTO>();

                string priority2 = "";
                string status2 = "";
                string AssignedBy = "";
                int LgRole = 0;
                foreach (User_Task x in tasks)
                {
                    if (x.Status != null)
                    {
                        status2 = General_Purpose.getStatusValue(x.Status.Value);
                    }
                    else
                    {
                        status2 = "";
                    }

                    if (x.Priority != null)
                    {
                        if (General_Purpose.getPriorityValue(x.Priority.Value) == "Medium")
                        {
                            priority2 = "<span class='mediumPriorityButton'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                        }
                        else if (General_Purpose.getPriorityValue(x.Priority.Value) == "Low")
                        {
                            priority2 = "<span class='lowPriorityButton'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                        }
                        else
                        {
                            priority2 = "<span class='highPriorityButton'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                        }

                    }
                    else
                        priority2 = "";


                    if (x.CreatedID == null)
                    {
                        AssignedBy = "Self Assigned";

                    }
                    else if (x.User.Id == logedinuser.Id)
                    {
                        AssignedBy = "You";
                    }
                    else
                    {
                        AssignedBy = x.User.FirstName + " " + x.User.LastName;
                    }


                    EmployeeDTO obj = new EmployeeDTO()
                    {

                        Task = x.Task.Name,
                        StartDate = x.StartDate.Value.ToString("MM/dd/yyyy"),
                        EndtDate = x.EndDate.Value.ToString("MM/dd/yyyy"),
                        WorkStatus = status2,
                        AssignedBy = AssignedBy,
                        Priority = priority2,
                        Name = x.User1.FirstName + " " + x.User1.LastName,
                        Id = x.Id,
                        EncryptedId = General_Purpose.EncryptId(x.Id)

                    };
                    mnglist.Add(obj);



                }

                return Json(new { data = mnglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public JsonResult GetManagerTaskBySearch(string startDate = "", string endDate = "", int tag = -1, string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int completionStatus = -1, int start = 0, int end = 0, int complete = 0)
        {
            int role = logedinuser.Role;
            List<User_Task> tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Role == 2 || x.User1.Role == 4).ToList();
            ViewBag.sd = 0;
            ViewBag.ed = 0;
            ViewBag.cd = 0;
            string complettionName = string.Empty;
            int id = logedinuser.Id;
            if (role == 2 || role == 4)
            {
                tasks = tasks.Where(x => x.UserID == id).ToList();
            }
            else
            {
                tasks = tasks.Where(x => x.IsPrivate == 0).ToList();
            }

            if (start == 1)
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
            if (end == 1)
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
            if (complete == 1)
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

            List<EmployeeDTO> mnglist = new List<EmployeeDTO>();

            string priority1 = "";
            string status1 = "";
            string AssignedBy = "";
            int LgRole = 0;
            foreach (User_Task x in tasks)
            {
                if (x.Status != null)
                {
                    status1 = General_Purpose.getStatusValue(x.Status.Value);
                }
                else
                {
                    status1 = "";
                }

                if (x.Priority != null)
                {
                    if (General_Purpose.getPriorityValue(x.Priority.Value) == "Medium")
                    {
                        priority1 = "<span class='label label-warning'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                    }
                    else if (General_Purpose.getPriorityValue(x.Priority.Value) == "Low")
                    {
                        priority1 = "<span class='label label-success'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                    }
                    else
                    {
                        priority1 = "<span class='label label-danger'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                    }

                }
                else
                    priority1 = "";

                if (logedinuser.Role == 1)
                {
                    ViewBag.role = 2;

                }

                if (x.CreatedID == null)
                {
                    AssignedBy = "Self Assigned";

                }
                else if (x.User.Id == logedinuser.Id)
                {
                    AssignedBy = "You";
                }
                else
                {
                    AssignedBy = x.User.FirstName + " " + x.User.LastName;
                }

                EmployeeDTO obj = new EmployeeDTO()
                {

                    Task = x.Task.Name,
                    StartDate = x.StartDate.Value.ToString("MM/dd/yyyy"),
                    EndtDate = x.EndDate.Value.ToString("MM/dd/yyyy"),
                    WorkStatus = status1,
                    AssignedBy = AssignedBy,
                    Name = x.User1.FirstName + " " + x.User1.LastName,
                    Priority = priority1,

                    Id = x.Id,
                    EncryptedId = General_Purpose.EncryptId(x.Id)

                };
                mnglist.Add(obj);

            }

            return Json(mnglist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult displayManagerGroupTask(string startDate = "", string endDate = "", string name = "", int status = -1, string startcompletionDate = "", int taskType = 0)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                ViewBag.sd = 0;
                ViewBag.ed = 0;
                ViewBag.cd = 0;
                int id = logedinuser.Id;
                List<GroupTasks_Details> gTasks = new GroupTasks_DetailsBL().getGroupTasks_DetailssList().Where(x => x.GroupTask_User.Where(y => y.UserId == id).Count() > 0).ToList();

                if (Request.Form["start"] != null)
                {
                    if (startDate != "")
                    {
                        gTasks = gTasks.Where(x => x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))).ToList();
                    }

                    if (endDate != "")
                    {
                        gTasks = gTasks.Where(x => x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))).ToList();
                    }

                    ViewBag.sd = 1;
                }
                if (Request.Form["end"] != null)
                {
                    if (startDate != "")
                    {
                        gTasks = gTasks.Where(x => x.EndDate != null && (x.EndDate > DateTime.Parse(startDate))).ToList();
                    }

                    if (endDate != "")
                    {
                        gTasks = gTasks.Where(x => x.EndDate != null && (x.EndDate < DateTime.Parse(endDate))).ToList();
                    }
                    ViewBag.ed = 1;
                }
                if (Request.Form["complete"] != null)
                {
                    if (startDate != "")
                    {
                        gTasks = gTasks.Where(x => x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate))).ToList();
                    }

                    if (endDate != "")
                    {
                        gTasks = gTasks.Where(x => x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate))).ToList();
                    }
                    ViewBag.cd = 1;
                }

                if (name != "")
                {
                    gTasks = gTasks.Where(x => x.GroupTask.Name.ToUpper().Contains(name.ToUpper())).ToList();
                }

                if (status != -1)
                {
                    gTasks = gTasks.Where(x => x.Status == status).ToList();
                }

                ViewBag.startDate = startDate;
                ViewBag.endDate = endDate;
                ViewBag.status = status;

                ViewBag.taskName = name;
                ViewBag.taskType = new TaskTypeBL().getTaskTypesById(taskType);
                ViewBag.taskTypes = new TaskTypeBL().getTaskTypesList();
                ViewBag.statuses = new List<int>() { 0, 1, 2, 3, 4 }.Where(x => x != status);
                ViewBag.gtasks = gTasks;

                return View("displayManagerGroupTask");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult deleteTaskFunction(int sid, string startDates = "", string endDates = "", int tags = -1, string taskNames = "", int taskTypes = 0, int statuss = -1, int completionStatuss = -1, int prioritys = -1, int isCompleted = -1)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                new User_TaskBL().DeleteUser_Tasks(sid);

                if(isCompleted != -1) 
                {
                    return RedirectToAction("DisplayCompletedManagerTask", new { startDate = startDates, endDate = endDates, tag = tags, taskName = taskNames, taskType = taskTypes, status = statuss, completionStatus = completionStatuss, priority = prioritys });
                }
                else 
                {
                    return RedirectToAction("displayManagerTask", new { startDate = startDates, endDate = endDates, tag = tags, taskName = taskNames, taskType = taskTypes, status = statuss, completionStatus = completionStatuss, priority = prioritys });
                }

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult deleteTaskFunctionManagerPage(int sid, int empid, string way="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                new User_TaskBL().DeleteUser_Tasks(sid);
                if(way == "Structure")
                {
                    return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(empid), division = "yes", message = "Task Deleted Successfully", way = way });
                }
                return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(empid) });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        public ActionResult taskDetails(string v, string displayMessage = null, string isModal = "", string way = "", string loginId = "", int replyId = -1, int viewrepliesid = -1, string message = "", string DxM="")
        {
            try
            {
                ViewBag.DxM = DxM;
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                int userId = logedinuser.Id;
                int taskId;
                string strr = "";

                if (isModal == "")
                {
                    taskId = Convert.ToInt32(v);
                }
                else
                {
                    taskId = General_Purpose.DecryptId(v);
                }

                if (taskId == 0)
                {
                    strr = HttpUtility.UrlEncode(v);
                    taskId = General_Purpose.DecryptId(strr);
                }

                User_Task task = new User_TaskBL().getUser_TasksById(taskId);
                if (task == null)
                {
                    return RedirectToAction("Index", "Auth", new { message = "Task has been deleted or removed for you." });
                }
                DateTime dd = Convert.ToDateTime(task.EndDate).Date;

                DateTime d7 = DateTime.Now.Date.AddDays(7);
                DateTime d30 = DateTime.Now.Date.AddDays(30);
                if (dd > DateTime.Now.Date)
                {
                    ViewBag.DailyRem = 1;

                    if (dd > d7)
                    {
                        ViewBag.WeeklyRem = 1;

                        if (dd > d30)
                        {
                            ViewBag.MonthlyRem = 1;
                        }
                    }
                }
                if (task.DailyReminder != null)
                {
                    ViewBag.DailyReminder = task.DailyReminder;
                }
                if (task.BeforeDDateReminder != null)
                {
                    TimeSpan days = Convert.ToDateTime(task.BeforeDDateReminder) - dd;
                    ViewBag.BeforeDDateReminder = Math.Abs(days.Days);
                }

                ViewBag.currentStatus = task.Status;
                ViewBag.status = new List<int>() { 1, 2, 3, 4 }/*.Where(x => x > task.Status)*/;

                //if (task.CanvasCourseId != null)
                //{
                //    Course course = General_Purpose.GetCanvasUserCoursesById(task.User1.Id).Where(x => x.id == task.CanvasCourseId).FirstOrDefault();
                //    if (course != null)
                //    {
                //        ViewBag.CanvasCourse = course;
                //        List<Quiz> quizes = General_Purpose.GetCanvasQuizes(course.id);
                //        List<Assignment> assignments = General_Purpose.GetCanvasAssignments(course.id);
                //        ViewBag.CanvasQuizes = quizes;
                //        ViewBag.CanvasAssignments = assignments;

                //        List<Task> tasklist = new TaskBL().getTasksList().Where(x => x.CourseId == course.id && x.IsResultAnnounced == 1).ToList();
                //        if (tasklist.Count > 0)
                //        {
                //            ViewBag.ResultAnnouncement = 1;
                //        }
                //        else
                //        {
                //            ViewBag.ResultAnnouncement = 0;
                //        }
                //        Enrollment en = General_Purpose.GetResult(task.User1.Id, course.id);

                //        if (en != null)
                //        {
                //            if (en.computed_final_score != null)
                //            {
                //                string s = en.computed_final_score;
                //                double x = Convert.ToDouble(s);


                //                ViewBag.result = x;




                //            }
                //            else
                //            {
                //                ViewBag.result = 0.0;
                //            }

                //        }
                //        else
                //            ViewBag.result = null;
                //        if (ViewBag.ResultAnnouncement != 1)
                //        {
                //            ViewBag.status = new List<int>() { 1, 2, 3 }/*.Where(x => x > task.Status)*/;
                //            ViewBag.CanvasMsg = "You're unable to complete the task because Canvas course result not announced yet.";
                //        }
                //        else
                //        {
                //            if (ViewBag.result == null)
                //            {
                //                ViewBag.status = new List<int>() { 1, 2, 3 }/*.Where(x => x > task.Status)*/;
                //                ViewBag.CanvasMsg = "You're unable to complete the task because you've not cleared your Canvas Course.";
                //            }
                //            else
                //            {
                //                if (Convert.ToInt32(ViewBag.result) < Convert.ToInt32(task.Grad))
                //                {
                //                    ViewBag.status = new List<int>() { 1, 2, 3 }/*.Where(x => x > task.Status)*/;
                //                    ViewBag.CanvasMsg = "You're unable to complete the task, your course passing marks are less than Grade.";
                //                }
                //            }
                //        }


                //    }
                //    else
                //    {
                //        ViewBag.status = new List<int>() { 1, 2, 3 }/*.Where(x => x > task.Status)*/;
                //        ViewBag.CanvasMsg = "You'r unable to complete the task because you've not cleared your Canvas Course.";
                //    }
                //}



                List<Task_Ticket> t = new Task_TicketBL().Task_TicketswithoutWreapper(taskId);
                ViewBag.completedTickets = t.Where(x => x.CompletionDatetime != null).ToList();
                ViewBag.ActiveToDo = t.Where(x => x.CompletionDatetime == null).ToList().Count();
                ViewBag.uncompletedTickets = t.Where(x => x.CompletionDatetime == null).ToList();



                List<TaskCommentDTO> files = new List<TaskCommentDTO>();
                BlobManager blob = new BlobManager();

                int loginrole = -1;
                ////////////////////////////////////////////////////
                User us = new UserBL().getUsersById(userId);

                if (us.IsMasterAdmin == 1)
                {
                    loginrole = 0;
                }
                else if (us.Role == 1)
                {
                    loginrole = 1;
                }
                else if (us.Role == 2 || us.Role == 2)
                {
                    loginrole = 2;
                }
                else if (us.Role == 3)
                {
                    loginrole = 3;
                }

                foreach (var item in task.TaskComments.Where(x => x.IsActive == 1))
                {
                    if (item.User == null)
                    {
                        User usernew = new UserBL().getUsersById((int)item.UserId);
                        item.User = usernew;
                    }
                    List<TaskCommentDTO2> files2 = new List<TaskCommentDTO2>();
                    if (item.ParentId == null)
                    {
                        List<TaskComment> replycomments = new TaskCommentBL().getTaskCommentsList().Where(x => x.ParentId == item.Id && x.IsActive == 1).ToList();

                        if (replycomments.Count() > 0)
                        {
                            TaskCommentDTO tcd = new TaskCommentDTO();

                            foreach (var repitem in replycomments)
                            {
                                TaskCommentDTO2 tcd2 = new TaskCommentDTO2();
                                tcd2.Comment = repitem.Comment;
                                tcd2.Id = repitem.Id;
                                tcd2.isManager = 1;
                                tcd2.userId = repitem.User.Id;
                                tcd2.Image = repitem.User.Image;
                                tcd2.IsDocMFile = repitem.IsDocMFile;
                                User uss = new UserBL().getUsersById(tcd2.userId);

                                if (loginrole == 0)
                                {
                                    if (uss.Role == 2 || uss.Role == 4)
                                        tcd2.ROLE = "(Manager)";
                                    else if (uss.Role == 1 && (uss.IsMasterAdmin == 0 || uss.IsMasterAdmin == null))
                                        tcd2.ROLE = "(Admin)";
                                    else if (uss.Role == 3)
                                        tcd2.ROLE = "(Employee)";
                                }
                                else if (loginrole == 1)
                                {
                                    if (uss.IsMasterAdmin == 1)
                                    {
                                        tcd2.ROLE = "(M Admin)";
                                    }
                                    else if (uss.Role == 2 || uss.Role == 4)
                                        tcd2.ROLE = "(Manager)";
                                    else if (uss.Role == 3)
                                        tcd2.ROLE = "(Employee)";
                                }
                                else if (loginrole == 2)
                                {
                                    if (uss.IsMasterAdmin == 1)
                                    {
                                        tcd2.ROLE = "(M Admin)";
                                    }
                                    else if (uss.Role == 1 && (uss.IsMasterAdmin == 0 || uss.IsMasterAdmin == null))
                                        tcd2.ROLE = "(Admin)";
                                    else if (uss.Role == 3)
                                        tcd2.ROLE = "(Employee)";
                                }
                                else if (loginrole == 3)
                                {
                                    if (uss.IsMasterAdmin == 1)
                                    {
                                        tcd2.ROLE = "(M Admin)";
                                    }
                                    else if (uss.Role == 2 || uss.Role == 4)
                                        tcd2.ROLE = "(Manager)";
                                    else if (uss.Role == 1 && (uss.IsMasterAdmin == 0 || uss.IsMasterAdmin == null))
                                        tcd2.ROLE = "(Admin)";
                                }

                                tcd2.CommentTime = Convert.ToDateTime(repitem.Date);
                                // tcd.ROLE =Convert.ToInt32( item.User.Role);
                                if (repitem.File != null)
                                {
                                    tcd2.File = repitem.FileName;
                                    tcd2.FilePath = repitem.File;
                                }
                                else
                                    tcd2.File = null;
                                tcd2.Name = repitem.User.FirstName + " " + repitem.User.LastName;
                                if (repitem.File != null)

                                    try
                                    {
                                        CloudBlockBlob blobObj = blob.getCloudBlockBlob(repitem.File);
                                        blobObj.FetchAttributes();
                                        tcd2.FileSize = General_Purpose.SizeSuffix(blobObj.Properties.Length);
                                    }
                                    catch (Exception e)
                                    {
                                        tcd2.FileSize = null;
                                        tcd2.FilePath = null;
                                        tcd2.File = null;

                                    }
                                else
                                    tcd2.FileSize = "";
                                files2.Add(tcd2);
                                tcd2.prime = -1;

                            }
                            tcd.obj = files2;
                            tcd.Comment = item.Comment;
                            tcd.Id = item.Id;
                            tcd.isManager = 1;
                            tcd.IsDocMFile = item.IsDocMFile;
                            tcd.userId = item.User.Id;
                            tcd.Image = item.User.Image;
                            User u = new UserBL().getUsersById(tcd.userId);

                            if (loginrole == 0)
                            {
                                if (u.Role == 2 || u.Role == 4)
                                    tcd.ROLE = "(Manager)";
                                else if (u.Role == 1 && (u.IsMasterAdmin == 0 || u.IsMasterAdmin == null))
                                    tcd.ROLE = "(Admin)";
                                else if (u.Role == 3)
                                    tcd.ROLE = "(Employee)";
                            }
                            else if (loginrole == 1)
                            {
                                if (u.IsMasterAdmin == 1)
                                {
                                    tcd.ROLE = "(M Admin)";
                                }
                                else if (u.Role == 2 || u.Role == 4)
                                    tcd.ROLE = "(Manager)";
                                else if (u.Role == 3)
                                    tcd.ROLE = "(Employee)";
                            }
                            else if (loginrole == 2)
                            {
                                if (u.IsMasterAdmin == 1)
                                {
                                    tcd.ROLE = "(M Admin)";
                                }
                                else if (u.Role == 1 && (u.IsMasterAdmin == 0 || u.IsMasterAdmin == null))
                                    tcd.ROLE = "(Admin)";
                                else if (u.Role == 3)
                                    tcd.ROLE = "(Employee)";
                            }
                            else if (loginrole == 3)
                            {
                                if (u.IsMasterAdmin == 1)
                                {
                                    tcd.ROLE = "(M Admin)";
                                }
                                else if (u.Role == 2 || u.Role == 4)
                                    tcd.ROLE = "(Manager)";
                                else if (u.Role == 1 && (u.IsMasterAdmin == 0 || u.IsMasterAdmin == null))
                                    tcd.ROLE = "(Admin)";
                            }

                            tcd.CommentTime = Convert.ToDateTime(item.Date);
                            // tcd.ROLE =Convert.ToInt32( item.User.Role);
                            if (item.File != null)
                            {
                                tcd.File = item.FileName;
                                tcd.FilePath = item.File;
                            }
                            else
                                tcd.File = null;
                            tcd.Name = item.User.FirstName + " " + item.User.LastName;
                            if (item.File != null)

                                try
                                {
                                    CloudBlockBlob blobObj = blob.getCloudBlockBlob(item.File);
                                    blobObj.FetchAttributes();
                                    tcd.FileSize = General_Purpose.SizeSuffix(blobObj.Properties.Length);
                                }
                                catch (Exception e)
                                {
                                    tcd.FileSize = null;
                                    tcd.FilePath = null;
                                    tcd.File = null;

                                }
                            else
                                tcd.FileSize = "";
                            files.Add(tcd);
                            tcd.prime = -1;

                        }
                        else
                        {
                            TaskCommentDTO tcd = new TaskCommentDTO();
                            tcd.Comment = item.Comment;
                            tcd.Id = item.Id;
                            tcd.isManager = 1;
                            tcd.userId = item.User.Id;
                            tcd.Image = item.User.Image;
                            tcd.IsDocMFile = item.IsDocMFile;
                            User u = new UserBL().getUsersById(tcd.userId);

                            if (loginrole == 0)
                            {
                                if (u.Role == 2 || u.Role == 4)
                                    tcd.ROLE = "(Manager)";
                                else if (u.Role == 1 && (u.IsMasterAdmin == 0 || u.IsMasterAdmin == null))
                                    tcd.ROLE = "(Admin)";
                                else if (u.Role == 3)
                                    tcd.ROLE = "(Employee)";
                            }
                            else if (loginrole == 1)
                            {
                                if (u.IsMasterAdmin == 1)
                                {
                                    tcd.ROLE = "(M Admin)";
                                }
                                else if (u.Role == 2 || u.Role == 4)
                                    tcd.ROLE = "(Manager)";
                                else if (u.Role == 3)
                                    tcd.ROLE = "(Employee)";
                            }
                            else if (loginrole == 2)
                            {
                                if (u.IsMasterAdmin == 1)
                                {
                                    tcd.ROLE = "(M Admin)";
                                }
                                else if (u.Role == 1 && (u.IsMasterAdmin == 0 || u.IsMasterAdmin == null))
                                    tcd.ROLE = "(Admin)";
                                else if (u.Role == 3)
                                    tcd.ROLE = "(Employee)";
                            }
                            else if (loginrole == 3)
                            {
                                if (u.IsMasterAdmin == 1)
                                {
                                    tcd.ROLE = "(M Admin)";
                                }
                                else if (u.Role == 2 || u.Role == 4)
                                    tcd.ROLE = "(Manager)";
                                else if (u.Role == 1 && (u.IsMasterAdmin == 0 || u.IsMasterAdmin == null))
                                    tcd.ROLE = "(Admin)";
                            }


                            tcd.CommentTime = Convert.ToDateTime(item.Date);
                            // tcd.ROLE =Convert.ToInt32( item.User.Role);
                            if (item.File != null)
                            {
                                tcd.File = item.FileName;
                                tcd.FilePath = item.File;
                            }
                            else
                                tcd.File = null;
                            tcd.Name = item.User.FirstName + " " + item.User.LastName;
                            if (item.File != null)

                                try
                                {
                                    CloudBlockBlob blobObj = blob.getCloudBlockBlob(item.File);
                                    blobObj.FetchAttributes();
                                    tcd.FileSize = General_Purpose.SizeSuffix(blobObj.Properties.Length);
                                }
                                catch (Exception e)
                                {
                                    tcd.FileSize = null;
                                    tcd.FilePath = null;
                                    tcd.File = null;

                                }
                            else
                                tcd.FileSize = "";
                            files.Add(tcd);
                            tcd.prime = -1;

                        }


                    }
                }


                ///////////////////////////////////////////////


                files = files.OrderBy(x => x.Id).ToList();
                ViewBag.comments = files;
                if (displayMessage != null)
                    ViewBag.request = displayMessage;
                else
                    ViewBag.request = null;


                List<TaskCommentDTO> Ticketfiles = new List<TaskCommentDTO>();

                foreach (Task_Ticket item in new Task_TicketBL().getTask_TicketsList().Where(x => x.IsActive == 1 && x.UserTask_Id == task.Id))
                {
                    if (item.Ticket_File != null)
                    {

                        TaskCommentDTO obj = new TaskCommentDTO();
                        obj.Name = logedinuser.Name;
                        obj.CommentTime = Convert.ToDateTime(item.CreationDatetime);
                        obj.File = item.Ticket_FileName;
                        obj.FilePath = item.Ticket_File;

                        try
                        {
                            CloudBlockBlob blobObj = blob.getCloudBlockBlob(item.Ticket_File);
                            blobObj.FetchAttributes();
                            obj.FileSize = General_Purpose.SizeSuffix(blobObj.Properties.Length);
                        }
                        catch (Exception e)
                        {
                            obj.FileSize = "";
                            obj.File = null;
                            obj.FilePath = null;
                        }

                        Ticketfiles.Add(obj);
                    }
                }
                ViewBag.Ticketfiles = Ticketfiles.OrderByDescending(x => x.CommentTime);



                ViewBag.page = isModal;
                ViewBag.way = way;
                ViewBag.loginId = loginId;
                ViewBag.replyId = replyId;
                ViewBag.v = v;
                ViewBag.isModal = isModal;
                ViewBag.viewrepliesId = viewrepliesid;
                ViewBag.message = message;

                Department departmentid = new Department();
                if (task.User1 != null)
                {
                    if (task.User1.DepartmentId == null && task.User1.DivisionId == null)
                    {
                        ViewBag.divid = task.Task.Department.DivisionID;
                        ViewBag.managers = new UserBL().getUsersList().Where(x => x.DivisionId == task.Task.Department.DivisionID || x.Role == 1 && x.IsActive == 1 && x.Id != task.User1.Id).ToList();

                    }
                    else if (task.User1.DepartmentId == null)
                    {
                        ViewBag.divid = task.User1.DivisionId; ;
                        ViewBag.managers = new UserBL().getUsersList().Where(x => x.DivisionId == task.User1.Division.Id || x.Role == 1 && x.IsActive == 1 && x.Id != task.User1.Id).ToList();

                    }
                    else
                    {
                        departmentid = new DepartmentBL().getDepartmentsById((int)task.User1.DepartmentId);
                        ViewBag.divid = departmentid.DivisionID;
                        ViewBag.managers = new UserBL().getUsersList().Where(x => x.DivisionId == departmentid.DivisionID || x.Role == 1 && x.IsActive == 1 && x.Id != task.User1.Id).ToList();

                    }
                }


                return View("ManagerTaskDetails", task);
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult updateStatus(int status, string managerGroupTask, string isModal = "", int cStatus = -1, string way = "", string loginId = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                DatabaseEntities de = new DatabaseEntities();
                int taskId = General_Purpose.DecryptId(managerGroupTask);

                List<Task_Ticket> t = new Task_TicketBL().Task_TicketswithoutWreapper(taskId, de).Where(x => x.CompletionDatetime == null).ToList();


                User_Task managerTask = new User_TaskBL().getUser_TasksByIdWrapper(General_Purpose.DecryptId(managerGroupTask), de);
                if (managerTask.Status != status)
                {
                    managerTask.Status = status;
                    if (cStatus == 4)
                    {
                        managerTask.CompletionDate = null;

                    }
                    if (status == 4)
                    {
                        managerTask.CompletionDate = DateTime.Now;
                        foreach (Task_Ticket x in t)
                        {
                            x.CompletedByUser = logedinuser.Id;
                            x.CompletionDatetime = DateTime.Now;
                            new Task_TicketBL().UpdateTask_Tickets(x, de);

                        }
                    }
                   // managerTask.CanvasCourseId = managerTask.CanvasCourseId;
                    new User_TaskBL().UpdateUser_Tasks(managerTask, de);
                    de.SaveChanges();
                    User u = new UserBL().getUsersById((int)managerTask.UserID);
                    Communication.sendMessage(new List<string>() { Messages.taskStatusUpdate(managerTask) }, new List<User>() { u });
                    data.TaskId = managerTask.Id.ToString();
                    General_Purpose.SendPushNotification(u.Player_Id, Messages.taskStatusUpdate(managerTask, 1), "Message from Zuptu", data, u.Id.ToString());

                }
                if (isModal == "")
                {
                    return RedirectToAction("taskDetails", new { v = General_Purpose.DecryptId(managerGroupTask), isModal = isModal, way = way, loginId = loginId });
                }
                return RedirectToAction("taskDetails", new { v = managerGroupTask, isModal = isModal, way = way, loginId = loginId });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        [HttpPost]
        public ActionResult addComment(string comment, string taskId, int repcomntid = -1, string isModal = "", string way = "", string loginId = "", int DocMID = -1, int FolderId = -1, string customLocPath = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                TaskComment mtc = new TaskComment();
                if (repcomntid != -1)
                {

                    mtc = new TaskComment() { TaskId = General_Purpose.DecryptId(taskId), Comment = comment, Date = DateTime.Now.ToString(), ParentId = (int)repcomntid };

                }
                else
                {
                    mtc = new TaskComment() { TaskId = General_Purpose.DecryptId(taskId), Comment = comment, Date = DateTime.Now.ToString() };
                }


                mtc.IsActive = 1;
                mtc.UserId = logedinuser.Id;

                if (DocMID != -1)
                {
                    File docfile = new FileBL().getFileById(DocMID);
                    
                    mtc.FileName = docfile.Name;
                    mtc.File = docfile.FilePath;
                }
                else
                {
                    int count = Request.Files.Count;
                    string path = null;
                    var file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        BlobManager BlobManagerObj = new BlobManager();
                        mtc.FileName = file.FileName;
                        path = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                        string FileAbsoluteUri = BlobManagerObj.UploadFile(file, path);

                        mtc.File = path;

                        //Wajeeh's Code for saving this file in document manager
                        if (customLocPath != "")
                        {
                            mtc.IsDocMFile = 1;

                            Models.File addfile = new Models.File()
                            {
                                Name = file.FileName,
                                FilePath = path,
                                IsActive = 1,
                                CreatedAt = DateTime.Now,
                                UploadingDate = DateTime.Now,
                                UploadedBy = logedinuser.Id,
                                LastModified = DateTime.Now
                            };
                            if (FolderId != -1)
                            {
                                Folder folder = new FolderBL().getFolderListByLogedinUser().Where(x => x.Id == FolderId).FirstOrDefault();
                                addfile.Privacy = folder.FolderPath.ToString();
                                addfile.FolderId = folder.Id;
                            }
                            else
                            {
                                addfile.Privacy = FolderId.ToString();
                            }

                            Models.File mainFileAdd = new Models.File();

                            mainFileAdd = new FileBL().AddFile(addfile);

                            if (mainFileAdd == null)
                            {
                                throw new Exception();
                            }
                            else
                            {
                                FileVersion version = new FileVersion()
                                {
                                    Name = mainFileAdd.Name,
                                    FilePath = mainFileAdd.FilePath,
                                    IsActive = mainFileAdd.IsActive,
                                    CreatedAt = mainFileAdd.CreatedAt,
                                    UploadingDate = mainFileAdd.UploadingDate,
                                    UploadedBy = mainFileAdd.UploadedBy,
                                    LastModified = mainFileAdd.LastModified,
                                    FileId = mainFileAdd.Id,
                                    FolderId = mainFileAdd.FolderId,
                                    Privacy = mainFileAdd.Privacy,
                                    ArchiveDate = mainFileAdd.ArchiveDate,
                                    CheckIn = mainFileAdd.CheckIn,
                                    CheckOut = mainFileAdd.CheckOut,
                                    CheckOutBy = mainFileAdd.CheckOutBy,
                                    CurrentVersion = 1,
                                    VersionNo = 1,
                                    CheckInTime = mainFileAdd.CheckInTime,
                                    CheckOutTime = mainFileAdd.CheckOutTime
                                };

                                if (version.Privacy == null)
                                {
                                    version.Privacy = "0";
                                }

                                if (new FileVersionBL().AddFileVersion(version) == null)
                                {
                                    throw new Exception();
                                }
                                else
                                {
                                    DateTime date = DateTime.Now;

                                    //File Log entry
                                    FileLog filesLog = new FileLog()
                                    {
                                        UserId = logedinuser.Id,
                                        FileId = mainFileAdd.Id,
                                        LogTime = date,
                                        NoOfVersions = new FileVersionBL().getFileVersionListByLogedinUser().Where(y => y.FileId == mainFileAdd.Id).Count(),
                                        Role = logedinuser.Role,
                                        IsActive = 1,
                                        CreatedAt = date,
                                        Type = "New File Uploaded"
                                    };

                                    if (new FileLogBL().AddFileLog(filesLog) == null)
                                    {
                                        throw new Exception();
                                    }
                                }
                            }
                        }
                    }          

                }
                //Additional attributes
                User_Task ut5 = new User_TaskBL().getUser_TasksById((int)mtc.TaskId);
                mtc.CompanyId = Convert.ToInt32(logedinuser.Company);
                mtc.TaskStartDate = ut5.StartDate;
                mtc.TaskEndDate = ut5.EndDate;
                mtc.DepartmentId = (int)ut5.Task.DepartmentID;
                mtc.DivisionId = (int)ut5.Task.Department.DivisionID;
                mtc.CreatedBy = logedinuser.Id;
                new TaskCommentBL().AddTaskComments(mtc);
                User_Task tm = new User_TaskBL().getUser_TasksById(mtc.TaskId.Value);
                Communication.sendMessage(new List<string>() { Messages.taskComment(tm) }, new List<User>() { tm.User1 });
                data.TaskId = tm.Id.ToString();
                General_Purpose.SendPushNotification(tm.User1.Player_Id, Messages.taskComment(tm, 1), "Message from Zuptu", data, tm.User1.Id.ToString());

                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(mtc.TaskId.Value), isModal = isModal, way = way, loginId = loginId });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult increaseFreqTask(string taskId, string isModal = "", string way = "", string loginId = "", string FeqEndingDate = "")
        {
            try
            {
                int tId = General_Purpose.DecryptId(taskId);
                User_Task utask = new User_TaskBL().getUser_TasksById(tId);
                if (utask.ParentID != null)
                {
                    User_Task u = new User_TaskBL().getUser_TasksList().Where(x => x.ParentID == utask.ParentID).Last();

                    if (FeqEndingDate == "")
                    {
                        return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(tId), message = "Task has not been assigned because you have not set frequency ending date.", isModal = isModal, way = way, loginId = loginId });
                    }
                    DateTime FEndingDate = Convert.ToDateTime(FeqEndingDate);
                    //if (FEndingDate < u.StartDate)
                    //{
                    //    return RedirectToAction("taskDetails", new { v = HttpUtility.UrlEncode(General_Purpose.Encrypt(tId)), message = "Frequency ending date should be greater than start date of last recursive task", isModal = isModal, way = way, loginId = loginId });

                    //}
                    if (FEndingDate <= Convert.ToDateTime(u.StartDate))
                    {
                        return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(tId), message = "Frequency ending date should be greater than start date of last recursive task", isModal = isModal, way = way, loginId = loginId });
                    }
                    // tm = ubl.AddUser_Tasks(tm);
                    // DateTime startDate = tm.StartDate.Value;
                    DateTime dt = u.StartDate.Value.AddDays(Convert.ToInt32(utask.RepeatTime));
                    while (FEndingDate.AddDays(1) > dt)
                    {
                        User_Task ut = new User_Task()
                        {
                            IsActive = 1,
                            IsPrivate = utask.IsPrivate,
                            StartDate = dt,
                            EndDate = dt,
                            Cost = utask.Cost,
                            CEU = utask.CEU,
                            Grad = utask.Grad,
                            UserID = utask.UserID,
                            TaskID = utask.TaskID,
                            CreatedID = logedinuser.Id,
                            CompletionDate = utask.CompletionDate,
                            File = utask.File,
                            RepeatDeadline = utask.RepeatDeadline,
                            Priority = utask.Priority,
                            //   CanvasCourseId = utask.CanvasCourseId,
                            Status = 0,
                            RepeatTime = utask.RepeatTime,
                            ParentID = utask.ParentID,





                        };
                        new User_TaskBL().AddUser_Tasks(ut);
                        int days = Convert.ToInt32(utask.RepeatTime);
                        if (days == 30)
                            dt = dt.AddMonths(1);
                        else if (days == 365)
                            dt = dt.AddYears(1);
                        else
                            dt = dt.AddDays(days);
                        //dt = dt.AddDays(Convert.ToInt32(utask.RepeatTime));




                    }
                }
                else
                {
                    User_Task uss = new User_TaskBL().getUser_TasksList().Where(x => x.ParentID == utask.Id).FirstOrDefault();
                    if (uss != null || utask != null)
                    {
                        User_Task u = new User_Task();
                        if (uss != null)
                            u = new User_TaskBL().getUser_TasksList().Where(x => x.ParentID == uss.ParentID).Last();
                        else
                        {
                            u = utask;
                            uss = utask;
                        }

                        if (FeqEndingDate == "")
                        {
                            return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(tId), message = "Task has not been assigned because you have not set frequency ending date.", isModal = isModal, way = way, loginId = loginId });

                        }
                        DateTime FEndingDate = Convert.ToDateTime(FeqEndingDate);

                        if (FEndingDate <= Convert.ToDateTime(u.StartDate))
                        {
                            return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(tId), message = "Frequency ending date should be greater than start date of last recursive task", isModal = isModal, way = way, loginId = loginId });
                        }

                        DateTime dt = u.StartDate.Value.AddDays(Convert.ToInt32(utask.RepeatTime));

                        if (uss.ParentID == null)
                            uss.ParentID = utask.Id;

                        while (FEndingDate.AddDays(1) > dt)
                        {
                            User_Task ut = new User_Task()
                            {
                                IsActive = 1,
                                IsPrivate = uss.IsPrivate,
                                StartDate = dt,
                                EndDate = dt,
                                Cost = uss.Cost,
                                CEU = uss.CEU,
                                Grad = uss.Grad,
                                UserID = uss.UserID,
                                TaskID = uss.TaskID,
                                CreatedID = logedinuser.Id,
                                CompletionDate = uss.CompletionDate,
                                File = uss.File,
                                RepeatDeadline = uss.RepeatDeadline,
                                Priority = uss.Priority,
                                // CanvasCourseId = uss.CanvasCourseId,
                                Status = 0,
                                RepeatTime = uss.RepeatTime,
                                ParentID = uss.ParentID,





                            };
                            new User_TaskBL().AddUser_Tasks(ut);
                            int days = Convert.ToInt32(uss.RepeatTime);
                            if (days == 30)
                                dt = dt.AddMonths(1);
                            else if (days == 365)
                                dt = dt.AddYears(1);
                            else
                                dt = dt.AddDays(days);
                            //     dt = dt.AddDays(Convert.ToInt32(uss.RepeatTime));




                        }
                    }
                    else
                    {
                        return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(tId), message = "Something went wrong!", isModal = isModal, way = way, loginId = loginId });


                    }

                }
                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(tId), message = "Recursive task increased successfully", isModal = isModal, way = way, loginId = loginId });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }

        }

        public ActionResult downloadFile(int v)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                TaskComment tc = new TaskCommentBL().getTaskCommentsById(v);

                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~") + ProjectVaraiables.IMAGEPATH + tc.File);
                string fileneame = tc.File;

                return File(Server.MapPath("~") + ProjectVaraiables.IMAGEPATH + tc.File, MediaTypeNames.Text.Plain, tc.File);
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult deleteAssignment()
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                User_TaskBL tmbl = new User_TaskBL();
                List<User_Task> tasks = tmbl.getUser_TasksList();

                GroupTasks_DetailsBL gtbl = new GroupTasks_DetailsBL();
                bool check = false;

                foreach (var item in tasks)
                {
                    string temp = "t" + item.Id;
                    if (Request.Form[temp] != null)
                    {
                        tmbl.DeleteUser_Tasks(item.Id);
                    }
                }

                return RedirectToAction("displayManagerTask");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult requestUpdate(ExtensionRequest er, string createdBy, string isModal = "", string way = "", string loginId = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                int maanagerTaskId = General_Purpose.DecryptId(createdBy);
                DatabaseEntities de = new DatabaseEntities();
                de.Configuration.ProxyCreationEnabled = true;
                de.Configuration.LazyLoadingEnabled = false;
                User_TaskBL tmbl = new User_TaskBL();
                User_Task tm = tmbl.getUser_TasksByIdWrapper(maanagerTaskId, de);


                string message = "You have updated the deadline.";

                if (er.RequestedDays < 0)
                {
                    message = "Added days cannot be negative.";
                }
                else
                {
                    tm.EndDate = tm.EndDate.Value.AddDays(er.RequestedDays.Value);

                   // tm.CanvasCourseId = tm.CanvasCourseId;
                    tmbl.UpdateUser_Tasks(tm, de);
                }
                de.SaveChanges();
                de.Configuration.ProxyCreationEnabled = false;
                de.Configuration.LazyLoadingEnabled = true;

                if (isModal == "")
                {
                    return RedirectToAction("taskDetails", new { v = maanagerTaskId, displayMessage = message, isModal = isModal, way = way, loginId = loginId });

                }
                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(maanagerTaskId), displayMessage = message, isModal = isModal, way = way, loginId = loginId });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        public ActionResult assigntoManager(string Time, int taskId, int priority, int? cost, int? freqDays, float? ceu, float? hours, int? Grad, int? CanvasCourseId, DateTime? startDate, DateTime? endDate, int days = -1, string notes = null, int managerId = -1, int divisionId = -1, string FeqEndingDate = "",int DReminder = -1)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                List<User> email = new List<User>();
                List<string> content = new List<string>();

                int role = logedinuser.Role;
                int id = logedinuser.Id;

                if (days != -1)
                {
                    switch (days)
                    {
                        case 1:
                            days = 1;
                            break;

                        case 2:
                            days = 7;

                            break;

                        case 3:
                            days = 30;
                            break;

                        case 4:
                            days = 365;
                            break;

                        case 5:
                            days = 730;
                            break;

                        case 6:
                            days = freqDays.Value;
                            break;

                        default:
                            Console.WriteLine("Default case");
                            break;
                    }
                }


                User_Task tm = new User_Task()
                {
                    TaskID = taskId,
                    UserID = managerId,
                    Cost = cost,
                    CEU = ceu,
                    Hours = hours,
                    Grad = Grad,
                    StartDate = startDate,
                    Notes = notes,
                    IsActive = 1,
                    Status = 0,
                    Priority = priority,
                    CanvasCourseId = CanvasCourseId
                };
                tm.CreatedID = id;
                if (Request["IsPrivate"] != null)
                {
                    tm.IsPrivate = 1;
                }
                else
                    tm.IsPrivate = 0;
                if (days != -1)
                {
                    tm.EndDate = tm.StartDate;
                    tm.RepeatTime = days;
                }
                else

                {
                    if (endDate == null)
                    {
                        return RedirectToAction("displayDivisionDetails", "Utilities", new { sid = General_Purpose.EncryptId(divisionId), message = "Task cannot be assigned to Manager because End date is not entered." });

                    }
                    tm.EndDate = endDate.Value;
                    DateTime dateTime = DateTime.Now;
                    DateTime timeValue = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0); ;
                    if (Time != null)
                    {
                        timeValue = Convert.ToDateTime(Time);
                    }

                    DateTime drt = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, timeValue.Hour, timeValue.Minute, timeValue.Second);
                    tm.EndDate = drt;
                }
                DatabaseEntities de = new DatabaseEntities();
                User User = new UserBL().getUsersById(tm.UserID.Value, de);

                email.Add(User);
                if (days == -1)
                {
                    if (DReminder != -1)
                    {
                        tm.DailyReminder = DReminder;
                    }
                    tm = new User_TaskBL().AddUser_Tasks(tm);
                    if (CanvasCourseId != null)
                    {
                        General_Purpose.AssignLMSCourse(tm.Id, (int)CanvasCourseId);
                    }
                }
                if (days != -1)
                {
                    if (FeqEndingDate == "")
                    {
                        return RedirectToAction("Index", "Auth", new { message = "Task has not been assigned because you have not set frequency ending date." });
                    }
                    DateTime FEndingDate = Convert.ToDateTime(FeqEndingDate);

                    if (FEndingDate < Convert.ToDateTime(tm.StartDate))
                    {
                        return RedirectToAction("Index", "Auth", new { message = "Task has not been assigned because task's start date must be greater than frequency ending date." });
                    }
                    if (DReminder != -1)
                    {
                        tm.DailyReminder = DReminder;
                    }
                    tm = new User_TaskBL().AddUser_Tasks(tm);
                    if (CanvasCourseId != null)
                        General_Purpose.AssignLMSCourse(tm.Id, (int)CanvasCourseId);
                    DateTime startDate1 = tm.StartDate.Value;
                    DateTime dt = tm.StartDate.Value.AddDays(days);
                    while (FEndingDate.AddDays(1) > dt)
                    {
                        User_Task ut = new User_Task()
                        {
                            IsActive = 1,
                            IsPrivate = tm.IsPrivate,
                            StartDate = dt,
                            EndDate = dt,
                            Cost = tm.Cost,
                            CEU = tm.CEU,
                            Grad = tm.Grad,
                            UserID = tm.UserID,
                            TaskID = tm.TaskID,
                            CreatedID = tm.CreatedID,
                            CompletionDate = tm.CompletionDate,
                            File = tm.File,
                            RepeatDeadline = tm.RepeatDeadline,
                            Priority = tm.Priority,
                            CanvasCourseId = tm.CanvasCourseId,
                            Status = 0,
                            RepeatTime = tm.RepeatTime,
                            ParentID = tm.Id,




                        };
                        if (DReminder != -1)
                        {
                            ut.DailyReminder = DReminder;
                        }
                        ut = new User_TaskBL().AddUser_Tasks(ut);
                        if (CanvasCourseId != null)
                        {
                            General_Purpose.AssignLMSCourse(ut.Id, (int)CanvasCourseId);
                        }
                        if (days == 30)
                            dt = dt.AddMonths(1);
                        else if (days == 365)
                            dt = dt.AddYears(1);
                        else
                            dt = dt.AddDays(days);
                      //  dt = dt.AddDays(days);


                    }

                }
                tm.User1 = User;
                //Canvas Course Assinging mail
                //if (CanvasCourseId != null)
                //{
                //    User admin = new UserBL().getUsersById(id);
                //    Course cr = General_Purpose.GetCanvasCourse().Where(x => x.id == CanvasCourseId).FirstOrDefault();
                //    string rle = "";
                //    if (User.Role == 3)
                //        rle = "Employee";
                //    else if (User.Role == 2 || User.Role == 4)
                //        rle = "Manager";
                //    else if (User.Role == 1 && User.IsMasterAdmin == 0)
                //        rle = "Admin";
                //    else
                //        rle = "Yourself";

                //    string maiil = "";
                //    if (User.CanvasLoginId != null)
                //        maiil = User.CanvasLoginId;
                //    else
                //    {
                //        maiil = User.Email;
                //        User.CanvasLoginId = User.Email;
                //    }



                //    new UserBL().UpdateUsers(User, de);

                //    string content1 = "Dear " + admin.FirstName + " " + admin.LastName + ",\n You assigned Canvas Course " + '"' + cr.name + '"' + " to " + rle + " " + '(' + User.FirstName + " " + User.LastName + ')' + " please send him Course assigning invitation from your Canvas Account on his Email " +
                //   maiil + "\n\n Thanks\n Team Zuptu";
                //    string title = "Canvas Course Assigning";
                //    MainMailClass mail = new MainMailClass();
                //    Course cour = General_Purpose.GetCanvasUserCoursesById(User.Id).Where(x => x.id == CanvasCourseId).FirstOrDefault();
                //    if (cour == null)
                //    {
                //        List<Course> Courses = General_Purpose.GetCanvasCourse();
                //        CanvasUser canvasuser = General_Purpose.IsUserInCanvasSystem(User.Id, Courses);
                //        if (canvasuser == null)
                //        {
                //            mail.CanvasCourseAssingingMail(admin.Email, content1, title);
                //        }
                //        else
                //        {
                //            string response = General_Purpose.UserEnrollmentInCanvas(canvasuser.id, Convert.ToInt32(CanvasCourseId));
                //            if (response == "OK")
                //            {
                //                ViewBag.msg = "Task Assigned Succesfully";
                //            }
                //            else
                //            {
                //                ViewBag.msg = "Oops! Something went wrong.";
                //            }
                //        }

                //    }






                //}

                tm.Task = new TaskBL().getTasksById(tm.TaskID.Value);
                content.Add(Messages.taskAward(tm));


                Communication.sendMessage(content, email);

                data.TaskId = tm.Id.ToString();
                General_Purpose.SendPushNotification(User.Player_Id, Messages.taskAward(tm, 1), "Message from Zuptu", data, User.Id.ToString());


                return RedirectToAction("displayDivisionDetails", "Utilities", new { sid = General_Purpose.EncryptId(divisionId), message = "Task has been assigned to Manager." });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult deleteComment(int commentId, int taskId, string isModal = "", string way = "", string loginId = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                //deleteing comment file from document manager - Wajeeh

                List<TaskComment> comments = new List<TaskComment>();

                TaskComment tm = new TaskCommentBL().getTaskCommentsById(commentId);

                if(tm.File != null)
                {
                    comments.Add(tm);
                }

                List<TaskComment> cms = new TaskCommentBL()
                    .getAllTaskCommentsList()
                    .Where(x => x.ParentId == tm.Id)
                    .ToList();

                if(cms.Count() > 0)
                {
                    foreach(TaskComment taskComment in cms)
                    {
                        if(taskComment.File != null)
                        {
                            comments.Add(taskComment);
                        }
                    }
                }

                if(comments.Count() > 0)
                {
                    foreach(TaskComment t in comments)
                    {
                        Models.File file = new FileBL().getFileListByLogedinUser().Where(x => x.FilePath == t.File).FirstOrDefault();
                        if (file != null)
                        {
                            file.IsActive = 0;
                            if (new FileBL().UpdateFile(file) != null)
                            {
                                List<FileVersion> versions = new FileVersionBL().getFileVersionListByLogedinUser()
                                    .Where(x => x.FileId == file.Id).ToList();

                                if (versions.Count() > 0)
                                {
                                    foreach (FileVersion o in versions)
                                    {
                                        o.IsActive = 0;
                                        if (new FileVersionBL().UpdateFileVersion(o) == null)
                                        {
                                            throw new Exception();
                                        }

                                    }
                                }
                                DateTime date = DateTime.Now;

                                //File Log entry
                                FileLog filesLog = new FileLog()
                                {
                                    UserId = logedinuser.Id,
                                    FileId = file.Id,
                                    LogTime = date,
                                    NoOfVersions = new FileVersionBL().getFileVersionListByLogedinUser().Where(y => y.FileId == file.Id).Count(),
                                    Role = logedinuser.Role,
                                    IsActive = 1,
                                    CreatedAt = date,
                                    Type = "File Deleted"
                                };

                                if (new FileLogBL().AddFileLog(filesLog) == null)
                                {
                                    throw new Exception();
                                }
                            }
                            else
                            {
                                throw new Exception();
                            }

                            t.IsDocMFile = null;
                        }
                    }
                }
                
                //ends

                new TaskCommentBL().DeleteTaskComments(commentId);

                return RedirectToAction("taskDetails", "Manager", new { v = General_Purpose.EncryptId(taskId), message = "Comment has been deleted.", isModal = isModal, way = way, loginId = loginId });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult updateComment(string comment, int commentId, int taskId, int removeFile, string isModal = "", string way = "", string loginId = "", int DocMID = -1, int FolderId = -1, string customLocPath = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                DatabaseEntities de = new DatabaseEntities();

                TaskComment tc = new TaskComment() { TaskId = taskId, Comment = comment };

                if (DocMID != -1)
                {
                    File docfile = new FileBL().getFileById(DocMID);

                    tc.FileName = docfile.Name;
                    tc.File = docfile.FilePath;
                }
                else
                {
                    int count = Request.Files.Count;
                    string path = null;
                    tc.File = null;
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        if (file.ContentLength > 0)
                        {

                            BlobManager BlobManagerObj = new BlobManager();
                            tc.FileName = file.FileName;
                            path = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                            string FileAbsoluteUri = BlobManagerObj.UploadFile(file, path);

                            tc.File = path;
                        }
                    }
                }


                TaskCommentBL gtmbl = new TaskCommentBL();
                TaskComment gtm = gtmbl.getTaskCommentsById(commentId, de);
                gtm.Comment = comment;
                if (tc.File != null)
                {
                    //Deleting previous file from document manager - Wajeeh
                    Models.File file = new FileBL().getFileListByLogedinUser().Where(x => x.FilePath == gtm.File).FirstOrDefault();
                    if (file != null)
                    {
                        file.IsActive = 0;
                        if (new FileBL().UpdateFile(file) != null)
                        {
                            List<FileVersion> versions = new FileVersionBL().getFileVersionListByLogedinUser()
                                .Where(x => x.FileId == file.Id).ToList();

                            if (versions.Count() > 0)
                            {
                                foreach (FileVersion o in versions)
                                {
                                    o.IsActive = 0;
                                    if (new FileVersionBL().UpdateFileVersion(o) == null)
                                    {
                                        throw new Exception();
                                    }

                                }
                            }
                            DateTime date = DateTime.Now;

                            //File Log entry
                            FileLog filesLog = new FileLog()
                            {
                                UserId = logedinuser.Id,
                                FileId = file.Id,
                                LogTime = date,
                                NoOfVersions = new FileVersionBL().getFileVersionListByLogedinUser().Where(y => y.FileId == file.Id).Count(),
                                Role = logedinuser.Role,
                                IsActive = 1,
                                CreatedAt = date,
                                Type = "File Deleted"
                            };

                            if (new FileLogBL().AddFileLog(filesLog) == null)
                            {
                                throw new Exception();
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }

                        gtm.IsDocMFile = null;
                    }
                    //ends

                    gtm.File = tc.File;
                    gtm.FileName = tc.FileName;

                    //Wajeeh's Code for saving this file in document manager
                    if (customLocPath != "")
                    {
                        gtm.IsDocMFile = 1;

                        Models.File addfile = new Models.File()
                        {
                            Name = tc.FileName,
                            FilePath = tc.File,
                            IsActive = 1,
                            CreatedAt = DateTime.Now,
                            UploadingDate = DateTime.Now,
                            UploadedBy = logedinuser.Id,
                            LastModified = DateTime.Now
                        };
                        if (FolderId != -1)
                        {
                            Folder folder = new FolderBL().getFolderListByLogedinUser().Where(x => x.Id == FolderId).FirstOrDefault();
                            addfile.Privacy = folder.FolderPath.ToString();
                            addfile.FolderId = folder.Id;
                        }
                        else
                        {
                            addfile.Privacy = FolderId.ToString();
                        }

                        Models.File mainFileAdd = new Models.File();

                        mainFileAdd = new FileBL().AddFile(addfile);

                        if (mainFileAdd == null)
                        {
                            throw new Exception();
                        }
                        else
                        {
                            FileVersion version = new FileVersion()
                            {
                                Name = mainFileAdd.Name,
                                FilePath = mainFileAdd.FilePath,
                                IsActive = mainFileAdd.IsActive,
                                CreatedAt = mainFileAdd.CreatedAt,
                                UploadingDate = mainFileAdd.UploadingDate,
                                UploadedBy = mainFileAdd.UploadedBy,
                                LastModified = mainFileAdd.LastModified,
                                FileId = mainFileAdd.Id,
                                FolderId = mainFileAdd.FolderId,
                                Privacy = mainFileAdd.Privacy,
                                ArchiveDate = mainFileAdd.ArchiveDate,
                                CheckIn = mainFileAdd.CheckIn,
                                CheckOut = mainFileAdd.CheckOut,
                                CheckOutBy = mainFileAdd.CheckOutBy,
                                CurrentVersion = 1,
                                VersionNo = 1,
                                CheckInTime = mainFileAdd.CheckInTime,
                                CheckOutTime = mainFileAdd.CheckOutTime
                            };

                            if (version.Privacy == null)
                            {
                                version.Privacy = "0";
                            }

                            if (new FileVersionBL().AddFileVersion(version) == null)
                            {
                                throw new Exception();
                            }
                            else
                            {
                                DateTime date = DateTime.Now;

                                //File Log entry
                                FileLog filesLog = new FileLog()
                                {
                                    UserId = logedinuser.Id,
                                    FileId = mainFileAdd.Id,
                                    LogTime = date,
                                    NoOfVersions = new FileVersionBL().getFileVersionListByLogedinUser().Where(y => y.FileId == mainFileAdd.Id).Count(),
                                    Role = logedinuser.Role,
                                    IsActive = 1,
                                    CreatedAt = date,
                                    Type = "New File Uploaded"
                                };

                                if (new FileLogBL().AddFileLog(filesLog) == null)
                                {
                                    throw new Exception();
                                }
                            }
                        }
                    }
                }
                else if (removeFile == 1)
                {
                    //Wajeeh's Code for deleting file in document manager

                    Models.File file = new FileBL().getFileListByLogedinUser().Where(x => x.FilePath == gtm.File).FirstOrDefault();
                    if(file != null)
                    {
                        file.IsActive = 0;
                        if(new FileBL().UpdateFile(file) != null)
                        {
                            List<FileVersion> versions = new FileVersionBL().getFileVersionListByLogedinUser()
                                .Where(x => x.FileId == file.Id).ToList();

                            if(versions.Count() > 0)
                            {
                                foreach(FileVersion o in versions)
                                {
                                    o.IsActive = 0;
                                    if(new FileVersionBL().UpdateFileVersion(o) == null)
                                    {
                                        throw new Exception();
                                    }
                                  
                                }
                            }
                            DateTime date = DateTime.Now;

                            //File Log entry
                            FileLog filesLog = new FileLog()
                            {
                                UserId = logedinuser.Id,
                                FileId = file.Id,
                                LogTime = date,
                                NoOfVersions = new FileVersionBL().getFileVersionListByLogedinUser().Where(y => y.FileId == file.Id).Count(),
                                Role = logedinuser.Role,
                                IsActive = 1,
                                CreatedAt = date,
                                Type = "File Deleted"
                            };

                            if (new FileLogBL().AddFileLog(filesLog) == null)
                            {
                                throw new Exception();
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }

                        gtm.IsDocMFile = null;
                    }
                   
                    //ends

                    gtm.File = null;
                    gtm.FileName = null;
                }
                gtmbl.UpdateTaskComments(gtm, de);


                return RedirectToAction("taskDetails", "Manager", new { v = General_Purpose.EncryptId(taskId), message = "Comment has been Updated.", isModal = isModal, way = way, loginId = loginId });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        public ActionResult addTasktoDO(Task_Ticket gtt, string Starttime = "", string Endtime = "", string isModal = "", string way = "", string loginId = "", int DocMID = -1, int FolderId = -1, string customLocPath = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                gtt.CreationDatetime = DateTime.Now;
                gtt.IsActive = 1;
                gtt.CreatedBy = logedinuser.Id;

                User_Task usertask = new User_TaskBL().getUser_TasksById(Convert.ToInt32(gtt.UserTask_Id));
                usertask.StartDate = Convert.ToDateTime(usertask.StartDate).Date;
                usertask.EndDate = Convert.ToDateTime(usertask.EndDate).Date;

                if (gtt.Name == null)
                {
                    return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "Todo name not be null.", isModal = isModal, way = way, loginId = loginId });

                }



                if (gtt.StartDate != null && gtt.EndDate != null)
                {




                    DateTime sd = Convert.ToDateTime(gtt.StartDate);
                    DateTime ed = Convert.ToDateTime(gtt.EndDate);

                    string StartDate = "";
                    string EndDate = "";
                    if (Starttime != "")
                        StartDate = sd.Date.ToString().Split(' ')[0];
                    else
                        StartDate = sd.Date.ToString();

                    if (Endtime != "")
                        EndDate = ed.Date.ToString().Split(' ')[0];
                    else
                        EndDate = ed.Date.ToString();

                    if (Starttime != "")
                    {
                        StartDate = StartDate + " " + Starttime;
                    }
                    if (Endtime != "")
                    {
                        EndDate = EndDate + " " + Endtime;
                    }
                    gtt.StartDate = Convert.ToDateTime(StartDate);
                    gtt.EndDate = Convert.ToDateTime(EndDate);

                    DateTime sd2 = Convert.ToDateTime(gtt.StartDate).Date;
                    DateTime ed2 = Convert.ToDateTime(gtt.EndDate).Date;

                    if (sd2 < usertask.StartDate)
                    {

                        return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "Start date must be equal or greater than Task's start date", isModal = isModal, way = way, loginId = loginId });

                    }
                    else if (ed2 > usertask.EndDate)
                    {

                        return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "End date must be equal or less than Task's end date.", isModal = isModal, way = way, loginId = loginId });


                    }
                    else if (Convert.ToDateTime(sd2).Date == Convert.ToDateTime(ed2).Date)
                    {

                        long sticks = Convert.ToDateTime(gtt.StartDate).Ticks;
                        long eticks = Convert.ToDateTime(gtt.EndDate).Ticks;
                        if (sticks < eticks)
                        {

                        }
                        else
                        {
                            return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "Time period must be valid.", isModal = isModal, way = way, loginId = loginId });

                        }

                        //string st = gtt.StartDate.ToString().Split(' ')[1] + " " + gtt.StartDate.ToString().Split(' ')[2];
                        //string et = gtt.EndDate.ToString().Split(' ')[1] + " " + gtt.EndDate.ToString().Split(' ')[2];
                        //string ampm = st.Split(' ')[1];
                        //string ampm2 = et.Split(' ')[1];

                        //if (ampm == "AM" && ampm2 == "PM")
                        //{

                        //}
                        //else
                        //{
                        //    return RedirectToAction("taskDetails", new { v = HttpUtility.UrlEncode(General_Purpose.Encrypt(gtt.UserTask_Id.Value)), message = "Time period must be valid.", isModal = isModal, way = way, loginId = loginId });

                        //}

                    }
                    else if (sd2 > ed2)
                    {
                        return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "Start date must be lees than end date.", isModal = isModal, way = way, loginId = loginId });

                    }


                }
                else
                {
                    return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "Start date and end date not be null.", isModal = isModal, way = way, loginId = loginId });

                }


                if (DocMID != -1)
                {
                    File docfile = new FileBL().getFileById(DocMID);

                    gtt.Ticket_FileName = docfile.Name;
                    gtt.Ticket_File = docfile.FilePath;
                    gtt.FileUploadDate = DateTime.Now;

                }
                else
                {
                    int count = Request.Files.Count;

                    var file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        BlobManager BlobManagerObj = new BlobManager(ProjectVaraiables.COMPANY_ALPHABET + Convert.ToInt32(logedinuser.Company));
                        gtt.Ticket_FileName = file.FileName;
                        string fileName = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                        string FileAbsoluteUri = BlobManagerObj.UploadFile(file, fileName);

                        gtt.Ticket_File = fileName;
                        gtt.FileUploadDate = DateTime.Now;

                        //Wajeeh's Code for saving this file in document manager
                        if (customLocPath != "")
                        {
                            gtt.IsDocMFile = 1;

                            Models.File addfile = new Models.File()
                            {
                                Name = file.FileName,
                                FilePath = fileName,
                                IsActive = 1,
                                CreatedAt = DateTime.Now,
                                UploadingDate = DateTime.Now,
                                UploadedBy = logedinuser.Id,
                                LastModified = DateTime.Now
                            };
                            if (FolderId != -1)
                            {
                                Folder folder = new FolderBL().getFolderListByLogedinUser().Where(x => x.Id == FolderId).FirstOrDefault();
                                addfile.Privacy = folder.FolderPath.ToString();
                                addfile.FolderId = folder.Id;
                            }
                            else
                            {
                                addfile.Privacy = FolderId.ToString();
                            }

                            Models.File mainFileAdd = new Models.File();

                            mainFileAdd = new FileBL().AddFile(addfile);

                            if (mainFileAdd == null)
                            {
                                throw new Exception();
                            }
                            else
                            {
                                FileVersion version = new FileVersion()
                                {
                                    Name = mainFileAdd.Name,
                                    FilePath = mainFileAdd.FilePath,
                                    IsActive = mainFileAdd.IsActive,
                                    CreatedAt = mainFileAdd.CreatedAt,
                                    UploadingDate = mainFileAdd.UploadingDate,
                                    UploadedBy = mainFileAdd.UploadedBy,
                                    LastModified = mainFileAdd.LastModified,
                                    FileId = mainFileAdd.Id,
                                    FolderId = mainFileAdd.FolderId,
                                    Privacy = mainFileAdd.Privacy,
                                    ArchiveDate = mainFileAdd.ArchiveDate,
                                    CheckIn = mainFileAdd.CheckIn,
                                    CheckOut = mainFileAdd.CheckOut,
                                    CheckOutBy = mainFileAdd.CheckOutBy,
                                    CurrentVersion = 1,
                                    VersionNo = 1,
                                    CheckInTime = mainFileAdd.CheckInTime,
                                    CheckOutTime = mainFileAdd.CheckOutTime
                                };

                                if (version.Privacy == null)
                                {
                                    version.Privacy = "0";
                                }

                                if (new FileVersionBL().AddFileVersion(version) == null)
                                {
                                    throw new Exception();
                                }
                                else
                                {
                                    DateTime date = DateTime.Now;

                                    //File Log entry
                                    FileLog filesLog = new FileLog()
                                    {
                                        UserId = logedinuser.Id,
                                        FileId = mainFileAdd.Id,
                                        LogTime = date,
                                        NoOfVersions = new FileVersionBL().getFileVersionListByLogedinUser().Where(y => y.FileId == mainFileAdd.Id).Count(),
                                        Role = logedinuser.Role,
                                        IsActive = 1,
                                        CreatedAt = date,
                                        Type = "New File Uploaded"
                                    };

                                    if (new FileLogBL().AddFileLog(filesLog) == null)
                                    {
                                        throw new Exception();
                                    }
                                }
                            }
                        }
                    }
                }

                int c = usertask.Task_Ticket.Max(x => x.Position).GetValueOrDefault();
                gtt.Position = c + 1;

                //Additional attributes
                User_Task ut4 = new User_TaskBL().getUser_TasksById((int)gtt.UserTask_Id);
                gtt.CompanyId = Convert.ToInt32(logedinuser.Company);
                gtt.TaskStartDate = ut4.StartDate;
                gtt.TaskEndDate = ut4.EndDate;
                gtt.DepartmentId = (int)ut4.Task.DepartmentID;
                gtt.DivisionId = (int)ut4.Task.Department.DivisionID;

                new Task_TicketBL().AddTask_Tickets(gtt);
                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "To Do has been added.", isModal = isModal, way = way, loginId = loginId });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult editTaskTicket(int ticketId, string name = "", string StartDate = "", string Starttime = "", string EndDate = "", string Endtime = "", string isModal = "", string way = "", string loginId = "", int DocMID = -1, int FolderId = -1, string customLocPath = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                DatabaseEntities de = new DatabaseEntities();
                de.Configuration.ProxyCreationEnabled = true;
                de.Configuration.LazyLoadingEnabled = false;
                Task_Ticket gtt = de.Task_Ticket.FirstOrDefault(x => x.Id == ticketId);

                User_Task usertask = new User_TaskBL().getUser_TasksById(Convert.ToInt32(gtt.UserTask_Id));
                usertask.StartDate = Convert.ToDateTime(usertask.StartDate).Date;
                usertask.EndDate = Convert.ToDateTime(usertask.EndDate).Date;

                int id = logedinuser.Id;
                int role = logedinuser.Id;

                gtt.Name = name + " qqqqaaaa ";

                if (name == "")
                {
                    return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "Todo name not be null.", isModal = isModal, way = way, loginId = loginId });

                }

                if (StartDate != "" && EndDate != "")
                {
                    DateTime sd = Convert.ToDateTime(StartDate);
                    DateTime ed = Convert.ToDateTime(EndDate);


                    string StartDat = "";
                    string EndDat = "";
                    if (Starttime != "")
                        StartDat = sd.Date.ToString().Split(' ')[0];
                    else
                        StartDat = sd.Date.ToString();

                    if (Endtime != "")
                        EndDat = ed.Date.ToString().Split(' ')[0];
                    else
                        EndDat = ed.Date.ToString();

                    if (Starttime != "")
                    {
                        StartDat = StartDat + " " + Starttime;
                    }
                    if (Endtime != "")
                    {
                        EndDat = EndDat + " " + Endtime;
                    }
                    gtt.StartDate = Convert.ToDateTime(StartDat);
                    gtt.EndDate = Convert.ToDateTime(EndDat);

                    DateTime sd2 = Convert.ToDateTime(gtt.StartDate).Date;
                    DateTime ed2 = Convert.ToDateTime(gtt.EndDate).Date;

                    if (sd2 < usertask.StartDate)
                    {

                        return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "Start date must be equal or greater than Task's start date", isModal = isModal, way = way, loginId = loginId });

                    }
                    else if (ed2 > usertask.EndDate)
                    {

                        return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "End date must be equal or less than Task's end date.", isModal = isModal, way = way, loginId = loginId });


                    }
                    else if (Convert.ToDateTime(sd2).Date == Convert.ToDateTime(ed2).Date)
                    {
                        long sticks = Convert.ToDateTime(gtt.StartDate).Ticks;
                        long eticks = Convert.ToDateTime(gtt.EndDate).Ticks;
                        if (sticks < eticks)
                        {

                        }
                        else
                        {
                            return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "Time period must be valid.", isModal = isModal, way = way, loginId = loginId });

                        }


                    }
                    else if (sd2 > ed2)
                    {
                        return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "Start date must be lees than end date.", isModal = isModal, way = way, loginId = loginId });

                    }



                }
                else
                {
                    return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "Start date and end date not be null.", isModal = isModal, way = way, loginId = loginId });

                }


                if (DocMID != -1)
                {
                    File docfile = new FileBL().getFileById(DocMID);

                    gtt.Ticket_FileName = docfile.Name;
                    gtt.Ticket_File = docfile.FilePath;
                    gtt.FileUploadDate = DateTime.Now;

                }
                else
                {
                    int count = Request.Files.Count;

                    var file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        //Deleting previous file from document manager - Wajeeh
                        Models.File file1 = new FileBL().getFileListByLogedinUser().Where(x => x.FilePath == gtt.Ticket_File).FirstOrDefault();
                        if (file1 != null)
                        {
                            file1.IsActive = 0;
                            if (new FileBL().UpdateFile(file1) != null)
                            {
                                List<FileVersion> versions = new FileVersionBL().getFileVersionListByLogedinUser()
                                    .Where(x => x.FileId == file1.Id).ToList();

                                if (versions.Count() > 0)
                                {
                                    foreach (FileVersion o in versions)
                                    {
                                        o.IsActive = 0;
                                        if (new FileVersionBL().UpdateFileVersion(o) == null)
                                        {
                                            throw new Exception();
                                        }

                                    }
                                }
                                DateTime date = DateTime.Now;

                                //File Log entry
                                FileLog filesLog = new FileLog()
                                {
                                    UserId = logedinuser.Id,
                                    FileId = file1.Id,
                                    LogTime = date,
                                    NoOfVersions = new FileVersionBL().getFileVersionListByLogedinUser().Where(y => y.FileId == file1.Id).Count(),
                                    Role = logedinuser.Role,
                                    IsActive = 1,
                                    CreatedAt = date,
                                    Type = "File Deleted"
                                };

                                if (new FileLogBL().AddFileLog(filesLog) == null)
                                {
                                    throw new Exception();
                                }
                            }
                            else
                            {
                                throw new Exception();
                            }

                            gtt.IsDocMFile = null;
                        }
                        //ends

                        BlobManager BlobManagerObj = new BlobManager(ProjectVaraiables.COMPANY_ALPHABET + Convert.ToInt32(logedinuser.Company));
                        gtt.Ticket_FileName = file.FileName;
                        string fileName = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                        string FileAbsoluteUri = BlobManagerObj.UploadFile(file, fileName);

                        gtt.Ticket_File = fileName;
                        gtt.FileUploadDate = DateTime.Now;

                        //Wajeeh's Code for saving this file in document manager
                        if (customLocPath != "")
                        {
                            gtt.IsDocMFile = 1;

                            Models.File addfile = new Models.File()
                            {
                                Name = file.FileName,
                                FilePath = fileName,
                                IsActive = 1,
                                CreatedAt = DateTime.Now,
                                UploadingDate = DateTime.Now,
                                UploadedBy = logedinuser.Id,
                                LastModified = DateTime.Now
                            };
                            if (FolderId != -1)
                            {
                                Folder folder = new FolderBL().getFolderListByLogedinUser().Where(x => x.Id == FolderId).FirstOrDefault();
                                addfile.Privacy = folder.FolderPath.ToString();
                                addfile.FolderId = folder.Id;
                            }
                            else
                            {
                                addfile.Privacy = FolderId.ToString();
                            }

                            Models.File mainFileAdd = new Models.File();

                            mainFileAdd = new FileBL().AddFile(addfile);

                            if (mainFileAdd == null)
                            {
                                throw new Exception();
                            }
                            else
                            {
                                FileVersion version = new FileVersion()
                                {
                                    Name = mainFileAdd.Name,
                                    FilePath = mainFileAdd.FilePath,
                                    IsActive = mainFileAdd.IsActive,
                                    CreatedAt = mainFileAdd.CreatedAt,
                                    UploadingDate = mainFileAdd.UploadingDate,
                                    UploadedBy = mainFileAdd.UploadedBy,
                                    LastModified = mainFileAdd.LastModified,
                                    FileId = mainFileAdd.Id,
                                    FolderId = mainFileAdd.FolderId,
                                    Privacy = mainFileAdd.Privacy,
                                    ArchiveDate = mainFileAdd.ArchiveDate,
                                    CheckIn = mainFileAdd.CheckIn,
                                    CheckOut = mainFileAdd.CheckOut,
                                    CheckOutBy = mainFileAdd.CheckOutBy,
                                    CurrentVersion = 1,
                                    VersionNo = 1,
                                    CheckInTime = mainFileAdd.CheckInTime,
                                    CheckOutTime = mainFileAdd.CheckOutTime
                                };

                                if (version.Privacy == null)
                                {
                                    version.Privacy = "0";
                                }

                                if (new FileVersionBL().AddFileVersion(version) == null)
                                {
                                    throw new Exception();
                                }
                                else
                                {
                                    DateTime date = DateTime.Now;

                                    //File Log entry
                                    FileLog filesLog = new FileLog()
                                    {
                                        UserId = logedinuser.Id,
                                        FileId = mainFileAdd.Id,
                                        LogTime = date,
                                        NoOfVersions = new FileVersionBL().getFileVersionListByLogedinUser().Where(y => y.FileId == mainFileAdd.Id).Count(),
                                        Role = logedinuser.Role,
                                        IsActive = 1,
                                        CreatedAt = date,
                                        Type = "New File Uploaded"
                                    };

                                    if (new FileLogBL().AddFileLog(filesLog) == null)
                                    {
                                        throw new Exception();
                                    }
                                }
                            }
                        }

                    }
                }

                de.Entry(gtt).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                de.Configuration.LazyLoadingEnabled = true;
                de.Configuration.ProxyCreationEnabled = false;
                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "Your To Do has been updated.", isModal = isModal, way = way, loginId = loginId });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult deleteTaskTicket(int ticketId, string isModal = "", string way = "", string loginId = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                DatabaseEntities de = new DatabaseEntities();
                de.Configuration.ProxyCreationEnabled = true;
                de.Configuration.LazyLoadingEnabled = false;
                Task_Ticket gtt = de.Task_Ticket.FirstOrDefault(x => x.Id == ticketId);
                int id = logedinuser.Id;
                int role = logedinuser.Role;

                //Deleting previous file from document manager - Wajeeh
                Models.File file = new FileBL().getFileListByLogedinUser().Where(x => x.FilePath == gtt.Ticket_File).FirstOrDefault();
                if (file != null)
                {
                    file.IsActive = 0;
                    if (new FileBL().UpdateFile(file) != null)
                    {
                        List<FileVersion> versions = new FileVersionBL().getFileVersionListByLogedinUser()
                            .Where(x => x.FileId == file.Id).ToList();

                        if (versions.Count() > 0)
                        {
                            foreach (FileVersion o in versions)
                            {
                                o.IsActive = 0;
                                if (new FileVersionBL().UpdateFileVersion(o) == null)
                                {
                                    throw new Exception();
                                }

                            }
                        }
                        DateTime date = DateTime.Now;

                        //File Log entry
                        FileLog filesLog = new FileLog()
                        {
                            UserId = logedinuser.Id,
                            FileId = file.Id,
                            LogTime = date,
                            NoOfVersions = new FileVersionBL().getFileVersionListByLogedinUser().Where(y => y.FileId == file.Id).Count(),
                            Role = logedinuser.Role,
                            IsActive = 1,
                            CreatedAt = date,
                            Type = "File Deleted"
                        };

                        if (new FileLogBL().AddFileLog(filesLog) == null)
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }

                    gtt.IsDocMFile = null;
                }
                //ends

                gtt.IsActive = 0;

                de.Entry(gtt).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                de.Configuration.LazyLoadingEnabled = true;
                de.Configuration.ProxyCreationEnabled = false;
                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), message = "Your To Do has been deleted.", isModal = isModal, way = way, loginId = loginId });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult completeTaskTicket(int ticketId, string isModal = "", string way = "", string loginId = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                DatabaseEntities de = new DatabaseEntities();
                de.Configuration.ProxyCreationEnabled = true;
                de.Configuration.LazyLoadingEnabled = false;
                Task_Ticket gtt = de.Task_Ticket.FirstOrDefault(x => x.Id == ticketId);
                int id = logedinuser.Id;
                int role = logedinuser.Role;

                gtt.CompletedByUser = id;

                gtt.CompletionDatetime = DateTime.Now;
                de.Entry(gtt).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                de.Configuration.LazyLoadingEnabled = true;
                de.Configuration.ProxyCreationEnabled = false;
                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), msg = "Your todo has been completed.", isModal = isModal, way = way, loginId = loginId });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }



        // Calender Modals
        public string getManagerDepartmentDTOs()
        {
            User loggedinuser = new UserBL().getUsersById(logedinuser.Id);
            List<Department> alldepartments = new DepartmentBL().getDepartmentsList().Where(x => x.DivisionID == Convert.ToInt32(loggedinuser.DivisionId)).ToList();
            List<DepartmentDTO> alldepartmentDTOs = new List<DepartmentDTO>();

            foreach (Department d in alldepartments)
            {
                DepartmentDTO dTo = new DepartmentDTO()
                {
                    Id = d.Id,
                    Name = d.Name,
                    DivisionID = d.DivisionID,
                    IsActive = d.IsActive,
                    SessionRole = logedinuser.Role
                };
                alldepartmentDTOs.Add(dTo);
            }

            return JsonConvert.SerializeObject(alldepartmentDTOs, Formatting.Indented,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
               });
        }

        public string getManagerTaskDTOs(string departmentId)
        {

            List<Task> alltasks = new TaskBL().getAllTasksList().Where(x => x.DepartmentID == Convert.ToInt32(departmentId) && x.IsPrivate == 0 && x.IsActive == 1).ToList();
            List<TaskDTO> alltaskDTOs = new List<TaskDTO>();

            foreach (Task d in alltasks)
            {
                TaskDTO dTo = new TaskDTO()
                {
                    Id = d.Id,
                    Name = d.Name,
                    TaskTypeID = d.TaskTypeID,
                    IsActive = d.IsActive,
                    DivisionId = d.DivisionId,
                    Description = d.Description,
                    Cost_ = d.Cost_,
                    Hours = d.Hours,
                    CEU = d.CEU,
                    DepartmentID = d.DepartmentID,
                    File = d.File,
                    UserId = d.UserId,
                    IsPrivate = d.IsPrivate,
                    SessionRole = logedinuser.Role
                };
                alltaskDTOs.Add(dTo);
            }

            return JsonConvert.SerializeObject(alltaskDTOs, Formatting.Indented,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
               });
        }

        public string getManagerUserDTOs(string divisionId, string departmentId, string taskId, string assignmentvalue)
        {
            if (taskId != "0")
            {
                Task task = new TaskBL().getTasksById(Convert.ToInt32(taskId));

                if (assignmentvalue == "assignManagerEmployee")
                {
                    List<User> allemployees = new UserBL().getAllUsersList().Where(x => x.DepartmentId == Convert.ToInt32(departmentId) && x.Role == 3 && x.IsActive == 1).ToList();
                    List<UserDTO> alluserDTOs = new List<UserDTO>();

                    foreach (User d in allemployees)
                    {
                        if (d.Id != task.UserId)
                        {
                            UserDTO dTo = new UserDTO()
                            {
                                Id = d.Id,
                                Name = d.FirstName + ' ' + d.LastName,
                                SessionRole = logedinuser.Role,
                                AssignUserRole = "Employee"
                            };
                            alluserDTOs.Add(dTo);
                        }
                    }

                    return JsonConvert.SerializeObject(alluserDTOs, Formatting.Indented,
                   new JsonSerializerSettings()
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });
                }
                else
                {
                    TaskDTO dTo = new TaskDTO()
                    {
                        Id = task.Id,
                        Name = task.Name,
                        TaskTypeID = task.TaskTypeID,
                        IsActive = task.IsActive,
                        DivisionId = task.DivisionId,
                        Description = task.Description,
                        Cost_ = task.Cost_,
                        Hours = task.Hours,
                        CEU = task.CEU,
                        DepartmentID = task.DepartmentID,
                        File = "No User",
                        UserId = task.UserId,
                        IsPrivate = task.IsPrivate,
                        SessionRole = logedinuser.Role,
                      //  CourseId = Convert.ToInt32(task.CourseId)
                    };

                    return JsonConvert.SerializeObject(dTo, Formatting.Indented,
                   new JsonSerializerSettings()
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });
                }
            }
            else
            {

                if (assignmentvalue == "assignManagerEmployee")
                {
                    List<User> allemployees = new UserBL().getAllUsersList().Where(x => x.DepartmentId == Convert.ToInt32(departmentId) && x.Role == 3 && x.IsActive == 1).ToList();
                    List<UserDTO> alluserDTOs = new List<UserDTO>();

                    foreach (User d in allemployees)
                    {

                        UserDTO dTo = new UserDTO()
                        {
                            Id = d.Id,
                            Name = d.FirstName + ' ' + d.LastName,
                            SessionRole = logedinuser.Role,
                            AssignUserRole = "Employee"
                        };
                        alluserDTOs.Add(dTo);

                    }

                    return JsonConvert.SerializeObject(alluserDTOs, Formatting.Indented,
                   new JsonSerializerSettings()
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });
                }

            }
            return JsonConvert.SerializeObject(null, Formatting.Indented,
                 new JsonSerializerSettings()
                 {
                     ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                 });
        }

        public string getManagerAssignTaskDTO(string taskId)
        {
            TaskDTO dTo2 = new TaskDTO();
            if (taskId != "0")
            {
                Task d = new TaskBL().getTasksById(Convert.ToInt32(taskId));

                TaskDTO dTo = new TaskDTO()
                {
                    Id = d.Id,
                    Name = d.Name,
                    TaskTypeID = d.TaskTypeID,
                    IsActive = d.IsActive,
                    DivisionId = d.DivisionId,
                    Description = d.Description,
                    Cost_ = d.Cost_,
                    Hours = d.Hours,
                    CEU = d.CEU,
                    DepartmentID = d.DepartmentID,
                    File = d.File,
                    UserId = d.UserId,
                    IsPrivate = d.IsPrivate,
                    SessionRole = logedinuser.Role,
                   // CourseId = Convert.ToInt32(d.CourseId)

                };

                return JsonConvert.SerializeObject(dTo, Formatting.Indented,
                   new JsonSerializerSettings()
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });
            }
            return JsonConvert.SerializeObject(dTo2, Formatting.Indented,
                  new JsonSerializerSettings()
                  {
                      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                  });
        }

        public ActionResult assigntaskManager(string managertimelineDate, string Time, int? priority, int? cost, int? freqDays, float? ceu, float? hours, int? Grad, int? CanvasCourseId, int taskId = -1, int departmentId = -1, int days = -1, string notes = null, string managerId = null, string employeeId = null, int divisionId = -1, string newemptaskname = "", int newemptasktype = -1, string FeqEndingDate = "",string EndDate="", string StartDate="", int DReminder=-1)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                if (departmentId == -1)
                {
                    return RedirectToAction("Index", "Auth", new { message = "Department Name not be null.Task didn't add successfully." });

                }
                if (taskId == -1)
                {
                    return RedirectToAction("Index", "Auth", new { message = "Task not be null.Task didn't add successfully." });

                }

                if (taskId == 0)
                {




                    Department dept = new DepartmentBL().getDepartmentsById(departmentId);
                    if (newemptasktype == -1)
                    {
                        return RedirectToAction("Index", "Auth", new { message = "Task Type not be null.Task didn't add successfully." });
                    }

                    int taskcount = 0;
                    if (newemptaskname != "")
                    {
                        taskcount = dept.Tasks.Where(x => x.IsActive == 1 && x.Name.ToUpper().Equals(newemptaskname.ToUpper())).Count();

                    }
                    else
                    {
                        return RedirectToAction("Index", "Auth", new { message = "Task name not be null.Task didn't add successfully." });
                    }





                    if (taskcount > 0)
                    {

                        return RedirectToAction("Index", "Auth", new { message = "Task name already exists in the department. Task didn't add successfully." });

                    }
                    else
                    {


                        int idu = logedinuser.Id;


                        Task obj = new Task()
                        {
                            IsPrivate = 0,
                            IsActive = 1,
                            UserId = idu,
                            DepartmentID = departmentId,
                            TaskTypeID = newemptasktype,
                            Name = newemptaskname,
                            CEU = ceu,
                            Cost_ = cost,
                            Hours = hours,
                            Description = notes,
                          //  CourseId = CanvasCourseId

                        };


                        taskId = new TaskBL().AddTasks(obj);


                    }


                }



                ///////////////////////////////////////////////
                ///
                if (days != -1)
                {
                    switch (days)
                    {
                        case 1:
                            days = 1;
                            break;

                        case 2:
                            days = 7;

                            break;

                        case 3:
                            days = 30;
                            break;

                        case 4:
                            days = 365;
                            break;

                        case 5:
                            days = 730;
                            break;

                        case 6:
                            days = freqDays.Value;
                            break;

                        default:
                            Console.WriteLine("Default case");
                            break;
                    }
                }

                List<User> email = new List<User>();
                List<string> content = new List<string>();

                int role = logedinuser.Role;
                int id = logedinuser.Id;

                //  DateTime TimelineDate = Convert.ToDateTime(managertimelineDate);
                DateTime STimelineDate = Convert.ToDateTime(StartDate);//Custom Date
                DateTime ETimelineDate = Convert.ToDateTime(EndDate).AddDays(1);//Custom End date
                ETimelineDate = ETimelineDate.AddMinutes(-1);
                DateTime dateTime = DateTime.Now;
                DateTime timeValue = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
                if (Time != null)
                {
                    timeValue = Convert.ToDateTime(Time);
                    DateTime drt = new DateTime(ETimelineDate.Year, ETimelineDate.Month, ETimelineDate.Day, timeValue.Hour, timeValue.Minute, timeValue.Second);
                    ETimelineDate = drt;
                }
                

                User_Task tm = new User_Task()
                {
                    TaskID = taskId,
                    Priority = priority,
                    Cost = cost,
                    CEU = ceu,
                    Hours = hours,
                    Grad = Grad,
                    Notes = notes,
                    StartDate = STimelineDate,
                    CanvasCourseId = CanvasCourseId,
                    IsActive = 1,
                    Status = 0
                };

                //if (CanvasCourseId != null)
                //{
                //    tm.CanvasCourseId = CanvasCourseId;

                //}

                if (days != -1)
                {
                    tm.EndDate = tm.StartDate;
                    tm.RepeatTime = days;
                }
                else
                {
                    tm.EndDate = ETimelineDate;
                }
                if (employeeId != "" && employeeId != null)
                {
                    if (id != Convert.ToInt32(employeeId))
                        tm.CreatedID = id;
                }

                if (employeeId != null && employeeId != "")
                {
                    tm.UserID = Convert.ToInt32(employeeId);
                }

                if (managerId != null && managerId != "")
                {
                    tm.UserID = logedinuser.Id;
                }

                if ((employeeId == "" || employeeId == null) && (managerId == "" || managerId == null))
                {
                    return RedirectToAction("Index", "Auth", new { message = "Something went wrong!" });
                }

                if (Request["IsPrivate"] != null)
                {
                    tm.IsPrivate = 1;
                }
                else
                {
                    tm.IsPrivate = 0;
                }
                DatabaseEntities de = new DatabaseEntities();
                User User = new UserBL().getUsersById(tm.UserID.Value, de);
                email.Add(User);
                User_TaskBL ubl = new User_TaskBL();
                if (days == -1)
                {
                    if (DReminder != -1)
                    {
                        tm.DailyReminder = DReminder;
                    }
                    tm = ubl.AddUser_Tasks(tm);
                    if (CanvasCourseId != null)
                        General_Purpose.AssignLMSCourse(tm.Id, (int)CanvasCourseId);
                }
                if (days != -1)
                {
                    if (FeqEndingDate == "")
                    {
                        return RedirectToAction("Index", "Auth", new { message = "Task has not been assigned because you have not set frequency ending date." });
                    }
                    DateTime FEndingDate = Convert.ToDateTime(FeqEndingDate);

                    if (FEndingDate < Convert.ToDateTime(tm.StartDate))
                    {
                        return RedirectToAction("Index", "Auth", new { message = "Task has not been assigned because task's start date must be greater than frequency ending date." });
                    }
                    if (DReminder != -1)
                    {
                        tm.DailyReminder = DReminder;
                    }
                    tm = ubl.AddUser_Tasks(tm);
                    if (CanvasCourseId != null)
                        General_Purpose.AssignLMSCourse(tm.Id, (int)CanvasCourseId);
                    DateTime startDate = tm.StartDate.Value;
                    DateTime dt = tm.StartDate.Value.AddDays(days);
                    while (FEndingDate.AddDays(1) > dt)
                    {
                        User_Task ut = new User_Task()
                        {
                            IsActive = 1,
                            IsPrivate = tm.IsPrivate,
                            StartDate = dt,
                            EndDate = dt,
                            Cost = tm.Cost,
                            CEU = tm.CEU,
                            Grad = tm.Grad,
                            UserID = tm.UserID,
                            TaskID = tm.TaskID,
                            CreatedID = tm.CreatedID,
                            CompletionDate = tm.CompletionDate,
                            File = tm.File,
                            RepeatDeadline = tm.RepeatDeadline,
                            Priority = tm.Priority,
                            CanvasCourseId = tm.CanvasCourseId,
                            Status = 0,
                            RepeatTime = tm.RepeatTime,
                            ParentID = tm.Id




                        };
                        if (DReminder != -1)
                        {
                            ut.DailyReminder = DReminder;
                        }
                    
                        ut = ubl.AddUser_Tasks(ut);
                        if (CanvasCourseId != null)
                            General_Purpose.AssignLMSCourse(ut.Id, (int)CanvasCourseId);
                        if (days == 30)
                            dt = dt.AddMonths(1);
                        else if (days == 365)
                            dt = dt.AddYears(1);
                        else
                            dt = dt.AddDays(days);
                       // dt = dt.AddDays(days);


                    }

                }
                tm.User1 = User;
                //if (CanvasCourseId != null)
                //{
                //    User admin = new UserBL().getUsersById(id);
                //    Course cr = General_Purpose.GetCanvasCourse().Where(x => x.id == CanvasCourseId).FirstOrDefault();
                //    string rle = "";
                //    if (User.Role == 3)
                //        rle = "Employee";
                //    else if (User.Role == 1 && User.IsMasterAdmin == 0)
                //        rle = "Admin";
                //    else if (User.Role == 1 && User.IsMasterAdmin == 1)
                //        rle = "Master Admin";
                //    else
                //        rle = "Yourself";

                //    string maiil = "";
                //    if (User.CanvasLoginId != null)
                //        maiil = User.CanvasLoginId;
                //    else
                //    {
                //        maiil = User.Email;
                //        User.CanvasLoginId = User.Email;
                //    }



                //    new UserBL().UpdateUsers(User, de);

                //    string content1 = "Dear " + admin.FirstName + " " + admin.LastName + ",\n You assigned Canvas Course " + '"' + cr.name + '"' + " to " + rle + " " + '(' + User.FirstName + " " + User.LastName + ')' + " please ask your Admin to send Course assigning invitation from his Canvas Account on this Email " +
                //   maiil + "\n\n Thanks\n Team Zuptu";
                //    string title = "Canvas Course Assigning";
                //    MainMailClass mail = new MainMailClass();
                //    Course cour = General_Purpose.GetCanvasUserCoursesById(User.Id).Where(x => x.id == CanvasCourseId).FirstOrDefault();
                //    if (cour == null)
                //    {
                //        List<Course> Courses = General_Purpose.GetCanvasCourse();
                //        CanvasUser canvasuser = General_Purpose.IsUserInCanvasSystem(User.Id, Courses);
                //        if (canvasuser == null)
                //        {
                //            mail.CanvasCourseAssingingMail(admin.Email, content1, title);
                //        }
                //        else
                //        {
                //            string response = General_Purpose.UserEnrollmentInCanvas(canvasuser.id, Convert.ToInt32(CanvasCourseId));
                //            if (response == "OK")
                //            {
                //                ViewBag.msg = "Task Assigned Succesfully";
                //            }
                //            else
                //            {
                //                ViewBag.msg = "Oops! Something went wrong.";
                //            }
                //        }
                //    }






                //}

                tm.Task = new TaskBL().getTasksById(tm.TaskID.Value);
                content.Add(Messages.taskAward(tm));


                Communication.sendMessage(content, email);
                data.TaskId = tm.Id.ToString();
                General_Purpose.SendPushNotification(User.Player_Id, Messages.taskAward(tm, 1), "Message from Zuptu", data, User.Id.ToString());

                return RedirectToAction("Index", "Auth");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        #region Completed Assignments

        public ActionResult DisplayCompletedManagerTask()
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                if (logedinuser.Role == 1)
                {
                    ViewBag.role = 2;

                }
                int role = logedinuser.Role;
                ViewBag.sd = 0;
                ViewBag.ed = 0;
                ViewBag.cd = 0;
                string complettionName = string.Empty;
                int id = logedinuser.Id;
                if (role == 3)
                {
                    return Content("Unauthorised Access");
                }
                else if (role == 2 || role == 4)
                {
                    ViewBag.way = "MyTask";
                }
                else
                {
                }



                ViewBag.tags = new TagBL().getTagsList();

                ViewBag.taskTypes = new TaskTypeBL().getTaskTypesList();
                ViewBag.statuses = new List<int>() { 0, 1, 2, 3, 4 };
                ViewBag.completionListId = new List<int>() { 1, 2, 3 };
                ViewBag.completionList = new List<string>() { "In Process", "Completed", "Late" };

                ViewBag.priorities = new List<int>() { 0, 1, 2 }.ToList();
                List<Task> taskslist = new TaskBL().getTasksList().Where(x => x.IsActive == 1).ToList();
                ViewBag.TaskName = taskslist;


                List<User> userslist = new UserBL().getUsersList().Where(x => x.IsActive == 1 && (x.Role == 2 || x.Role == 4)).ToList();
                ViewBag.FirstLastName = userslist;
                List<Division> divlist = new DivisionBL().getDivisionsList().Where(x => x.IsActive == 1).ToList();
                ViewBag.DivList = divlist;

                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        #endregion

        #region DocM By Wajeeh
        [HttpPost]
        public string get_allfolders()
        {
            List<Folder> folders = new FolderBL().getFolderListByLogedinUser().ToList();
            List<FolderDTO> fols = new List<FolderDTO>();

            foreach(Folder fol in folders)
            {
                FolderDTO dto = new FolderDTO()
                {
                    Id = fol.Id,
                    Name = fol.Name,
                    UserId = fol.UserId,
                    FolderPath = fol.FolderPath,
                    ArchivedBy = fol.ArchivedBy,
                    ArchivedDate = fol.ArchivedDate,
                    ParentId = fol.ParentId,
                    LastModified = fol.LastModified,
                    IsActive = fol.IsActive
                };

                fols.Add(dto);
            }

            return JsonConvert.SerializeObject(fols, Formatting.Indented,
                       new JsonSerializerSettings()
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });
        }

        [HttpPost]
        public string get_folder(int Id)
        {
            Folder folder = new Folder();
            FolderDTO fol = new FolderDTO();

            string message = "";

            if (Id == -1)
            {
                fol.Id = Id;
                message = "Home";
                fol.PathText = message;

                return JsonConvert.SerializeObject(fol, Formatting.Indented,
                       new JsonSerializerSettings()
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });
            }
            else
            {
                folder = new FolderBL().getFolderListByLogedinUser().Where(x => x.Id == Id).FirstOrDefault();
            }

            

            string[] locations = folder.FolderPath.Split(',');
            if (locations.Count() > 1)
            {
                message = "Home / " + new FolderBL().getFolderById(Convert.ToInt32(locations[0])).Name;

                for (int i = 1; i < locations.Count(); i++)
                {
                    message = message + " / " + new FolderBL().getFolderById(Convert.ToInt32(locations[i])).Name;
                }
            }
            else if (locations.Count() == 1 && locations[0] != "0")
            {
                message = "Home / " + new FolderBL().getFolderById(Convert.ToInt32(locations[0])).Name;
            }
            else if (locations.Count() == 1 && locations[0] == "0")
            {
                message = "Home";
            }

            fol = new FolderDTO()
            {
                Id = folder.Id,
                Name = folder.Name,
                UserId = folder.UserId,
                FolderPath = folder.FolderPath,
                ArchivedBy = folder.ArchivedBy,
                ArchivedDate = folder.ArchivedDate,
                ParentId = folder.ParentId,
                LastModified = folder.LastModified,
                IsActive = folder.IsActive,
                PathText = message
            };

            return JsonConvert.SerializeObject(fol, Formatting.Indented,
                       new JsonSerializerSettings()
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });
        }

        [HttpPost]
        public string CreateFolder(int ParentId, string Name)
        {
            if (Name == null || Name == "")
            {
                return "Folder name is null!";
            }

            List<Folder> folders = new List<Folder>();

            if (ParentId == -1)
            {
                folders = new FolderBL().getFolderListByLogedinUser().Where(x => x.Name.ToString().ToUpper().Trim() == Name.ToString().ToUpper().Trim() && x.ParentId == null).ToList();
            }
            else
            {
                folders = new FolderBL().getFolderListByLogedinUser().Where(x => x.Name.ToString().ToUpper().Trim() == Name.ToString().ToUpper().Trim() && x.ParentId == ParentId).ToList();
            }

            if (folders.Count > 0)
            {
                return "Folder with same name exists already!";
            }

            string setForCreating = DateTime.Now.ToString("yymmddfff");

            Folder folder = new Folder()
            {
                Name = Name.ToString().Trim(),
                CreatedAt = DateTime.Now,
                IsActive = 1,
                UserId = logedinuser.Id,
                LastModified = DateTime.Now
            };

            if (ParentId != -1)
            {
                folder.ParentId = ParentId;
            }

            Folder returnedFolder = new FolderBL().AddFolderWithReturnValues(folder);

            if (returnedFolder != null)
            {
                //updating path
                if (ParentId == -1)
                {
                    returnedFolder.FolderPath = returnedFolder.Id.ToString();

                    if (new FolderBL().UpdateFolderWithReturnValues(returnedFolder) != null)
                    {
                        return JsonConvert.SerializeObject(Formatting.Indented,
                                                        new JsonSerializerSettings()
                                                        {
                                                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                        });
                    }
                    else
                    {
                        return "Error in creating folder!";
                    }
                }
                else
                {
                    Folder parentFolder = new FolderBL().getFolderById(ParentId);
                    returnedFolder.FolderPath = parentFolder.FolderPath + "," + returnedFolder.Id;

                    if (new FolderBL().UpdateFolderWithReturnValues(returnedFolder) != null)
                    {
                        return JsonConvert.SerializeObject(Formatting.Indented,
                                                        new JsonSerializerSettings()
                                                        {
                                                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                        });
                    }
                    else
                    {
                        return "Error in creating folder!";
                    }
                }
            }
            else
            {
                return "Error in creating folder!";
            }

            
        }
        #endregion
    }
}