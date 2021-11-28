using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingTracker.Models;
using TrainingTracker.BL;
using TrainingTracker.HelpingClasses;


namespace TrainingTracker.Controllers
{
    public class OrgStructureController : Controller
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

            if (logedinuser.Role != user.Role && logedinuser.Role != 1)
            {
                clearCache();
                return false;
            }

            return true;
        }

        // GET: OrgStructure
        public ActionResult ViewStructure(string message = "")
        {
            
            if (AuthenticateUser() == false)
            {
                return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
            }

            Company company = new CompanyBL().getCompanyById(Convert.ToInt32(logedinuser.Company));
            List<Division> dlist = new DivisionBL().getDivisionsList().ToList();
            List<User> ulist = new UserBL().getAllActiveUsersList().Where(x => x.Role == 1 && x.CompanyID == company.Id).ToList();

            ViewBag.Company = company;
            ViewBag.Divisions = dlist;
            ViewBag.Admin = ulist;

            ViewBag.Message = message;

            return View();
        }

        #region AJAX

        [HttpPost]
        public ActionResult GetManagerList(int DivId)
        {
            List<User> ulist = new UserBL().getAllUsersList().Where(x => x.Role == 2 && x.DivisionId == DivId && x.IsActive != 0).ToList();
            List<UserDTO> udto = new List<UserDTO>();
            foreach (User u in ulist)
            {
                UserDTO obj = new UserDTO()
                {
                    Id = u.Id,
                    Name = u.FirstName + " " + u.LastName,
                    IsActive = (int)u.IsActive
                };

                udto.Add(obj);
            }

            return Json(udto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDepartmentList(int DivId)
        {
            List<Department> dlist = new DepartmentBL().getDepartmentsList().Where(x => x.DivisionID == DivId).ToList();

            List<Department> ddto = new List<Department>();
            foreach (Department d in dlist)
            {


                Department obj = new Department()
                {
                    Id = d.Id,
                    Name = d.Name
                };

                ddto.Add(obj);
            }

            return Json(ddto, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetEmployeeList(int DepId)
        {
            List<User> ulist = new UserBL().getAllUsersList().Where(x => x.Role == 3 && x.DepartmentId == DepId && x.IsActive != 0).ToList();

            List<UserDTO> udto = new List<UserDTO>();
            foreach (User u in ulist)
            {
                UserDTO obj = new UserDTO()
                {
                    Id = u.Id,
                    Name = u.FirstName + " " + u.LastName,
                    IsActive = (int)u.IsActive
                };

                udto.Add(obj);
            }

            return Json(udto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCompanyDetail(int CompId)
        {
            List<User> ulist = new UserBL().getAllUsersList().Where(x => x.Role == 1 && x.CompanyID == CompId && x.IsActive != 0).ToList();
            
            //int divCount = new DivisionBL().getDivisionsList().Count();
            int depCount = new DepartmentBL().getDepartmentsList().Where(x => x.CompanyId == CompId).Count();
            int userCount = new UserBL().getAllActiveUsersList().Where(x => x.CompanyID == CompId).Count();

            List<UserDTO> udto = new List<UserDTO>();
            foreach (User u in ulist)
            {
                UserDTO obj = new UserDTO()
                {
                    Id = u.Id,
                    Name = u.FirstName + " " + u.LastName,
                    IsActive = (int)u.IsActive
                };

                udto.Add(obj);
            }

            //udto[0].Division = divCount.ToString();
            udto[0].Department = depCount.ToString();
            udto[0].Profile = userCount.ToString();
            return Json(udto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetUserList(int Id, int Role )
        {

            List<User> userList = new List<User>();

            if (Role == 1)
            {
                userList = new UserBL().getAllUsersList().Where(x => x.CompanyID == Id && x.Role == 1 && x.IsActive != 0).ToList();
            }
            else if (Role == 2)
            {
                userList = new UserBL().getAllUsersList().Where(x => x.Role == 2 && x.DivisionId == Id && x.IsActive != 0).ToList();
            }
            else if (Role == 3)
            {
                userList = new UserBL().getAllUsersList().Where(x => x.Role == 3 && x.DepartmentId == Id && x.IsActive != 0).ToList();
            }

            List<UserDTO> udto = new List<UserDTO>();

            foreach (User i in userList)
            {
                UserDTO obj = new UserDTO()
                {
                    Id = i.Id,
                    EncriptedId = General_Purpose.Encrypt(i.Id),
                    Name = i.FirstName + " " + i.LastName,
                    IsActive = (int)i.IsActive
                };

                udto.Add(obj);
            }

            return Json(udto, JsonRequestBehavior.AllowGet);

        }

        #endregion

        protected override void OnException(ExceptionContext filterContext)
        {
            string action = filterContext.RouteData.Values["action"].ToString();
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
            filterContext.Result = RedirectToAction("Error", "NotFound");
        }
    }
}