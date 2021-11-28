using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingTracker.BL;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;
namespace TrainingTracker.Controllers
{
    public class AnalysisController : Controller
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
        // GET: Analysis
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GroupTaskAnalysis(string gid, string sid, string isModal="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("Login", "Auth");
                }
                
                ViewBag.GrpTskId = sid;
                ViewBag.id = gid;
                ViewBag.isModal = isModal;
                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }

        }


        //[HttpGet]
        //public ActionResult getGantChart(string gid)
        //{
        //    if (AuthenticateUser() == false)
        //    {
        //        return RedirectToAction("Login", "Auth");
        //    }

        //    int groupTaskId = General_Purpose.DecryptId(gid);
        //    GroupTasks_Details gtd = new GroupTasks_DetailsBL().getGroupTasks_DetailssByIdWrapepr(groupTaskId);

        //    List<dynamic> list = new List<dynamic>();
        //    int id = 1;
        //    foreach (var item in gtd.GroupTask_Ticket.Where(x=>x.IsActive==1))
        //    {
        //        if (item.IsActive == 1)
        //        {
        //            if (item.CompletionDatetime != null)
        //            {
        //                dynamic obj = new System.Dynamic.ExpandoObject();
        //                if (item.Name.Contains("qqqqaaaa"))
        //                {
        //                    item.Name = item.Name.Replace(" qqqqaaaa ", "");
        //                }
        //                obj.title = item.Name;
        //                obj.startDate = item.CreationDatetime.Value.ToString("yyyy-MM-dd");
        //                obj.endDate = item.CompletionDatetime.Value.ToString("yyyy-MM-dd");
        //                obj.id = id;
        //                id++;
        //                if (item.CompletedByUser.HasValue)
        //                {
        //                    User t = new UserBL().getUsersById(item.CompletedByUser.Value);
        //                    obj.FirstName = t.FirstName;

        //                }
        //                else
        //                {
        //                    obj.FirstName = "";
        //                }
        //                list.Add(obj);
        //            }
        //        }

        //    }
        //    if(list.Count == 0)
        //    {
        //        list = null;
        //    }
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult getGantChart(string gid)
        {
            if (AuthenticateUser() == false)
            {
                return RedirectToAction("Login", "Auth");
            }

            int groupTaskId = General_Purpose.DecryptId(gid);
            GroupTasks_Details gtd = new GroupTasks_DetailsBL().getGroupTasks_DetailssByIdWrapepr(groupTaskId);
            
            List<TodoGantChartDTO> todolist = new List<TodoGantChartDTO>();
            int id = 1;
            foreach (var item in gtd.GroupTask_Ticket.Where(x => x.IsActive == 1))
            {
                if (item.IsActive == 1)
                {
                    if (item.CompletionDatetime != null)
                    {
                        TodoGantChartDTO obj = new TodoGantChartDTO();
                        if (item.Name.Contains("qqqqaaaa"))
                        {
                            item.Name = item.Name.Replace(" qqqqaaaa ", "");
                        }
                        obj.Id = id;
                        obj.TaskName = item.Name;

                        DateTime a = item.CreationDatetime.Value;
                        DateTime b = item.CompletionDatetime.Value;

                        TimeSpan duration = b - a;
                        int days = duration.Days;
                        if(days < 1)
                        {
                            b = b.AddDays(1);
                        }
                        obj.StartDate = a.ToString("MMMM dd, yyyy");
                        obj.EndDate = b.ToString("MMMM dd, yyyy");
                        
                        id++;
                        
                        if (item.CompletedByUser.HasValue)
                        {
                            User t = new UserBL().getUsersById(item.CompletedByUser.Value);
                            obj.UserName = t.FirstName;

                        }
                        else
                        {
                            obj.UserName = "";
                        }
                        todolist.Add(obj);
                    }
                }

            }
            if (todolist.Count == 0)
            {
                todolist = null;
            }
            return Json(todolist, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult getGroupPie(string gid)
        {
            if (AuthenticateUser() == false)
            {
                return RedirectToAction("Login", "Auth");
            }

            int groupTaskId = General_Purpose.DecryptId(gid);
            GroupTasks_Details gtd = new GroupTasks_DetailsBL().getGroupTasks_DetailssByIdWrapepr(groupTaskId);
            List<dynamic> list = new List<dynamic>();
            foreach (var item in gtd.GroupTask_User)
            {
                dynamic obj = new System.Dynamic.ExpandoObject();
                int count = 0;
                foreach (var tickets in gtd.GroupTask_Ticket)
                {
                   
                        if (tickets.CompletedByUser.HasValue)
                            if (tickets.CompletedByUser.Value == item.UserId)
                                count++;
                  
                }
                obj.Name = item.User.FirstName;
                obj.count = count;
                list.Add(obj);
            }
           
            dynamic obj1 = new System.Dynamic.ExpandoObject();
            obj1.Name = "Open";
            obj1.count = gtd.GroupTask_Ticket.Where(x => x.IsActive == 1 && x.CompletionDatetime == null).Count();
            list.Add(obj1);
            return Json(list, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult getTickets(string gid)
        {
            if (AuthenticateUser() == false)
            {
                return RedirectToAction("Login", "Auth");
            }

            int groupTaskId = General_Purpose.DecryptId(gid);
            GroupTasks_Details gtd = new GroupTasks_DetailsBL().getGroupTasks_DetailssByIdWrapepr(groupTaskId);
            List<dynamic> list = new List<dynamic>();

            int days = gtd.CompletionDate.HasValue ? gtd.CompletionDate.Value.Subtract(gtd.StartDate.Value).Days : DateTime.Now.Subtract(gtd.StartDate.Value).Days;
            int k = 0;
            for (int i = 0; i <= days; i++)
            {
                string dat = gtd.StartDate.Value.AddDays(i).ToString("yyyy-MM-dd");
                int count = gtd.GroupTask_Ticket.Where(x => x.IsActive == 1 && x.CompletionDatetime.HasValue && x.CompletionDatetime.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();
                if (count != 0)
                {
                    dynamic obj2 = new System.Dynamic.ExpandoObject();
                    obj2.date = dat;
                    obj2.count = count;
                    list.Add(obj2);
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult employeeTaskCompleted(string id)
        {
            if (AuthenticateUser() == false)
            {
                return RedirectToAction("Login", "Auth");
            }

            int employeeId = General_Purpose.DecryptId(id);
            User trainee = new UserBL().getUsersById(employeeId);
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-30);
            List<dynamic> list = new List<dynamic>();

            for (int i = 0; i < 30; i++)
            {

                string dat = dateTime.AddDays(i).ToString("yyyy-MM-dd");
                int count = trainee.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate != null && x.CompletionDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();
                dynamic obj2 = new System.Dynamic.ExpandoObject();
                obj2.date = dat;
                obj2.count = count;
                list.Add(obj2);
            }

            return Json(null, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult employeegroupTaskContribution(string id)
        {
            if (AuthenticateUser() == false)
            {
                return RedirectToAction("Login", "Auth");
            }

            int employeeId = General_Purpose.DecryptId(id);
            User trainee = new UserBL().getUsersById(employeeId);
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-30);
            List<dynamic> list = new List<dynamic>();

            for (int i = 0; i < 30; i++)
            {

                string dat = dateTime.AddDays(i).ToString("yyyy-MM-dd");
                int count = trainee.GroupTask_Ticket.Where(x => x.IsActive == 1 && x.CompletedByUser.HasValue && x.CompletionDatetime.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();
                dynamic obj2 = new System.Dynamic.ExpandoObject();
                obj2.date = dat;
                obj2.count = count;
                list.Add(obj2);
            }

            return Json(null, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult assignedCompletedTask(string id)
        {
            if (AuthenticateUser() == false)
            {
                return RedirectToAction("Login", "Auth");
            }

            int employeeId = General_Purpose.DecryptId(id);
            User trainee = new UserBL().getUsersById(employeeId);
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-30);
            List<dynamic> list = new List<dynamic>();

            for (int i = 0; i < 30; i++)
            {

                string dat = dateTime.AddDays(i).ToString("yyyy-MM-dd");
                int completed = trainee.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate != null && x.CompletionDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();
                int assigned = trainee.User_Task1.Where(x => x.IsActive == 1 && x.StartDate != null && x.StartDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();

                dynamic obj2 = new System.Dynamic.ExpandoObject();
                obj2.date = dat;
                obj2.count = completed;
                list.Add(obj2);
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }


        public ActionResult EmployeeAnalysis(string id, string id2, string sid="",string  way="")
        {
            try
            {
              
             
                int employeeId = General_Purpose.DecryptId(id);
                if (employeeId == 0)
                {
                    id = HttpUtility.UrlEncode(id);
                    ViewBag.emp1 = id;
                    employeeId = General_Purpose.DecryptId(id);
                }
                else
                {
                    ViewBag.emp1 = id;
                }


                int employeeId2 = General_Purpose.DecryptId(id2);
                if (employeeId2 == 0)
                {
                    id2 = HttpUtility.UrlEncode(id2);
                    ViewBag.emp2 = id2;
                    employeeId2 = General_Purpose.DecryptId(id2);
                }
                else
                {
                    ViewBag.emp2 = id2;
                }


                User employeeOne = new UserBL().getUsersById(employeeId);
                User employeeTwo = new UserBL().getUsersById(employeeId2);
                ViewBag.firstName = employeeOne.FirstName+" "+employeeOne.LastName;
                ViewBag.secondName = employeeTwo.FirstName+" "+employeeTwo.LastName;
                ViewBag.sid = sid;
                ViewBag.way = way;
                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }

        }


        [HttpGet]
        public ActionResult assignedAndCompletedDates(string id, string id2)
        {
            if (AuthenticateUser() == false)
            {
                return RedirectToAction("Login", "Auth");
            }
            DateTime dateTime = DateTime.Now.AddDays(-30);
            int employeeId = General_Purpose.DecryptId(id);
            int employeeId2 = General_Purpose.DecryptId(id2);
            User employeeOne = new UserBL().getUsersById(employeeId);
            User employeeTwo = new UserBL().getUsersById(employeeId2);
            List<dynamic> list = new List<dynamic>();
            for (int i = 0; i < 30; i++)
            {

                string dat = dateTime.AddDays(i).ToString("yyyy-MM-dd");
                int completedOne = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate != null && x.CompletionDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();
                int assignedOne = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.StartDate != null && x.StartDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();
                int completedTwo = employeeTwo.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate != null && x.CompletionDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();
                int assignedTwo = employeeTwo.User_Task1.Where(x => x.IsActive == 1 && x.StartDate != null && x.StartDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();

                dynamic obj2 = new System.Dynamic.ExpandoObject();
                obj2.date = dat;
                obj2.CompletedOne = completedOne;
                obj2.AssignedOne = assignedOne;
                obj2.CompletedTwo = completedTwo;

                list.Add(obj2);
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Percentage of completed Tasks per department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        #region departmentAnalysis

        public ActionResult departmentAnalysis(string sid, string way="")
        {
            try
            {
                string qa = HttpUtility.UrlEncode(sid);
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("Login", "Auth");
                }
                ViewBag.Id = sid;
                int x = General_Purpose.DecryptId(sid);
                string str = "";
                if (x == 0)
                {
                    str = HttpUtility.UrlEncode(sid);
                    x = General_Purpose.DecryptId(str);
                }
                Department dep = new DepartmentBL().getDepartmentsById(x);

                ViewBag.way = way;
                ViewBag.divId = dep.DivisionID;
                ViewBag.department = dep;
                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }

        }

        [HttpGet]
        public ActionResult departmentAnalysisData(string departmentId)
        {
            if (AuthenticateUser() == false)
            {
                return RedirectToAction("Login", "Auth");
            }
            int depId = General_Purpose.DecryptId(departmentId);
            string str = "";
            if(depId==0)
            {
                str = HttpUtility.UrlEncode(departmentId);
                depId= General_Purpose.DecryptId(str);
            }

            DateTime dat = DateTime.Now.AddDays(-30);
            Department department = new DepartmentBL().getDepartmentsById(depId);

            int completedIndividualTask = 0;

            List<string> employeesName = new List<string>();
            List<int> employeesCompletedTask = new List<int>();
            List<int> employeesIncompletedTask = new List<int>();
            List<int> employeesAssignedTask = new List<int>();
            List<int> employeesHighPriorityTasks = new List<int>();
            List<int> employeesLowPriorityTasks = new List<int>();
            List<int> employeesMediumPriorityTasks = new List<int>();
            List<int> employeesLateTask = new List<int>();
            //Task completion percentage in Last Month
            dynamic obj = new System.Dynamic.ExpandoObject();
            foreach (var item in department.Users.Where(x=>x.IsActive==1))
            {

                int completedOne = item.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate != null && x.CompletionDate.Value > dat).Count();
                int assignedOne = item.User_Task1.Where(x => x.IsActive == 1 && x.StartDate != null && x.StartDate.Value > dat).Count();
                completedIndividualTask = completedIndividualTask + completedOne;
                int lateTask = item.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate == null && x.EndDate < DateTime.Now).Count();
                int inc = item.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate == null && x.EndDate > DateTime.Now).Count();
                int high = item.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate == null && x.Priority == 2).Count();
                int medium = item.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate == null && x.Priority == 1).Count();
                int low = item.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate == null && x.Priority == 0).Count();

                employeesName.Add(item.FirstName);
                employeesAssignedTask.Add(assignedOne);
                employeesCompletedTask.Add(completedOne);
                employeesIncompletedTask.Add(inc);
                employeesLateTask.Add(lateTask);
                employeesLowPriorityTasks.Add(low);
                employeesHighPriorityTasks.Add(high);
                employeesMediumPriorityTasks.Add(medium);
            }

            obj.Names = employeesName;
            obj.High = employeesHighPriorityTasks;
            obj.Low = employeesLowPriorityTasks;
            obj.Medium = employeesMediumPriorityTasks;
            obj.Late = employeesLateTask;
            obj.Incomplete = employeesIncompletedTask;
            obj.Complete = employeesCompletedTask;
            obj.Assigned = employeesAssignedTask;
            return Json(obj, JsonRequestBehavior.AllowGet);

        }



        #endregion


        #region twoEmployeesComparision
        [HttpGet]
        public ActionResult employeesComparision(string id, string id2)
        {
            if (AuthenticateUser() == false)
            {
                return RedirectToAction("Login", "Auth");
            }
            DateTime dateTime = DateTime.Now.AddDays(-30);
            int employeeId = General_Purpose.DecryptId(id);
            int employeeId2 = General_Purpose.DecryptId(id2);
            User employeeOne = new UserBL().getUsersById(employeeId);
            User employeeTwo = new UserBL().getUsersById(employeeId2);


            int ticketsClosedOne = employeeOne.GroupTask_Ticket.Where(x => x.IsActive == 1 && x.IsActive == 1 && x.CompletedByUser.HasValue && x.CompletionDatetime > dateTime).Count();
            int ticketsClosedTwo = employeeTwo.GroupTask_Ticket.Where(x => x.IsActive == 1 && x.CompletedByUser.HasValue && x.CompletionDatetime > dateTime).Count();

            int employe1TaskCompleted = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate > dateTime).Count();
            int employe1TaskAssigned = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.StartDate > dateTime).Count();
            int employe1TaskLate = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate.HasValue == false && x.EndDate.Value < DateTime.Now && x.EndDate.Value > dateTime).Count();
            int employe1TaskOngoing = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate.HasValue == false && x.EndDate.Value > DateTime.Now && x.EndDate.Value > dateTime).Count();
            int employe1HighPriority = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 2 && x.EndDate.Value > dateTime).Count();
            int employe1MediumPriority = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 1 && x.EndDate.Value > dateTime).Count();
            int employe1LowPriority = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 0 && x.EndDate.Value > dateTime).Count();

            int employe2TaskCompleted = employeeTwo.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate > dateTime).Count();
            int employe2TaskAssigned = employeeTwo.User_Task1.Where(x => x.IsActive == 1 && x.StartDate > dateTime).Count();
            int employe2TaskLate = employeeTwo.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate.HasValue == false && x.EndDate.Value < DateTime.Now && x.EndDate.Value > dateTime).Count();
            int employe2TaskOngoing = employeeTwo.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate.HasValue == false && x.EndDate.Value > DateTime.Now && x.EndDate.Value > dateTime).Count();
            int employe2HighPriority = employeeTwo.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 2 && x.EndDate.Value > dateTime).Count();
            int employe2MediumPriority = employeeTwo.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 1 && x.EndDate.Value > dateTime).Count();
            int employe2LowPriority = employeeTwo.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 0 && x.EndDate.Value > dateTime).Count();


            List<int> emp1TComInD1 = new List<int>();
            List<int> emp2TComInD1 = new List<int>();
            List<string> dates = new List<string>();
            for (int i = 0; i < 30; i++)
            {

                string dat = dateTime.AddDays(i).ToString("yyyy-MM-dd");
                dates.Add(dat);
                int completedOneCount = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate != null && x.CompletionDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();

                int completedTwoCount = employeeTwo.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate != null && x.CompletionDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();
                emp1TComInD1.Add(completedOneCount);
                emp2TComInD1.Add(completedTwoCount);
            }
            dynamic obj = new System.Dynamic.ExpandoObject();
            obj.Employee1TaskCompleted = employe1TaskCompleted;
            obj.E1TCName = "Completed Tasks";
            obj.Employee1TaskAssigned = employe1TaskAssigned;
            obj.E1TAName = "Assigned Tasks";
            obj.Employee1TaskLate = employe1TaskLate;
            obj.E1TLName = "Late Tasks";
            obj.Employee1TaskOngoing = employe1TaskOngoing;
            obj.E1TOName = "Ongoing Tasks";

            obj.Employee2TaskCompleted = employe2TaskCompleted;
            obj.E2TCName = "Completed Tasks";
            obj.Employee2TaskAssigned = employe2TaskAssigned;
            obj.E2TAName = "Assigned Tasks";
            obj.Employee2TaskLate = employe2TaskLate;
            obj.E2TLName = "Late Tasks";
            obj.Employee2TaskOngoing = employe2TaskOngoing;
            obj.E2TOName = "Ongoing Tasks";

            obj.Employee1Last30dDaysTask = emp1TComInD1;

            obj.Employee2Last30dDaysTask = emp2TComInD1;
            obj.dates = dates;
            obj.firstEmployeeName = employeeOne.FirstName;
            obj.secondEmployeeName = employeeTwo.FirstName;

            obj.oneHigh = employe1HighPriority;
            obj.oneMedium = employe1MediumPriority;
            obj.oneLow = employe1LowPriority;

            obj.twoHigh = employe2HighPriority;
            obj.twpMedium = employe2MediumPriority;
            obj.twoLow = employe2LowPriority;


            return Json(obj, JsonRequestBehavior.AllowGet);

        }
        #endregion


        #region employeeAnalysis
        public ActionResult displayEmpAnalaysis(string sid="", string way="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("Login", "Auth");
                }
                int employeeId = General_Purpose.DecryptId(sid);
                User emp = new UserBL().getUsersById(employeeId);
                List<User> Users = new UserBL().getUsersList().Where(x => x.IsActive == 1 && x.Id != employeeId && x.Role == 3 && x.DepartmentId == emp.DepartmentId).ToList();
                User u = new UserBL().getUsersList().Where(x => x.Id == employeeId).FirstOrDefault();
                ViewBag.user = u;
                ViewBag.employeeName = Users;
                ViewBag.Id = sid;
                //additional 
                ViewBag.depId = emp.DepartmentId;
                ViewBag.way = way;
                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        [HttpGet]
        public ActionResult employeesAnalysisAjax(string id)
        {
            int employeeId = General_Purpose.DecryptId(id);

            DateTime dateTime = DateTime.Now.AddDays(-30);
            User employeeOne = new UserBL().getUsersById(employeeId);

            List<int> emp1TComInD1 = new List<int>();

            List<string> dates = new List<string>();
            for (int i = 0; i < 30; i++)
            {

                string dat = dateTime.AddDays(i).ToString("yyyy-MM-dd");
                dates.Add(dat);
                int completedOneCount = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate != null && x.CompletionDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();

                emp1TComInD1.Add(completedOneCount);
            }
            int ticketsClosedOne = employeeOne.GroupTask_Ticket.Where(x => x.IsActive == 1 && x.CompletedByUser.HasValue && x.CompletionDatetime > dateTime).Count();
            int employe1TaskCompleted = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate > dateTime).Count();
            int employe1TaskAssigned = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.StartDate > dateTime).Count();
            int employe1TaskLate = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate.HasValue == false && x.EndDate.Value < DateTime.Now && x.EndDate.Value > dateTime).Count();
            int employe1TaskOngoing = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate.HasValue == false && x.EndDate.Value > DateTime.Now && x.EndDate.Value > dateTime).Count();
            int employe1HighPriority = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 2 && x.EndDate.Value > dateTime).Count();
            int employe1MediumPriority = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 1 && x.EndDate.Value > dateTime).Count();
            int employe1LowPriority = employeeOne.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 0 && x.EndDate.Value > dateTime).Count();

            dynamic obj = new System.Dynamic.ExpandoObject();
            obj.Employee1TaskCompleted = employe1TaskCompleted;
            obj.E1TCName = "Completed Tasks";
            obj.Employee1TaskAssigned = employe1TaskAssigned;
            obj.E1TAName = "Assigned Tasks";
            obj.Employee1TaskLate = employe1TaskLate;
            obj.E1TLName = "Late Tasks";
            obj.Employee1TaskOngoing = employe1TaskOngoing;
            obj.E1TOName = "Ongoing Tasks";

            obj.oneHigh = employe1HighPriority;
            obj.oneMedium = employe1MediumPriority;
            obj.oneLow = employe1LowPriority;

            obj.dates = dates;
            obj.Employee1Last30dDaysTask = emp1TComInD1;
            obj.firstName = employeeOne.FirstName;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public ActionResult displayMangAnalaysis(string sid, string page = "", string page1="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("Login", "Auth");
                }
                int managerId = General_Purpose.DecryptId(sid);
                User manager = new UserBL().getUsersById(managerId);
                if (manager != null)
                {

                    List<User> Users = new UserBL().getUsersList().Where(x => x.IsActive == 1 && x.Id != managerId && (x.Role == 2 || x.Role == 4) && x.DivisionId == manager.DivisionId).ToList();
                    ViewBag.ManagerName = Users;
                    ViewBag.Id = sid;
                    ViewBag.divId = manager.DivisionId;
                    ViewBag.page = page;
                    ViewBag.page1 = page1;

                    ViewBag.user = manager;
                }
                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        [HttpGet]
        public ActionResult managerAnalysisAjax(string id)
        {
            int managerId = General_Purpose.DecryptId(id);

            DateTime dateTime = DateTime.Now.AddDays(-30);
            User managerOne = new UserBL().getUsersById(managerId, new DatabaseEntities());

            List<int> mang1TComInD1 = new List<int>();

            List<string> dates = new List<string>();
            for (int i = 0; i < 30; i++)
            {

                string dat = dateTime.AddDays(i).ToString("yyyy-MM-dd");
                dates.Add(dat);
                int completedOneCount = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate != null && x.CompletionDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();

                mang1TComInD1.Add(completedOneCount);
            }
            int ticketsClosedOne = managerOne.GroupTask_Ticket.Where(x => x.IsActive == 1 && x.CompletedByUser.HasValue && x.CompletionDatetime > dateTime).Count();
            int managerTaskCompleted = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate > dateTime).Count();
            int managerTaskAssigned = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.StartDate > dateTime).Count();
            int managerTaskLate = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate.HasValue == false && x.EndDate.Value < DateTime.Now && x.EndDate.Value > dateTime).Count();
            int managerTaskOngoing = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate.HasValue == false && x.EndDate.Value > DateTime.Now && x.EndDate.Value > dateTime).Count();
            int managerHighPriority = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 2 && x.EndDate.Value > dateTime).Count();
            int managerMediumPriority = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 1 && x.EndDate.Value > dateTime).Count();
            int managerLowPriority = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 0 && x.EndDate.Value > dateTime).Count();

            dynamic obj = new System.Dynamic.ExpandoObject();
            obj.Employee1TaskCompleted = managerTaskCompleted;
            obj.E1TCName = "Completed Tasks";
            obj.Employee1TaskAssigned = managerTaskAssigned;
            obj.E1TAName = "Assigned Tasks";
            obj.Employee1TaskLate = managerTaskLate;
            obj.E1TLName = "Late Tasks";
            obj.Employee1TaskOngoing = managerTaskOngoing;
            obj.E1TOName = "Ongoing Tasks";

            obj.oneHigh = managerHighPriority;
            obj.oneMedium = managerMediumPriority;
            obj.oneLow = managerLowPriority;

            obj.dates = dates;
            obj.Employee1Last30dDaysTask = mang1TComInD1;
            obj.firstName = managerOne.FirstName;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult managersComparision(string id, string id2)
        {
            if (AuthenticateUser() == false)
            {
                return RedirectToAction("Login", "Auth");
            }
            DateTime dateTime = DateTime.Now.AddDays(-30);
            int managerId = General_Purpose.DecryptId(id);
            int managerId2 = General_Purpose.DecryptId(id2);
            User managerOne = new UserBL().getUsersById(managerId);
            User managerTwo = new UserBL().getUsersById(managerId2);


            int ticketsClosedOne = managerOne.GroupTask_Ticket.Where(x => x.IsActive == 1 && x.IsActive == 1 && x.CompletedByUser.HasValue && x.CompletionDatetime > dateTime).Count();
            int ticketsClosedTwo = managerTwo.GroupTask_Ticket.Where(x => x.IsActive == 1 && x.CompletedByUser.HasValue && x.CompletionDatetime > dateTime).Count();

            int manager1TaskCompleted = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate > dateTime).Count();
            int manager1TaskAssigned = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.StartDate > dateTime).Count();
            int manager1TaskLate = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate.HasValue == false && x.EndDate.Value < DateTime.Now && x.EndDate.Value > dateTime).Count();
            int manager1TaskOngoing = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate.HasValue == false && x.EndDate.Value > DateTime.Now && x.EndDate.Value > dateTime).Count();
            int manager1HighPriority = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 2 && x.EndDate.Value > dateTime).Count();
            int manager1MediumPriority = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 1 && x.EndDate.Value > dateTime).Count();
            int manager1LowPriority = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 0 && x.EndDate.Value > dateTime).Count();

            int manager2TaskCompleted = managerTwo.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate > dateTime).Count();
            int manager2TaskAssigned = managerTwo.User_Task1.Where(x => x.IsActive == 1 && x.StartDate > dateTime).Count();
            int manager2TaskLate = managerTwo.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate.HasValue == false && x.EndDate.Value < DateTime.Now && x.EndDate.Value > dateTime).Count();
            int manager2TaskOngoing = managerTwo.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate.HasValue == false && x.EndDate.Value > DateTime.Now && x.EndDate.Value > dateTime).Count();
            int manager2HighPriority = managerTwo.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 2 && x.EndDate.Value > dateTime).Count();
            int manager2MediumPriority = managerTwo.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 1 && x.EndDate.Value > dateTime).Count();
            int manager2LowPriority = managerTwo.User_Task1.Where(x => x.IsActive == 1 && x.Priority == 0 && x.EndDate.Value > dateTime).Count();


            List<int> man1TComInD1 = new List<int>();
            List<int> man2TComInD1 = new List<int>();
            List<string> dates = new List<string>();
            for (int i = 0; i < 30; i++)
            {

                string dat = dateTime.AddDays(i).ToString("yyyy-MM-dd");
                dates.Add(dat);
                int completedOneCount = managerOne.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate != null && x.CompletionDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();

                int completedTwoCount = managerTwo.User_Task1.Where(x => x.IsActive == 1 && x.CompletionDate != null && x.CompletionDate.Value.ToString("yyyy-MM-dd").Equals(dat)).Count();
                man1TComInD1.Add(completedOneCount);
                man2TComInD1.Add(completedTwoCount);
            }
            dynamic obj = new System.Dynamic.ExpandoObject();
            obj.manager1TaskCompleted = manager1TaskCompleted;
            obj.E1TCName = "Completed Tasks";
            obj.manager1TaskAssigned = manager1TaskAssigned;
            obj.E1TAName = "Assigned Tasks";
            obj.manager1TaskLate = manager1TaskLate;
            obj.E1TLName = "Late Tasks";
            obj.manager1TaskOngoing = manager1TaskOngoing;
            obj.E1TOName = "Ongoing Tasks";

            obj.manager2TaskCompleted = manager2TaskCompleted;
            obj.E2TCName = "Completed Tasks";
            obj.manager2TaskAssigned = manager2TaskAssigned;
            obj.E2TAName = "Assigned Tasks";
            obj.manager2TaskLate = manager2TaskLate;
            obj.E2TLName = "Late Tasks";
            obj.manager2TaskOngoing = manager2TaskOngoing;
            obj.E2TOName = "Ongoing Tasks";

            obj.manager1Last30dDaysTask = man1TComInD1;

            obj.manager2Last30dDaysTask = man2TComInD1;
            obj.dates = dates;
            obj.firstmanagerName = managerOne.FirstName;
            obj.secondmanagerName = managerTwo.FirstName;

            obj.oneHigh = manager1HighPriority;
            obj.oneMedium = manager1MediumPriority;
            obj.oneLow = manager1LowPriority;

            obj.twoHigh = manager2HighPriority;
            obj.twpMedium = manager2MediumPriority;
            obj.twoLow = manager2LowPriority;


            return Json(obj, JsonRequestBehavior.AllowGet);

        }


        public ActionResult ManagerAnalysis(string id, string id2, string sid = "", string page1 = "")
        {
            try
            {
                ViewBag.emp1 = id;
                ViewBag.emp2 = id2;
                int ManagerId = General_Purpose.DecryptId(id);
                int ManagerId2 = General_Purpose.DecryptId(id2);
                User UserOne = new UserBL().getUsersById(ManagerId);
                User UserTwo = new UserBL().getUsersById(ManagerId2);
                ViewBag.firstName = UserOne.FirstName+" "+UserOne.LastName;
                ViewBag.secondName = UserTwo.FirstName+" "+UserTwo.LastName;
                ViewBag.sid = sid;
                ViewBag.page1 = page1;
                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }

        }


    }


}