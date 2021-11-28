using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using TrainingTracker.BL;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;
using iTextSharp.text;
using TrainingTracker.Helper_Classes;
using System.Net.Mail;
using System.Net;
using System.Threading;
using NLog.Fluent;

namespace TrainingTracker.Controllers
{
    public class ReportController : Controller
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

        // GET: Report
        public ActionResult TaskReport()
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

        public ActionResult Error()
        {
            return View();
        }


        public ActionResult TrainingRequirements()
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



        #region Async PDF & Excel Reports

        public FileResult AsyncDivisionReportExcel(int division = -1, string FilterDate = "", string startDate = "", string endDate = "", int department = -1, string taskId = "", int taskType = 0, int status = -1, int priority = -1, int start = 0, int end = 0, int complete = 0, int workertype = -1)
        {
            // var local = new Byte[1000000];
            string reportName = "Division Report";

            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = SearchStatusReportByDivisionExcel(division, FilterDate, startDate, endDate, department, taskId, taskType, status, priority, start, end, complete, workertype);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }


        public FileResult AsyncDivisionReportPdf(int division = -1, string FilterDate = "", string startDate = "", string endDate = "", int department = -1, int workertype = -1, string taskId = "", int taskType = 0, int status = -1, int priority = -1, int start = 0, int end = 0, int complete = 0)
        {
            
            string reportName = "Division Report";
            
            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = SearchStatusReportByDivision(division, FilterDate, startDate, endDate, department, workertype, taskId, taskType, status, priority, start, end, complete);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }



        public FileResult AsyncDepartmentReportExcel(string FilterDate = "", string startDate = "", string endDate = "", int department = -1, string taskId = "", int taskType = 0, int status = -1, int priority = -1, int workertype = -1)
        {
            string reportName = "Department Report";

            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = SearchStatusReportByDepartmentExcel(FilterDate, startDate, endDate, department, taskId, taskType, status, priority, workertype);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }


        public FileResult AsyncDepartmentReportPdf(string FilterDate, string startDate = "", string endDate = "", int department = -1, string taskId = "", int taskType = 0, int status = -1, int priority = -1, int workertype = -1)
        {

            string reportName = "Department Report";

            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = SearchStatusReportByDepartment(FilterDate, startDate, endDate, department, taskId, taskType, status, priority, workertype);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }




        public FileResult AsyncEmployeeReportExcel(string FilterDate = "", string startDate = "", string endDate = "", string taskId = "", int taskType = 0, int status = -1, int priority = -1, int empId = -1)
        {
            string reportName = "Employee Report";

            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = SearchStatusReportByEmployeeExcel(FilterDate, startDate, endDate, taskId, taskType, status, priority, empId);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }


        public FileResult AsyncEmployeeReportPdf(string FilterDate = "", string startDate = "", string endDate = "", string taskId = "", int taskType = 0, int status = -1, int priority = -1, int empId = -1)
        {

            string reportName = "Employee Report";

            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = SearchStatusReportByEmployee(FilterDate, startDate, endDate, taskId, taskType, status, priority, empId, 1);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }



        public FileResult AsyncUserTaskReportExcel(string FilterDate = "", string UserTypeID = "", int DivisionID = -1, int DepartmentID = -1, int UserName = -1, string startDate = "", string endDate = "")
        {
            string reportName = "User Task Report";

            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = UserTaskExcelReport(FilterDate, UserTypeID, DivisionID, DepartmentID, UserName, startDate, endDate);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }


        public FileResult AsyncUserTaskReportPdf(string FilterDate = "", string UserTypeID = "", int DivisionID = -1, int DepartmentID = -1, int UserName = -1, string startDate = "", string endDate = "")
        {

            string reportName = "User Task Report";

            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = UserTaskpdfReport(FilterDate, UserTypeID, DivisionID, DepartmentID, UserName, startDate, endDate);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }

        #endregion



        public ActionResult StatusReportDivision(string frId = "", string FilterDate = "", string startDate = "", string endDate = "", int tag = -1, int department = -1, int division = 0, string taskId = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int priority = -1, int completionStatus = -1, string message = "")
        {
            try
            {
                FavoriteReport fr = null;

                if (frId != "")
                {
                    fr = new FavoriteReportBL().getFavoriteReportById(Convert.ToInt32(StringCipher.Base64Decode(frId)));
                }

                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                if (logedinuser.Role == 3)
                {
                    return Content("Acess Restricted !");
                }

                List<User_Task> tasks = new User_TaskBL().getUser_TasksList().Where(x => x.IsPrivate == 0).ToList();

                string complettionName = string.Empty;

                if (logedinuser.Role != 1)
                {
                    User manager = new UserBL().getUsersById(logedinuser.Id);
                    tasks = tasks.Where(x => x.User1.Department.DivisionID == manager.DivisionId).ToList();
                }

                if (department != -1)
                {
                    tasks = tasks.Where(x => x.User1.Department != null && x.User1.DepartmentId == department).ToList();
                }

                if (taskId != "")
                {
                    tasks = tasks.Where(x => x.TaskID == Convert.ToInt32(taskId) && x.IsPrivate == 0).ToList();
                }

                if (FilterDate != "")
                {
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    int daysTillCurrentDate = currentDay - DayOfWeek.Monday;
                    DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDate);
                    DateTime lastWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDate - 7);
                    DateTime nextWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDate + 7);

                    var today = DateTime.Today;
                    var tomorrow = today.AddDays(1);
                    var yesterday = today.AddDays(-1);

                    int lastMonth = DateTime.Now.AddMonths(-1).Month;
                    int currentMonth = DateTime.Now.Month;
                    int nextMonth = DateTime.Now.AddMonths(1).Month;

                    int currentYear = DateTime.Now.Year;
                    int lastYear = currentYear - 1;
                    int nextYear = currentYear + 1;

                    if (FilterDate == "Tomorrow")
                    {
                        tasks = tasks.Where(x => x.StartDate == tomorrow || x.EndDate == tomorrow || x.CompletionDate == tomorrow).ToList();
                    }
                    if (FilterDate == "Today")
                    {
                        tasks = tasks.Where(x => x.StartDate == today || x.EndDate == today || x.CompletionDate == today).ToList();
                    }
                    if (FilterDate == "Yesterday")
                    {
                        tasks = tasks.Where(x => x.StartDate == yesterday || x.EndDate == yesterday || x.CompletionDate == yesterday).ToList();
                    }
                    if (FilterDate == "ThisWeek")
                    {
                        tasks = tasks.Where(x => (x.StartDate == currentWeekStartDate || x.StartDate == currentWeekStartDate.AddDays(+1) || x.StartDate == currentWeekStartDate.AddDays(+2) || x.StartDate == currentWeekStartDate.AddDays(+3) || x.StartDate == currentWeekStartDate.AddDays(+4) || x.StartDate == currentWeekStartDate.AddDays(+5) || x.StartDate == currentWeekStartDate.AddDays(+6))
                        || (x.EndDate == currentWeekStartDate || x.EndDate == currentWeekStartDate.AddDays(+1) || x.EndDate == currentWeekStartDate.AddDays(+2) || x.EndDate == currentWeekStartDate.AddDays(+3) || x.EndDate == currentWeekStartDate.AddDays(+4) || x.EndDate == currentWeekStartDate.AddDays(+5) || x.EndDate == currentWeekStartDate.AddDays(+6))
                        || (x.CompletionDate == currentWeekStartDate || x.CompletionDate == currentWeekStartDate.AddDays(+1) || x.CompletionDate == currentWeekStartDate.AddDays(+2) || x.CompletionDate == currentWeekStartDate.AddDays(+3) || x.CompletionDate == currentWeekStartDate.AddDays(+4) || x.CompletionDate == currentWeekStartDate.AddDays(+5) || x.CompletionDate == currentWeekStartDate.AddDays(+6))).ToList();
                    }
                    if (FilterDate == "NextWeek")
                    {
                        tasks = tasks.Where(x => (x.StartDate == nextWeekStartDate || x.StartDate == nextWeekStartDate.AddDays(+1) || x.StartDate == nextWeekStartDate.AddDays(+2) || x.StartDate == nextWeekStartDate.AddDays(+3) || x.StartDate == nextWeekStartDate.AddDays(+4) || x.StartDate == nextWeekStartDate.AddDays(+5) || x.StartDate == nextWeekStartDate.AddDays(+6))
                                                    || (x.EndDate == nextWeekStartDate || x.EndDate == nextWeekStartDate.AddDays(+1) || x.EndDate == nextWeekStartDate.AddDays(+2) || x.EndDate == nextWeekStartDate.AddDays(+3) || x.EndDate == nextWeekStartDate.AddDays(+4) || x.EndDate == nextWeekStartDate.AddDays(+5) || x.EndDate == nextWeekStartDate.AddDays(+6))
                                                    || (x.CompletionDate == nextWeekStartDate || x.CompletionDate == nextWeekStartDate.AddDays(+1) || x.CompletionDate == nextWeekStartDate.AddDays(+2) || x.CompletionDate == nextWeekStartDate.AddDays(+3) || x.CompletionDate == nextWeekStartDate.AddDays(+4) || x.CompletionDate == nextWeekStartDate.AddDays(+5) || x.CompletionDate == nextWeekStartDate.AddDays(+6))).ToList();

                    }
                    if (FilterDate == "LastWeek")
                    {
                        tasks = tasks.Where(x => (x.StartDate == lastWeekStartDate || x.StartDate == lastWeekStartDate.AddDays(+1) || x.StartDate == lastWeekStartDate.AddDays(+2) || x.StartDate == lastWeekStartDate.AddDays(+3) || x.StartDate == lastWeekStartDate.AddDays(+4) || x.StartDate == lastWeekStartDate.AddDays(+5) || x.StartDate == lastWeekStartDate.AddDays(+6))
                        || (x.EndDate == lastWeekStartDate || x.EndDate == lastWeekStartDate.AddDays(+1) || x.EndDate == lastWeekStartDate.AddDays(+2) || x.EndDate == lastWeekStartDate.AddDays(+3) || x.EndDate == lastWeekStartDate.AddDays(+4) || x.EndDate == lastWeekStartDate.AddDays(+5) || x.EndDate == lastWeekStartDate.AddDays(+6))
                        || (x.CompletionDate == lastWeekStartDate || x.CompletionDate == lastWeekStartDate.AddDays(+1) || x.CompletionDate == lastWeekStartDate.AddDays(+2) || x.CompletionDate == lastWeekStartDate.AddDays(+3) || x.CompletionDate == lastWeekStartDate.AddDays(+4) || x.CompletionDate == lastWeekStartDate.AddDays(+5) || x.CompletionDate == lastWeekStartDate.AddDays(+6))).ToList();

                    }
                    if (FilterDate == "NextMonth")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Month == nextMonth || Convert.ToDateTime(x.EndDate).Month == nextMonth || Convert.ToDateTime(x.CompletionDate).Month == nextMonth).ToList();
                    }
                    if (FilterDate == "ThisMonth")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Month == currentMonth || Convert.ToDateTime(x.EndDate).Month == currentMonth || Convert.ToDateTime(x.CompletionDate).Month == currentMonth).ToList();
                    }
                    if (FilterDate == "LastMonth")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Month == lastMonth || Convert.ToDateTime(x.EndDate).Month == lastMonth || Convert.ToDateTime(x.CompletionDate).Month == lastMonth).ToList();
                    }
                    if (FilterDate == "NextQuarter")
                    {
                        if(currentMonth == 1 || currentMonth == 2 || currentMonth == 3)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 4 || Convert.ToDateTime(x.EndDate).Month == 4 || Convert.ToDateTime(x.CompletionDate).Month == 4)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 5 || Convert.ToDateTime(x.EndDate).Month == 5 || Convert.ToDateTime(x.CompletionDate).Month == 5)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 6 || Convert.ToDateTime(x.EndDate).Month == 6 || Convert.ToDateTime(x.CompletionDate).Month == 6)).ToList();
                        }
                        if (currentMonth == 4 || currentMonth == 5 || currentMonth == 6)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 7 || Convert.ToDateTime(x.EndDate).Month == 7 || Convert.ToDateTime(x.CompletionDate).Month == 7)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 8 || Convert.ToDateTime(x.EndDate).Month == 8 || Convert.ToDateTime(x.CompletionDate).Month == 8)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 9 || Convert.ToDateTime(x.EndDate).Month == 9 || Convert.ToDateTime(x.CompletionDate).Month == 9)).ToList();
                        }
                        if (currentMonth == 7 || currentMonth == 8 || currentMonth == 9)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 10 || Convert.ToDateTime(x.EndDate).Month == 10 || Convert.ToDateTime(x.CompletionDate).Month == 10)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 11 || Convert.ToDateTime(x.EndDate).Month == 11 || Convert.ToDateTime(x.CompletionDate).Month == 11)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 12 || Convert.ToDateTime(x.EndDate).Month == 12 || Convert.ToDateTime(x.CompletionDate).Month == 12)).ToList();
                        }
                        if (currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
                        {
                            tasks = tasks.Where(x => ((Convert.ToDateTime(x.StartDate).Year == nextYear && Convert.ToDateTime(x.StartDate).Month == 1) || (Convert.ToDateTime(x.EndDate).Year == nextYear && Convert.ToDateTime(x.EndDate).Month == 1) || (Convert.ToDateTime(x.CompletionDate).Year == nextYear && Convert.ToDateTime(x.CompletionDate).Month == 1))
                                                        || ((Convert.ToDateTime(x.StartDate).Year == nextYear && Convert.ToDateTime(x.StartDate).Month == 2) || (Convert.ToDateTime(x.EndDate).Year == nextYear && Convert.ToDateTime(x.EndDate).Month == 2) || (Convert.ToDateTime(x.CompletionDate).Year == nextYear && Convert.ToDateTime(x.CompletionDate).Month == 2))
                                                        || ((Convert.ToDateTime(x.StartDate).Year == nextYear && Convert.ToDateTime(x.StartDate).Month == 3) || (Convert.ToDateTime(x.EndDate).Year == nextYear && Convert.ToDateTime(x.EndDate).Month == 3) || (Convert.ToDateTime(x.CompletionDate).Year == nextYear && Convert.ToDateTime(x.CompletionDate).Month == 3))).ToList();
                        }

                    }
                    if (FilterDate == "ThisQuarter")
                    {
                        if (currentMonth == 1 || currentMonth == 2 || currentMonth == 3)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 1 || Convert.ToDateTime(x.EndDate).Month == 1 || Convert.ToDateTime(x.CompletionDate).Month == 1)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 2 || Convert.ToDateTime(x.EndDate).Month == 2 || Convert.ToDateTime(x.CompletionDate).Month == 2)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 3 || Convert.ToDateTime(x.EndDate).Month == 3 || Convert.ToDateTime(x.CompletionDate).Month == 3)).ToList();
                        }
                        if (currentMonth == 4 || currentMonth == 5 || currentMonth == 6)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 4 || Convert.ToDateTime(x.EndDate).Month == 4 || Convert.ToDateTime(x.CompletionDate).Month == 4)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 5 || Convert.ToDateTime(x.EndDate).Month == 5 || Convert.ToDateTime(x.CompletionDate).Month == 5)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 6 || Convert.ToDateTime(x.EndDate).Month == 6 || Convert.ToDateTime(x.CompletionDate).Month == 6)).ToList();
                        }
                        if (currentMonth == 7 || currentMonth == 8 || currentMonth == 9)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 7 || Convert.ToDateTime(x.EndDate).Month == 7 || Convert.ToDateTime(x.CompletionDate).Month == 7)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 8 || Convert.ToDateTime(x.EndDate).Month == 8 || Convert.ToDateTime(x.CompletionDate).Month == 8)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 9 || Convert.ToDateTime(x.EndDate).Month == 9 || Convert.ToDateTime(x.CompletionDate).Month == 9)).ToList();
                        }
                        if (currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 10 || Convert.ToDateTime(x.EndDate).Month == 10 || Convert.ToDateTime(x.CompletionDate).Month == 10)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 11 || Convert.ToDateTime(x.EndDate).Month == 11 || Convert.ToDateTime(x.CompletionDate).Month == 11)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 12 || Convert.ToDateTime(x.EndDate).Month == 12 || Convert.ToDateTime(x.CompletionDate).Month == 12)).ToList();
                        }
                    }
                    if (FilterDate == "LastQuarter")
                    {
                        if (currentMonth == 1 || currentMonth == 2 || currentMonth == 3)
                        {
                            tasks = tasks.Where(x => ((Convert.ToDateTime(x.StartDate).Year == lastYear && Convert.ToDateTime(x.StartDate).Month == 10) || (Convert.ToDateTime(x.EndDate).Year == lastYear && Convert.ToDateTime(x.EndDate).Month == 10) || (Convert.ToDateTime(x.CompletionDate).Year == lastYear && Convert.ToDateTime(x.CompletionDate).Month == 10))
                                                        || ((Convert.ToDateTime(x.StartDate).Year == lastYear && Convert.ToDateTime(x.StartDate).Month == 11) || (Convert.ToDateTime(x.EndDate).Year == lastYear && Convert.ToDateTime(x.EndDate).Month == 11) || (Convert.ToDateTime(x.CompletionDate).Year == lastYear && Convert.ToDateTime(x.CompletionDate).Month == 11))
                                                        || ((Convert.ToDateTime(x.StartDate).Year == lastYear && Convert.ToDateTime(x.StartDate).Month == 12) || (Convert.ToDateTime(x.EndDate).Year == lastYear && Convert.ToDateTime(x.EndDate).Month == 12) || (Convert.ToDateTime(x.CompletionDate).Year == lastYear && Convert.ToDateTime(x.CompletionDate).Month == 12))).ToList();


                        }
                        if (currentMonth == 4 || currentMonth == 5 || currentMonth == 6)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 1 || Convert.ToDateTime(x.EndDate).Month == 1 || Convert.ToDateTime(x.CompletionDate).Month == 1)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 2 || Convert.ToDateTime(x.EndDate).Month == 2 || Convert.ToDateTime(x.CompletionDate).Month == 2)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 3 || Convert.ToDateTime(x.EndDate).Month == 3 || Convert.ToDateTime(x.CompletionDate).Month == 3)).ToList();
                        }
                        if (currentMonth == 7 || currentMonth == 8 || currentMonth == 9)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 4 || Convert.ToDateTime(x.EndDate).Month == 4 || Convert.ToDateTime(x.CompletionDate).Month == 4)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 5 || Convert.ToDateTime(x.EndDate).Month == 5 || Convert.ToDateTime(x.CompletionDate).Month == 5)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 6 || Convert.ToDateTime(x.EndDate).Month == 6 || Convert.ToDateTime(x.CompletionDate).Month == 6)).ToList();
                        }
                        if (currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 7 || Convert.ToDateTime(x.EndDate).Month == 7 || Convert.ToDateTime(x.CompletionDate).Month == 7)
                                                       || (Convert.ToDateTime(x.StartDate).Month == 8 || Convert.ToDateTime(x.EndDate).Month == 8 || Convert.ToDateTime(x.CompletionDate).Month == 8)
                                                       || (Convert.ToDateTime(x.StartDate).Month == 9 || Convert.ToDateTime(x.EndDate).Month == 9 || Convert.ToDateTime(x.CompletionDate).Month == 9)).ToList();
                        }
                    }
                    if (FilterDate == "NextYear")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                    }
                    if (FilterDate == "ThisYear")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                    }
                    if (FilterDate == "LastYear")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                    }
                }

                if (tag != -1)
                {
                    tasks = tasks.Where(x => x.Task.TaskTags.Where(z => z.TagId == tag).Count() > 0).ToList();
                }

                if (taskType != 0)
                {
                    tasks = tasks.Where(x => x.Task.TaskTypeID == taskType).ToList();
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

                if (startDate != null && startDate != "")
                {
                    tasks = tasks.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                            (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate)))
                    //(x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate)))
                    ).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    tasks = tasks.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                            (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate)))
                    //(x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate)))
                    ).ToList();
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


                if (fr != null && fr.ReportType == 1)
                {
                    if (fr.Name != "" && fr.Name != null)
                    {
                        ViewBag.selectedreportname = fr.Name;
                    }

                    if (fr.Division != "" && fr.Division != null)
                    {
                        ViewBag.selecteddivision = new DivisionBL().getDivisionsById(Convert.ToInt32(fr.Division));
                    }

                    if (fr.Department != "" && fr.Department != null)
                    {
                        ViewBag.selecteddepartment = new DepartmentBL().getDepartmentsById(Convert.ToInt32(fr.Department));
                    }

                    if (fr.WorkerType != "" && fr.WorkerType != null)
                    {
                        ViewBag.selectedworkertype = new WorktypeBL().getWorktypesById(Convert.ToInt32(fr.WorkerType));
                    }

                    if (fr.TaskName != "" && fr.TaskName != null)
                    {
                        ViewBag.selectedtask = new TaskBL().getTasksById(Convert.ToInt32(fr.TaskName));
                    }

                    if (fr.TaskType != "" && fr.TaskType != null)
                    {
                        ViewBag.selectedtasktype = new TaskTypeBL().getTaskTypesById(Convert.ToInt32(fr.TaskType));
                    }

                    if (fr.Status != "" && fr.Status != null)
                    {
                        ViewBag.selectedstatus = fr.Status;
                    }

                    if (fr.Priority != "" && fr.Priority != null)
                    {
                        ViewBag.selectedpriority = fr.Priority;
                    }

                    if (fr.StartingDate != "" && fr.StartingDate != null)
                    {
                        ViewBag.selectedstartdate = Convert.ToDateTime(fr.StartingDate);
                    }

                    if (fr.EndingDate != "" && fr.EndingDate != null)
                    {
                        ViewBag.selectedendDate = Convert.ToDateTime(fr.EndingDate);
                    }

                    if (fr.StartDate != "" && fr.StartDate != null)
                    {
                        ViewBag.selectedstartId = fr.StartDate;
                    }

                    if (fr.EndDate != "" && fr.EndDate != null)
                    {
                        ViewBag.selectedendId = fr.EndDate;
                    }

                    if (fr.CompleteDate != "" && fr.CompleteDate != null)
                    {
                        ViewBag.selectedcompleteId = fr.CompleteDate;
                    }
                    
                    if(fr.FilterDate != "" && fr.FilterDate != null)
                    {
                        ViewBag.filterdate = fr.FilterDate;
                    }
                }
                else
                {
                    ViewBag.filterdate = null;
                    ViewBag.selecteddivision = null;
                    ViewBag.selecteddepartment = null;
                    ViewBag.selectedworkertype = null;
                    ViewBag.selectedtaskname = null;
                    ViewBag.selectedtasktype = null;
                    ViewBag.selectedstatus = null;
                    ViewBag.selectedpriority = null;
                    ViewBag.selectedstartingdate = null;
                    ViewBag.selectedendDate = null;
                    ViewBag.selectedstartId = null;
                    ViewBag.selectedendId = null;
                    ViewBag.selectedcompleteId = null;
                }


                ViewBag.tasks = tasks;
                ViewBag.tag = new TagBL().getTagsById(tag);
                ViewBag.tags = new TagBL().getTagsList();

                ViewBag.startDate = startDate;
                ViewBag.endDate = endDate;
                ViewBag.status = status;
                ViewBag.department = new DepartmentBL().getDepartmentsById(department);
                ViewBag.departments = new DepartmentBL().getDepartmentsList();
                ViewBag.divisions = new DivisionBL().getDivisionsList();
                //ViewBag.taskName = taskName;
                ViewBag.taskType = new TaskTypeBL().getTaskTypesById(taskType);
                ViewBag.taskTypes = new TaskTypeBL().getTaskTypesList();
                ViewBag.statuses = new List<int>() { 0, 1, 2, 3, 4 }.Where(x => x != status);
                ViewBag.completionListId = new List<int>() { 1, 2, 3 };
                ViewBag.completionList = new List<string>() { "In Process", "Completed", "Late" };
                ViewBag.completionId = completionStatus;
                ViewBag.completionIdName = complettionName;

                ViewBag.frId = frId;
                ViewBag.message = message;

                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        public List<string> SearchStatusReportByDivision(int division = -1, string FilterDate = "", string startDate = "", string endDate = "", int department = -1, int workertype = -1, string taskId = "", int taskType = 0, int status = -1, int priority = -1, int start = 0, int end = 0, int complete = 0)
        {

            Log.Info("Pdf generation method called");

            List<string> objlist = new List<string>();
            objlist.Add(logedinuser.Id.ToString());
            User manager = new UserBL().getUsersById(logedinuser.Id);


            List<User_Task> taskss = new User_TaskBL().getUser_TasksList().Where(x => x.IsPrivate == 0 && x.UserID != null).ToList();
            string complettionName = string.Empty;

            if (division != -1)
            {

                taskss = taskss.Where(x => x.User1.DivisionId == division || (x.User1.DepartmentId != null && x.User1.Department.DivisionID == division)).ToList();
                Division div = new DivisionBL().getDivisionsById(division);


                if (department != -1)
                {
                    taskss = taskss.Where(x => x.User1.Department != null && x.User1.DepartmentId == department).ToList();
                }

                if (FilterDate != "")
                {
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

                    #region Days
                    DateTime currentDate = DateTime.Now;
                    DateTime lastDate = currentDate.AddDays(-1);
                    DateTime nextDate = currentDate.AddDays(1);
                    #endregion


                    #region Weeks
                    DateTime currentWeekStartDate = currentDate.AddDays(-daysTillCurrentDate);
                    DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                    DateTime lastWeekStartDate = currentDate.AddDays(-daysTillCurrentDate - 7);
                    DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

                    DateTime nextWeekStartDate = currentDate.AddDays(-daysTillCurrentDate + 7);
                    DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);
                    #endregion


                    #region Months
                    DateTime currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    DateTime currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1);

                    DateTime lastMonthStartDate = currentDate.AddMonths(-1);
                    lastMonthStartDate = new DateTime(lastMonthStartDate.Year, lastMonthStartDate.Month, 1);
                    DateTime lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddDays(-1);

                    DateTime nextMonthStartDate = currentDate.AddMonths(1);
                    nextMonthStartDate = new DateTime(nextMonthStartDate.Year, nextMonthStartDate.Month, 1);
                    DateTime nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1);
                    #endregion


                    #region Quarters
                    DateTime currentQuarterStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    DateTime currentQuarterEndDate = currentQuarterStartDate.AddMonths(4).AddDays(-1);


                    DateTime lastQuarterStartDate = currentQuarterStartDate.AddMonths(-4);
                    DateTime lastQuarterEndDate = currentQuarterStartDate.AddDays(-1);

                    DateTime nextQuarterStartDate = currentQuarterEndDate.AddDays(1);
                    DateTime nextQuarterEndDate = nextQuarterStartDate.AddMonths(4).AddDays(-1);
                    #endregion


                    #region Years
                    int currentYear = currentDate.Year;
                    int lastYear = currentYear - 1;
                    int nextYear = currentYear + 1;
                    #endregion


                    if (FilterDate == "Tomorrow")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextDate.Date && Convert.ToDateTime(x.StartDate).Date < nextDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "Today")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentDate.Date && Convert.ToDateTime(x.StartDate).Date < currentDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "Yesterday")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastDate.Date && Convert.ToDateTime(x.StartDate).Date < lastDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                    }
                    if (FilterDate == "ThisYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                    }
                    if (FilterDate == "LastYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                    }

                }

                if (taskId != "")
                {
                    taskss = new User_TaskBL().getUser_TasksList().Where(x => x.TaskID == Convert.ToInt32(taskId) && x.IsPrivate == 0).ToList();
                }

                if (taskType != 0)
                {
                    taskss = taskss.Where(x => x.Task.TaskTypeID == taskType).ToList();
                }

                if (status != -1)
                {
                    taskss = taskss.Where(x => x.Status == status).ToList();
                }

                if (priority != -1)
                {
                    taskss = taskss.Where(x => x.Priority == priority).ToList();
                }


                if (startDate != null && startDate != "")
                {
                    taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                            (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate)))
                    ).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                            (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate)))
                    ).ToList();
                }

                if (workertype != -1)
                {
                    // taskss = taskss.Where(x => new User_WorktypeBL().getUser_WorktypesList().Where(c => c.UserId == x.UserID && c.WorktypeId == workertype).FirstOrDefault().WorktypeId == workertype).ToList();
                    List<User_Task> list = new List<User_Task>();
                    foreach (User_Task item in taskss)
                    {
                        User_Worktype wt = new User_WorktypeBL().getUser_WorktypesList().Where(x => x.UserId == item.UserID && x.WorktypeId == workertype).FirstOrDefault();
                        if (wt != null)
                        {
                            list.Add(item);
                        }
                    }
                    taskss = list;
                }

                List<DivisionReportDTO> mnglist = new List<DivisionReportDTO>();

                string priority1 = "";
                string status1 = "";
                string AssignedBy = "";
                string AssignedTo = "";
                foreach (User_Task x in taskss)
                {
                    if (x.CompletionDate != null)
                    {
                        status1 = "Completed";
                    }
                    else
                    {
                        if (x.EndDate < DateTime.Now)
                        {
                            status1 = "Late";
                        }
                        else
                        {
                            if (x.Status != null)
                                status1 = General_Purpose.getStatusValue(x.Status.Value);
                            else
                                status1 = "Started";
                        }
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

                    if (x.CreatedID == null)
                    {
                        AssignedBy = "Self Assigned";

                    }
                    else if (x.User.Id == logedinuser.Id)
                    {
                        //  AssignedBy = "You";
                        AssignedBy = x.User.FirstName + " " + x.User.LastName; ;
                    }
                    else
                    {
                        AssignedBy = x.User.FirstName + " " + x.User.LastName;
                    }

                    AssignedTo = x.User1.FirstName + " " + x.User1.LastName;

                    DivisionReportDTO obj = new DivisionReportDTO();

                    obj.AssignedBy = AssignedBy;
                    obj.AssignedTo = AssignedTo;

                    if (x.Task.Division == null)
                    {
                        obj.DivisionName = "No Division Assigned";
                    }
                    else
                    {
                        obj.DivisionName = x.Task.Division.Name;

                    }

                    if (x.User1.Department == null)
                    {
                        obj.DepartmentName = "Manager in the Division";
                    }
                    else
                    {
                        obj.DepartmentName = x.User1.Department.Name;

                    }

                    if (x.Task == null)
                    {
                        obj.TaskType = "No Task Type Assigned";
                        obj.TaskName = "No Task Name Assigned";
                    }
                    else
                    {
                        obj.TaskType = x.Task.TaskType.Name;
                        obj.TaskName = x.Task.Name;
                    }

                    if (x.StartDate == null)
                    {
                        obj.StartDate = "No Start Date Assigned";
                    }
                    else
                    {
                        obj.StartDate = x.StartDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (x.EndDate == null)
                    {
                        obj.DueDate = "No Due Date Assigned";
                    }
                    else
                    {
                        obj.DueDate = x.EndDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (status1 == null)
                    {
                        obj.CompletionStatus = "No Status Assigned";
                    }
                    else
                    {
                        obj.CompletionStatus = status1;
                    }

                    if (priority1 == null)
                    {
                        obj.Priority = "No Priority Assigned";
                    }
                    else
                    {
                        obj.Priority = priority1;
                    }
                    if (x.CompletionDate != null)
                    {
                        obj.CompletionDate = x.CompletionDate.Value.ToString("MM/dd/yyyy");
                    }
                    mnglist.Add(obj);
                }

                Log.Info("Pdf generation method search completed");

                string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Division" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".pdf";
                string title = "Status Report by Division: " + (div != null ? div.Name : "NA");
                new PDF_Export().generate_division_report(mnglist, path, title);



                List<string> objlist2 = new List<string>();

                objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
                objlist.AddRange(objlist2);

                return objlist;

                //return File(path, MediaTypeNames.Text.Plain, "Division Report.pdf");
            }
            else
            {
                return null;
            }
            
        }



        //[HttpPost]
        //public ActionResult SearchStatusReportByDivision(int division = -1, string FilterDate = "", string startDate = "", string endDate = "", int department = -1, int workertype = -1, string taskId = "", int taskType = 0, int status = -1, int priority = -1, int start = 0, int end = 0, int complete = 0)
        //{
        //    try
        //    {
        //        List<User_Task> taskss = new User_TaskBL().getUser_TasksList().Where(x => x.IsPrivate == 0 && x.UserID !=null).ToList();
        //        string complettionName = string.Empty;

        //        if (division != -1)
        //        {
        //            taskss = taskss.Where(x => x.User1.DivisionId == division || (x.User1.DepartmentId != null && x.User1.Department.DivisionID == division)).ToList();
        //            Division div = new DivisionBL().getDivisionsById(division);


        //            if (department != -1)
        //            {
        //                taskss = taskss.Where(x => x.User1.Department != null && x.User1.DepartmentId == department).ToList();
        //            }

        //            if (FilterDate != "")
        //            {
        //                DayOfWeek currentDay = DateTime.Now.DayOfWeek;
        //                int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

        //                #region Days
        //                    DateTime currentDate = DateTime.Now;
        //                    DateTime lastDate = currentDate.AddDays(-1);
        //                    DateTime nextDate = currentDate.AddDays(1);
        //                #endregion


        //                #region Weeks
        //                    DateTime currentWeekStartDate = currentDate.AddDays(-daysTillCurrentDate);
        //                    DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

        //                    DateTime lastWeekStartDate = currentDate.AddDays(-daysTillCurrentDate - 7);
        //                    DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

        //                    DateTime nextWeekStartDate = currentDate.AddDays(-daysTillCurrentDate + 7);
        //                    DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);
        //                #endregion


        //                #region Months
        //                    DateTime currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
        //                    DateTime currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1);

        //                    DateTime lastMonthStartDate = currentDate.AddMonths(-1);
        //                    lastMonthStartDate = new DateTime(lastMonthStartDate.Year, lastMonthStartDate.Month, 1);
        //                    DateTime lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddDays(-1);

        //                    DateTime nextMonthStartDate = currentDate.AddMonths(1);
        //                    nextMonthStartDate = new DateTime(nextMonthStartDate.Year, nextMonthStartDate.Month, 1);
        //                    DateTime nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1);
        //                #endregion


        //                #region Quarters
        //                    DateTime currentQuarterStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
        //                    DateTime currentQuarterEndDate = currentQuarterStartDate.AddMonths(4).AddDays(-1);


        //                    DateTime lastQuarterStartDate = currentQuarterStartDate.AddMonths(-4);
        //                    DateTime lastQuarterEndDate = currentQuarterStartDate.AddDays(-1);

        //                    DateTime nextQuarterStartDate = currentQuarterEndDate.AddDays(1);
        //                    DateTime nextQuarterEndDate = nextQuarterStartDate.AddMonths(4).AddDays(-1);
        //                #endregion


        //                #region Years
        //                    int currentYear = currentDate.Year;
        //                    int lastYear = currentYear - 1;
        //                    int nextYear = currentYear + 1;
        //                #endregion


        //                if (FilterDate == "Tomorrow")
        //                {
        //                    taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date == nextDate.Date)
        //                    || (Convert.ToDateTime(x.StartDate).Date == nextDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date > nextDate.Date && Convert.ToDateTime(x.StartDate).Date < nextDate.Date)
        //                     ).ToList();
        //                }
        //                if (FilterDate == "Today")
        //                {
        //                    taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date == currentDate.Date)
        //                    || (Convert.ToDateTime(x.StartDate).Date == currentDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date > currentDate.Date && Convert.ToDateTime(x.StartDate).Date < currentDate.Date)
        //                     ).ToList();
        //                }
        //                if (FilterDate == "Yesterday")
        //                {
        //                    taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date == lastDate.Date)
        //                    || (Convert.ToDateTime(x.StartDate).Date == lastDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date > lastDate.Date && Convert.ToDateTime(x.StartDate).Date < lastDate.Date)
        //                     ).ToList();
        //                }
        //                if (FilterDate == "ThisWeek")
        //                {
        //                    taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
        //                    || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
        //                     ).ToList();
        //                }
        //                if (FilterDate == "NextWeek")
        //                {
        //                    taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
        //                    || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
        //                     ).ToList();
        //                }
        //                if (FilterDate == "LastWeek")
        //                {
        //                    taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
        //                    || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
        //                     ).ToList();
        //                }
        //                if (FilterDate == "NextMonth")
        //                {
        //                    taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextMonthEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date == nextMonthStartDate.Date)
        //                    || (Convert.ToDateTime(x.StartDate).Date == nextMonthEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date > nextMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextMonthEndDate.Date)
        //                     ).ToList();
        //                }
        //                if (FilterDate == "ThisMonth")
        //                {
        //                    taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentMonthEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date == currentMonthStartDate.Date)
        //                    || (Convert.ToDateTime(x.StartDate).Date == currentMonthEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date > currentMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentMonthEndDate.Date)
        //                     ).ToList();
        //                }
        //                if (FilterDate == "LastMonth")
        //                {
        //                    taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastMonthEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date == lastMonthStartDate.Date)
        //                    || (Convert.ToDateTime(x.StartDate).Date == lastMonthEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date > lastMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastMonthEndDate.Date)
        //                     ).ToList();
        //                }
        //                if (FilterDate == "NextQuarter")
        //                {
        //                    taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextQuarterEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date == nextQuarterStartDate.Date)
        //                    || (Convert.ToDateTime(x.StartDate).Date == nextQuarterEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date > nextQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextQuarterEndDate.Date)
        //                     ).ToList();
        //                }
        //                if (FilterDate == "ThisQuarter")
        //                {
        //                    taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentQuarterEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date == currentQuarterStartDate.Date)
        //                    || (Convert.ToDateTime(x.StartDate).Date == currentQuarterEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date > currentQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentQuarterEndDate.Date)
        //                     ).ToList();
        //                }
        //                if (FilterDate == "LastQuarter")
        //                {
        //                    taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastQuarterEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date == lastQuarterStartDate.Date)
        //                    || (Convert.ToDateTime(x.StartDate).Date == lastQuarterEndDate.Date)
        //                    || (Convert.ToDateTime(x.EndDate).Date > lastQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastQuarterEndDate.Date)
        //                     ).ToList();
        //                }
        //                if (FilterDate == "NextYear")
        //                {
        //                    taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
        //                }
        //                if (FilterDate == "ThisYear")
        //                {
        //                    taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
        //                }
        //                if (FilterDate == "LastYear")
        //                {
        //                    taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
        //                }

        //            }

        //            if (taskId != "")
        //            {
        //                taskss = new User_TaskBL().getUser_TasksList().Where(x => x.TaskID == Convert.ToInt32(taskId) && x.IsPrivate == 0).ToList();
        //            }

        //            if (taskType != 0)
        //            {
        //                taskss = taskss.Where(x => x.Task.TaskTypeID == taskType).ToList();
        //            }

        //            if (status != -1)
        //            {
        //                taskss = taskss.Where(x => x.Status == status).ToList();
        //            }

        //            if (priority != -1)
        //            {
        //                taskss = taskss.Where(x => x.Priority == priority).ToList();
        //            }


        //            if (startDate != null && startDate != "")
        //            {
        //                taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
        //                                        (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate)))
        //                ).ToList();
        //            }

        //            if (endDate != null && endDate != "")
        //            {
        //                taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
        //                                        (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate)))
        //                ).ToList();
        //            }

        //            if (workertype != -1)
        //            {
        //                // taskss = taskss.Where(x => new User_WorktypeBL().getUser_WorktypesList().Where(c => c.UserId == x.UserID && c.WorktypeId == workertype).FirstOrDefault().WorktypeId == workertype).ToList();
        //                List<User_Task> list = new List<User_Task>();
        //                foreach (User_Task item in taskss)
        //                {
        //                    User_Worktype wt = new User_WorktypeBL().getUser_WorktypesList().Where(x => x.UserId == item.UserID && x.WorktypeId == workertype).FirstOrDefault();
        //                    if (wt != null)
        //                    {
        //                        list.Add(item);
        //                    }
        //                }
        //                taskss = list;
        //            }

        //            List<DivisionReportDTO> mnglist = new List<DivisionReportDTO>();

        //            string priority1 = "";
        //            string status1 = "";
        //            string AssignedBy = "";
        //            string AssignedTo = "";
        //            foreach (User_Task x in taskss)
        //            {
        //                if (x.CompletionDate != null)
        //                {
        //                    status1 = "Completed";
        //                }
        //                else
        //                {
        //                    if (x.EndDate < DateTime.Now)
        //                    {
        //                        status1 = "Late";
        //                    }
        //                    else
        //                    {
        //                        if (x.Status != null)
        //                            status1 = General_Purpose.getStatusValue(x.Status.Value);
        //                        else
        //                            status1 = "Started";
        //                    }
        //                }


        //                if (x.Priority != null)
        //                {
        //                    if (General_Purpose.getPriorityValue(x.Priority.Value) == "Medium")
        //                    {
        //                        priority1 = "<span class='label label-warning'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
        //                    }
        //                    else if (General_Purpose.getPriorityValue(x.Priority.Value) == "Low")
        //                    {
        //                        priority1 = "<span class='label label-success'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
        //                    }
        //                    else
        //                    {
        //                        priority1 = "<span class='label label-danger'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
        //                    }

        //                }
        //                else
        //                    priority1 = "";

        //                if (x.CreatedID == null)
        //                {
        //                    AssignedBy = "Self Assigned";

        //                }
        //                else if (x.User.Id == logedinuser.Id)
        //                {
        //                  //  AssignedBy = "You";
        //                    AssignedBy = x.User.FirstName + " " + x.User.LastName; ;
        //                }
        //                else
        //                {
        //                    AssignedBy = x.User.FirstName + " " + x.User.LastName;
        //                }

        //                AssignedTo= x.User1.FirstName + " " + x.User1.LastName;

        //                DivisionReportDTO obj = new DivisionReportDTO();

        //                obj.AssignedBy = AssignedBy;
        //                obj.AssignedTo = AssignedTo;

        //                if (x.Task.Division == null)
        //                {
        //                    obj.DivisionName = "No Division Assigned";
        //                }
        //                else
        //                {
        //                    obj.DivisionName = x.Task.Division.Name;

        //                }

        //                if (x.User1.Department == null)
        //                {
        //                    obj.DepartmentName = "Manager in the Division";
        //                }
        //                else
        //                {
        //                    obj.DepartmentName = x.User1.Department.Name;

        //                }

        //                if (x.Task == null)
        //                {
        //                    obj.TaskType = "No Task Type Assigned";
        //                    obj.TaskName = "No Task Name Assigned";
        //                }
        //                else
        //                {
        //                    obj.TaskType = x.Task.TaskType.Name;
        //                    obj.TaskName = x.Task.Name;
        //                }

        //                if (x.StartDate == null)
        //                {
        //                    obj.StartDate = "No Start Date Assigned";
        //                }
        //                else
        //                {
        //                    obj.StartDate = x.StartDate.Value.ToString("MM/dd/yyyy");
        //                }

        //                if (x.EndDate == null)
        //                {
        //                    obj.DueDate = "No Due Date Assigned";
        //                }
        //                else
        //                {
        //                    obj.DueDate = x.EndDate.Value.ToString("MM/dd/yyyy");
        //                }

        //                if (status1 == null)
        //                {
        //                    obj.CompletionStatus = "No Status Assigned";
        //                }
        //                else
        //                {
        //                    obj.CompletionStatus = status1;
        //                }

        //                if (priority1 == null)
        //                {
        //                    obj.Priority = "No Priority Assigned";
        //                }
        //                else
        //                {
        //                    obj.Priority = priority1;
        //                }
        //                if (x.CompletionDate != null)
        //                {
        //                    obj.CompletionDate = x.CompletionDate.Value.ToString("MM/dd/yyyy");
        //                }
        //                mnglist.Add(obj);
        //            }

        //            string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Division" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".pdf";
        //            string title = "Status Report by Division: " + (div != null ? div.Name : "NA");
        //            new PDF_Export().generate_division_report(mnglist, path, title);

        //            return File(path, MediaTypeNames.Text.Plain, "Division Report.pdf");
        //        }
        //        else
        //        {
        //            return RedirectToAction("StatusReportDivision", "Report", new { message = "PDF is not generated because Division name was not given." });


        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return RedirectToAction("StatusReportDivision", "Report", new { message = "PDF is not generated because of an unexpected error." });
        //    }
        //}

        public List<string> SearchStatusReportByDivisionExcel(int division = -1, string FilterDate = "", string startDate = "", string endDate = "", int department = -1, string taskId = "", int taskType = 0, int status = -1, int priority = -1, int start = 0, int end = 0, int complete = 0,int workertype=-1)
        {

            Log.Info("Excel generation method called");

            List<string> objlist = new List<string>();
            objlist.Add(logedinuser.Id.ToString());
            User manager = new UserBL().getUsersById(logedinuser.Id);


            List<User_Task> taskss = new User_TaskBL().getUser_TasksList().Where(x => x.IsPrivate == 0 && x.UserID!=null).ToList();
            string complettionName = string.Empty;

            if (division != -1)
            {
                taskss = taskss.Where(x => x.User1.DivisionId == division || (x.User1.DepartmentId != null && x.User1.Department.DivisionID == division)).ToList();
                Division div = new DivisionBL().getDivisionsById(division);

                if (department != -1)
                {
                    taskss = taskss.Where(x => x.User1.Department != null && x.User1.DepartmentId == department).ToList();
                }
 

                if (FilterDate != "")
                {
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

                    #region Days
                        DateTime currentDate = DateTime.Now;
                        DateTime lastDate = currentDate.AddDays(-1);
                        DateTime nextDate = currentDate.AddDays(1);
                    #endregion


                    #region Weeks
                        DateTime currentWeekStartDate = currentDate.AddDays(-daysTillCurrentDate);
                        DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                        DateTime lastWeekStartDate = currentDate.AddDays(-daysTillCurrentDate - 7);
                        DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

                        DateTime nextWeekStartDate = currentDate.AddDays(-daysTillCurrentDate + 7);
                        DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);
                    #endregion


                    #region Months
                        DateTime currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                        DateTime currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1);

                        DateTime lastMonthStartDate = currentDate.AddMonths(-1);
                        lastMonthStartDate = new DateTime(lastMonthStartDate.Year, lastMonthStartDate.Month, 1);
                        DateTime lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddDays(-1);

                        DateTime nextMonthStartDate = currentDate.AddMonths(1);
                        nextMonthStartDate = new DateTime(nextMonthStartDate.Year, nextMonthStartDate.Month, 1);
                        DateTime nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1);
                    #endregion


                    #region Quarters
                        DateTime currentQuarterStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                        DateTime currentQuarterEndDate = currentQuarterStartDate.AddMonths(4).AddDays(-1);
                        

                        DateTime lastQuarterStartDate = currentQuarterStartDate.AddMonths(-4);
                        DateTime lastQuarterEndDate = currentQuarterStartDate.AddDays(-1);

                        DateTime nextQuarterStartDate = currentQuarterEndDate.AddDays(1);
                        DateTime nextQuarterEndDate = nextQuarterStartDate.AddMonths(4).AddDays(-1);
                    #endregion


                    #region Years
                        int currentYear = currentDate.Year;
                        int lastYear = currentYear - 1;
                        int nextYear = currentYear + 1;
                    #endregion

                    //int lastMonthStartDate = DateTime.Now.AddMonths(-1).Month;
                    //int currentMonth = DateTime.Now.Month;
                    //int nextMonth = DateTime.Now.AddMonths(1).Month;


                    if (FilterDate == "Tomorrow")
                    {
                        //taskss = taskss.Where(x => x.StartDate == tomorrow || x.EndDate == tomorrow || x.CompletionDate == tomorrow).ToList();
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextDate.Date && Convert.ToDateTime(x.StartDate).Date < nextDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "Today")
                    {
                        //taskss = taskss.Where(x => x.StartDate == today || x.EndDate == today || x.CompletionDate == today).ToList();
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentDate.Date && Convert.ToDateTime(x.StartDate).Date < currentDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "Yesterday")
                    {
                        //taskss = taskss.Where(x => x.StartDate == yesterday || x.EndDate == yesterday || x.CompletionDate == yesterday).ToList();
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastDate.Date && Convert.ToDateTime(x.StartDate).Date < lastDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextMonth")
                    {
                        //taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Month == nextMonth || Convert.ToDateTime(x.EndDate).Month == nextMonth || Convert.ToDateTime(x.CompletionDate).Month == nextMonth).ToList();
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastMonth")
                    {
                        //taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Month == lastMonth || Convert.ToDateTime(x.EndDate).Month == lastMonth || Convert.ToDateTime(x.CompletionDate).Month == lastMonth).ToList();
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                    }
                    if (FilterDate == "ThisYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                    }
                    if (FilterDate == "LastYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                    }


                    //if (FilterDate == "NextQuarter")
                    //{
                    //    if (currentMonth == 1 || currentMonth == 2 || currentMonth == 3)
                    //    {
                    //        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Month == 4 || Convert.ToDateTime(x.EndDate).Month == 4 || Convert.ToDateTime(x.CompletionDate).Month == 4)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 5 || Convert.ToDateTime(x.EndDate).Month == 5 || Convert.ToDateTime(x.CompletionDate).Month == 5)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 6 || Convert.ToDateTime(x.EndDate).Month == 6 || Convert.ToDateTime(x.CompletionDate).Month == 6)).ToList();
                    //    }
                    //    if (currentMonth == 4 || currentMonth == 5 || currentMonth == 6)
                    //    {
                    //        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Month == 7 || Convert.ToDateTime(x.EndDate).Month == 7 || Convert.ToDateTime(x.CompletionDate).Month == 7)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 8 || Convert.ToDateTime(x.EndDate).Month == 8 || Convert.ToDateTime(x.CompletionDate).Month == 8)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 9 || Convert.ToDateTime(x.EndDate).Month == 9 || Convert.ToDateTime(x.CompletionDate).Month == 9)).ToList();
                    //    }
                    //    if (currentMonth == 7 || currentMonth == 8 || currentMonth == 9)
                    //    {
                    //        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Month == 10 || Convert.ToDateTime(x.EndDate).Month == 10 || Convert.ToDateTime(x.CompletionDate).Month == 10)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 11 || Convert.ToDateTime(x.EndDate).Month == 11 || Convert.ToDateTime(x.CompletionDate).Month == 11)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 12 || Convert.ToDateTime(x.EndDate).Month == 12 || Convert.ToDateTime(x.CompletionDate).Month == 12)).ToList();
                    //    }
                    //    if (currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
                    //    {
                    //        taskss = taskss.Where(x => ((Convert.ToDateTime(x.StartDate).Year == nextYear && Convert.ToDateTime(x.StartDate).Month == 1) || (Convert.ToDateTime(x.EndDate).Year == nextYear && Convert.ToDateTime(x.EndDate).Month == 1) || (Convert.ToDateTime(x.CompletionDate).Year == nextYear && Convert.ToDateTime(x.CompletionDate).Month == 1))
                    //                                    || ((Convert.ToDateTime(x.StartDate).Year == nextYear && Convert.ToDateTime(x.StartDate).Month == 2) || (Convert.ToDateTime(x.EndDate).Year == nextYear && Convert.ToDateTime(x.EndDate).Month == 2) || (Convert.ToDateTime(x.CompletionDate).Year == nextYear && Convert.ToDateTime(x.CompletionDate).Month == 2))
                    //                                    || ((Convert.ToDateTime(x.StartDate).Year == nextYear && Convert.ToDateTime(x.StartDate).Month == 3) || (Convert.ToDateTime(x.EndDate).Year == nextYear && Convert.ToDateTime(x.EndDate).Month == 3) || (Convert.ToDateTime(x.CompletionDate).Year == nextYear && Convert.ToDateTime(x.CompletionDate).Month == 3))).ToList();
                    //    }

                    //}
                    //if (FilterDate == "ThisQuarter")
                    //{
                    //    if (currentMonth == 1 || currentMonth == 2 || currentMonth == 3)
                    //    {
                    //        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Month == 1 || Convert.ToDateTime(x.EndDate).Month == 1 || Convert.ToDateTime(x.CompletionDate).Month == 1)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 2 || Convert.ToDateTime(x.EndDate).Month == 2 || Convert.ToDateTime(x.CompletionDate).Month == 2)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 3 || Convert.ToDateTime(x.EndDate).Month == 3 || Convert.ToDateTime(x.CompletionDate).Month == 3)).ToList();
                    //    }
                    //    if (currentMonth == 4 || currentMonth == 5 || currentMonth == 6)
                    //    {
                    //        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Month == 4 || Convert.ToDateTime(x.EndDate).Month == 4 || Convert.ToDateTime(x.CompletionDate).Month == 4)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 5 || Convert.ToDateTime(x.EndDate).Month == 5 || Convert.ToDateTime(x.CompletionDate).Month == 5)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 6 || Convert.ToDateTime(x.EndDate).Month == 6 || Convert.ToDateTime(x.CompletionDate).Month == 6)).ToList();
                    //    }
                    //    if (currentMonth == 7 || currentMonth == 8 || currentMonth == 9)
                    //    {
                    //        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Month == 7 || Convert.ToDateTime(x.EndDate).Month == 7 || Convert.ToDateTime(x.CompletionDate).Month == 7)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 8 || Convert.ToDateTime(x.EndDate).Month == 8 || Convert.ToDateTime(x.CompletionDate).Month == 8)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 9 || Convert.ToDateTime(x.EndDate).Month == 9 || Convert.ToDateTime(x.CompletionDate).Month == 9)).ToList();
                    //    }
                    //    if (currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
                    //    {
                    //        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Month == 10 || Convert.ToDateTime(x.EndDate).Month == 10 || Convert.ToDateTime(x.CompletionDate).Month == 10)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 11 || Convert.ToDateTime(x.EndDate).Month == 11 || Convert.ToDateTime(x.CompletionDate).Month == 11)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 12 || Convert.ToDateTime(x.EndDate).Month == 12 || Convert.ToDateTime(x.CompletionDate).Month == 12)).ToList();
                    //    }
                    //}
                    //if (FilterDate == "LastQuarter")
                    //{
                    //    if (currentMonth == 1 || currentMonth == 2 || currentMonth == 3)
                    //    {
                    //        taskss = taskss.Where(x => ((Convert.ToDateTime(x.StartDate).Year == lastYear && Convert.ToDateTime(x.StartDate).Month == 10) || (Convert.ToDateTime(x.EndDate).Year == lastYear && Convert.ToDateTime(x.EndDate).Month == 10) || (Convert.ToDateTime(x.CompletionDate).Year == lastYear && Convert.ToDateTime(x.CompletionDate).Month == 10))
                    //                                    || ((Convert.ToDateTime(x.StartDate).Year == lastYear && Convert.ToDateTime(x.StartDate).Month == 11) || (Convert.ToDateTime(x.EndDate).Year == lastYear && Convert.ToDateTime(x.EndDate).Month == 11) || (Convert.ToDateTime(x.CompletionDate).Year == lastYear && Convert.ToDateTime(x.CompletionDate).Month == 11))
                    //                                    || ((Convert.ToDateTime(x.StartDate).Year == lastYear && Convert.ToDateTime(x.StartDate).Month == 12) || (Convert.ToDateTime(x.EndDate).Year == lastYear && Convert.ToDateTime(x.EndDate).Month == 12) || (Convert.ToDateTime(x.CompletionDate).Year == lastYear && Convert.ToDateTime(x.CompletionDate).Month == 12))).ToList();


                    //    }
                    //    if (currentMonth == 4 || currentMonth == 5 || currentMonth == 6)
                    //    {
                    //        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Month == 1 || Convert.ToDateTime(x.EndDate).Month == 1 || Convert.ToDateTime(x.CompletionDate).Month == 1)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 2 || Convert.ToDateTime(x.EndDate).Month == 2 || Convert.ToDateTime(x.CompletionDate).Month == 2)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 3 || Convert.ToDateTime(x.EndDate).Month == 3 || Convert.ToDateTime(x.CompletionDate).Month == 3)).ToList();
                    //    }
                    //    if (currentMonth == 7 || currentMonth == 8 || currentMonth == 9)
                    //    {
                    //        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Month == 4 || Convert.ToDateTime(x.EndDate).Month == 4 || Convert.ToDateTime(x.CompletionDate).Month == 4)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 5 || Convert.ToDateTime(x.EndDate).Month == 5 || Convert.ToDateTime(x.CompletionDate).Month == 5)
                    //                                    || (Convert.ToDateTime(x.StartDate).Month == 6 || Convert.ToDateTime(x.EndDate).Month == 6 || Convert.ToDateTime(x.CompletionDate).Month == 6)).ToList();
                    //    }
                    //    if (currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
                    //    {
                    //        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Month == 7 || Convert.ToDateTime(x.EndDate).Month == 7 || Convert.ToDateTime(x.CompletionDate).Month == 7)
                    //                                   || (Convert.ToDateTime(x.StartDate).Month == 8 || Convert.ToDateTime(x.EndDate).Month == 8 || Convert.ToDateTime(x.CompletionDate).Month == 8)
                    //                                   || (Convert.ToDateTime(x.StartDate).Month == 9 || Convert.ToDateTime(x.EndDate).Month == 9 || Convert.ToDateTime(x.CompletionDate).Month == 9)).ToList();
                    //    }
                    //}
                        
                }

                if (taskId != "")
                {
                    taskss = new User_TaskBL().getUser_TasksList().Where(x => x.TaskID == Convert.ToInt32(taskId) && x.IsPrivate == 0).ToList();
                }

                if (taskType != 0)
                {
                    taskss = taskss.Where(x => x.Task.TaskTypeID == taskType).ToList();
                }

                if (status != -1)
                {
                    taskss = taskss.Where(x => x.Status == status).ToList();
                }
                if (priority != -1)
                {
                    taskss = taskss.Where(x => x.Priority == priority).ToList();
                }

                if (startDate != null && startDate != "")
                {
                    taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                            (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate)))
                    ).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                            (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate)))
                    ).ToList();
                }

                if (workertype != -1)
                {
                    // taskss = taskss.Where(x => new User_WorktypeBL().getUser_WorktypesList().Where(c => c.UserId == x.UserID && c.WorktypeId == workertype).FirstOrDefault().WorktypeId == workertype).ToList();
                    List<User_Task> list = new List<User_Task>();
                    foreach (User_Task item in taskss)
                    {
                        User_Worktype wt = new User_WorktypeBL().getUser_WorktypesList().Where(x => x.UserId == item.UserID && x.WorktypeId == workertype).FirstOrDefault();
                        if (wt != null)
                        {
                            list.Add(item);
                        }
                    }
                    taskss = list;
                }

                List<DivisionReportDTO> mnglist = new List<DivisionReportDTO>();

                string priority1 = "";
                string status1 = "";
                string AssignedBy = "";
                string AssignedTo = "";
                foreach (User_Task x in taskss)
                {
                    if (x.CreatedID.HasValue)
                    {

                    }
                        
                    if (x.CompletionDate != null)
                    {
                        status1 = "Completed";
                    }
                    else
                    {
                        if (x.EndDate < DateTime.Now)
                        {
                            status1 = "Late";
                        }
                        else
                        {
                            if (x.Status != null)
                                status1 = General_Purpose.getStatusValue(x.Status.Value);
                            else
                                status1 = "Started";
                        }
                    }


                    if (x.Priority != null)
                    {
                        if (General_Purpose.getPriorityValue(x.Priority.Value) == "Medium")
                        {
                            //priority1 = "<span class='label label-warning'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                            priority1 = "Medium";
                        }
                        else if (General_Purpose.getPriorityValue(x.Priority.Value) == "Low")
                        {
                            //priority1 = "<span class='label label-success'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                            priority1 = "Low";
                        }
                        else
                        {
                            //priority1 = "<span class='label label-danger'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                            priority1 = "High";
                        }

                    }
                    else
                        priority1 = "";

                    if (x.CreatedID == null)
                    {
                        AssignedBy = "Self Assigned";

                    }
                    else {
                        User u = new UserBL().getUsersById((int)x.CreatedID);
                        if (u.Id == logedinuser.Id)
                        {
                            // AssignedBy = "You";
                            AssignedBy = u.FirstName + " " + u.LastName; ;
                        }
                        else
                        {
                            AssignedBy = u.FirstName + " " + u.LastName;
                        }
                    }
                    AssignedTo = x.User1.FirstName + " " + x.User1.LastName;


                       
                    DivisionReportDTO obj = new DivisionReportDTO();

                    obj.AssignedBy = AssignedBy;
                    obj.AssignedTo = AssignedTo;

                    if (x.Task.Division == null)
                    {
                        obj.DivisionName = "No Division Assigned";
                    }
                    else
                    {
                        obj.DivisionName = x.Task.Division.Name;

                    }

                    if (x.User1.Department == null)
                    {
                        obj.DepartmentName = "Manager in the Division";
                    }
                    else
                    {
                        obj.DepartmentName = x.User1.Department.Name;

                    }

                    if (x.Task == null)
                    {
                        obj.TaskType = "No Task Type Assigned";
                        obj.TaskName = "No Task Name Assigned";
                    }
                    else
                    {
                        obj.TaskType = x.Task.TaskType.Name;
                        obj.TaskName = x.Task.Name;
                    }

                    if (x.StartDate == null)
                    {
                        obj.StartDate = "No Start Date Assigned";
                    }
                    else
                    {
                        obj.StartDate = x.StartDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (x.EndDate == null)
                    {
                        obj.DueDate = "No Due Date Assigned";
                    }
                    else
                    {
                        obj.DueDate = x.EndDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (status1 == null)
                    {
                        obj.CompletionStatus = "No Status Assigned";
                    }
                    else
                    {
                        obj.CompletionStatus = status1;
                    }

                    if (priority1 == null)
                    {
                        obj.Priority = "No Priority Assigned";
                    }
                    else
                    {
                        obj.Priority = priority1;
                    }

                    if (x.CompletionDate != null)
                    {
                        obj.CompletionDate = x.CompletionDate.Value.ToString("MM/dd/yyyy");
                    }

                    mnglist.Add(obj);
                }

                Log.Info("Excel generation method search completed");

                string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Division" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
                string title = "Status Report by Division: " + (div != null ? div.Name : "NA");
                ExcelManagement.generateDivisionExcel(path, mnglist, title);

                List<string> objlist2 = new List<string>();

                objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
                objlist.AddRange(objlist2);

                return objlist;

                //return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Division" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");
            }
            else
            {
                return null;
            }
            
        }


        public ActionResult StatusReportDepartment(string frId = "", string FilterDate = "", string startDate = "", string endDate = "", int tag = -1, int department = -1, string taskId = "", int taskType = 0, int status = -1, string fname = "", string lname = "", int priority = -1, int completionStatus = -1, string message = "")
        {
            try
            {
                FavoriteReport fr = null;

                if (frId != "")
                {
                    fr = new FavoriteReportBL().getFavoriteReportById(Convert.ToInt32(StringCipher.Base64Decode(frId)));
                }

                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                if (logedinuser.Role == 3)
                {
                    return Content("Acess Restricted !");
                }

                List<User_Task> tasks = new User_TaskBL().getUser_TasksList().Where(x => x.IsPrivate == 0).ToList();

                string complettionName = string.Empty;
                if (logedinuser.Role == 3)
                {
                    User manager = new UserBL().getUsersById(logedinuser.Id);
                    tasks = tasks.Where(x => x.User1.Department.DivisionID == manager.DivisionId).ToList();
                }


                if (department != -1)
                {
                    tasks = tasks.Where(x => x.User1.Department != null && x.User1.DepartmentId == department).ToList();
                }


                if (taskId != "")
                {
                    tasks = new User_TaskBL().getUser_TasksList().Where(x => x.TaskID == Convert.ToInt32(taskId) && x.IsPrivate == 0).ToList();
                }

                if (FilterDate != "")
                {
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

                    DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDate);
                    DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                    DateTime lastWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDate - 7);
                    DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

                    DateTime nextWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDate + 7);
                    DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);

                    //DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDate);
                    //DateTime lastWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDate - 7);
                    //DateTime nextWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDate + 7);

                    var today = DateTime.Today;
                    var tomorrow = today.AddDays(1);
                    var yesterday = today.AddDays(-1);

                    int lastMonth = DateTime.Now.AddMonths(-1).Month;
                    int currentMonth = DateTime.Now.Month;
                    int nextMonth = DateTime.Now.AddMonths(1).Month;

                    int currentYear = DateTime.Now.Year;
                    int lastYear = currentYear - 1;
                    int nextYear = currentYear + 1;

                    if (FilterDate == "Tomorrow")
                    {
                        tasks = tasks.Where(x => x.StartDate == tomorrow || x.EndDate == tomorrow || x.CompletionDate == tomorrow).ToList();
                    }
                    if (FilterDate == "Today")
                    {
                        tasks = tasks.Where(x => x.StartDate == today || x.EndDate == today || x.CompletionDate == today).ToList();
                    }
                    if (FilterDate == "Yesterday")
                    {
                        tasks = tasks.Where(x => x.StartDate == yesterday || x.EndDate == yesterday || x.CompletionDate == yesterday).ToList();
                    }
                    if (FilterDate == "ThisWeek")
                    {
                        tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
                             ).ToList();
                    }
                    if (FilterDate == "NextWeek")
                    {
                        tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
                             ).ToList();
                    }
                    if (FilterDate == "LastWeek")
                    {
                        //tasks = tasks.Where(x => (x.StartDate == lastWeekStartDate || x.StartDate == lastWeekStartDate.AddDays(+1) || x.StartDate == lastWeekStartDate.AddDays(+2) || x.StartDate == lastWeekStartDate.AddDays(+3) || x.StartDate == lastWeekStartDate.AddDays(+4) || x.StartDate == lastWeekStartDate.AddDays(+5) || x.StartDate == lastWeekStartDate.AddDays(+6))
                        //|| (x.EndDate == lastWeekStartDate || x.EndDate == lastWeekStartDate.AddDays(+1) || x.EndDate == lastWeekStartDate.AddDays(+2) || x.EndDate == lastWeekStartDate.AddDays(+3) || x.EndDate == lastWeekStartDate.AddDays(+4) || x.EndDate == lastWeekStartDate.AddDays(+5) || x.EndDate == lastWeekStartDate.AddDays(+6))
                        //|| (x.CompletionDate == lastWeekStartDate || x.CompletionDate == lastWeekStartDate.AddDays(+1) || x.CompletionDate == lastWeekStartDate.AddDays(+2) || x.CompletionDate == lastWeekStartDate.AddDays(+3) || x.CompletionDate == lastWeekStartDate.AddDays(+4) || x.CompletionDate == lastWeekStartDate.AddDays(+5) || x.CompletionDate == lastWeekStartDate.AddDays(+6))).ToList();

                        tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
                             ).ToList();
                    }
                    if (FilterDate == "NextMonth")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Month == nextMonth || Convert.ToDateTime(x.EndDate).Month == nextMonth || Convert.ToDateTime(x.CompletionDate).Month == nextMonth).ToList();
                    }
                    if (FilterDate == "ThisMonth")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Month == currentMonth || Convert.ToDateTime(x.EndDate).Month == currentMonth || Convert.ToDateTime(x.CompletionDate).Month == currentMonth).ToList();
                    }
                    if (FilterDate == "LastMonth")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Month == lastMonth || Convert.ToDateTime(x.EndDate).Month == lastMonth || Convert.ToDateTime(x.CompletionDate).Month == lastMonth).ToList();
                    }
                    if (FilterDate == "NextQuarter")
                    {
                        if (currentMonth == 1 || currentMonth == 2 || currentMonth == 3)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 4 || Convert.ToDateTime(x.EndDate).Month == 4 || Convert.ToDateTime(x.CompletionDate).Month == 4)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 5 || Convert.ToDateTime(x.EndDate).Month == 5 || Convert.ToDateTime(x.CompletionDate).Month == 5)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 6 || Convert.ToDateTime(x.EndDate).Month == 6 || Convert.ToDateTime(x.CompletionDate).Month == 6)).ToList();
                        }
                        if (currentMonth == 4 || currentMonth == 5 || currentMonth == 6)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 7 || Convert.ToDateTime(x.EndDate).Month == 7 || Convert.ToDateTime(x.CompletionDate).Month == 7)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 8 || Convert.ToDateTime(x.EndDate).Month == 8 || Convert.ToDateTime(x.CompletionDate).Month == 8)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 9 || Convert.ToDateTime(x.EndDate).Month == 9 || Convert.ToDateTime(x.CompletionDate).Month == 9)).ToList();
                        }
                        if (currentMonth == 7 || currentMonth == 8 || currentMonth == 9)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 10 || Convert.ToDateTime(x.EndDate).Month == 10 || Convert.ToDateTime(x.CompletionDate).Month == 10)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 11 || Convert.ToDateTime(x.EndDate).Month == 11 || Convert.ToDateTime(x.CompletionDate).Month == 11)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 12 || Convert.ToDateTime(x.EndDate).Month == 12 || Convert.ToDateTime(x.CompletionDate).Month == 12)).ToList();
                        }
                        if (currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
                        {
                            tasks = tasks.Where(x => ((Convert.ToDateTime(x.StartDate).Year == nextYear && Convert.ToDateTime(x.StartDate).Month == 1) || (Convert.ToDateTime(x.EndDate).Year == nextYear && Convert.ToDateTime(x.EndDate).Month == 1) || (Convert.ToDateTime(x.CompletionDate).Year == nextYear && Convert.ToDateTime(x.CompletionDate).Month == 1))
                                                        || ((Convert.ToDateTime(x.StartDate).Year == nextYear && Convert.ToDateTime(x.StartDate).Month == 2) || (Convert.ToDateTime(x.EndDate).Year == nextYear && Convert.ToDateTime(x.EndDate).Month == 2) || (Convert.ToDateTime(x.CompletionDate).Year == nextYear && Convert.ToDateTime(x.CompletionDate).Month == 2))
                                                        || ((Convert.ToDateTime(x.StartDate).Year == nextYear && Convert.ToDateTime(x.StartDate).Month == 3) || (Convert.ToDateTime(x.EndDate).Year == nextYear && Convert.ToDateTime(x.EndDate).Month == 3) || (Convert.ToDateTime(x.CompletionDate).Year == nextYear && Convert.ToDateTime(x.CompletionDate).Month == 3))).ToList();
                        }

                    }
                    if (FilterDate == "ThisQuarter")
                    {
                        if (currentMonth == 1 || currentMonth == 2 || currentMonth == 3)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 1 || Convert.ToDateTime(x.EndDate).Month == 1 || Convert.ToDateTime(x.CompletionDate).Month == 1)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 2 || Convert.ToDateTime(x.EndDate).Month == 2 || Convert.ToDateTime(x.CompletionDate).Month == 2)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 3 || Convert.ToDateTime(x.EndDate).Month == 3 || Convert.ToDateTime(x.CompletionDate).Month == 3)).ToList();
                        }
                        if (currentMonth == 4 || currentMonth == 5 || currentMonth == 6)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 4 || Convert.ToDateTime(x.EndDate).Month == 4 || Convert.ToDateTime(x.CompletionDate).Month == 4)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 5 || Convert.ToDateTime(x.EndDate).Month == 5 || Convert.ToDateTime(x.CompletionDate).Month == 5)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 6 || Convert.ToDateTime(x.EndDate).Month == 6 || Convert.ToDateTime(x.CompletionDate).Month == 6)).ToList();
                        }
                        if (currentMonth == 7 || currentMonth == 8 || currentMonth == 9)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 7 || Convert.ToDateTime(x.EndDate).Month == 7 || Convert.ToDateTime(x.CompletionDate).Month == 7)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 8 || Convert.ToDateTime(x.EndDate).Month == 8 || Convert.ToDateTime(x.CompletionDate).Month == 8)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 9 || Convert.ToDateTime(x.EndDate).Month == 9 || Convert.ToDateTime(x.CompletionDate).Month == 9)).ToList();
                        }
                        if (currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 10 || Convert.ToDateTime(x.EndDate).Month == 10 || Convert.ToDateTime(x.CompletionDate).Month == 10)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 11 || Convert.ToDateTime(x.EndDate).Month == 11 || Convert.ToDateTime(x.CompletionDate).Month == 11)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 12 || Convert.ToDateTime(x.EndDate).Month == 12 || Convert.ToDateTime(x.CompletionDate).Month == 12)).ToList();
                        }
                    }
                    if (FilterDate == "LastQuarter")
                    {
                        if (currentMonth == 1 || currentMonth == 2 || currentMonth == 3)
                        {
                            tasks = tasks.Where(x => ((Convert.ToDateTime(x.StartDate).Year == lastYear && Convert.ToDateTime(x.StartDate).Month == 10) || (Convert.ToDateTime(x.EndDate).Year == lastYear && Convert.ToDateTime(x.EndDate).Month == 10) || (Convert.ToDateTime(x.CompletionDate).Year == lastYear && Convert.ToDateTime(x.CompletionDate).Month == 10))
                                                        || ((Convert.ToDateTime(x.StartDate).Year == lastYear && Convert.ToDateTime(x.StartDate).Month == 11) || (Convert.ToDateTime(x.EndDate).Year == lastYear && Convert.ToDateTime(x.EndDate).Month == 11) || (Convert.ToDateTime(x.CompletionDate).Year == lastYear && Convert.ToDateTime(x.CompletionDate).Month == 11))
                                                        || ((Convert.ToDateTime(x.StartDate).Year == lastYear && Convert.ToDateTime(x.StartDate).Month == 12) || (Convert.ToDateTime(x.EndDate).Year == lastYear && Convert.ToDateTime(x.EndDate).Month == 12) || (Convert.ToDateTime(x.CompletionDate).Year == lastYear && Convert.ToDateTime(x.CompletionDate).Month == 12))).ToList();


                        }
                        if (currentMonth == 4 || currentMonth == 5 || currentMonth == 6)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 1 || Convert.ToDateTime(x.EndDate).Month == 1 || Convert.ToDateTime(x.CompletionDate).Month == 1)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 2 || Convert.ToDateTime(x.EndDate).Month == 2 || Convert.ToDateTime(x.CompletionDate).Month == 2)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 3 || Convert.ToDateTime(x.EndDate).Month == 3 || Convert.ToDateTime(x.CompletionDate).Month == 3)).ToList();
                        }
                        if (currentMonth == 7 || currentMonth == 8 || currentMonth == 9)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 4 || Convert.ToDateTime(x.EndDate).Month == 4 || Convert.ToDateTime(x.CompletionDate).Month == 4)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 5 || Convert.ToDateTime(x.EndDate).Month == 5 || Convert.ToDateTime(x.CompletionDate).Month == 5)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 6 || Convert.ToDateTime(x.EndDate).Month == 6 || Convert.ToDateTime(x.CompletionDate).Month == 6)).ToList();
                        }
                        if (currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 7 || Convert.ToDateTime(x.EndDate).Month == 7 || Convert.ToDateTime(x.CompletionDate).Month == 7)
                                                       || (Convert.ToDateTime(x.StartDate).Month == 8 || Convert.ToDateTime(x.EndDate).Month == 8 || Convert.ToDateTime(x.CompletionDate).Month == 8)
                                                       || (Convert.ToDateTime(x.StartDate).Month == 9 || Convert.ToDateTime(x.EndDate).Month == 9 || Convert.ToDateTime(x.CompletionDate).Month == 9)).ToList();
                        }
                    }
                    if (FilterDate == "NextYear")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                    }
                    if (FilterDate == "ThisYear")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                    }
                    if (FilterDate == "LastYear")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                    }
                }

                if (tag != -1)
                {
                    tasks = tasks.Where(x => x.Task.TaskTags.Where(z => z.TagId == tag).Count() > 0).ToList();
                }
                if (taskType != 0)
                {
                    tasks = tasks.Where(x => x.Task.TaskTypeID == taskType).ToList();
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

                if (startDate != null && startDate != "")
                {
                    tasks = tasks.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                            (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate)))
                    ).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    tasks = tasks.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                            (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate)))
                    ).ToList();
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


                if (fr != null && fr.ReportType == 2)
                {
                    if (fr.Name != "" && fr.Name != null)
                    {
                        ViewBag.selectedreportname = fr.Name;
                    }

                    if (fr.Department != "" && fr.Department != null)
                    {
                        ViewBag.selecteddepartment = new DepartmentBL().getDepartmentsById(Convert.ToInt32(fr.Department));
                    }

                    if (fr.WorkerType != "" && fr.WorkerType != null)
                    {
                        ViewBag.selectedworkertype = new WorktypeBL().getWorktypesById(Convert.ToInt32(fr.WorkerType));
                    }

                    if (fr.TaskName != "" && fr.TaskName != null)
                    {
                        ViewBag.selectedtask = new TaskBL().getTasksById(Convert.ToInt32(fr.TaskName));
                    }

                    if (fr.TaskType != "" && fr.TaskType != null)
                    {
                        ViewBag.selectedtasktype = new TaskTypeBL().getTaskTypesById(Convert.ToInt32(fr.TaskType));
                    }

                    if (fr.Status != "" && fr.Status != null)
                    {
                        ViewBag.selectedstatus = fr.Status;
                    }

                    if (fr.Priority != "" && fr.Priority != null)
                    {
                        ViewBag.selectedpriority = fr.Priority;
                    }

                    if (fr.StartingDate != "" && fr.StartingDate != null)
                    {
                        ViewBag.selectedstartdate = Convert.ToDateTime(fr.StartingDate);
                    }

                    if (fr.EndingDate != "" && fr.EndingDate != null)
                    {
                        ViewBag.selectedendDate = Convert.ToDateTime(fr.EndingDate);
                    }

                    if (fr.StartDate != "" && fr.StartDate != null)
                    {
                        ViewBag.selectedstartId = fr.StartDate;
                    }

                    if (fr.EndDate != "" && fr.EndDate != null)
                    {
                        ViewBag.selectedendId = fr.EndDate;
                    }

                    if (fr.CompleteDate != "" && fr.CompleteDate != null)
                    {
                        ViewBag.selectedcompleteId = fr.CompleteDate;
                    }

                    if (fr.FilterDate != "" && fr.FilterDate != null)
                    {
                        ViewBag.filterdate = fr.FilterDate;
                    }
                }
                else
                {
                    ViewBag.filterdate = null;
                    ViewBag.selecteddepartment = null;
                    ViewBag.selectedworkertype = null;
                    ViewBag.selectedtaskname = null;
                    ViewBag.selectedtasktype = null;
                    ViewBag.selectedstatus = null;
                    ViewBag.selectedpriority = null;
                    ViewBag.selectedstartingdate = null;
                    ViewBag.selectedendDate = null;
                    ViewBag.selectedstartId = null;
                    ViewBag.selectedendId = null;
                    ViewBag.selectedcompleteId = null;
                }



                ViewBag.tasks = tasks;

                ViewBag.tag = new TagBL().getTagsById(tag);
                ViewBag.tags = new TagBL().getTagsList();

                ViewBag.startDate = startDate;
                ViewBag.endDate = endDate;
                ViewBag.status = status;
                ViewBag.department = new DepartmentBL().getDepartmentsById(department);
                if (logedinuser.Role == 2 || logedinuser.Role == 4)
                    ViewBag.departments = new DepartmentBL().getDepartmentsList().Where(x => x.DivisionID == new UserBL().getUsersById(logedinuser.Id).DivisionId);
                else
                    ViewBag.departments = new DepartmentBL().getDepartmentsList();
                //ViewBag.taskName = taskName;
                ViewBag.taskType = new TaskTypeBL().getTaskTypesById(taskType);
                ViewBag.taskTypes = new TaskTypeBL().getTaskTypesList();
                ViewBag.statuses = new List<int>() { 0, 1, 2, 3, 4 }.Where(x => x != status);
                ViewBag.completionListId = new List<int>() { 1, 2, 3 };
                ViewBag.completionList = new List<string>() { "In Process", "Completed", "Late" };
                ViewBag.completionId = completionStatus;
                ViewBag.completionIdName = complettionName;
                ViewBag.frId = frId;
                ViewBag.message = message;

                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        //[HttpPost]
        public List<string> SearchStatusReportByDepartment(string FilterDate, string startDate = "", string endDate = "", int department = -1, string taskId = "", int taskType = 0, int status = -1, int priority = -1, int workertype = -1)
        {
            Log.Info("Pdf generation method called");

            List<string> objlist = new List<string>();
            objlist.Add(logedinuser.Id.ToString());
            User manager = new UserBL().getUsersById(logedinuser.Id);

            if (department != -1)
            {
                Department dept = new DepartmentBL().getDepartmentsById(department);
                List<User_Task> taskss = new User_TaskBL().getUser_TasksList().Where(x => x.IsPrivate == 0).ToList();
                string complettionName = string.Empty;


                if (department != -1)
                {
                    taskss = taskss.Where(x => x.User1.Department != null && x.User1.DepartmentId == department).ToList();
                }


                if (taskId != "")
                {
                    taskss = new User_TaskBL().getUser_TasksList().Where(x => x.TaskID == Convert.ToInt32(taskId) && x.IsPrivate == 0).ToList();
                }

                if (FilterDate != "")
                {
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

                    #region Days
                        DateTime currentDate = DateTime.Now;
                        DateTime lastDate = currentDate.AddDays(-1);
                        DateTime nextDate = currentDate.AddDays(1);
                    #endregion


                    #region Weeks
                        DateTime currentWeekStartDate = currentDate.AddDays(-daysTillCurrentDate);
                        DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                        DateTime lastWeekStartDate = currentDate.AddDays(-daysTillCurrentDate - 7);
                        DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

                        DateTime nextWeekStartDate = currentDate.AddDays(-daysTillCurrentDate + 7);
                        DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);
                    #endregion


                    #region Months
                        DateTime currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                        DateTime currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1);

                        DateTime lastMonthStartDate = currentDate.AddMonths(-1);
                        lastMonthStartDate = new DateTime(lastMonthStartDate.Year, lastMonthStartDate.Month, 1);
                        DateTime lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddDays(-1);

                        DateTime nextMonthStartDate = currentDate.AddMonths(1);
                        nextMonthStartDate = new DateTime(nextMonthStartDate.Year, nextMonthStartDate.Month, 1);
                        DateTime nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1);
                    #endregion


                    #region Quarters
                        DateTime currentQuarterStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                        DateTime currentQuarterEndDate = currentQuarterStartDate.AddMonths(4).AddDays(-1);


                        DateTime lastQuarterStartDate = currentQuarterStartDate.AddMonths(-4);
                        DateTime lastQuarterEndDate = currentQuarterStartDate.AddDays(-1);

                        DateTime nextQuarterStartDate = currentQuarterEndDate.AddDays(1);
                        DateTime nextQuarterEndDate = nextQuarterStartDate.AddMonths(4).AddDays(-1);
                    #endregion


                    #region Years
                        int currentYear = currentDate.Year;
                        int lastYear = currentYear - 1;
                        int nextYear = currentYear + 1;
                    #endregion


                    if (FilterDate == "Tomorrow")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextDate.Date && Convert.ToDateTime(x.StartDate).Date < nextDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "Today")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentDate.Date && Convert.ToDateTime(x.StartDate).Date < currentDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "Yesterday")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastDate.Date && Convert.ToDateTime(x.StartDate).Date < lastDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                    }
                    if (FilterDate == "ThisYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                    }
                    if (FilterDate == "LastYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                    }

                }

                if (taskType != 0)
                {
                    taskss = taskss.Where(x => x.Task.TaskTypeID == taskType).ToList();
                }
                if (status != -1)
                {
                    taskss = taskss.Where(x => x.Status == status).ToList();
                }
                if (priority != -1)
                {
                    taskss = taskss.Where(x => x.Priority == priority).ToList();
                }
                if (startDate != null && startDate != "")
                {
                    taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                            (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate)))
                    ).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                            (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate)))
                    ).ToList();
                }

                if (workertype != -1)
                {
                    // taskss = taskss.Where(x => new User_WorktypeBL().getUser_WorktypesList().Where(c => c.UserId == x.UserID && c.WorktypeId == workertype).FirstOrDefault().WorktypeId == workertype).ToList();
                    List<User_Task> list = new List<User_Task>();
                    foreach (User_Task item in taskss)
                    {
                        User_Worktype wt = new User_WorktypeBL().getUser_WorktypesList().Where(x => x.UserId == item.UserID && x.WorktypeId == workertype).FirstOrDefault();
                        if (wt != null)
                        {
                            list.Add(item);
                        }
                    }
                    taskss = list;
                }

                List<DepartmentReportDTO> mnglist = new List<DepartmentReportDTO>();

                string priority1 = "";
                string status1 = "";
                string AssignedBy = "";
                foreach (User_Task x in taskss)
                {
                    if (x.CompletionDate != null)
                    {
                        status1 = "Completed";
                    }
                    else
                    {
                        if (x.EndDate < DateTime.Now)
                        {
                            status1 = "Late";
                        }
                        else
                        {
                            if (x.Status != null)
                                status1 = General_Purpose.getStatusValue(x.Status.Value);
                            else
                                status1 = "Started";
                        }
                    }


                    if (x.Priority != null)
                    {
                        if (General_Purpose.getPriorityValue(x.Priority.Value) == "Medium")
                        {
                            priority1 = General_Purpose.getPriorityValue(x.Priority.Value);
                        }
                        else if (General_Purpose.getPriorityValue(x.Priority.Value) == "Low")
                        {
                            priority1 = General_Purpose.getPriorityValue(x.Priority.Value);
                        }
                        else
                        {
                            priority1 = General_Purpose.getPriorityValue(x.Priority.Value);
                        }

                    }
                    else
                        priority1 = "";

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

                    DepartmentReportDTO obj = new DepartmentReportDTO();

                    obj.AssignedBy = AssignedBy;

                    if (x.User1.FirstName == null && x.User1.FirstName == null)
                    {
                        obj.EmployeeName = "No User Assigned";
                    }
                    else
                    {
                        obj.EmployeeName = x.User1.FirstName + ' ' + x.User1.LastName;

                    }

                    if (x.User1.Department == null)
                    {
                        obj.DepartmentName = "No Department Assigned";
                    }
                    else
                    {
                        obj.DepartmentName = x.User1.Department.Name;

                    }

                    if (x.Task == null)
                    {
                        obj.TaskType = "No Task Type Assigned";
                        obj.TaskName = "No Task Name Assigned";
                    }
                    else
                    {
                        obj.TaskType = x.Task.TaskType.Name;
                        obj.TaskName = x.Task.Name;
                    }

                    if (x.StartDate == null)
                    {
                        obj.StartDate = "No Start Date Assigned";
                    }
                    else
                    {
                        obj.StartDate = x.StartDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (x.EndDate == null)
                    {
                        obj.DueDate = "No Due Date Assigned";
                    }
                    else
                    {
                        obj.DueDate = x.EndDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (status1 == null)
                    {
                        obj.CompletionStatus = "No Status Assigned";
                    }
                    else
                    {
                        obj.CompletionStatus = status1;
                    }
                    if (x.CompletionDate != null)
                    {
                        if (x.CompletionDate.HasValue)
                        {
                            obj.CompletionDate = x.CompletionDate.Value.ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            obj.CompletionDate = string.Empty;
                        }
                    }

                    if (priority1 == null)
                    {
                        obj.Priority = "No Priority Assigned";
                    }
                    else
                    {
                        obj.Priority = priority1;
                    }

                    mnglist.Add(obj);
                }

                Log.Info("Pdf generation method search completed");

                string title = "Status Report by Department: " + (dept != null ? dept.Name : "NA");
                string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Department" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".pdf";
                new PDF_Export().generate_department_report(mnglist, path, title);

                List<string> objlist2 = new List<string>();

                objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
                objlist.AddRange(objlist2);

                return objlist;

                //return File(path, MediaTypeNames.Text.Plain, "Department Report.pdf");
            }
            else
            {
                //return RedirectToAction("StatusReportDepartment", "Report", new { message = "Couldn't generate PDF because department name wasn't added." });
                return null;
            }
            
            //catch
            //{
            //    return RedirectToAction("StatusReportDepartment", "Report", new { message = "Couldn't generate PDF because an unexpected error." });

            //}

        }

        public List<string> SearchStatusReportByDepartmentExcel(string FilterDate = "", string startDate = "", string endDate = "", int department = -1, string taskId = "", int taskType = 0, int status = -1, int priority = -1, int workertype = -1)
        {
            Log.Info("Excel generation method called");

            List<string> objlist = new List<string>();
            objlist.Add(logedinuser.Id.ToString());
            User manager = new UserBL().getUsersById(logedinuser.Id);

            if (department != -1)
            {
                Department dept = new DepartmentBL().getDepartmentsById(department);
                List<User_Task> taskss = new User_TaskBL().getUser_TasksList().Where(x => x.IsPrivate == 0 && x.UserID!=null).ToList();
                string complettionName = string.Empty;


                if (department != -1)
                {
                    taskss = taskss.Where(x => x.User1.Department != null && x.User1.DepartmentId == department).ToList();
                }


                if (taskId != "")
                {
                    taskss = new User_TaskBL().getUser_TasksList().Where(x => x.TaskID == Convert.ToInt32(taskId) && x.IsPrivate == 0).ToList();
                }

                if (FilterDate != "")
                {
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

                    #region Days
                        DateTime currentDate = DateTime.Now;
                        DateTime lastDate = currentDate.AddDays(-1);
                        DateTime nextDate = currentDate.AddDays(1);
                    #endregion


                    #region Weeks
                        DateTime currentWeekStartDate = currentDate.AddDays(-daysTillCurrentDate);
                        DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                        DateTime lastWeekStartDate = currentDate.AddDays(-daysTillCurrentDate - 7);
                        DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

                        DateTime nextWeekStartDate = currentDate.AddDays(-daysTillCurrentDate + 7);
                        DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);
                    #endregion


                    #region Months
                        DateTime currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                        DateTime currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1);

                        DateTime lastMonthStartDate = currentDate.AddMonths(-1);
                        lastMonthStartDate = new DateTime(lastMonthStartDate.Year, lastMonthStartDate.Month, 1);
                        DateTime lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddDays(-1);

                        DateTime nextMonthStartDate = currentDate.AddMonths(1);
                        nextMonthStartDate = new DateTime(nextMonthStartDate.Year, nextMonthStartDate.Month, 1);
                        DateTime nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1);
                    #endregion


                    #region Quarters
                        DateTime currentQuarterStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                        DateTime currentQuarterEndDate = currentQuarterStartDate.AddMonths(4).AddDays(-1);


                        DateTime lastQuarterStartDate = currentQuarterStartDate.AddMonths(-4);
                        DateTime lastQuarterEndDate = currentQuarterStartDate.AddDays(-1);

                        DateTime nextQuarterStartDate = currentQuarterEndDate.AddDays(1);
                        DateTime nextQuarterEndDate = nextQuarterStartDate.AddMonths(4).AddDays(-1);
                    #endregion


                    #region Years
                        int currentYear = currentDate.Year;
                        int lastYear = currentYear - 1;
                        int nextYear = currentYear + 1;
                    #endregion


                    if (FilterDate == "Tomorrow")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextDate.Date && Convert.ToDateTime(x.StartDate).Date < nextDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "Today")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentDate.Date && Convert.ToDateTime(x.StartDate).Date < currentDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "Yesterday")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastDate.Date && Convert.ToDateTime(x.StartDate).Date < lastDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                    }
                    if (FilterDate == "ThisYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                    }
                    if (FilterDate == "LastYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                    }

                }

                if (taskType != 0)
                {
                    taskss = taskss.Where(x => x.Task.TaskTypeID == taskType).ToList();
                }

                if (status != -1)
                {
                    taskss = taskss.Where(x => x.Status == status).ToList();
                }
                if (priority != -1)
                {
                    taskss = taskss.Where(x => x.Priority == priority).ToList();
                }
                if (startDate != null && startDate != "")
                {
                    taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                            (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate)))
                    ).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                            (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate)))
                    ).ToList();
                }

                if (workertype != -1)
                {
                    // taskss = taskss.Where(x => new User_WorktypeBL().getUser_WorktypesList().Where(c => c.UserId == x.UserID && c.WorktypeId == workertype).FirstOrDefault().WorktypeId == workertype).ToList();
                    List<User_Task> list = new List<User_Task>();
                    foreach (User_Task item in taskss)
                    {
                        User_Worktype wt = new User_WorktypeBL().getUser_WorktypesList().Where(x => x.UserId == item.UserID && x.WorktypeId == workertype).FirstOrDefault();
                        if (wt != null)
                        {
                            list.Add(item);
                        }
                    }
                    taskss = list;
                }

                List<DepartmentReportDTO> mnglist = new List<DepartmentReportDTO>();

                string priority1 = "";
                string status1 = "";
                string AssignedBy = "";
                string AssignedTo = "";
                foreach (User_Task x in taskss)
                {
                    if (x.CompletionDate != null)
                    {
                        status1 = "Completed";
                    }
                    else
                    {
                        if (x.EndDate < DateTime.Now)
                        {
                            status1 = "Late";
                        }
                        else
                        {
                            if (x.Status != null)
                                status1 = General_Purpose.getStatusValue(x.Status.Value);
                            else
                                status1 = "Started";
                        }
                    }


                    if (x.Priority != null)
                    {
                        if (General_Purpose.getPriorityValue(x.Priority.Value) == "Medium")
                        {
                            priority1 = General_Purpose.getPriorityValue(x.Priority.Value);
                        }
                        else if (General_Purpose.getPriorityValue(x.Priority.Value) == "Low")
                        {
                            priority1 = General_Purpose.getPriorityValue(x.Priority.Value);
                        }
                        else
                        {
                            priority1 = General_Purpose.getPriorityValue(x.Priority.Value);
                        }

                    }
                    else
                        priority1 = "";

                    if (x.CreatedID == null)
                    {
                        AssignedBy = "Self Assigned";

                    }
                    else if (x.User.Id == logedinuser.Id)
                    {
                        // AssignedBy = "You";
                        AssignedBy = x.User.FirstName + " " + x.User.LastName; ;
                    }
                    else
                    {
                        AssignedBy = x.User.FirstName + " " + x.User.LastName;
                    }

                    AssignedTo= x.User1.FirstName + " " + x.User1.LastName;
                    DepartmentReportDTO obj = new DepartmentReportDTO();

                    obj.AssignedBy = AssignedBy;
                    //  obj.AssignedTo = AssignedTo;
                    if (x.User1.FirstName == null && x.User1.FirstName == null)
                    {
                        obj.EmployeeName = "No User Assigned";
                    }
                    else
                    {
                        obj.EmployeeName = x.User1.FirstName + ' ' + x.User1.LastName;

                    }

                    if (x.User1.Department == null)
                    {
                        obj.DepartmentName = "No Department Assigned";
                    }
                    else
                    {
                        obj.DepartmentName = x.User1.Department.Name;

                    }

                    if (x.Task == null)
                    {
                        obj.TaskType = "No Task Type Assigned";
                        obj.TaskName = "No Task Name Assigned";
                    }
                    else
                    {
                        obj.TaskType = x.Task.TaskType.Name;
                        obj.TaskName = x.Task.Name;
                    }

                    if (x.StartDate == null)
                    {
                        obj.StartDate = "No Start Date Assigned";
                    }
                    else
                    {
                        obj.StartDate = x.StartDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (x.EndDate == null)
                    {
                        obj.DueDate = "No Due Date Assigned";
                    }
                    else
                    {
                        obj.DueDate = x.EndDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (status1 == null)
                    {
                        obj.CompletionStatus = "No Status Assigned";
                    }
                    else
                    {
                        obj.CompletionStatus = status1;
                    }
                    if (x.CompletionDate != null)
                    {
                        if (x.CompletionDate.HasValue)
                        {
                            obj.CompletionDate = x.CompletionDate.Value.ToString("MM/d/yyyy");
                        }
                        else
                        {
                            obj.CompletionDate = string.Empty;
                        }
                    }

                    if (priority1 == null)
                    {
                        obj.Priority = "No Priority Assigned";
                    }
                    else
                    {
                        obj.Priority = priority1;
                    }

                    mnglist.Add(obj);
                }

                Log.Info("Excel generation method search completed");

                string title = "Status Report by Department: " + (dept != null ? dept.Name : "NA");
                string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Department Report" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
                ExcelManagement.generateDepartmentExcel(path, mnglist, title);

                List<string> objlist2 = new List<string>();

                objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
                objlist.AddRange(objlist2);

                return objlist;

                //return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Department Report" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");
            }
            else
            {
                //return RedirectToAction("StatusReportDepartment", "Report", new { message = "Couldn't generate Excel because department name wasn't added." });
                return null;
            }
            
            //catch (Exception e)
            //{
            //    return RedirectToAction("StatusReportDepartment", "Report", new { message = "Couldn't generate Excel because of an unknown error." });

            //}

        }


        public ActionResult StatusReportEmployee(string frId = "", string FilterDate = "", string startDate = "", string endDate = "", int tag = -1, string taskId = "", int taskType = 0, int status = -1, int priority = -1, int empId = -1, string message = "")
        {
            try
            {
                FavoriteReport fr = null;

                if (frId != "")
                {
                    fr = new FavoriteReportBL().getFavoriteReportById(Convert.ToInt32(StringCipher.Base64Decode(frId)));
                }

                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                if (logedinuser.Role == 3)
                {
                    return Content("Acess Restricted !");
                }

                ViewBag.sd = 0;
                ViewBag.ed = 0;
                ViewBag.cd = 0;

                List<User_Task> tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Role == 3 && x.IsPrivate == 0).ToList();

                string complettionName = string.Empty;
                if (logedinuser.Role != 1)
                {
                    User manager = new UserBL().getUsersById(logedinuser.Id);
                    tasks = tasks.Where(x => x.User1.Department.DivisionID == manager.DivisionId).ToList();
                }

                if (empId != -1)
                {
                    tasks = tasks.Where(x => x.UserID == Convert.ToInt32(empId)).ToList();
                }

                if (taskId != "")
                {
                    tasks = tasks.Where(x => x.TaskID == Convert.ToInt32(taskId)).ToList();
                }
                if (FilterDate != "")
                {
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

                    DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDate);
                    DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                    DateTime lastWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDate - 7);
                    DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

                    DateTime nextWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDate + 7);
                    DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);

                    var today = DateTime.Today;
                    var tomorrow = today.AddDays(1);
                    var yesterday = today.AddDays(-1);

                    int lastMonth = DateTime.Now.AddMonths(-1).Month;
                    int currentMonth = DateTime.Now.Month;
                    int nextMonth = DateTime.Now.AddMonths(1).Month;

                    int currentYear = DateTime.Now.Year;
                    int lastYear = currentYear - 1;
                    int nextYear = currentYear + 1;

                    if (FilterDate == "Tomorrow")
                    {
                        tasks = tasks.Where(x => x.StartDate == tomorrow || x.EndDate == tomorrow || x.CompletionDate == tomorrow).ToList();
                    }
                    if (FilterDate == "Today")
                    {
                        tasks = tasks.Where(x => x.StartDate == today || x.EndDate == today || x.CompletionDate == today).ToList();
                    }
                    if (FilterDate == "Yesterday")
                    {
                        tasks = tasks.Where(x => x.StartDate == yesterday || x.EndDate == yesterday || x.CompletionDate == yesterday).ToList();
                    }
                    if (FilterDate == "ThisWeek")
                    {
                        tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "NextWeek")
                    {
                        tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "LastWeek")
                    {
                        tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "NextMonth")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Month == nextMonth || Convert.ToDateTime(x.EndDate).Month == nextMonth || Convert.ToDateTime(x.CompletionDate).Month == nextMonth).ToList();
                    }
                    if (FilterDate == "ThisMonth")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Month == currentMonth || Convert.ToDateTime(x.EndDate).Month == currentMonth || Convert.ToDateTime(x.CompletionDate).Month == currentMonth).ToList();
                    }
                    if (FilterDate == "LastMonth")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Month == lastMonth || Convert.ToDateTime(x.EndDate).Month == lastMonth || Convert.ToDateTime(x.CompletionDate).Month == lastMonth).ToList();
                    }
                    if (FilterDate == "NextQuarter")
                    {
                        if (currentMonth == 1 || currentMonth == 2 || currentMonth == 3)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 4 || Convert.ToDateTime(x.EndDate).Month == 4 || Convert.ToDateTime(x.CompletionDate).Month == 4)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 5 || Convert.ToDateTime(x.EndDate).Month == 5 || Convert.ToDateTime(x.CompletionDate).Month == 5)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 6 || Convert.ToDateTime(x.EndDate).Month == 6 || Convert.ToDateTime(x.CompletionDate).Month == 6)).ToList();
                        }
                        if (currentMonth == 4 || currentMonth == 5 || currentMonth == 6)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 7 || Convert.ToDateTime(x.EndDate).Month == 7 || Convert.ToDateTime(x.CompletionDate).Month == 7)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 8 || Convert.ToDateTime(x.EndDate).Month == 8 || Convert.ToDateTime(x.CompletionDate).Month == 8)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 9 || Convert.ToDateTime(x.EndDate).Month == 9 || Convert.ToDateTime(x.CompletionDate).Month == 9)).ToList();
                        }
                        if (currentMonth == 7 || currentMonth == 8 || currentMonth == 9)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 10 || Convert.ToDateTime(x.EndDate).Month == 10 || Convert.ToDateTime(x.CompletionDate).Month == 10)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 11 || Convert.ToDateTime(x.EndDate).Month == 11 || Convert.ToDateTime(x.CompletionDate).Month == 11)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 12 || Convert.ToDateTime(x.EndDate).Month == 12 || Convert.ToDateTime(x.CompletionDate).Month == 12)).ToList();
                        }
                        if (currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
                        {
                            tasks = tasks.Where(x => ((Convert.ToDateTime(x.StartDate).Year == nextYear && Convert.ToDateTime(x.StartDate).Month == 1) || (Convert.ToDateTime(x.EndDate).Year == nextYear && Convert.ToDateTime(x.EndDate).Month == 1) || (Convert.ToDateTime(x.CompletionDate).Year == nextYear && Convert.ToDateTime(x.CompletionDate).Month == 1))
                                                        || ((Convert.ToDateTime(x.StartDate).Year == nextYear && Convert.ToDateTime(x.StartDate).Month == 2) || (Convert.ToDateTime(x.EndDate).Year == nextYear && Convert.ToDateTime(x.EndDate).Month == 2) || (Convert.ToDateTime(x.CompletionDate).Year == nextYear && Convert.ToDateTime(x.CompletionDate).Month == 2))
                                                        || ((Convert.ToDateTime(x.StartDate).Year == nextYear && Convert.ToDateTime(x.StartDate).Month == 3) || (Convert.ToDateTime(x.EndDate).Year == nextYear && Convert.ToDateTime(x.EndDate).Month == 3) || (Convert.ToDateTime(x.CompletionDate).Year == nextYear && Convert.ToDateTime(x.CompletionDate).Month == 3))).ToList();
                        }

                    }
                    if (FilterDate == "ThisQuarter")
                    {
                        if (currentMonth == 1 || currentMonth == 2 || currentMonth == 3)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 1 || Convert.ToDateTime(x.EndDate).Month == 1 || Convert.ToDateTime(x.CompletionDate).Month == 1)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 2 || Convert.ToDateTime(x.EndDate).Month == 2 || Convert.ToDateTime(x.CompletionDate).Month == 2)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 3 || Convert.ToDateTime(x.EndDate).Month == 3 || Convert.ToDateTime(x.CompletionDate).Month == 3)).ToList();
                        }
                        if (currentMonth == 4 || currentMonth == 5 || currentMonth == 6)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 4 || Convert.ToDateTime(x.EndDate).Month == 4 || Convert.ToDateTime(x.CompletionDate).Month == 4)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 5 || Convert.ToDateTime(x.EndDate).Month == 5 || Convert.ToDateTime(x.CompletionDate).Month == 5)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 6 || Convert.ToDateTime(x.EndDate).Month == 6 || Convert.ToDateTime(x.CompletionDate).Month == 6)).ToList();
                        }
                        if (currentMonth == 7 || currentMonth == 8 || currentMonth == 9)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 7 || Convert.ToDateTime(x.EndDate).Month == 7 || Convert.ToDateTime(x.CompletionDate).Month == 7)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 8 || Convert.ToDateTime(x.EndDate).Month == 8 || Convert.ToDateTime(x.CompletionDate).Month == 8)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 9 || Convert.ToDateTime(x.EndDate).Month == 9 || Convert.ToDateTime(x.CompletionDate).Month == 9)).ToList();
                        }
                        if (currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 10 || Convert.ToDateTime(x.EndDate).Month == 10 || Convert.ToDateTime(x.CompletionDate).Month == 10)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 11 || Convert.ToDateTime(x.EndDate).Month == 11 || Convert.ToDateTime(x.CompletionDate).Month == 11)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 12 || Convert.ToDateTime(x.EndDate).Month == 12 || Convert.ToDateTime(x.CompletionDate).Month == 12)).ToList();
                        }
                    }
                    if (FilterDate == "LastQuarter")
                    {
                        if (currentMonth == 1 || currentMonth == 2 || currentMonth == 3)
                        {
                            tasks = tasks.Where(x => ((Convert.ToDateTime(x.StartDate).Year == lastYear && Convert.ToDateTime(x.StartDate).Month == 10) || (Convert.ToDateTime(x.EndDate).Year == lastYear && Convert.ToDateTime(x.EndDate).Month == 10) || (Convert.ToDateTime(x.CompletionDate).Year == lastYear && Convert.ToDateTime(x.CompletionDate).Month == 10))
                                                        || ((Convert.ToDateTime(x.StartDate).Year == lastYear && Convert.ToDateTime(x.StartDate).Month == 11) || (Convert.ToDateTime(x.EndDate).Year == lastYear && Convert.ToDateTime(x.EndDate).Month == 11) || (Convert.ToDateTime(x.CompletionDate).Year == lastYear && Convert.ToDateTime(x.CompletionDate).Month == 11))
                                                        || ((Convert.ToDateTime(x.StartDate).Year == lastYear && Convert.ToDateTime(x.StartDate).Month == 12) || (Convert.ToDateTime(x.EndDate).Year == lastYear && Convert.ToDateTime(x.EndDate).Month == 12) || (Convert.ToDateTime(x.CompletionDate).Year == lastYear && Convert.ToDateTime(x.CompletionDate).Month == 12))).ToList();


                        }
                        if (currentMonth == 4 || currentMonth == 5 || currentMonth == 6)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 1 || Convert.ToDateTime(x.EndDate).Month == 1 || Convert.ToDateTime(x.CompletionDate).Month == 1)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 2 || Convert.ToDateTime(x.EndDate).Month == 2 || Convert.ToDateTime(x.CompletionDate).Month == 2)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 3 || Convert.ToDateTime(x.EndDate).Month == 3 || Convert.ToDateTime(x.CompletionDate).Month == 3)).ToList();
                        }
                        if (currentMonth == 7 || currentMonth == 8 || currentMonth == 9)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 4 || Convert.ToDateTime(x.EndDate).Month == 4 || Convert.ToDateTime(x.CompletionDate).Month == 4)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 5 || Convert.ToDateTime(x.EndDate).Month == 5 || Convert.ToDateTime(x.CompletionDate).Month == 5)
                                                        || (Convert.ToDateTime(x.StartDate).Month == 6 || Convert.ToDateTime(x.EndDate).Month == 6 || Convert.ToDateTime(x.CompletionDate).Month == 6)).ToList();
                        }
                        if (currentMonth == 10 || currentMonth == 11 || currentMonth == 12)
                        {
                            tasks = tasks.Where(x => (Convert.ToDateTime(x.StartDate).Month == 7 || Convert.ToDateTime(x.EndDate).Month == 7 || Convert.ToDateTime(x.CompletionDate).Month == 7)
                                                       || (Convert.ToDateTime(x.StartDate).Month == 8 || Convert.ToDateTime(x.EndDate).Month == 8 || Convert.ToDateTime(x.CompletionDate).Month == 8)
                                                       || (Convert.ToDateTime(x.StartDate).Month == 9 || Convert.ToDateTime(x.EndDate).Month == 9 || Convert.ToDateTime(x.CompletionDate).Month == 9)).ToList();
                        }
                    }
                    if (FilterDate == "NextYear")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                    }
                    if (FilterDate == "ThisYear")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                    }
                    if (FilterDate == "LastYear")
                    {
                        tasks = tasks.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                    }
                }
                if (taskType != 0)
                {
                    tasks = tasks.Where(x => x.Task.TaskTypeID == taskType).ToList();
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
                if (startDate != null && startDate != "")
                {
                    tasks = tasks.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                            (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate)))
                    ).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    tasks = tasks.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                            (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate)))
                    ).ToList();
                }


                if (fr != null && fr.ReportType == 3)
                {
                    if (fr.Name != "" && fr.Name != null)
                    {
                        ViewBag.selectedreportname = fr.Name;
                    }

                    if (fr.Employee != "" && fr.Employee != null)
                    {
                        ViewBag.selectedemployee = new UserBL().getUsersById(Convert.ToInt32(fr.Employee));
                    }

                    if (fr.TaskName != "" && fr.TaskName != null)
                    {
                        ViewBag.selectedtask = new TaskBL().getTasksById(Convert.ToInt32(fr.TaskName));
                    }

                    if (fr.TaskType != "" && fr.TaskType != null)
                    {
                        ViewBag.selectedtasktype = new TaskTypeBL().getTaskTypesById(Convert.ToInt32(fr.TaskType));
                    }

                    if (fr.Status != "" && fr.Status != null)
                    {
                        ViewBag.selectedstatus = fr.Status;
                    }

                    if (fr.Priority != "" && fr.Priority != null)
                    {
                        ViewBag.selectedpriority = fr.Priority;
                    }

                    if (fr.StartingDate != "" && fr.StartingDate != null)
                    {
                        ViewBag.selectedstartdate = Convert.ToDateTime(fr.StartingDate);
                    }

                    if (fr.EndingDate != "" && fr.EndingDate != null)
                    {
                        ViewBag.selectedendDate = Convert.ToDateTime(fr.EndingDate);
                    }

                    if (fr.StartDate != "" && fr.StartDate != null)
                    {
                        ViewBag.selectedstartId = fr.StartDate;
                    }

                    if (fr.EndDate != "" && fr.EndDate != null)
                    {
                        ViewBag.selectedendId = fr.EndDate;
                    }

                    if (fr.CompleteDate != "" && fr.CompleteDate != null)
                    {
                        ViewBag.selectedcompleteId = fr.CompleteDate;
                    }

                    if (fr.FilterDate != "" && fr.FilterDate != null)
                    {
                        ViewBag.filterdate = fr.FilterDate;
                    }
                }
                else
                {
                    ViewBag.filterdate = null;
                    ViewBag.selectedemployee = null;
                    ViewBag.selectedtaskname = null;
                    ViewBag.selectedtasktype = null;
                    ViewBag.selectedstatus = null;
                    ViewBag.selectedpriority = null;
                    ViewBag.selectedstartingdate = null;
                    ViewBag.selectedendDate = null;
                    ViewBag.selectedstartId = null;
                    ViewBag.selectedendId = null;
                    ViewBag.selectedcompleteId = null;
                }


                ViewBag.tasks = tasks;

                ViewBag.tag = new TagBL().getTagsById(tag);
                ViewBag.tags = new TagBL().getTagsList();

                ViewBag.startDate = startDate;
                ViewBag.endDate = endDate;
                ViewBag.status = status;
                User u = new UserBL().getUsersById(logedinuser.Id);
                ViewBag.name = u.FirstName + " " + u.LastName;
                if (empId != -1)
                    ViewBag.emp = new UserBL().getUsersById(empId);
                else
                {
                    ViewBag.emp = null;
                }
                if (logedinuser.Role == 2 || logedinuser.Role == 4)
                {
                    List<User> userlist1 = new UserBL().getUsersList().Where(x => x.Role == 3).ToList();
                    userlist1 = userlist1.Where(x => x.Department.DivisionID == new UserBL().getUsersById(logedinuser.Id).DivisionId).ToList();
                    ViewBag.employees = userlist1;
                }
                else
                    ViewBag.employees = new UserBL().getUsersList().Where(x => x.Role == 3);

                //ViewBag.taskName = taskName;
                ViewBag.taskType = new TaskTypeBL().getTaskTypesById(taskType);
                ViewBag.taskTypes = new TaskTypeBL().getTaskTypesList();
                ViewBag.statuses = new List<int>() { 0, 1, 2, 3, 4 }.Where(x => x != status);
                ViewBag.completionListId = new List<int>() { 1, 2, 3 };
                ViewBag.completionList = new List<string>() { "In Process", "Completed", "Late" };
                //ViewBag.completionId = completionStatus;
                ViewBag.completionIdName = complettionName;
                ViewBag.message = message;

                if (logedinuser.Role == 2 || logedinuser.Role == 4)
                {
                    List<Task> taskslist = new TaskBL().getTasksList().Where(x => x.IsActive == 1 && x.Department.DivisionID == new UserBL().getUsersById(logedinuser.Id).DivisionId).ToList();
                    ViewBag.TasksName = taskslist;
                }
                else
                {
                    List<Task> taskslist = new TaskBL().getTasksList().Where(x => x.IsActive == 1).ToList();
                    ViewBag.TasksName = taskslist;
                }

                ViewBag.frId = frId;

                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        //[HttpPost]
        public List<string> SearchStatusReportByEmployee(string FilterDate = "", string startDate = "", string endDate = "", string taskId = "", int taskType = 0, int status = -1, int priority = -1, int empId = -1, int isEmpReport = 0)
        {
            Log.Info("pdf generation method called");

            List<string> objlist = new List<string>();
            objlist.Add(logedinuser.Id.ToString());
            User manager = new UserBL().getUsersById(logedinuser.Id);

            if (empId != -1)
            {
                List<User_Task> taskss = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == empId && x.IsPrivate == 0).ToList();
                string complettionName = string.Empty;
                User userObj = new UserBL().getUsersById(empId);


                if (empId != -1)
                {
                    taskss = taskss.Where(x => x.UserID == Convert.ToInt32(empId)).ToList();
                }

                if (taskId != "")
                {
                    taskss = taskss.Where(x => x.TaskID == Convert.ToInt32(taskId)).ToList();
                }

                if (FilterDate != "")
                {
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

                    #region Days
                        DateTime currentDate = DateTime.Now;
                        DateTime lastDate = currentDate.AddDays(-1);
                        DateTime nextDate = currentDate.AddDays(1);
                    #endregion


                    #region Weeks
                        DateTime currentWeekStartDate = currentDate.AddDays(-daysTillCurrentDate);
                        DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                        DateTime lastWeekStartDate = currentDate.AddDays(-daysTillCurrentDate - 7);
                        DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

                        DateTime nextWeekStartDate = currentDate.AddDays(-daysTillCurrentDate + 7);
                        DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);
                    #endregion


                    #region Months
                        DateTime currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                        DateTime currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1);

                        DateTime lastMonthStartDate = currentDate.AddMonths(-1);
                        lastMonthStartDate = new DateTime(lastMonthStartDate.Year, lastMonthStartDate.Month, 1);
                        DateTime lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddDays(-1);

                        DateTime nextMonthStartDate = currentDate.AddMonths(1);
                        nextMonthStartDate = new DateTime(nextMonthStartDate.Year, nextMonthStartDate.Month, 1);
                        DateTime nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1);
                    #endregion


                    #region Quarters
                        DateTime currentQuarterStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                        DateTime currentQuarterEndDate = currentQuarterStartDate.AddMonths(4).AddDays(-1);


                        DateTime lastQuarterStartDate = currentQuarterStartDate.AddMonths(-4);
                        DateTime lastQuarterEndDate = currentQuarterStartDate.AddDays(-1);

                        DateTime nextQuarterStartDate = currentQuarterEndDate.AddDays(1);
                        DateTime nextQuarterEndDate = nextQuarterStartDate.AddMonths(4).AddDays(-1);
                    #endregion


                    #region Years
                        int currentYear = currentDate.Year;
                        int lastYear = currentYear - 1;
                        int nextYear = currentYear + 1;
                    #endregion


                    if (FilterDate == "Tomorrow")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextDate.Date && Convert.ToDateTime(x.StartDate).Date < nextDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "Today")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentDate.Date && Convert.ToDateTime(x.StartDate).Date < currentDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "Yesterday")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastDate.Date && Convert.ToDateTime(x.StartDate).Date < lastDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastMonthEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "ThisQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "LastQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastQuarterEndDate.Date)
                            ).ToList();
                    }
                    if (FilterDate == "NextYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                    }
                    if (FilterDate == "ThisYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                    }
                    if (FilterDate == "LastYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                    }

                }

                if (taskType != 0)
                {
                    taskss = taskss.Where(x => x.Task.TaskTypeID == taskType).ToList();
                }
                if (status != -1)
                {
                    taskss = taskss.Where(x => x.Status == status).ToList();
                }
                if (priority != -1)
                {
                    taskss = taskss.Where(x => x.Priority == priority).ToList();
                }
                if (startDate != null && startDate != "")
                {
                    taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                            (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate)))
                    ).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                            (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate)))
                    ).ToList();
                }

                List<DivisionReportDTO> mnglist = new List<DivisionReportDTO>();

                string priority1 = "";
                string status1 = "";
                string AssignedBy = "";
                foreach (User_Task x in taskss)
                {
                    if (x.CompletionDate != null)
                    {
                        status1 = "Completed";
                    }
                    else
                    {
                        if (x.EndDate < DateTime.Now)
                        {
                            status1 = "Late";
                        }
                        else
                        {
                            if (x.Status != null)
                                status1 = General_Purpose.getStatusValue(x.Status.Value);
                            else
                                status1 = "Started";
                        }
                    }

                    if (x.Priority != null)
                    {
                        if (General_Purpose.getPriorityValue(x.Priority.Value) == "Medium")
                        {
                            priority1 = General_Purpose.getPriorityValue(x.Priority.Value);
                        }
                        else if (General_Purpose.getPriorityValue(x.Priority.Value) == "Low")
                        {
                            priority1 = General_Purpose.getPriorityValue(x.Priority.Value);
                        }
                        else
                        {
                            priority1 = General_Purpose.getPriorityValue(x.Priority.Value);
                        }

                    }
                    else
                        priority1 = "";

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

                    DivisionReportDTO obj = new DivisionReportDTO();

                    if (x.User1.Department == null)
                    {
                        obj.DepartmentName = "No Department Assigned";
                    }
                    else
                    {
                        obj.DepartmentName = x.User1.Department.Name;

                    }

                    if (x.Task == null)
                    {
                        obj.TaskType = "No Task Type Assigned";
                        obj.TaskName = "No Task Name Assigned";
                    }
                    else
                    {
                        obj.TaskType = x.Task.TaskType.Name;
                        obj.TaskName = x.Task.Name;
                    }

                    if (x.StartDate == null)
                    {
                        obj.StartDate = "No Start Date Assigned";
                    }
                    else
                    {
                        obj.StartDate = x.StartDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (x.EndDate == null)
                    {
                        obj.DueDate = "No Due Date Assigned";
                    }
                    else
                    {
                        obj.DueDate = x.EndDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (status1 == null)
                    {
                        obj.CompletionStatus = "No Status Assigned";
                    }
                    else
                    {
                        obj.CompletionStatus = status1;
                    }
                    if (x.CompletionDate != null)
                    {
                        obj.CompletionDate = x.CompletionDate.Value.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.CompletionDate = string.Empty;
                    }

                    if (priority1 == null)
                    {
                        obj.Priority = "No Priority Assigned";
                    }
                    else
                    {
                        obj.Priority = priority1;
                    }

                    mnglist.Add(obj);
                }

                Log.Info("Pdf generation method search completed");

                string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Division" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".pdf";
                string title = "Status Report by Employee: " + (userObj != null ? userObj.FirstName + " " + userObj.LastName : "NA");
                new PDF_Export().generate_division_report(mnglist, path, title, isEmpReport);


                List<string> objlist2 = new List<string>();

                objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
                objlist.AddRange(objlist2);

                return objlist;

                //return File(path, MediaTypeNames.Text.Plain, "Employee Report.pdf");
            }
            else
            {
                //return RedirectToAction("StatusReportEmployee", "Report", new { message = "PDF could not be generated because Employee ID was invalid." });
                return null;
            }
            
            //catch
            //{
            //    return RedirectToAction("StatusReportEmployee", "Report", new { message = "PDF could not be generated  because an unexpected error." });


            //}
        }

        public List<string> SearchStatusReportByEmployeeExcel(string FilterDate = "", string startDate = "", string endDate = "", string taskId = "", int taskType = 0, int status = -1, int priority = -1, int empId = -1)
        {
            Log.Info("Excel generation method called");

            List<string> objlist = new List<string>();
            objlist.Add(logedinuser.Id.ToString());
            User manager = new UserBL().getUsersById(logedinuser.Id);

            if (empId != -1)
            {
                List<User_Task> taskss = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == empId && x.IsPrivate == 0).ToList();
                string complettionName = string.Empty;
                User userObj = new UserBL().getUsersById(empId);

                if (empId != -1)
                {
                    taskss = taskss.Where(x => x.UserID == Convert.ToInt32(empId)).ToList();
                }

                if (taskId != "")
                {
                    taskss = taskss.Where(x => x.TaskID == Convert.ToInt32(taskId)).ToList();
                }

                if (FilterDate != "")
                {
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

                    #region Days
                    DateTime currentDate = DateTime.Now;
                    DateTime lastDate = currentDate.AddDays(-1);
                    DateTime nextDate = currentDate.AddDays(1);
                    #endregion


                    #region Weeks
                    DateTime currentWeekStartDate = currentDate.AddDays(-daysTillCurrentDate);
                    DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                    DateTime lastWeekStartDate = currentDate.AddDays(-daysTillCurrentDate - 7);
                    DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

                    DateTime nextWeekStartDate = currentDate.AddDays(-daysTillCurrentDate + 7);
                    DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);
                    #endregion


                    #region Months
                    DateTime currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    DateTime currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1);

                    DateTime lastMonthStartDate = currentDate.AddMonths(-1);
                    lastMonthStartDate = new DateTime(lastMonthStartDate.Year, lastMonthStartDate.Month, 1);
                    DateTime lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddDays(-1);

                    DateTime nextMonthStartDate = currentDate.AddMonths(1);
                    nextMonthStartDate = new DateTime(nextMonthStartDate.Year, nextMonthStartDate.Month, 1);
                    DateTime nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1);
                    #endregion


                    #region Quarters
                    DateTime currentQuarterStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    DateTime currentQuarterEndDate = currentQuarterStartDate.AddMonths(4).AddDays(-1);


                    DateTime lastQuarterStartDate = currentQuarterStartDate.AddMonths(-4);
                    DateTime lastQuarterEndDate = currentQuarterStartDate.AddDays(-1);

                    DateTime nextQuarterStartDate = currentQuarterEndDate.AddDays(1);
                    DateTime nextQuarterEndDate = nextQuarterStartDate.AddMonths(4).AddDays(-1);
                    #endregion


                    #region Years
                    int currentYear = currentDate.Year;
                    int lastYear = currentYear - 1;
                    int nextYear = currentYear + 1;
                    #endregion


                    if (FilterDate == "Tomorrow")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextDate.Date && Convert.ToDateTime(x.StartDate).Date < nextDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "Today")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentDate.Date && Convert.ToDateTime(x.StartDate).Date < currentDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "Yesterday")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastDate.Date && Convert.ToDateTime(x.StartDate).Date < lastDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "ThisWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "NextWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "LastWeek")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "NextMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextMonthEndDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "ThisMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentMonthEndDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "LastMonth")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastMonthStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastMonthEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastMonthEndDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "NextQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == nextQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == nextQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > nextQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextQuarterEndDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "ThisQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == currentQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == currentQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > currentQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentQuarterEndDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "LastQuarter")
                    {
                        taskss = taskss.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date == lastQuarterStartDate.Date)
                        || (Convert.ToDateTime(x.StartDate).Date == lastQuarterEndDate.Date)
                        || (Convert.ToDateTime(x.EndDate).Date > lastQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastQuarterEndDate.Date)
                         ).ToList();
                    }
                    if (FilterDate == "NextYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                    }
                    if (FilterDate == "ThisYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                    }
                    if (FilterDate == "LastYear")
                    {
                        taskss = taskss.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                    }

                }

                if (taskType != 0)
                {
                    taskss = taskss.Where(x => x.Task.TaskTypeID == taskType).ToList();
                }
                if (status != -1)
                {
                    taskss = taskss.Where(x => x.Status == status).ToList();
                }
                if (priority != -1)
                {
                    taskss = taskss.Where(x => x.Priority == priority).ToList();
                }
                if (startDate != null && startDate != "")
                {
                    taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                            (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate)))
                    ).ToList();
                }

                if (endDate != null && endDate != "")
                {
                    taskss = taskss.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                            (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate)))
                    ).ToList();
                }

                List<DivisionReportDTO> mnglist = new List<DivisionReportDTO>();

                string priority1 = "";
                string status1 = "";
                string AssignedBy = "";
                foreach (User_Task x in taskss)
                {
                    if (x.CompletionDate != null)
                    {
                        status1 = "Completed";
                    }
                    else
                    {
                        if (x.EndDate < DateTime.Now)
                        {
                            status1 = "Late";
                        }
                        else
                        {
                            if (x.Status != null)
                                status1 = General_Purpose.getStatusValue(x.Status.Value);
                            else
                                status1 = "Started";
                        }
                    }

                    if (x.Priority != null)
                    {
                        if (General_Purpose.getPriorityValue(x.Priority.Value) == "Medium")
                        {
                            priority1 = General_Purpose.getPriorityValue(x.Priority.Value);
                        }
                        else if (General_Purpose.getPriorityValue(x.Priority.Value) == "Low")
                        {
                            priority1 = General_Purpose.getPriorityValue(x.Priority.Value);
                        }
                        else
                        {
                            priority1 = General_Purpose.getPriorityValue(x.Priority.Value);
                        }

                    }
                    else
                        priority1 = "";

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

                    DivisionReportDTO obj = new DivisionReportDTO();

                    if (x.User1.Department == null)
                    {
                        obj.DepartmentName = "No Department Assigned";
                    }
                    else
                    {
                        obj.DepartmentName = x.User1.Department.Name;

                    }

                    if (x.Task == null)
                    {
                        obj.TaskType = "No Task Type Assigned";
                        obj.TaskName = "No Task Name Assigned";
                    }
                    else
                    {
                        obj.TaskType = x.Task.TaskType.Name;
                        obj.TaskName = x.Task.Name;
                    }

                    if (x.StartDate == null)
                    {
                        obj.StartDate = "No Start Date Assigned";
                    }
                    else
                    {
                        obj.StartDate = x.StartDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (x.EndDate == null)
                    {
                        obj.DueDate = "No Due Date Assigned";
                    }
                    else
                    {
                        obj.DueDate = x.EndDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (status1 == null)
                    {
                        obj.CompletionStatus = "No Status Assigned";
                    }
                    else
                    {
                        obj.CompletionStatus = status1;
                    }
                    if (x.CompletionDate != null)
                    {
                        obj.CompletionDate = x.CompletionDate.Value.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.CompletionDate = string.Empty;
                    }

                    if (priority1 == null)
                    {
                        obj.Priority = "No Priority Assigned";
                    }
                    else
                    {
                        obj.Priority = priority1;
                    }

                    mnglist.Add(obj);
                }

                Log.Info("Excel generation method search completed");

                string title = "Status Report by Employee: " + (userObj != null ? userObj.FirstName + " " + userObj.LastName : "NA");
                string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + "Employee Report" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
                ExcelManagement.generateEmployeeExcel(path, mnglist, title);

                List<string> objlist2 = new List<string>();

                objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
                objlist.AddRange(objlist2);

                return objlist;

                //return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employee" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");


            }
            else
            {
                //return RedirectToAction("StatusReportEmployee", "Report", new { message = "Excel could not be generated because Employee ID was invalid." });
                return null;
            }
            //catch (Exception e)
            //{
            //    return RedirectToAction("StatusReportEmployee", "Report", new { message = "Excel could not be generated because of an unknown issue." });
            //}
        }


        public ActionResult GetStatusReportByEmployee()
        {
            try
            {
                List<User_Task> tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == logedinuser.Id && x.IsPrivate == 0).ToList();
                string complettionName = string.Empty;

                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];



                int totalrows = tasks.Count();
                //filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    tasks = tasks.Where(x => x.User1.FirstName.ToLower().Contains(searchValue.ToLower()) || x.User1.LastName.ToLower().Contains(searchValue.ToLower()) || x.Task.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                int totalrowsafterfilterinig = tasks.Count();
                //sorting

                // pagination
                tasks = tasks.Skip(start).Take(length).ToList();

                List<EmployeeReportDTO> mnglist = new List<EmployeeReportDTO>();

                string priority = "";
                string status = "";
                string AssignedBy = "";
                foreach (User_Task x in tasks)
                {
                    if (x.Status != null)
                    {
                        status = General_Purpose.getStatusValue(x.Status.Value);
                    }
                    else
                    {
                        status = "";
                    }

                    if (x.Priority != null)
                    {
                        if (General_Purpose.getPriorityValue(x.Priority.Value) == "Medium")
                        {
                            priority = "<td><span class='label label-warning'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span></td>";
                        }
                        else if (General_Purpose.getPriorityValue(x.Priority.Value) == "Low")
                        {
                            priority = "<td><span class='label label-success'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span></td>";
                        }
                        else
                        {
                            priority = "<td><span class='label label-danger'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span></td>";
                        }

                    }
                    else
                        priority = "";


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


                    EmployeeReportDTO obj = new EmployeeReportDTO();

                    if (x.User1.Department == null)
                    {
                        obj.DepartmentName = "No Department Assigned";
                    }
                    else
                    {
                        obj.DepartmentName = x.User1.Department.Name;

                    }

                    if (x.Task == null)
                    {
                        obj.TaskType = "No Task Type Assigned";
                        obj.TaskName = "No Task Name Assigned";
                    }
                    else
                    {
                        obj.TaskType = x.Task.TaskType.Name;
                        obj.TaskName = x.Task.Name;
                    }

                    if (x.StartDate == null)
                    {
                        obj.StartDate = "No Start Date Assigned";
                    }
                    else
                    {
                        obj.StartDate = x.StartDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (x.EndDate == null)
                    {
                        obj.DueDate = "No Due Date Assigned";
                    }
                    else
                    {
                        obj.DueDate = x.EndDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (status == null)
                    {
                        obj.CompletionStatus = "No Status Assigned";
                    }
                    else
                    {
                        obj.CompletionStatus = status;
                    }

                    if (priority == null)
                    {
                        obj.Priority = "No Priority Assigned";
                    }
                    else
                    {
                        obj.Priority = priority;
                    }




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

        public ActionResult UserTaskReport(string frId = "", string message = "")
        {
            try
            {
                FavoriteReport fr = null;

                if (frId != "")
                {
                    fr = new FavoriteReportBL().getFavoriteReportById(Convert.ToInt32(StringCipher.Base64Decode(frId)));
                }

                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                if (logedinuser.Role == 2 || logedinuser.Role == 4)
                {
                    List<Division> divlist = new DivisionBL().getDivisionsList().Where(x => x.IsActive == 1 && x.Id == new UserBL().getUsersById(logedinuser.Id).DivisionId).ToList();
                    ViewBag.DivList = divlist;
                }
                else
                {
                    List<Division> divlist = new DivisionBL().getDivisionsList().Where(x => x.IsActive == 1).ToList();
                    ViewBag.DivList = divlist;
                }

                if (fr != null && fr.ReportType == 4)
                {

                    if (fr.Name != "" && fr.Name != null)
                    {
                        ViewBag.selectedreportname = fr.Name;
                    }

                    if (fr.UserType != "" && fr.UserType != null)
                    {
                        ViewBag.selectedusertype = fr.UserType;
                    }

                    if (fr.Division != "" && fr.Division != null)
                    {
                        ViewBag.selecteddivision = new DivisionBL().getDivisionsById(Convert.ToInt32(fr.Division));
                    }

                    if (fr.Department != "" && fr.Department != null)
                    {
                        ViewBag.selecteddepartment = new DepartmentBL().getDepartmentsById(Convert.ToInt32(fr.Department));
                    }

                    //if (fr.User_Id != null)
                    //{
                    //    ViewBag.selecteduser = new UserBL().getUsersById(Convert.ToInt32(fr.User_Id));
                    //}
                    if (fr.Employee != null)
                    {
                        ViewBag.selecteduser = new UserBL().getUsersById(Convert.ToInt32(fr.Employee));
                    }

                    if (fr.StartingDate != "" && fr.StartingDate != null)
                    {
                        ViewBag.selectedstartdate = Convert.ToDateTime(fr.StartingDate);
                    }

                    if (fr.EndingDate != "" && fr.EndingDate != null)
                    {
                        ViewBag.selectedendDate = Convert.ToDateTime(fr.EndingDate);
                    }

                    if (fr.StartDate != "" && fr.StartDate != null)
                    {
                        ViewBag.selectedstartId = fr.StartDate;
                    }

                    if (fr.EndDate != "" && fr.EndDate != null)
                    {
                        ViewBag.selectedendId = fr.EndDate;
                    }

                    if (fr.CompleteDate != "" && fr.CompleteDate != null)
                    {
                        ViewBag.selectedcompleteId = fr.CompleteDate;
                    }
                    if (fr.FilterDate != "" && fr.FilterDate != null)
                    {
                        ViewBag.filterdate = fr.FilterDate;
                    }
                }
                else
                {
                    ViewBag.filterdate = null;
                    ViewBag.selectedusertype = null;
                    ViewBag.selectedstartingdate = null;
                    ViewBag.selectedendDate = null;
                    ViewBag.selectedstartId = null;
                    ViewBag.selectedendId = null;
                    ViewBag.selectedcompleteId = null;
                }

                ViewBag.frId = frId;
                ViewBag.message = message;
                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }

        }


        public string getusers(int DepartmentID = -1, int DivisionID = -1)
        {
            if (DepartmentID == -1 && DivisionID != -1)
            {
                List<User> allmanagers = new List<User>();
                allmanagers = new UserBL().getAllUsersList().Where(x => x.DivisionId == Convert.ToInt32(DivisionID) && (x.Role == 2 || x.Role == 4) && x.IsActive == 1).ToList();
                List<UserDTO> alluserDTOs = new List<UserDTO>();

                foreach (User d in allmanagers.Where(x => x.IsActive == 1))
                {

                    UserDTO dTo = new UserDTO()
                    {
                        Id = d.Id,
                        Name = d.FirstName + ' ' + d.LastName,
                        SessionRole = logedinuser.Role,
                        AssignUserRole = "Manager"
                    };
                    alluserDTOs.Add(dTo);


                }

                return JsonConvert.SerializeObject(alluserDTOs, Formatting.Indented,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
               });
            }
            else
            {
                List<User> allemployees = new List<User>();
                allemployees = new UserBL().getAllUsersList().Where(x => x.DepartmentId == Convert.ToInt32(DepartmentID) && x.Role == 3 && x.IsActive == 1).ToList();
                List<UserDTO> alluserDTOs = new List<UserDTO>();

                foreach (User d in allemployees.Where(x => x.IsActive == 1))
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


        public string getUsersByType(int UserType=-1)
        {    
            List<User> allusers = new List<User>();
            if(UserType!=-1)
            allusers = new UserBL().getUsersList().Where(x => x.IsActive == 1).ToList();
            if (UserType == 1)
            {
                if(logedinuser.Role==1)
                allusers = allusers.Where(x => x.Role == 1 && x.Id!=logedinuser.Id).ToList();

            }else if (UserType == 2)
            {
                if (logedinuser.Role == 1)
                    allusers = allusers.Where(x => x.Role == 2).ToList();
                if (logedinuser.Role == 2)
                {
                    User lUser = new UserBL().getUsersById(logedinuser.Id);  
                    allusers = allusers.Where(x => x.Role == 2).ToList();
                    allusers = allusers.Where(x => x.DivisionId == lUser.DivisionId).ToList();
                }

            }
            else if (UserType == 3)
            {
                if (logedinuser.Role == 1)
                    allusers = allusers.Where(x => x.Role == 3).ToList();
                if (logedinuser.Role == 2)
                {
                    User lUser = new UserBL().getUsersById(logedinuser.Id);
                    allusers = allusers.Where(x => x.Role == 3).ToList();
                    allusers = allusers.Where(x => x.Department.DivisionID == lUser.DivisionId).ToList();
                }
                if (logedinuser.Role == 3)
                {
                    User lUser = new UserBL().getUsersById(logedinuser.Id);
                    allusers = allusers.Where(x => x.Role == 3).ToList();
                    allusers = allusers.Where(x => x.DepartmentId == lUser.DepartmentId).ToList();
                }
            }

            List<UserDTO> alluserDTOs = new List<UserDTO>();

                foreach (User d in allusers.Where(x => x.IsActive == 1))
                {

                    UserDTO dTo = new UserDTO()
                    {
                        Id = d.Id,
                        Name = d.FirstName + ' ' + d.LastName,
                        //SessionRole = logedinuser.Role,
                        //AssignUserRole = "Manager"
                    };
                    alluserDTOs.Add(dTo);


                }

                return JsonConvert.SerializeObject(alluserDTOs, Formatting.Indented,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
               });
            
           
        } 
        [HttpPost]
        public string FavoriteReportInfo(int FID)
        {

            FavoriteReport obj = new FavoriteReportBL().getFavoriteReportById(FID);
            string IsShared="", IsStatic = "", ReportType = "", SharedBy = "";
            DateTime CreatedAt = new DateTime();
            if (obj.Created_At.HasValue)
            {
                CreatedAt =(DateTime)obj.Created_At;
            }
            if (IsShared != null)
            {
                IsShared = obj.IsShared.ToString();
            }
            if (IsStatic != null)
            {
                IsStatic = obj.IsStatic.ToString();
            }
            if (ReportType != null)
            {
                ReportType = obj.ReportType.ToString();
            } 
            if (SharedBy != null)
            {
                SharedBy = obj.SharedBy.ToString();
            }

            FavoriteReportDTO dt = new FavoriteReportDTO()
            {
                IsActive=(int)obj.IsActive,
                Created_At= CreatedAt,
                CompleteDate= obj.CompleteDate,
                Department= obj.Department,
                Division= obj.Division,
                Employee= obj.Employee,
                EndDate= obj.EndDate,
                EndingDate= obj.EndingDate,
                FilterDate= obj.FilterDate,
                Id= obj.Id,
                IsShared= IsShared,
                IsStatic= IsStatic,
                Name= obj.Name,
                Priority= obj.Priority,
                ReportType= ReportType,
                SharedBy= SharedBy,
                SharedDescription= obj.SharedDescription,
                StartDate= obj.StartDate,
                StartingDate= obj.StartingDate,
                Status= obj.Status,
                TaskName= obj.TaskName,
                TaskType= obj.TaskType,
                UserType= obj.UserType,
                User_Id= (int)obj.User_Id,
                WorkerType= obj.WorkerType

            };
            

                return JsonConvert.SerializeObject(dt, Formatting.Indented,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
               });
            
           
        }


        public List<string> UserTaskExcelReport(string FilterDate = "", string UserTypeID = "", int DivisionID = -1, int DepartmentID = -1, int UserName = -1, string startDate = "", string endDate = "")
        {

            Log.Info("Excel generation method called");

            List<string> objlist = new List<string>();
            objlist.Add(logedinuser.Id.ToString());
            User manager = new UserBL().getUsersById(logedinuser.Id);

            string name = "";
            List<User_Task> usertasks = new List<User_Task>();
            if (UserName == -1)
            {
                //return RedirectToAction("UserTaskReport", new { message = "Excel file could not be generated.User name must be selected." });
                return null;
            }
            else
            {

                User u = new UserBL().getUsersById(UserName);
                if (u != null)
                {
                    name = u.FirstName + " " + u.LastName;
                }

            }


            if (UserTypeID == "Manager")
            {
                if (DivisionID != -1)
                {
                    usertasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == UserName && x.IsActive == 1).ToList();

                    if (startDate != null && startDate != "")
                    {
                        usertasks = usertasks.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                                (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate)))
                        //(x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate)))
                        ).ToList();
                    }

                    if (endDate != null && endDate != "")
                    {
                        usertasks = usertasks.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                                (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate)))
                        //(x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate)))
                        ).ToList();
                    }

                    if (FilterDate != "")
                    {
                        DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                        int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

                        #region Days
                            DateTime currentDate = DateTime.Now;
                            DateTime lastDate = currentDate.AddDays(-1);
                            DateTime nextDate = currentDate.AddDays(1);
                        #endregion


                        #region Weeks
                            DateTime currentWeekStartDate = currentDate.AddDays(-daysTillCurrentDate);
                            DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                            DateTime lastWeekStartDate = currentDate.AddDays(-daysTillCurrentDate - 7);
                            DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

                            DateTime nextWeekStartDate = currentDate.AddDays(-daysTillCurrentDate + 7);
                            DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);
                        #endregion


                        #region Months
                            DateTime currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                            DateTime currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1);

                            DateTime lastMonthStartDate = currentDate.AddMonths(-1);
                            lastMonthStartDate = new DateTime(lastMonthStartDate.Year, lastMonthStartDate.Month, 1);
                            DateTime lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddDays(-1);

                            DateTime nextMonthStartDate = currentDate.AddMonths(1);
                            nextMonthStartDate = new DateTime(nextMonthStartDate.Year, nextMonthStartDate.Month, 1);
                            DateTime nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1);
                        #endregion


                        #region Quarters
                            DateTime currentQuarterStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                            DateTime currentQuarterEndDate = currentQuarterStartDate.AddMonths(4).AddDays(-1);


                            DateTime lastQuarterStartDate = currentQuarterStartDate.AddMonths(-4);
                            DateTime lastQuarterEndDate = currentQuarterStartDate.AddDays(-1);

                            DateTime nextQuarterStartDate = currentQuarterEndDate.AddDays(1);
                            DateTime nextQuarterEndDate = nextQuarterStartDate.AddMonths(4).AddDays(-1);
                        #endregion


                        #region Years
                            int currentYear = currentDate.Year;
                            int lastYear = currentYear - 1;
                            int nextYear = currentYear + 1;
                        #endregion


                        if (FilterDate == "Tomorrow")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextDate.Date && Convert.ToDateTime(x.StartDate).Date < nextDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "Today")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentDate.Date && Convert.ToDateTime(x.StartDate).Date < currentDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "Yesterday")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastDate.Date && Convert.ToDateTime(x.StartDate).Date < lastDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "ThisWeek")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextWeek")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "LastWeek")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextMonth")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextMonthStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextMonthEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "ThisMonth")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentMonthStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentMonthEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "LastMonth")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastMonthStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastMonthEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextQuarter")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextQuarterStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextQuarterEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "ThisQuarter")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentQuarterStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentQuarterEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "LastQuarter")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastQuarterStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastQuarterEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextYear")
                        {
                            usertasks = usertasks.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                        }
                        if (FilterDate == "ThisYear")
                        {
                            usertasks = usertasks.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                        }
                        if (FilterDate == "LastYear")
                        {
                            usertasks = usertasks.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                        }

                    }

                    //if (Request.Form["start"] != null)
                    //{
                    //    if (startDate != null && startDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.StartDate != null && (x.StartDate >= DateTime.Parse(startDate))).ToList();
                    //    }

                    //    if (endDate != null && endDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.StartDate != null && (x.StartDate <= DateTime.Parse(endDate))).ToList();
                    //    }

                    //    ViewBag.sd = 1;
                    //}
                    //if (Request.Form["end"] != null)
                    //{
                    //    if (startDate != "" && startDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.EndDate != null && (x.EndDate >= DateTime.Parse(startDate))).ToList();
                    //    }

                    //    if (endDate != null && endDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.EndDate != null && (x.EndDate <= DateTime.Parse(endDate))).ToList();
                    //    }
                    //    ViewBag.ed = 1;
                    //}
                    //if (Request.Form["complete"] != null)
                    //{
                    //    if (startDate != null && startDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.CompletionDate != null && (x.CompletionDate >= DateTime.Parse(startDate))).ToList();
                    //    }

                    //    if (endDate != null && endDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.CompletionDate != null && (x.CompletionDate <= DateTime.Parse(endDate))).ToList();
                    //    }
                    //    ViewBag.cd = 1;
                    //}


                    Log.Info("Excel generation method search completed");


                    string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + name + " Task (Manager)" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
                    ExcelManagement.generateGenericExcelFile(path, null, null, null, null, null, usertasks, null);

                    List<string> objlist2 = new List<string>();

                    objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
                    objlist.AddRange(objlist2);

                    return objlist;

                    //return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "" + name + " Task (Manager)" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");
                }
                else
                {
                    //return RedirectToAction("UserTaskReport", new { message = "Excel file could not be generated.Division must be selected." });
                    return null;
                }
            }
            else if (UserTypeID == "Employee")
            {
                if (DivisionID != -1 && DepartmentID != -1)
                {
                    usertasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == UserName && x.IsActive == 1).ToList();

                    if (startDate != null && startDate != "")
                    {
                        usertasks = usertasks.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                                (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate))) ||
                                                (x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate)))
                        ).ToList();
                    }

                    if (endDate != null && endDate != "")
                    {
                        usertasks = usertasks.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                                (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate))) ||
                                                (x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate)))
                        ).ToList();
                    }


                    if (FilterDate != "")
                    {
                        DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                        int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

                        #region Days
                            DateTime currentDate = DateTime.Now;
                            DateTime lastDate = currentDate.AddDays(-1);
                            DateTime nextDate = currentDate.AddDays(1);
                        #endregion


                        #region Weeks
                            DateTime currentWeekStartDate = currentDate.AddDays(-daysTillCurrentDate);
                            DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                            DateTime lastWeekStartDate = currentDate.AddDays(-daysTillCurrentDate - 7);
                            DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

                            DateTime nextWeekStartDate = currentDate.AddDays(-daysTillCurrentDate + 7);
                            DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);
                        #endregion


                        #region Months
                            DateTime currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                            DateTime currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1);

                            DateTime lastMonthStartDate = currentDate.AddMonths(-1);
                            lastMonthStartDate = new DateTime(lastMonthStartDate.Year, lastMonthStartDate.Month, 1);
                            DateTime lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddDays(-1);

                            DateTime nextMonthStartDate = currentDate.AddMonths(1);
                            nextMonthStartDate = new DateTime(nextMonthStartDate.Year, nextMonthStartDate.Month, 1);
                            DateTime nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1);
                        #endregion


                        #region Quarters
                            DateTime currentQuarterStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                            DateTime currentQuarterEndDate = currentQuarterStartDate.AddMonths(4).AddDays(-1);


                            DateTime lastQuarterStartDate = currentQuarterStartDate.AddMonths(-4);
                            DateTime lastQuarterEndDate = currentQuarterStartDate.AddDays(-1);

                            DateTime nextQuarterStartDate = currentQuarterEndDate.AddDays(1);
                            DateTime nextQuarterEndDate = nextQuarterStartDate.AddMonths(4).AddDays(-1);
                        #endregion


                        #region Years
                            int currentYear = currentDate.Year;
                            int lastYear = currentYear - 1;
                            int nextYear = currentYear + 1;
                        #endregion


                        if (FilterDate == "Tomorrow")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextDate.Date && Convert.ToDateTime(x.StartDate).Date < nextDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "Today")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentDate.Date && Convert.ToDateTime(x.StartDate).Date < currentDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "Yesterday")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastDate.Date && Convert.ToDateTime(x.StartDate).Date < lastDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "ThisWeek")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextWeek")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "LastWeek")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextMonth")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextMonthStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextMonthEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "ThisMonth")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentMonthStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentMonthEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "LastMonth")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastMonthStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastMonthEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextQuarter")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextQuarterStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextQuarterEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "ThisQuarter")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentQuarterStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentQuarterEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "LastQuarter")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastQuarterStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastQuarterEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextYear")
                        {
                            usertasks = usertasks.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                        }
                        if (FilterDate == "ThisYear")
                        {
                            usertasks = usertasks.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                        }
                        if (FilterDate == "LastYear")
                        {
                            usertasks = usertasks.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                        }

                    }

                    //if (Request.Form["start"] != null)
                    //{
                    //    if (startDate != null && startDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.StartDate != null && (x.StartDate >= DateTime.Parse(startDate))).ToList();
                    //    }

                    //    if (endDate != null && endDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.StartDate != null && (x.StartDate <= DateTime.Parse(endDate))).ToList();
                    //    }

                    //    ViewBag.sd = 1;
                    //}
                    //if (Request.Form["end"] != null)
                    //{
                    //    if (startDate != "" && startDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.EndDate != null && (x.EndDate >= DateTime.Parse(startDate))).ToList();
                    //    }

                    //    if (endDate != null && endDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.EndDate != null && (x.EndDate <= DateTime.Parse(endDate))).ToList();
                    //    }
                    //    ViewBag.ed = 1;
                    //}
                    //if (Request.Form["complete"] != null)
                    //{
                    //    if (startDate != null && startDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.CompletionDate != null && (x.CompletionDate >= DateTime.Parse(startDate))).ToList();
                    //    }

                    //    if (endDate != null && endDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.CompletionDate != null && (x.CompletionDate <= DateTime.Parse(endDate))).ToList();
                    //    }
                    //    ViewBag.cd = 1;
                    //}

                    Log.Info("Excel generation method search completed");

                    string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + name + " Task (Employee)" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
                    ExcelManagement.generateGenericExcelFile(path, null, null, null, null, usertasks);

                    List<string> objlist2 = new List<string>();

                    objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
                    objlist.AddRange(objlist2);

                    return objlist;

                    //return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "" + name + " Task (Employee)" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");
                }
                else
                {
                    //return RedirectToAction("UserTaskReport", new { message = "Excel file could not be generated. Division & Department must be selected." });
                    return null;
                }
            }
            else
            {
                //return RedirectToAction("UserTaskReport", new { message = "Excel file could not be generated.User type must be selected." });
                return null;
            }
        }

        public List<string> UserTaskpdfReport(string FilterDate = "", string UserTypeID = "", int DivisionID = -1, int DepartmentID = -1, int UserName = -1, string startDate = "", string endDate = "")
        {

            Log.Info("pdf generation method called");

            List<string> objlist = new List<string>();
            objlist.Add(logedinuser.Id.ToString());
            User manager = new UserBL().getUsersById(logedinuser.Id);

            string name = "";
            List<User_Task> usertasks = new List<User_Task>();
            if (UserName == -1)
            {
                //return RedirectToAction("UserTaskReport", new { message = "PDF file could not be generated.User name must be selected." });
                return null;
            }
            else
            {

                User u = new UserBL().getUsersById(UserName);
                if (u != null)
                {
                    name = u.FirstName + " " + u.LastName;
                }

            }

            if (UserTypeID == "Manager")
            {
                if (DivisionID != -1)
                {
                    usertasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == UserName && x.IsActive == 1).ToList();

                    if (startDate != null && startDate != "")
                    {
                        usertasks = usertasks.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                                (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate)))
                        //(x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate)))
                        ).ToList();
                    }

                    if (endDate != null && endDate != "")
                    {
                        usertasks = usertasks.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                                (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate)))
                        //(x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate)))
                        ).ToList();
                    }

                    if (FilterDate != "")
                    {
                        DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                        int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

                        #region Days
                            DateTime currentDate = DateTime.Now;
                            DateTime lastDate = currentDate.AddDays(-1);
                            DateTime nextDate = currentDate.AddDays(1);
                        #endregion


                        #region Weeks
                            DateTime currentWeekStartDate = currentDate.AddDays(-daysTillCurrentDate);
                            DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                            DateTime lastWeekStartDate = currentDate.AddDays(-daysTillCurrentDate - 7);
                            DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

                            DateTime nextWeekStartDate = currentDate.AddDays(-daysTillCurrentDate + 7);
                            DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);
                        #endregion


                        #region Months
                            DateTime currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                            DateTime currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1);

                            DateTime lastMonthStartDate = currentDate.AddMonths(-1);
                            lastMonthStartDate = new DateTime(lastMonthStartDate.Year, lastMonthStartDate.Month, 1);
                            DateTime lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddDays(-1);

                            DateTime nextMonthStartDate = currentDate.AddMonths(1);
                            nextMonthStartDate = new DateTime(nextMonthStartDate.Year, nextMonthStartDate.Month, 1);
                            DateTime nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1);
                        #endregion


                        #region Quarters
                            DateTime currentQuarterStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                            DateTime currentQuarterEndDate = currentQuarterStartDate.AddMonths(4).AddDays(-1);


                            DateTime lastQuarterStartDate = currentQuarterStartDate.AddMonths(-4);
                            DateTime lastQuarterEndDate = currentQuarterStartDate.AddDays(-1);

                            DateTime nextQuarterStartDate = currentQuarterEndDate.AddDays(1);
                            DateTime nextQuarterEndDate = nextQuarterStartDate.AddMonths(4).AddDays(-1);
                        #endregion


                        #region Years
                            int currentYear = currentDate.Year;
                            int lastYear = currentYear - 1;
                            int nextYear = currentYear + 1;
                        #endregion


                        if (FilterDate == "Tomorrow")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextDate.Date && Convert.ToDateTime(x.StartDate).Date < nextDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "Today")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentDate.Date && Convert.ToDateTime(x.StartDate).Date < currentDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "Yesterday")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastDate.Date && Convert.ToDateTime(x.StartDate).Date < lastDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "ThisWeek")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextWeek")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "LastWeek")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextMonth")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextMonthStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextMonthEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "ThisMonth")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentMonthStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentMonthEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "LastMonth")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastMonthStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastMonthEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextQuarter")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextQuarterStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextQuarterEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "ThisQuarter")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentQuarterStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentQuarterEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "LastQuarter")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastQuarterStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastQuarterEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextYear")
                        {
                            usertasks = usertasks.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                        }
                        if (FilterDate == "ThisYear")
                        {
                            usertasks = usertasks.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                        }
                        if (FilterDate == "LastYear")
                        {
                            usertasks = usertasks.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                        }

                    }

                    //if (Request.Form["start"] != null)
                    //{
                    //    if (startDate != null && startDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.StartDate != null && (x.StartDate >= DateTime.Parse(startDate))).ToList();
                    //    }

                    //    if (endDate != null && endDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.StartDate != null && (x.StartDate <= DateTime.Parse(endDate))).ToList();
                    //    }

                    //    ViewBag.sd = 1;
                    //}
                    //if (Request.Form["end"] != null)
                    //{
                    //    if (startDate != "" && startDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.EndDate != null && (x.EndDate >= DateTime.Parse(startDate))).ToList();
                    //    }

                    //    if (endDate != null && endDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.EndDate != null && (x.EndDate <= DateTime.Parse(endDate))).ToList();
                    //    }
                    //    ViewBag.ed = 1;
                    //}
                    //if (Request.Form["complete"] != null)
                    //{
                    //    if (startDate != null && startDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.CompletionDate != null && (x.CompletionDate >= DateTime.Parse(startDate))).ToList();
                    //    }

                    //    if (endDate != null && endDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.CompletionDate != null && (x.CompletionDate <= DateTime.Parse(endDate))).ToList();
                    //    }
                    //    ViewBag.cd = 1;
                    //}


                    Log.Info("pdf generation method search completed");

                    string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + name + " Manager" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".pdf";
                    new PDF_Export().generate_manager_assignments(usertasks, path);

                    List<string> objlist2 = new List<string>();

                    objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
                    objlist.AddRange(objlist2);

                    return objlist;

                    //return File(path, MediaTypeNames.Text.Plain, name + " Manager.pdf");
                }
                else
                {
                    //return RedirectToAction("UserTaskReport", new { message = "PDF file could not be generated.Division must be selected." });
                    return null;
                }
            }
            else if (UserTypeID == "Employee")
            {
                if (DivisionID != -1 && DepartmentID != -1)
                {
                    usertasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == UserName && x.IsActive == 1).ToList();

                    if (startDate != null && startDate != "")
                    {
                        usertasks = usertasks.Where(x => (x.StartDate != null && (x.StartDate > DateTime.Parse(startDate))) ||
                                                (x.EndDate != null && (x.EndDate > DateTime.Parse(startDate))) ||
                                                (x.CompletionDate != null && (x.CompletionDate > DateTime.Parse(startDate)))
                        ).ToList();
                    }

                    if (endDate != null && endDate != "")
                    {
                        usertasks = usertasks.Where(x => (x.StartDate != null && (x.StartDate < DateTime.Parse(endDate))) ||
                                                (x.EndDate != null && (x.EndDate < DateTime.Parse(endDate))) ||
                                                (x.CompletionDate != null && (x.CompletionDate < DateTime.Parse(endDate)))
                        ).ToList();
                    }

                    if (FilterDate != "")
                    {
                        DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                        int daysTillCurrentDate = currentDay - DayOfWeek.Monday;

                        #region Days
                            DateTime currentDate = DateTime.Now;
                            DateTime lastDate = currentDate.AddDays(-1);
                            DateTime nextDate = currentDate.AddDays(1);
                        #endregion


                        #region Weeks
                            DateTime currentWeekStartDate = currentDate.AddDays(-daysTillCurrentDate);
                            DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                            DateTime lastWeekStartDate = currentDate.AddDays(-daysTillCurrentDate - 7);
                            DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

                            DateTime nextWeekStartDate = currentDate.AddDays(-daysTillCurrentDate + 7);
                            DateTime nextWeekEndDate = nextWeekStartDate.AddDays(6);
                        #endregion


                        #region Months
                            DateTime currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                            DateTime currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1);

                            DateTime lastMonthStartDate = currentDate.AddMonths(-1);
                            lastMonthStartDate = new DateTime(lastMonthStartDate.Year, lastMonthStartDate.Month, 1);
                            DateTime lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddDays(-1);

                            DateTime nextMonthStartDate = currentDate.AddMonths(1);
                            nextMonthStartDate = new DateTime(nextMonthStartDate.Year, nextMonthStartDate.Month, 1);
                            DateTime nextMonthEndDate = nextMonthStartDate.AddMonths(1).AddDays(-1);
                        #endregion


                        #region Quarters
                            DateTime currentQuarterStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                            DateTime currentQuarterEndDate = currentQuarterStartDate.AddMonths(4).AddDays(-1);


                            DateTime lastQuarterStartDate = currentQuarterStartDate.AddMonths(-4);
                            DateTime lastQuarterEndDate = currentQuarterStartDate.AddDays(-1);

                            DateTime nextQuarterStartDate = currentQuarterEndDate.AddDays(1);
                            DateTime nextQuarterEndDate = nextQuarterStartDate.AddMonths(4).AddDays(-1);
                        #endregion


                        #region Years
                            int currentYear = currentDate.Year;
                            int lastYear = currentYear - 1;
                            int nextYear = currentYear + 1;
                        #endregion


                        if (FilterDate == "Tomorrow")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextDate.Date && Convert.ToDateTime(x.StartDate).Date < nextDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "Today")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentDate.Date && Convert.ToDateTime(x.StartDate).Date < currentDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "Yesterday")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastDate.Date && Convert.ToDateTime(x.StartDate).Date < lastDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "ThisWeek")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentWeekEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextWeek")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextWeekEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "LastWeek")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastWeekStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastWeekStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastWeekEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastWeekStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastWeekEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextMonth")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextMonthStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextMonthEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "ThisMonth")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentMonthStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentMonthEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "LastMonth")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastMonthStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastMonthStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastMonthEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastMonthStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastMonthEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextQuarter")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= nextQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= nextQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == nextQuarterStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == nextQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > nextQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < nextQuarterEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "ThisQuarter")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= currentQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= currentQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == currentQuarterStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == currentQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > currentQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < currentQuarterEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "LastQuarter")
                        {
                            usertasks = usertasks.Where(x => (Convert.ToDateTime(x.StartDate).Date >= lastQuarterStartDate.Date && Convert.ToDateTime(x.EndDate).Date <= lastQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date == lastQuarterStartDate.Date)
                            || (Convert.ToDateTime(x.StartDate).Date == lastQuarterEndDate.Date)
                            || (Convert.ToDateTime(x.EndDate).Date > lastQuarterStartDate.Date && Convert.ToDateTime(x.StartDate).Date < lastQuarterEndDate.Date)
                             ).ToList();
                        }
                        if (FilterDate == "NextYear")
                        {
                            usertasks = usertasks.Where(x => Convert.ToDateTime(x.StartDate).Year == nextYear || Convert.ToDateTime(x.EndDate).Year == nextYear || Convert.ToDateTime(x.CompletionDate).Year == nextYear).ToList();
                        }
                        if (FilterDate == "ThisYear")
                        {
                            usertasks = usertasks.Where(x => Convert.ToDateTime(x.StartDate).Year == currentYear || Convert.ToDateTime(x.EndDate).Year == currentYear || Convert.ToDateTime(x.CompletionDate).Year == currentYear).ToList();
                        }
                        if (FilterDate == "LastYear")
                        {
                            usertasks = usertasks.Where(x => Convert.ToDateTime(x.StartDate).Year == lastYear || Convert.ToDateTime(x.EndDate).Year == lastYear || Convert.ToDateTime(x.CompletionDate).Year == lastYear).ToList();
                        }

                    }

                    //if (Request.Form["start"] != null)
                    //{
                    //    if (startDate != null && startDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.StartDate != null && (x.StartDate >= DateTime.Parse(startDate))).ToList();
                    //    }

                    //    if (endDate != null && endDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.StartDate != null && (x.StartDate <= DateTime.Parse(endDate))).ToList();
                    //    }

                    //    ViewBag.sd = 1;
                    //}
                    //if (Request.Form["end"] != null)
                    //{
                    //    if (startDate != "" && startDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.EndDate != null && (x.EndDate >= DateTime.Parse(startDate))).ToList();
                    //    }

                    //    if (endDate != null && endDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.EndDate != null && (x.EndDate <= DateTime.Parse(endDate))).ToList();
                    //    }
                    //    ViewBag.ed = 1;
                    //}
                    //if (Request.Form["complete"] != null)
                    //{
                    //    if (startDate != null && startDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.CompletionDate != null && (x.CompletionDate >= DateTime.Parse(startDate))).ToList();
                    //    }

                    //    if (endDate != null && endDate != "")
                    //    {
                    //        usertasks = usertasks.Where(x => x.CompletionDate != null && (x.CompletionDate <= DateTime.Parse(endDate))).ToList();
                    //    }
                    //    ViewBag.cd = 1;
                    //}

                    Log.Info("pdf generation method search completed");

                    string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + name + " Employee" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".pdf";
                    new PDF_Export().generate_manager_assignments(usertasks, path, name, 3);

                    List<string> objlist2 = new List<string>();

                    objlist2 = General_Purpose.GetBlobFilePath(path, manager.Id);
                    objlist.AddRange(objlist2);

                    return objlist;

                    //return File(path, MediaTypeNames.Text.Plain, "Employee Report.pdf");
                }
                else
                {
                    //return RedirectToAction("UserTaskReport", new { message = "PDF file could not be generated. Division & Department must be selected." });
                    return null;
                }
            }
            else
            {
                //return RedirectToAction("UserTaskReport", new { message = "PDF file could not be generated.User type must be selected." });
                return null;
            }
        }

        public ActionResult LateReport(string message = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                ViewBag.divisions = new DivisionBL().getDivisionsList();
                ViewBag.taskTypes = new TaskTypeBL().getTaskTypesList();

                ViewBag.message = message;

                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        public ActionResult GetDisplayTask(string division, string department, string taskType, string days, string taskId)
        {
            try
            {
                List<User_Task> usertasks = new User_TaskBL().getAllUser_TasksList().Where(x => x.IsActive == 1 && x.CompletionDate == null).ToList();

                List<LateReportDTO> searchedUserTasks = new List<LateReportDTO>();


                if (division != "-1" && division != "" && division != null)
                {
                    usertasks = usertasks.Where(x => x.Task.Department.DivisionID == Convert.ToInt32(division)).ToList();
                }
                if (department != "" && department != null)
                {
                    usertasks = usertasks.Where(x => x.Task.DepartmentID == Convert.ToInt32(department)).ToList();
                }
                if (taskType != "" && taskType != null)
                {
                    usertasks = usertasks.Where(x => x.Task.TaskType.Id == Convert.ToInt32(taskType)).ToList();
                }
                if (taskId != "" && taskId != null)
                {
                    usertasks = usertasks.Where(x => x.TaskID == Convert.ToInt32(taskId)).ToList();
                }
                if (days != "" && days != null)
                {
                    if (days == "30")
                    {
                        foreach (User_Task ut in usertasks)
                        {
                            int diff = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                            if(ut.User1 ==null && ut.User==null)
                            {
                                continue;
                            }

                            if (diff > 0 && diff <= 30)
                            {
                                LateReportDTO obj = new LateReportDTO();
                                obj.Id = Convert.ToInt32(ut.Id);
                                obj.TaskName = ut.Task.Name;
                                obj.UserName = ut.User1==null ? ut.User.FirstName + ' ' + ut.User.LastName: ut.User1.FirstName + ' ' + ut.User1.LastName;
                                obj.Division = ut.Task.Department.Division.Name;
                                obj.Department = ut.Task.Department.Name;
                                obj.StartDate = ut.StartDate.Value.ToString("MM/dd/yyyy");
                                obj.EndDate = ut.EndDate.Value.ToString("MM/dd/yyyy");
                                obj.LateDays = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                                searchedUserTasks.Add(obj);
                            }
                        }
                    }

                    if (days == "60")
                    {
                        foreach (User_Task ut in usertasks)
                        {
                            int diff = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                            if (ut.User1 == null && ut.User == null)
                            {
                                continue;
                            }

                            if (diff >= 31 && diff <= 60)
                            {
                                LateReportDTO obj = new LateReportDTO();
                                obj.Id = Convert.ToInt32(ut.Id);
                                obj.TaskName = ut.Task.Name;
                                obj.UserName = ut.User1 == null ? ut.User.FirstName + ' ' + ut.User.LastName : ut.User1.FirstName + ' ' + ut.User1.LastName;
                                obj.Division = ut.Task.Department.Division.Name;
                                obj.Department = ut.Task.Department.Name;
                                obj.StartDate = ut.StartDate.Value.ToString("MM/dd/yyyy");
                                obj.EndDate = ut.EndDate.Value.ToString("MM/dd/yyyy");
                                obj.LateDays = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                                searchedUserTasks.Add(obj);
                            }
                        }
                    }

                    if (days == "90")
                    {
                        foreach (User_Task ut in usertasks)
                        {
                            int diff = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                            if (ut.User1 == null && ut.User == null)
                            {
                                continue;
                            }

                            if (diff >= 61 && diff <= 90)
                            {
                                LateReportDTO obj = new LateReportDTO();
                                obj.Id = Convert.ToInt32(ut.Id);
                                obj.TaskName = ut.Task.Name;
                                obj.UserName = ut.User1 == null ? ut.User.FirstName + ' ' + ut.User.LastName : ut.User1.FirstName + ' ' + ut.User1.LastName;
                                obj.Division = ut.Task.Department.Division.Name;
                                obj.Department = ut.Task.Department.Name;
                                obj.StartDate = ut.StartDate.Value.ToString("MM/dd/yyyy");
                                obj.EndDate = ut.EndDate.Value.ToString("MM/dd/yyyy");
                                obj.LateDays = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                                searchedUserTasks.Add(obj);
                            }
                        }
                    }

                    if (days == "91")
                    {
                        foreach (User_Task ut in usertasks)
                        {
                            int diff = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                            if (ut.User1 == null && ut.User == null)
                            {
                                continue;
                            }

                            if (diff >= 91)
                            {
                                LateReportDTO obj = new LateReportDTO();
                                obj.Id = Convert.ToInt32(ut.Id);
                                obj.TaskName = ut.Task.Name;
                                obj.UserName = ut.User1 == null ? ut.User.FirstName + ' ' + ut.User.LastName : ut.User1.FirstName + ' ' + ut.User1.LastName;
                                obj.Division = ut.Task.Department.Division.Name;
                                obj.Department = ut.Task.Department.Name;
                                obj.StartDate = ut.StartDate.Value.ToString("MM/dd/yyyy");
                                obj.EndDate = ut.EndDate.Value.ToString("MM/dd/yyyy");
                                obj.LateDays = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                                searchedUserTasks.Add(obj);
                            }
                        }
                    }

                    if (days == "92") //used to hanlde less than 90 days (By Waqas)
                    {
                        foreach (User_Task ut in usertasks)
                        {
                            int diff = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                            if (ut.User1 == null && ut.User == null)
                            {
                                continue;
                            }

                            if (diff > 0 && diff <= 90)
                            {
                                LateReportDTO obj = new LateReportDTO();
                                obj.Id = Convert.ToInt32(ut.Id);
                                obj.TaskName = ut.Task.Name;
                                obj.UserName = ut.User1 == null ? ut.User.FirstName + ' ' + ut.User.LastName : ut.User1.FirstName + ' ' + ut.User1.LastName;
                                obj.Division = ut.Task.Department.Division.Name;
                                obj.Department = ut.Task.Department.Name;
                                obj.StartDate = ut.StartDate.Value.ToString("MM/dd/yyyy");
                                obj.EndDate = ut.EndDate.Value.ToString("MM/dd/yyyy");
                                obj.LateDays = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                                searchedUserTasks.Add(obj);
                            }
                        }
                    }
                }

                searchedUserTasks = searchedUserTasks.OrderByDescending(x => x.LateDays).ToList();

                List<LateReportDTO> mnglist = new List<LateReportDTO>();

                foreach (LateReportDTO x in searchedUserTasks)
                {
                    LateReportDTO obj = new LateReportDTO();
                    obj.Id = Convert.ToInt32(x.Id);
                    obj.TaskName = x.TaskName;
                    obj.UserName = x.UserName;
                    obj.Division = x.Division;
                    obj.Department = x.Department;
                    obj.StartDate = x.StartDate;
                    obj.EndDate = x.EndDate;
                    obj.LateDays = x.LateDays;

                    mnglist.Add(obj);
                }


                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];



                int totalrows = mnglist.Count();
                //filter
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    searchedUserTasks = searchedUserTasks.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()) || x.Department.Division.Name.ToLower().Contains(searchValue.ToLower()) || x.Hours.Equals(searchValue) || x.Cost_.Equals(searchValue) || x.CEU.Equals(searchValue)).ToList();
                //}

                int totalrowsafterfilterinig = mnglist.Count();
                //sorting

                // pagination
                mnglist = mnglist.Skip(start).Take(length).ToList();


                return Json(new { data = mnglist.OrderByDescending(x => x.EndDate), draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        [HttpPost]
        public string getLateEmployeeDTO(string Id)
        {
            User_Task ut = new User_TaskBL().getUser_TasksById(Convert.ToInt32(Id));

            LateReportDTO ldtp = new LateReportDTO()
            {
                Id = ut.Id,
                TaskName = ut.Task.Name,
                UserName = ut.User1.FirstName + ' ' + ut.User1.LastName,
                UserEmail = ut.User1.Email,
                PhoneNumber = ut.User1.PhoneNumber
            };

            return JsonConvert.SerializeObject(ldtp, Formatting.Indented,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
               });
        }

        [HttpPost]
        public ActionResult LateReportNotification(int usertaskId, string username, string taskname, string useremail, string userPhoneNumber, string subject, string emailcontent, string Notification1, string Notification2)
        {
            try
            {
                User_Task usertask = new User_TaskBL().getUser_TasksById(usertaskId);

                var fromAddress = new MailAddress(ProjectVaraiables.EMAIL, "Zuptu System");
                string fromPassword = ProjectVaraiables.PASSWORD;
                string temp = "<html><head></head><body> <div><p >Dear " + username + ",<br><p>" + emailcontent + "</p></div> Thanks and Regards, <br> Team Zuptu";

                temp += "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";

                var smtp = new SmtpClient
                {
                    Host = ProjectVaraiables.WEBHOST,
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, new MailAddress(useremail))
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = temp
                })
                {
                    smtp.Send(message);
                }

                if (Notification2 == "2")
                {
                    SMS_Service.sendMessage(emailcontent, userPhoneNumber);
                }

                return RedirectToAction("LateReport", new { message = "Notification has been sent successfully" });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }


        public ActionResult AllLateReportNotifications(string senddivision, string senddepartment, string sendtaskType, string senddays, string sendsubject, string sendcontent, string Notification1, string Notification2)
        {
            try
            {
                List<User_Task> usertasks = new User_TaskBL().getAllUser_TasksList().Where(x => x.IsActive == 1 && x.CompletionDate == null).ToList();

                List<User_Task> searchedUserTasks = new List<User_Task>();


                if (senddivision != "-1" && senddivision != "" && senddivision != null)
                {
                    usertasks = usertasks.Where(x => x.Task.Department.DivisionID == Convert.ToInt32(senddivision)).ToList();
                }
                if (senddepartment != "" && senddepartment != null)
                {
                    usertasks = usertasks.Where(x => x.Task.DepartmentID == Convert.ToInt32(senddepartment)).ToList();
                }
                if (sendtaskType != "" && sendtaskType != null)
                {
                    usertasks = usertasks.Where(x => x.Task.TaskType.Id == Convert.ToInt32(sendtaskType)).ToList();
                }

                if (senddays != "" && senddays != null)
                {
                    if (senddays == "30")
                    {
                        foreach (User_Task ut in usertasks)
                        {
                            int diff = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                            if (diff > 0 && diff <= 30)
                            {
                                searchedUserTasks.Add(ut);
                            }
                        }
                    }

                    if (senddays == "60")
                    {
                        foreach (User_Task ut in usertasks)
                        {
                            int diff = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                            if (diff >= 31 && diff <= 60)
                            {
                                searchedUserTasks.Add(ut);
                            }
                        }
                    }

                    if (senddays == "90")
                    {
                        foreach (User_Task ut in usertasks)
                        {
                            int diff = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                            if (diff >= 61 && diff <= 90)
                            {
                                searchedUserTasks.Add(ut);
                            }
                        }
                    }

                    if (senddays == "91")
                    {
                        foreach (User_Task ut in usertasks)
                        {
                            int diff = (DateTime.Now.Date - Convert.ToDateTime(ut.EndDate).Date).Days;

                            if (diff >= 91)
                            {
                                searchedUserTasks.Add(ut);
                            }
                        }
                    }
                }


                List<LateReportDTO> mnglist = new List<LateReportDTO>();

                foreach (User_Task x in searchedUserTasks)
                {

                    var fromAddress = new MailAddress(ProjectVaraiables.EMAIL, "Zuptu System");
                    string fromPassword = ProjectVaraiables.PASSWORD;
                    string temp = "<html><head></head><body> <div><p >Dear " + x.User1.FirstName + ' ' + x.User1.LastName + ",<br><p>" + sendcontent + "</p></div> Thanks and Regards, <br> Team Zuptu";

                    temp += "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";

                    var smtp = new SmtpClient
                    {
                        Host = ProjectVaraiables.WEBHOST,
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };

                    using (var message = new MailMessage(fromAddress, new MailAddress(x.User1.Email))
                    {
                        IsBodyHtml = true,
                        Subject = sendsubject,
                        Body = temp
                    })
                    {
                        smtp.Send(message);
                    }

                    if (Notification2 == "2")
                    {
                        SMS_Service.sendMessage(sendcontent, x.User1.PhoneNumber);
                    }
                }


                return RedirectToAction("LateReport", new { message = "Notifications has beeen sent succesfully." });

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult SharedReport(string message="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }
        [HttpPost]
        public ActionResult GetSharedReports()
        {
            try
            {

                List<FavoriteReport> freport = new List<FavoriteReport>();
                freport = new FavoriteReportBL().getSharedReportList().Where(x => x.User_Id == logedinuser.Id).ToList();
                
                string complettionName = string.Empty;

                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];



                int totalrows = freport.Count();
                //filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                   // tasks = tasks.Where(x => x.Task.Name.ToLower().Contains(searchValue.ToLower()) || x.TemplateName.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                int totalrowsafterfilterinig = freport.Count();
                //sorting

                // pagination
                freport = freport.Skip(start).Take(length).ToList();

                List<FavoriteReport> mnglist = new List<FavoriteReport>();

                
                foreach (FavoriteReport item in freport)
                {

                    User sharedby = new UserBL().getUsersById((int)item.SharedBy);
                    FavoriteReport obj = new FavoriteReport()
                    {

                        Id = item.Id,
                        Name=item.Name,
                        SharedDescription=item.SharedDescription,
                        Department = sharedby.FirstName + " " + sharedby.LastName,//Shared By User Name 
                        ReportType=item.ReportType



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
    }
}