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
    public class CourseController : Controller
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        MainMailClass errormail = new MainMailClass();

        public ActionResult Error()
        {
            return View();
        }

        // GET: Course
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
        public ActionResult CompletedCourses(string message = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });

                }
                // User user = new UserBL().getUsersById(logedinuser.Id);
                ViewBag.message = message;
                return View();
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public ActionResult GetCompletedCoursesList()
        {
          
            List<Course_UserTask> Temp = new Course_UserTaskBL().getCourse_UserTaskList().Where(x => x.UserId == logedinuser.Id && (x.ResultStatus == 1 || x.ResultStatus == 0)).ToList();
            List<Course_UserTask> List = new List<Course_UserTask>();
            foreach(Course_UserTask item in Temp)
            {
                Course cou = new CourseBL().getCourseById((int)item.CourseID);
                if (cou == null)
                    continue;
                if (cou.CreateCertificate != 1 || cou.IsActive==0)
                {
                    continue;
                }
               // c++;
              //  User us = new UserBL().getUsersById((int)item.User_Task.UserID);
                User_Task ut = new User_TaskBL().getUser_TasksById((int)item.User_TaskID);
               // Course course = new CourseBL().getCourseById((int)item.CourseID);
                if (ut == null)
                {
                    continue;
                }
                if (ut.IsActive == 0)
                {
                    continue;
                }

                List.Add(item);
            }

            int start2 = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];


            int totalrows = List.Count();
            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                List = List.Where(x => new CourseBL().getCourseById((int)x.CourseID).Name.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalrowsafterfilterinig = List.Count();
            //sorting

            // pagination
            List = List.Skip(start2).Take(length).ToList();

            List<CourseDTO> mnglist = new List<CourseDTO>();
            int c = 1;
            foreach (Course_UserTask item in List.OrderByDescending(y => y.CompletedAt))
            {
                Course cou = new CourseBL().getCourseById((int)item.CourseID);
                if (cou.CreateCertificate != 1)
                {
                    continue;
                }
             
                User us = new UserBL().getUsersById((int)item.User_Task.UserID);
                User_Task ut = new User_TaskBL().getUser_TasksById((int)item.User_TaskID);
                Course course = new CourseBL().getCourseById((int)item.CourseID);
                if (ut == null || course == null)
                {
                    continue;
                }
                string assinto = us.FirstName + " " + us.LastName;
                string assinby = "";
                User us1 = new User();
                if (ut.CreatedID.HasValue)
                {
                    us1 = new UserBL().getUsersById((int)ut.CreatedID);
                    assinby = us1.FirstName + " " + us1.LastName;

                }
                else
                {
                    us1 = new UserBL().getUsersById(us.Id);
                    assinby = "Self Assigned";

                }
                CourseDTO obj = new CourseDTO()
                {
                    Id = item.Id,
                    EncryptedId=General_Purpose.EncryptId(item.Id),
                    CourseName=course.Name,
                    TaskName=ut.Task.Name,
                    AssignedTo=assinto,
                    AssignedBy=assinby,
                    CompletedAt=item.CompletedAt.Value.ToString("MM/dd/yyyy"),
                    SerialNumber=c,
                    ResultStatus=item.ResultStatus==1?"PASS":"FAIL"

                };
                mnglist.Add(obj);
                c++;
            }



            return Json(new { data = mnglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);




        }
    }
}