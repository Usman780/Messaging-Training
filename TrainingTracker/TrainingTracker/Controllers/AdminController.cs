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

namespace TrainingTracker.Controllers
{
    public class AdminController : Controller
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
        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult displayAdminTask(string v="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                int role = logedinuser.Role;
                ViewBag.sd = 0;
                ViewBag.ed = 0;
                ViewBag.cd = 0;
                ViewBag.v = v;
                string complettionName = string.Empty;
                int id = logedinuser.Id;

                ViewBag.tags = new TagBL().getTagsList();


                ViewBag.taskTypes = new TaskTypeBL().getTaskTypesList();
                ViewBag.statuses = new List<int>() { 0, 1, 2, 3, 4 };
                ViewBag.completionListId = new List<int>() { 1, 2, 3 };
                ViewBag.completionList = new List<string>() { "In Process", "Completed", "Late" };

                ViewBag.completionIdName = complettionName;

                ViewBag.priorities = new List<int>() { 0, 1, 2 }.ToList();
                List<Task> taskslist = new TaskBL().getTasksList().Where(x => x.IsActive == 1).ToList();
                ViewBag.TaskName = taskslist;
                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult GetdisplayAdminTask(string v="",string startDate = "", string endDate = "", int tag = -1, string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int completionStatus = -1, int startD = 0, int end = 0, int complete = 0, int isCompleted = -1)
        {
            
            if (AuthenticateUser() == false)
            {
                return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
            }
            int role = logedinuser.Role;
            //List<User_Task> tasks = new User_TaskBL().getUser_TasksList().OrderByDescending(c => c.Id).Where(x => x.User1.Role == 1 && x.DivisionID==null && x.DepartmentID==null).ToList();
            List<User_Task> tasks = new List<User_Task>();

            if(isCompleted != -1)
            {
                tasks = new User_TaskBL().spGetUserTasksByRole(1, Convert.ToInt32(logedinuser.Company)).OrderByDescending(c => c.Id).Where(x => x.User1 != null && x.CompletionDate != null).ToList();
            }
            else
            {
                tasks = new User_TaskBL().spGetUserTasksByRole(1, Convert.ToInt32(logedinuser.Company)).OrderByDescending(c => c.Id).Where(x => x.User1 != null && x.CompletionDate==null).ToList();
            }
            tasks = tasks.Where(x => x.User1.Role == 1 && x.DivisionID == null && x.DepartmentID == null).ToList();


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
                return Content("Unauthorised Access");
            }
            else if (v != "")
            {
                if(logedinuser.IsMasterAdmin==1)
                tasks = tasks.Where(x => x.UserID != id).ToList();
                else
                tasks = tasks.Where(x => x.UserID != id && x.User1.IsMasterAdmin==0).ToList();
                

            }
            else
            {
                tasks = tasks.Where(x => x.UserID == id).ToList();
            }
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

            int start2 = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            //List<Department> departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();
            // List<User> managerlist = new UserBL().getActiveandInvitedUser().Where(x => x.CompanyID == (int)Session["Company"] && (x.Role == 2 || x.Role == 4) && (x.IsActive == 1 || x.IsActive == 2)).OrderBy(s => s.FirstName).ToList();


            int totalrows = tasks.Count();
            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                tasks = tasks.Where(x => x.User1.FirstName.ToLower().Contains(searchValue.ToLower()) || x.Task.Name.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalrowsafterfilterinig = tasks.Count();
            //sorting
            //users = users.OrderBy(sortColumnName + " " + sortDirection).ToList();

            // pagination
            tasks = tasks.Skip(start2).Take(length).ToList();

            //List<User> mnglist = new List<User>();
            List<EmployeeDTO> mnglist = new List<EmployeeDTO>();

            string priority2 = "";
            string status2 = "";
           
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



                EmployeeDTO obj = new EmployeeDTO()
                {

                    //IsActive=(int)x.IsActive,
                    Task = x.Task.Name,
                    StartDate = x.StartDate.Value.ToString("MM/dd/yyyy"),
                    EndtDate = x.EndDate.Value.ToString("MM/dd/yyyy"),
                    WorkStatus = status2,
                    Priority = priority2,
                    Name = x.User1.FirstName + " " + x.User1.LastName,
                    Id = x.Id,
                    //EncryptedId = HttpUtility.UrlEncode(General_Purpose.Encrypt(x.Id))
                    EncryptedId = General_Purpose.EncryptId(x.Id)

                };
                mnglist.Add(obj);



            }

            return Json(new { data = mnglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);

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

        public ActionResult deleteTaskFunction(int sid, string startDates = "", string endDates = "", int tags = -1, string taskNames = "", int taskTypes = 0, int statuss = -1, int completionStatuss = -1, int prioritys = -1, int isCompleted = -1, string v = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                new User_TaskBL().DeleteUser_Tasks(sid);

                if (isCompleted != -1)
                {
                    return RedirectToAction("DisplayCompletedAdminTask", new { v = v, startDate = startDates, endDate = endDates, tag = tags, taskName = taskNames, taskType = taskTypes, status = statuss, completionStatus = completionStatuss, priority = prioritys });
                }
                else
                {
                    return RedirectToAction("displayAdminTask", new { v = v, startDate = startDates, endDate = endDates, tag = tags, taskName = taskNames, taskType = taskTypes, status = statuss, completionStatus = completionStatuss, priority = prioritys });
                }
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult deleteTaskFunctionManagerPage(int sid, int empid)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                new User_TaskBL().DeleteUser_Tasks(sid);
                return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(empid) });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }



        //[OutputCache(Duration = 3600, VaryByParam = "v", Location = System.Web.UI.OutputCacheLocation.Client, NoStore = true)]
        public ActionResult taskDetails(string v, string displayMessage = null,string IsModal="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                int taskId = General_Purpose.DecryptId(v);
                User_Task task = new User_TaskBL().getUser_TasksById(taskId);
                ViewBag.currentStatus = task.Status;
                ViewBag.status = new List<int>() { 1, 2, 3, 4 }/*.Where(x => x > task.Status)*/;

                List<Task_Ticket> t = new Task_TicketBL().Task_TicketswithoutWreapper(taskId);
                ViewBag.completedTickets = t.Where(x => x.CompletionDatetime != null).ToList();
                ViewBag.uncompletedTickets = t.Where(x => x.CompletionDatetime == null).ToList();


                List<TaskCommentDTO> files = new List<TaskCommentDTO>();
                foreach (var item in task.TaskComments)
                {
                    TaskCommentDTO tcd = new TaskCommentDTO();
                    tcd.Comment = item.Comment;
                    tcd.Id = item.Id;
                    tcd.isManager = 1;
                    tcd.userId = item.User.Id;
                    tcd.CommentTime = Convert.ToDateTime(item.Date);
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
                            tcd.FileSize = General_Purpose.SizeSuffix(new FileInfo(Server.MapPath("~") + ProjectVaraiables.IMAGEPATH + item.File).Length);
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
                files = files.OrderBy(x => x.CommentTime).ToList();
                ViewBag.comments = files;
                if (displayMessage != null)
                    ViewBag.request = displayMessage;
                else
                    ViewBag.request = null;
                ViewBag.modal = IsModal;
                return View("AdminTaskDetails", task);
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult updateStatus(int status, string managerGroupTask,string isModal="", int cStatus = -1)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                DatabaseEntities de = new DatabaseEntities();
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
                    }
                    managerTask.CanvasCourseId = managerTask.CanvasCourseId;
                    new User_TaskBL().UpdateUser_Tasks(managerTask, de);
                    de.SaveChanges();
                    Communication.sendMessage(new List<string>() { Messages.taskStatusUpdate(managerTask) }, new List<User>() { managerTask.User1 });
                }
                //Communication.sendMessage(new List<string>() { Messages.taskStatusUpdate(managerTask) }, new List<User>() { managerTask.Manager });
                return RedirectToAction("taskDetails", new { v = managerGroupTask, isModal = isModal });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult addComment(string comment, string taskId)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                TaskComment mtc = new TaskComment() { TaskId = General_Purpose.DecryptId(taskId), Comment = comment, Date = DateTime.Now.ToString() };
                mtc.IsActive = 1;
                mtc.UserId = logedinuser.Id;
                int count = Request.Files.Count;
                string path = null;
                var file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    int id = logedinuser.Id;

                    mtc.FileName = file.FileName;
                    path = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                    mtc.File = path;
                    path = Server.MapPath("~") + ProjectVaraiables.IMAGEPATH + path;
                    file.SaveAs(path);
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
                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(mtc.TaskId.Value) });
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

        public ActionResult requestUpdate(ExtensionRequest er, string createdBy, string IsModal="")
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

                    tm.CanvasCourseId = tm.CanvasCourseId;
                    tmbl.UpdateUser_Tasks(tm, de);
                }
                de.SaveChanges();
                de.Configuration.ProxyCreationEnabled = false;
                de.Configuration.LazyLoadingEnabled = true;

                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(maanagerTaskId), displayMessage = message, IsModal = IsModal });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult assigntoManager(string Time, int taskId, int priority, int? cost, int? freqDays, float? ceu, float? hours,int? Grad, DateTime? startDate, DateTime? endDate, int days = -1, string notes = null,int managerId=-1,int divisionId=-1)
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
                    Priority = priority
                };
                if (Request["IsPrivate"] != null)
                {
                    tm.IsPrivate = 1;
                }
                else
                    tm.IsPrivate = 0;
                if (days != -1)
                {
                    tm.EndDate = tm.StartDate.Value.AddDays(days);
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

                User User = new UserBL().getUsersById(tm.UserID.Value);
                email.Add(User);

                tm = new User_TaskBL().AddUser_Tasks(tm);

                tm.User1 = User;
                tm.Task = new TaskBL().getTasksById(tm.TaskID.Value);
                content.Add(Messages.taskAward(tm));


                Communication.sendMessage(content, email);
                return RedirectToAction("displayDivisionDetails", "Utilities", new { sid = General_Purpose.EncryptId(divisionId), message = "Task has been assigned to Manager." });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult deleteComment(int commentId, int taskId)
        {
            try
            {
                new TaskCommentBL().DeleteTaskComments(commentId);

                return RedirectToAction("taskDetails", "Admin", new { v = General_Purpose.EncryptId(taskId), message = "Comment has been deleted." });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult updateComment(string comment, int commentId, int taskId,int removeFile)
        {
            try
            {
                DatabaseEntities de = new DatabaseEntities();

                TaskComment tc = new TaskComment() { TaskId = taskId, Comment = comment };
                int count = Request.Files.Count;
                string path = null;
                tc.File = null;
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        int id = logedinuser.Id;

                        path = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                        tc.File = path;
                        tc.FileName = file.FileName;
                        path = Server.MapPath("~") + ProjectVaraiables.IMAGEPATH + path;
                        file.SaveAs(path);
                    }
                }



                TaskCommentBL gtmbl = new TaskCommentBL();
                TaskComment gtm = gtmbl.getTaskCommentsById(commentId, de);
                gtm.Comment = comment;
                if (tc.File != null)
                {
                    gtm.File = tc.File;
                    gtm.FileName = tc.FileName;
                }
                else if (removeFile == 1)
                {
                    gtm.File = null;
                    gtm.FileName = null;
                }
                gtmbl.UpdateTaskComments(gtm, de);


                return RedirectToAction("taskDetails", "Admin", new { v = General_Purpose.EncryptId(taskId), message = "Comment has been Updated." });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }


        public ActionResult addTasktoDO(Task_Ticket gtt, string IsModal="")
        {
            try
            {
                gtt.CreationDatetime = DateTime.Now;
                gtt.IsActive = 1;
                gtt.CreatedBy = logedinuser.Id;

                //Additional attributes
                User_Task ut4 = new User_TaskBL().getUser_TasksById((int)gtt.UserTask_Id);
                gtt.CompanyId = Convert.ToInt32(logedinuser.Company);
                gtt.TaskStartDate = ut4.StartDate;
                gtt.TaskEndDate = ut4.EndDate;
                gtt.DepartmentId = (int)ut4.Task.DepartmentID;
                gtt.DivisionId = (int)ut4.Task.Department.DivisionID;

                new Task_TicketBL().AddTask_Tickets(gtt);
                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), msg = "Todo has been added.", IsModal = IsModal });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult editTaskTicket(int ticketId, string name)
        {
            try
            {
                DatabaseEntities de = new DatabaseEntities();
                de.Configuration.ProxyCreationEnabled = true;
                de.Configuration.LazyLoadingEnabled = false;
                Task_Ticket gtt = de.Task_Ticket.FirstOrDefault(x => x.Id == ticketId);
                int id = logedinuser.Id;
                int role = logedinuser.Role;

                gtt.Name = name;

                de.Entry(gtt).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                de.Configuration.LazyLoadingEnabled = true;
                de.Configuration.ProxyCreationEnabled = false;
                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), msg = "Your todo has been updated." });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult deleteTaskTicket(int ticketId)
        {
            try
            {
                DatabaseEntities de = new DatabaseEntities();
                de.Configuration.ProxyCreationEnabled = true;
                de.Configuration.LazyLoadingEnabled = false;
                Task_Ticket gtt = de.Task_Ticket.FirstOrDefault(x => x.Id == ticketId);
                int id = logedinuser.Id;
                int role = logedinuser.Role;

                gtt.IsActive = 0;

                de.Entry(gtt).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                de.Configuration.LazyLoadingEnabled = true;
                de.Configuration.ProxyCreationEnabled = false;
                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), msg = "Your todo has been deleted." });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }
        public ActionResult completeTaskTicket(int ticketId)
        {
            try
            {
                DatabaseEntities de = new DatabaseEntities();
                de.Configuration.ProxyCreationEnabled = true;
                de.Configuration.LazyLoadingEnabled = false;
                Task_Ticket gtt = de.Task_Ticket.FirstOrDefault(x => x.Id == ticketId);
                int id = logedinuser.Id;
                int role = logedinuser.Role;
                //if(role==(int)Enums.Role.Cordinator || role==(int)Enums.Role.Manager)
                //{
                gtt.CompletedByUser = id;
                //}
                //else if(role==(int)Enums.Role.Trainee)
                //{
                //    gtt.CompletedByTrainee = id;
                //}
                gtt.CompletionDatetime = DateTime.Now;
                de.Entry(gtt).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                de.Configuration.LazyLoadingEnabled = true;
                de.Configuration.ProxyCreationEnabled = false;
                return RedirectToAction("taskDetails", new { v = General_Purpose.EncryptId(gtt.UserTask_Id.Value), msg = "Your todo has been completed." });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }


        #region Completed Assignments

        public ActionResult DisplayCompletedAdminTask(string v = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                int role = logedinuser.Role;
                ViewBag.sd = 0;
                ViewBag.ed = 0;
                ViewBag.cd = 0;
                ViewBag.v = v;
                string complettionName = string.Empty;
                int id = logedinuser.Id;

                ViewBag.tags = new TagBL().getTagsList();


                ViewBag.taskTypes = new TaskTypeBL().getTaskTypesList();
                ViewBag.statuses = new List<int>() { 0, 1, 2, 3, 4 };
                ViewBag.completionListId = new List<int>() { 1, 2, 3 };
                ViewBag.completionList = new List<string>() { "In Process", "Completed", "Late" };

                ViewBag.completionIdName = complettionName;

                ViewBag.priorities = new List<int>() { 0, 1, 2 }.ToList();
                List<Task> taskslist = new TaskBL().getTasksList().Where(x => x.IsActive == 1).ToList();
                ViewBag.TaskName = taskslist;
                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        #endregion


    }

}