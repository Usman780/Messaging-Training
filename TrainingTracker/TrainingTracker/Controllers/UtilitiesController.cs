using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Xml;
using TrainingTracker.BL;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;
using TrainingTracker.HelpingClasses.GoogleCalendar;
using TrainingTracker.HelpingClasses.Logging;
using TrainingTracker.Models;
using WebMarkupMin.AspNet4.Mvc;

namespace TrainingTracker.Controllers
{
    public class UtilitiesController : Controller, IRequiresSessionState
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
        // GET: Utilities
        [CompressContent]
        [MinifyXml]
        [OutputCache(CacheProfile = "CacheCompressedContent5Minutes")]
        public ActionResult Index()
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                List<Division> divisions = new DivisionBL().getDivisionsList().OrderBy(s => s.Name).ToList();
                List<Tag> tags = new TagBL().getAllTagsList().OrderBy(s => s.Name).ToList();
                List<Department> department = new DepartmentBL().getAllDepartmentsList().OrderBy(s => s.Name).ToList();

                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
    
        #region Division

        public ActionResult displayDivision(string message = "")
        {
            try
            {
                if (logedinuser.Name == null || logedinuser.Role == 3 || logedinuser.Role == 2 || logedinuser.Role == 4)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                List<Division> divisons = new DivisionBL().getDivisionsList().OrderBy(s => s.Name).ToList();
                ViewBag.divisions = divisons;
                ViewBag.message = message;
                return View("AddDivisions");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult AddDivisions(Division division, string Way="")
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
                    if (new DivisionBL().getDivisionsList().Where(x => x.Name.ToUpper() == division.Name.ToUpper()).Count() > 0)
                    {
                        if (Way == "structure")
                        {
                            return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Division of same name exists. Can't add division with your entered name!" });
                        }
                        else
                        {
                            return RedirectToAction("displayDivision", new { message = "Division of same name exists. Can't add division with your entered name!" });
                        }
                    }
                    else
                    {
                        int id = Convert.ToInt32(logedinuser.Company);
                        division.CompanyID = id;
                        division.IsActive = 1;
                        new DivisionBL().AddDivisions(division);
                    }
                }
                if (Way == "structure")
                {
                    return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Division has been added." });
                }
                else
                {
                    return RedirectToAction("displayDivision", new { message = "Division has been added." });
                }
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult UpdateDivisions(int id)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                List<Division> divisons = new DivisionBL().getDivisionsList().OrderBy(s => s.Name).ToList();
                ViewBag.divisions = divisons;
                ViewBag.division = divisons.Where(x => x.Id == id).FirstOrDefault();

                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult deleteDivision(int id)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                new DivisionBL().DeleteDivisions(id);
                return RedirectToAction("displayDivision", new { message = "Your division has been deleted." });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult UpdateDivisionAction(Division division)
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
                    int i = new DivisionBL().getDivisionsList().Where(x => x.Name == division.Name).Count();
                    if (new DivisionBL().getDivisionsList().Where(x => x.Name == division.Name).Count() > 1)
                    {
                        return RedirectToAction("displayDivision", new { message = "Division of same name exists. Can't add division with your entered name!" });
                    }
                    else
                    {
                        int id = Convert.ToInt32(logedinuser.Company);
                        division.CompanyID = id;
                        division.IsActive = 1;
                        new DivisionBL().UpdateDivisions(division);
                    }
                }
                return RedirectToAction("displayDivision", new { message = "Your division has been updated." });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult addDivision(string name)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                new DivisionBL().AddDivisions(new Division() { Name = name, CompanyID = Convert.ToInt32(logedinuser.Company) });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        #endregion Division


        #region Worktype
        public ActionResult displayWorkType(string message = "")
        { 
            try 
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                //List<TaskType> divisons = new TaskTypeBL().getTaskTypesList().OrderBy(s => s.Name).ToList();
                //ViewBag.TaskTypes = divisons;
                //if (message.Contains('_'))
                //{
                //   message= message.Replace('_', ' ');
                //}
                ViewBag.message = message;
                List<Division> listdiv = new DivisionBL().getDivisionsList().Where(x => x.IsActive == 1).ToList();
                ViewBag.DivList = listdiv;
                return View("WorkType/AddWorkType");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        public ActionResult GetdisplayWorkType()
        {
            try
            {

                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                List<Worktype> divisons = new WorktypeBL().getWorktypesList().OrderBy(x => x.Id).ToList();
                int totalrows = divisons.Count();
                //filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    divisons = divisons.Where(x => x.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                int totalrowsafterfilterinig = divisons.Count();
                //sorting

                // pagination
                divisons = divisons.Skip(start).Take(length).ToList();

                List<WorktypeDTO> worktypelist = new List<WorktypeDTO>();
                int c = 0;
                string str = "";
                foreach (Worktype x in divisons)
                {
                    if (x.DivisionID != null)
                    {
                        str = new DivisionBL().getDivisionsById((int)x.DivisionID).Name;
                    }
                    else
                    {
                        str = "NA";
                    }

                    if (x.DivisionID == null)
                    {
                        x.DivisionID = -1;
                    }
                    
                    WorktypeDTO obj = new WorktypeDTO()
                    {
                        CompanyID = c + 1,
                        Name = x.Name,
                        Id = x.Id,
                        DivisionName=str,
                        DivisionID=(int)x.DivisionID
                        
                    };
                    worktypelist.Add(obj);
                    c++;
                }
                
                return Json(new { data = worktypelist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult GetWorkType(int worktypeid = -1)
        {
            Worktype worktype = new WorktypeBL().getWorktypesById(worktypeid);


            return Json(worktype.Name);
        }

        public ActionResult AddWorkTypes(Worktype WorkType)
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
                    int id = logedinuser.Id;
                    int company = Convert.ToInt32(logedinuser.Company);

                    Worktype wt = new Worktype()
                    {
                        Name = WorkType.Name,
                        Id = WorkType.Id,
                        CompanyID = company,
                        IsActive = 1,
                        AddedBy = id,
                        CreatedAt = DateTime.Now
                    };
                    new WorktypeBL().AddWorktypes(wt);
                }

                return RedirectToAction("displayWorkType", new { message = "Your Work type has been added." });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

       

        public ActionResult deleteWorkType(int id)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                new WorktypeBL().DeleteWorktypes(id);
                return RedirectToAction("displayWorkType", new { message = "Worker type deleted successfully." });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult UpdateWorkTypeAction(Worktype Worktype,int UpdateDivisionID=-1)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                int adminid = logedinuser.Role;
                if(Worktype.Name==null ||  Worktype.Name=="" || UpdateDivisionID == -1)
                {
                    return RedirectToAction("displayWorkType", new { message = "Worker type could not updated.Worktype name & Division name not be empty." });
                }
               int value = Convert.ToInt32(new WorktypeBL().getWorktypesList().Where(x => x.Name.ToUpper().Equals(Worktype.Name.ToUpper()) && x.DivisionID == UpdateDivisionID).Count());
                if (value > 0)
                {
                    return RedirectToAction("displayWorkType", new { message = "Worker type already exists." });

                }

                if (adminid == (int)Enums.Role.Admin)
                {
                    int id = logedinuser.Id;
                    int company = Convert.ToInt32(logedinuser.Company);

                    Worktype wt = new Worktype()
                    {
                        Name = Worktype.Name,
                        Id = Worktype.Id,
                        CompanyID = company,
                        IsActive = 1,
                        AddedBy = id,
                        CreatedAt = DateTime.Now,
                        DivisionID= UpdateDivisionID
                    };
                    //wt.CompanyID = Convert.ToInt32(logedinuser.Company);
                    new WorktypeBL().UpdateWorktypes(wt);
                }
                return RedirectToAction("displayWorkType",new { message="Work type updated successfully."});
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult addWorkType(string name="",int DivisionID=-1)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                if (name == "" || DivisionID == -1)
                {
                    return RedirectToAction("displayWorkType", new { message = "Worker type name & Division name not be empty." });
                }


                int value = Convert.ToInt32(new WorktypeBL().getWorktypesList().Where(x => x.Name.ToUpper().Equals(name.ToUpper()) && x.DivisionID == DivisionID).Count());
                if (value > 0)
                {
                    return RedirectToAction("displayWorkType", new { message = "Worker type already exists" });
                }

                int company = Convert.ToInt32(logedinuser.Company);
                new WorktypeBL().AddWorktypes(new Worktype() { Name = name, CompanyID = company, IsActive = 1, CreatedAt = DateTime.Now, AddedBy = logedinuser.Id, DivisionID = DivisionID });
                return RedirectToAction("displayWorkType", new { message = "Worker_type_added_successfully." });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        #endregion


        #region TaskType

        public ActionResult displayTaskType(string message = "")
        {
            try {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

              
                ViewBag.message = message;

                return View("TaskType/AddTaskType");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        public ActionResult GetdisplayTaskType()
        {
            try
            {
                // ViewBag.TaskTypes = divisons;
                // Server side parameter
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                List<TaskType> divisons = new TaskTypeBL().getTaskTypesList().OrderBy(s => s.Id).ToList();
                // List<User> users = new UserBL().getUserList().OrderBy(x => x.Role).ToList();
                int totalrows = divisons.Count();
                //filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    divisons = divisons.Where(x => x.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                int totalrowsafterfilterinig = divisons.Count();
                //sorting
                //users = users.OrderBy(sortColumnName + " " + sortDirection).ToList();

                // pagination
                divisons = divisons.Skip(start).Take(length).ToList();

                List<TaskType> tasktypelist = new List<TaskType>();
                int c = 0;
                foreach (TaskType x in divisons)
                {
                    TaskType obj = new TaskType()
                    {
                        CompanyID = c + 1,
                        Name = x.Name,
                        Id = x.Id
                    };
                    tasktypelist.Add(obj);
                    c++;
                }
               

                return Json(new { data = tasktypelist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult GetTaskType(int tasktypeid = -1)
        {
            try
            {
                TaskType tasktype = new TaskTypeBL().getTaskTypesById(tasktypeid);


                return Json(tasktype.Name);
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public string GetUserById(int Id)
        {
            User user = new UserBL().getUsersById(Id);

            UserDTO dto = new UserDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                HomeNumber = user.HomeNumber,
                Password = StringCipher.Decrypt(user.Password, "zuptu"),
                IsMasterAdmin = (int)user.IsMasterAdmin
            };

            return JsonConvert.SerializeObject(dto, Newtonsoft.Json.Formatting.Indented,
                  new JsonSerializerSettings()
                  {
                      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                  });
        }

        public ActionResult PostUpdateAdmin(User admin, string confirmPassword, string way="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                if (confirmPassword == null
               || confirmPassword == ""
               || confirmPassword != admin.Password
               )
                {
                    if(way == "structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Password did not match!" });
                    }
                    else
                    {
                        return RedirectToAction("DisplayAdmin", "Utilities", new { message = "Password did not match!" });
                    }
                }

                User loggedInUser = new UserBL().getUsersById(logedinuser.Id);

                DatabaseEntities de = new DatabaseEntities();
                User existingUser = new UserBL().getUsersById(admin.Id, de);
                existingUser.FirstName = admin.FirstName;
                existingUser.LastName = admin.LastName;
                existingUser.Password = StringCipher.Encrypt(admin.Password, "zuptu");
                existingUser.PhoneNumber = admin.PhoneNumber;
                existingUser.HomeNumber = admin.HomeNumber;

                if(loggedInUser.IsPrimary == 1)
                {
                    if(admin.IsMasterAdmin == null)
                    {
                        existingUser.IsMasterAdmin = 0;
                    }
                    else
                    {
                        existingUser.IsMasterAdmin = admin.IsMasterAdmin;
                    }
                }

                if (!new UserBL().UpdateUsers(existingUser, de))
                {
                    if (way == "structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Ops, something bad happens!" });
                    }
                    else
                    {
                        return RedirectToAction("DisplayAdmin", "Utilities", new { message = "Ops, something bad happens!" });
                    }
                }

                if (way == "structure")
                {
                    return RedirectToAction("ViewStructure", "OrgStructure", new { message = "User updated successfully!" });
                }
                else
                {
                    return RedirectToAction("DisplayAdmin", "Utilities", new { message = "User updated successfully!" });
                }

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }           
        }
        public ActionResult AddTaskTypes(TaskType TaskType)
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
                    int id = logedinuser.Id;
                    int company = Convert.ToInt32(logedinuser.Company);

                    TaskType.IsActive = 1;
                    TaskType.CompanyID = company;
                    new TaskTypeBL().AddTaskTypes(TaskType);
                }

                return RedirectToAction("displayTaskType", new { message = "Your task type has been added." });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult UpdateTaskTypes(int id)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                List<TaskType> divisons = new TaskTypeBL().getTaskTypesList().OrderBy(s => s.Name).ToList();
                ViewBag.TaskTypes = divisons;
                ViewBag.TaskType = divisons.Where(x => x.Id == id).FirstOrDefault();

                return View("TaskType/UpdateTaskType");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult deleteTaskType(int id)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                new TaskTypeBL().DeleteTaskTypes(id);
                return RedirectToAction("displayTaskType", new { message = "You have deleted task type." });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult UpdateTaskTypeAction(TaskType TaskType)
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
                    int id = logedinuser.Id;
                    int company = Convert.ToInt32(logedinuser.Company);
                    TaskType.CompanyID = company;
                    TaskType.IsActive = 1;
                    new TaskTypeBL().UpdateTaskTypes(TaskType);
                }
                return RedirectToAction("displayTaskType");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult addTaskType(string name)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                name = name.Trim();

                int value = Convert.ToInt32((new TaskTypeBL().getTaskTypesList().Where(x => x.Name.ToUpper().Equals(name.ToUpper())).Count() > 0));

                if (value != 0)
                {
                    return RedirectToAction("displayTaskType",new { message="Task type already exists in the system"});

                }

                int company = Convert.ToInt32(logedinuser.Company);
                new TaskTypeBL().AddTaskTypes(new TaskType() { Name = name, CompanyID = company, IsActive = 1 });
                return RedirectToAction("displayTaskType");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        #endregion TaskType

        #region Department

        public ActionResult displayDepartment(string sid, string message = "")
        {
            try
            {
                if (logedinuser.Name == null || logedinuser.Role == 3 || logedinuser.Role == 4)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }


                List<Division> divisions= new List<Division>();
                if (logedinuser.Role == 1)
                {
                    divisions = new DivisionBL().getDivisionsList().OrderBy(s => s.Name).ToList();
                }
                else if (logedinuser.Role == 2)
                {
                    User us = new UserBL().getUsersById(logedinuser.Id);
                    divisions = new DivisionBL().getDivisionsList().OrderBy(s => s.Name).Where(x => x.Id == us.DivisionId).ToList();
                }
                //ViewBag.divisions = new DivisionBL().getDivisionsList().OrderBy(s => s.Name).ToList();
                ViewBag.divisions = divisions;
                ViewBag.message = message;
                ViewBag.Id = sid;
                return View("Departments/AddDepartment");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        public ActionResult GetdisplayDepartment()
        {
            try
            {

                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                List<Department> departments = new List<Department>();
                if (logedinuser.Role == 1)
                {
                    departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();
                }
                else if (logedinuser.Role == 2)
                {
                    User us = new UserBL().getUsersById(logedinuser.Id);
                    departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).Where(x => x.DivisionID == us.DivisionId).ToList();
                }
                //List<Department> departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();


                int totalrows = departments.Count();
                //filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    departments = departments.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()) || x.Division.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                int totalrowsafterfilterinig = departments.Count();
                //sorting

                // pagination
                departments = departments.Skip(start).Take(length).ToList();

                List<ManagerDTO> mnglist = new List<ManagerDTO>();
                int c = 0;

                foreach (Department x in departments)
                {
                    if (new DepartmentBL().getDepartmentsById(x.Id).Users.Where(z => z.Role == 3 && z.IsActive == 1).Count() == 0 && new DepartmentBL().getDepartmentsById(x.Id).Tasks.Where(z => z.IsActive == 1).Count() == 0)
                    {
                        x.IsActive = 11;
                    }
                    ManagerDTO obj = new ManagerDTO()
                    {

                        SerialNo = c + 1, // serial number 
                        DepartmentName = x.Name, //Name
                        IsActive = (int)x.IsActive,
                        DivisionName = x.Division.Name, // division Name
                        DivisionId = x.Division.Id,
                        Id = x.Id,
                        EncryptedId = General_Purpose.EncryptId(x.Id).ToString()

                    };
                    mnglist.Add(obj);
                    c++;


                }

                return Json(new { data = mnglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        public ActionResult AddDepartments(Department Department,string DeptId="",string way="")
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
                    int value = Convert.ToInt32((new DepartmentBL().getDepartmentsList().Where(x => x.DivisionID == Department.DivisionID && x.Name.ToUpper().Equals(Department.Name.ToUpper())).Count() > 0));

                    if (value > 0)
                    {
                        if (way != "")
                        {
                            if (way == "structure")
                            {
                                return RedirectToAction("displayDepartment", "ViewStructure", new { message = "Department of same name exists. Department can't be added." });
                            }
                            else
                            {
                                return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(Department.DivisionID.Value), message = "Department of same name exists. Department can't be added." });
                            }
                        }
                        else
                        {
                            return RedirectToAction("displayDepartment", new { message = "Department of same name exists. Department can't be added.", sid = DeptId });
                        }
                    }
                    else
                    {
                        int id = logedinuser.Id;

                        Department.IsActive = 1;
                        new DepartmentBL().AddDepartments(Department);
                    }
                }

                if (way != "")
                {
                    if (way == "structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Department has been added." });
                    }
                    else
                    {
                        return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(Department.DivisionID.Value), message = "Department has been added." });
                    }
                }
                else
                {
                    return RedirectToAction("displayDepartment", new { message = "Department has been added.", sid = DeptId });
                }

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult UpdateDepartments(int id)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                List<Department> departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();
                ViewBag.Departments = departments;
                Department department = ViewBag.Department = departments.Where(x => x.Id == id).FirstOrDefault();
                ViewBag.divisions = new DivisionBL().getAllDivisionsList().OrderBy(s => s.Name).ToList();
                ViewBag.division = department.Division;
                return View("Departments/UpdateDepartment");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult deleteDepartment(int id, int divisionId = -1,string DeptId="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                new DepartmentBL().DeleteDepartments(id);
                if (divisionId != -1)
                {
                    return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(divisionId), message = "A department has been deleted." });
                }

                return RedirectToAction("displayDepartment", new { message = "You have deleted the department.",sid=DeptId });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult UpdateDepartmentAction(Department Department, string way = "",string DeptId="")
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
                    Department dep = new DepartmentBL().getDepartmentsById(Department.Id);
                    int value = -1;
                    if (dep.Name != Department.Name)
                    {
                        value = Convert.ToInt32((new DepartmentBL().getDepartmentsList().Where(x => x.DivisionID == Department.DivisionID && x.Name.Equals(Department.Name)).Count() > 0));

                    }

                    if (value > 0)
                    {
                        if(way!="")
                        return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(Department.DivisionID.Value), page1 = "Division", message = "Department of same name exists. Department can't be added." });
                        else
                            return RedirectToAction("displayDepartment", new { message = "Department of same name exists. Department can't be added.",sid=DeptId });


                    }
                    else
                    {
                        int id = logedinuser.Id;

                        Department.IsActive = 1;
                        Department.CompanyId = Convert.ToInt32(logedinuser.Company);
                        new DepartmentBL().UpdateDepartments(Department);
                    }
                }
                if (way != "")
                {
                    return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(Department.DivisionID.Value), page1 = "Division", message = "You have updated the department." });

                }
                return RedirectToAction("displayDepartment", new { message = "You have updated the department.", sid = DeptId });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        #endregion Department

        #region Tag

        public ActionResult displayTag(string message = "", string sid = "", string way = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                List<Tag> Tags = new TagBL().getTagsList().OrderByDescending(s => s.Id).ToList();
                ViewBag.Tags = Tags;
                ViewBag.divisions = new DivisionBL().getDivisionsList().OrderBy(s => s.Name).ToList();
                ViewBag.message = message;
                ViewBag.way = way;
                ViewBag.sid = sid;
                return View("Tags/AddTag");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult GetdisplayTag()
        {
            try
            {

                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                List<Tag> Tags = new TagBL().getTagsList().OrderByDescending(s => s.Id).ToList();
                //List<Department> departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();
                // List<User> managerlist = new UserBL().getActiveandInvitedUser().Where(x => x.CompanyID == (int)logedinuser.Company && (x.Role == 2 || x.Role == 4) && (x.IsActive == 1 || x.IsActive == 2)).OrderBy(s => s.FirstName).ToList();


                int totalrows = Tags.Count();
                //filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Tags = Tags.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()) || x.Division.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                int totalrowsafterfilterinig = Tags.Count();
                //sorting
                //users = users.OrderBy(sortColumnName + " " + sortDirection).ToList();

                // pagination
                Tags = Tags.Skip(start).Take(length).ToList();

                //List<User> mnglist = new List<User>();
                List<ManagerDTO> mnglist = new List<ManagerDTO>();
                int c = 0;

                foreach (Tag x in Tags)
                {

                    ManagerDTO obj = new ManagerDTO()
                    {

                        SerialNo = c + 1, // serial number 
                        Name = x.Name, //Name
                        DivisionName = x.Division.Name, // division Name
                        DivisionId = x.Division.Id,
                        Id = x.Id,

                    };
                    mnglist.Add(obj);
                    c++;


                }

                return Json(new { data = mnglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        public ActionResult AddTags(Tag Tag, string page, string way = "", string sid = "")
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
                    int count = new TagBL().getTagsList().Where(x => x.Name == Tag.Name).Count();

                    if (count > 0)
                    {
                        return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(Tag.DivisionId.Value), message = "Tag of same name exists. Cannot add this tag!" });
                    }
                    else
                    {
                        int id = logedinuser.Id;

                        Tag.IsActive = 1;
                        new TagBL().AddTags(Tag);
                    }
                }
                if (way != "" && sid != "")
                {
                    return RedirectToAction("displayTag", "Utilities", new { sid = sid, way = way });
                }
                else if (page == "home")
                {
                    return RedirectToAction("displayTag", "Utilities");
                }
                else
                {
                    return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(Tag.DivisionId.Value), page1 = "Division", message = "A tag has been added." });
                }
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult UpdateTags(int id)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                List<Tag> Tags = new TagBL().getTagsList().OrderBy(s => s.Name).ToList();
                ViewBag.Tags = Tags;
                Tag Tag = ViewBag.Tag = Tags.Where(x => x.Id == id).FirstOrDefault();
                ViewBag.divisions = new DivisionBL().getAllDivisionsList().OrderBy(s => s.Name).ToList();
                ViewBag.division = Tag.Division;
                return View("Tags/UpdateTag");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult deleteTag(int id, int divisionId = -1)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                new TagBL().DeleteTags(id);
                if (divisionId != -1)
                {
                    return RedirectToAction("displayDivisionDetails", "utilities", new { sid = General_Purpose.EncryptId(divisionId), message = "A tag has been deleted." });
                }
                return RedirectToAction("displayTag");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult UpdateTagAction(Tag Tag)
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
                    int id = logedinuser.Id;

                    Tag.IsActive = 1;
                    Tag.CompanyId = Convert.ToInt32(logedinuser.Company);
                    new TagBL().UpdateTags(Tag);
                }
                return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(Tag.DivisionId.Value), message = "Tag has been updated." });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }


        public ActionResult addComment()
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

        #endregion Tag

        #region Manager

        public ActionResult AddManager(string message="", string divisionId = null, string page = null)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                int adminid = logedinuser.Role;
                List<Division> divisions;
                List<Department> departments;
                if (adminid == (int)Enums.Role.Admin)
                {
                    divisions = new DivisionBL().getDivisionsList().OrderBy(s => s.Name).ToList();
                    departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();
                    ViewBag.divisions = divisions;
                    ViewBag.departments = departments;
                    ViewBag.page = page;
                    ViewBag.DivisionId = divisionId;
                    ViewBag.Message = message;
                    //List<Worktype> wt = new WorktypeBL().getWorktypesList().Where(x => x.CompanyID == Convert.ToInt32(logedinuser.Company)).ToList();
                    //ViewBag.Worktype = wt;
                    return View("Users/AddManager");
                }


                return Content("UnAuthorized Action");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult DisplayManager(string sid = "", string message = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                ViewBag.message = message;
                
                ViewBag.Id = sid;
                ViewBag.Divisions = new DivisionBL().getDivisionsList();
                return View("Users/DisplayManager");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult GetDisplayManager()
        {
            try
            {
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                int adminid = logedinuser.Role;
                if (adminid == (int)Enums.Role.Admin)
                {
                    List<User> managerlist = new UserBL().getActiveandInvitedUser().Where(x => x.CompanyID == Convert.ToInt32(logedinuser.Company) && (x.Role == 2 || x.Role == 4) && (x.IsActive == 1 || x.IsActive == 2)).OrderBy(s => s.FirstName).ToList();


                    int totalrows = managerlist.Count();
                    //filter
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        managerlist = managerlist.Where(x => x.FirstName.ToLower().Contains(searchValue.ToLower()) || x.LastName.ToLower().Contains(searchValue.ToLower())
                    || x.Division.Name.ToString().ToLower().Contains(searchValue.ToLower()) || x.Email.ToLower().Contains(searchValue.ToLower())).ToList();
                    }

                    int totalrowsafterfilterinig = managerlist.Count();
                    //sorting
                    //users = users.OrderBy(sortColumnName + " " + sortDirection).ToList();

                    // pagination
                    managerlist = managerlist.Skip(start).Take(length).ToList();

                    //List<User> mnglist = new List<User>();
                    List<ManagerDTO> mnglist = new List<ManagerDTO>();
                    int c = 0;
                    string str = "";
                    string str2 = "";
                    int temp = 0;
                    foreach (User x in managerlist)
                    {
                        //int ucount = new User_TaskBL().getUser_TasksList().Where(z => (z.UserID == x.Id || z.CreatedID == x.Id) && z.CompletionDate==null).Count();
                        int ucount = 0; 
                            //sList<User_Task> usertasks = new User_TaskBL().getAllUser_TasksList().Where(x => x.UserID == x.Id) || x.CreatedID == x.Id) && x.User.CompanyID == Convert.ToInt32(logedinuser.Company)).ToList();
                        // usertasks = usertasks.Where(z => z.CompletionDate == null).ToList();


                        // int gTasks = new GroupTasks_DetailsBL().getGroupTasks_DetailssList().Where(z => z.GroupTask_User.Where(y => y.UserId == x.Id).Count() > 0 && z.CompletionDate == null).Count();
                        int gTasks = 0;


                        if (ucount != 0 || gTasks != 0)
                        {
                            temp = 1;
                        }
                        if (x.Role == 2 || x.Role == 4)
                        {
                            str2 = "Manager";
                        }
                        if (x.Division == null)
                        {
                            str = "";
                        }
                        else
                        {
                            str = x.Division.Name;
                        }
                        ManagerDTO obj = new ManagerDTO()
                        {

                            SerialNo = c + 1, // serial number 
                            Name = x.FirstName + " " + x.LastName, //Name
                            Temp = temp,
                            DivisionName = str, // division Name
                            Role = str2, // Role  Name  (Manager or any  other)
                            Email = x.Email,
                            //Email = "<span title='"+x.Email+"'>'"+x.Email+"'</span>",
                            FirstName = x.FirstName,
                            IsActive = (int)x.IsActive,
                            Id = x.Id,
                            EncryptedId = General_Purpose.EncryptId(x.Id).ToString()

                        };
                        mnglist.Add(obj);
                        c++;
                        str = "";
                        str2 = "";
                        temp = 0;

                    }


                    return Json(new { data = mnglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = 0, draw = Request["draw"], recordsTotal = 0, recordsFiltered = 0 }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        public ActionResult UpdateManager(string sid, string division = "", string message = "", string way = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                Log.Info("Update Manger Page");

                int uId = General_Purpose.DecryptId(sid);
                string str = "";
                if (uId == 0)
                {
                    str = HttpUtility.UrlEncode(sid);
                    uId= General_Purpose.DecryptId(str);
                }
                User manager = new UserBL().getUsersById(uId, new DatabaseEntities());

                Log.Info("Canvas LogIn Mail retreived from Datbase: " + manager.CanvasLoginId);

                int adminid = logedinuser.Role;
                ViewBag.message = message;
                if (adminid == (int)Enums.Role.Admin || uId == logedinuser.Id)
                {
                    ViewBag.UserWorktypes = new User_WorktypeBL().getUser_WorktypesList().Where(x => x.UserId == manager.Id).ToList();
                    List<Worktype> wt = new WorktypeBL().getWorktypesList().Where(x=>x.DivisionID==manager.DivisionId).ToList();
                    ViewBag.Worktype = wt;

                    ViewBag.departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();
                    ViewBag.division = new DivisionBL().getDivisionsList().OrderBy(s => s.Name).ToList();
                    ViewBag.isDivision = division;
                    ViewBag.divId = manager.DivisionId;
                    ViewBag.sid = sid;
                    ViewBag.way = way;
                    manager.Password = StringCipher.Decrypt(manager.Password, "zuptu");

                    Log.Info("Manager's info sending to Update Manager Page");

                    return View("Users/UpdateManager", manager);
                }

                return Content("Unauthorised Action");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult GetUpdateManagerTask(string sid, string Archived = "")
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

                int did = -1;
                int tem = General_Purpose.DecryptId(sid);
                if (tem == 0)
                {
                    sid = HttpUtility.UrlEncode(sid);
                    did = General_Purpose.DecryptId(sid);
                    //User trainee = new UserBL().getUsersById(did);
                }
                else
                {
                    did = General_Purpose.DecryptId(sid);

                }
                User trainee = new User();
                //int uId = General_Purpose.Decrypt(HttpUtility.UrlDecode(sid));
                if (Archived != "")
                {
                    trainee = new UserBL().getInActiveUserById(did);
                }
                else
                {
                    trainee = new UserBL().getUsersById(did);
                }
                List<TrainingTracker.Models.User_Task> tasks = trainee.User_Task1.Where(x => x.IsActive == 1).ToList();

                

                if (!((logedinuser.Role == 2 || logedinuser.Role == 4) && logedinuser.Id == trainee.Id))

                {
                    tasks = trainee.User_Task1.Where(x => x.IsActive == 1 && x.IsPrivate == 0).Where(x => x.Status != null).ToList().OrderBy(x => x.Status.Value).ToList();

                }


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

                List<EmployeeDTO> mnglist = new List<EmployeeDTO>();

                string priority = "";
                string status = "";
                string AssignedBy = "";
                int temp = 0;

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
                            priority = "<span class='label label-warning'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                        }
                        else if (General_Purpose.getPriorityValue(x.Priority.Value) == "Low")
                        {
                            priority = "<span class='label label-success'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                        }
                        else
                        {
                            priority = "<span class='label label-danger'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                        }

                    }
                    else
                        priority = "";

                    if (x.CreatedID == null)
                    {
                        AssignedBy = "Self Assigned";
                        temp = 1;
                    }
                    else
                    {
                        AssignedBy = x.User.FirstName + " " + x.User.LastName;
                    }

                    EmployeeDTO obj = new EmployeeDTO()
                    {

                        //IsActive=(int)x.IsActive,
                        Task = x.Task.Name,
                        StartDate = x.StartDate.Value.ToString("MM/dd/yyyy"),
                        EndtDate = x.EndDate.Value.ToString("MM/dd/yyyy"),
                        Name = AssignedBy,
                        Priority = priority,
                        WorkStatus = status,
                        IsActive = temp,

                        Id = x.Id,
                        EncryptedId = General_Purpose.EncryptId(x.Id)

                    };
                    mnglist.Add(obj);
                    temp = 0;


                }

                return Json(new { data = mnglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        public ActionResult UpdateManagerAction(User Manager, List<int> Worktypes, string repeat_password, string isDivision = "", string way = "", string CanvasLoginId = "",string RemoveFile="")
        {
            try
            {
                Log.Info("UpdateManagerAction Function Starting...");
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                if (logedinuser.Id == Manager.Id)
                {
                    var input = Manager.Password;
                    //  Regex r = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
                    var hasNumber = new Regex(@"[0-9]+");
                    var hasUpperChar = new Regex(@"[A-Z]+");
                    var hasLowerChar = new Regex(@"[a-z]+");
                    var hasspecialChar = new Regex(@"[$!%*?@/]+");
                    var hasMinimum8Chars = new Regex(@".{8,}");

                    var isValidated = hasNumber.IsMatch(input) && hasUpperChar.IsMatch(input) && hasLowerChar.IsMatch(input) && hasspecialChar.IsMatch(input) && hasMinimum8Chars.IsMatch(input);
                    if (isValidated == false)
                    {
                        if (isDivision != "")
                        {
                            return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(Manager.DivisionId.Value), message = "Password pattern dosen't match.", page1 = "Division" });

                        }
                        return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(logedinuser.Id), message = "Password pattern dosen't match.", way = way });

                    }
                    Models.User uss = new UserBL().getUsersById(Manager.Id);
                    if (Manager.Password != StringCipher.Decrypt(uss.Password, "zuptu"))
                    {

                        OldPassword olpass = new OldPassword()
                        {
                            UserId = uss.Id,
                            Password = uss.Password,
                            CreatedAt = DateTime.Now,
                            IsActive = 1,
                            CompanyId=uss.CompanyID
                        };
                        new OldPasswordBL().AddOldPassword(olpass);

                        OldPassword p = new OldPasswordBL().getOldPasswordsList().Where(x => StringCipher.Decrypt(x.Password, "zuptu") == Manager.Password && x.UserId == uss.Id).FirstOrDefault();
                        if (p != null)
                        {
                            int days = DateTime.Now.Subtract(Convert.ToDateTime(p.CreatedAt)).Days;
                            if (isDivision != "")
                            {
                                return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(Manager.DivisionId.Value), message = "You cannot use your old password.", page1 = "Division" });

                            }
                            return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(logedinuser.Id), message = "You cannot use your old password.", way = way });

                        }
                    }


                    Log.Info("Canvas Login Mail Recieved from Update Manager Page:" + CanvasLoginId);

                    if (repeat_password != null)
                        if (Manager.Password != repeat_password)
                        {
                            if (isDivision != "")
                            {
                                return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(Manager.DivisionId.Value), message = "Password and Repeat Password doesn't match.", page1 = "Division" });

                            }
                            return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(logedinuser.Id), message = "Password and Repeat Password doesn't match.", way = way });
                        }
                }
               // if (CanvasLoginId != "")
                //{
                    Log.Info("Set Canvas Login Mail to Manager Info Object.");
                User userman = new UserBL().getUsersById(Manager.Id);
                if (userman != null)
                {
                    Manager.CanvasLoginId = userman.CanvasLoginId;
                    Manager.Email = userman.Email;
                }
                //}

                Manager.Password = StringCipher.Encrypt(Manager.Password, "zuptu");

                if (Manager.Role == null)
                    Manager.Role = 2;

                if (Request.Form["isSMS"] != null)
                {
                    Manager.isSMS = 1;
                }
                if (Request.Form["isMail"] != null)
                {
                    Manager.isMail = 1;
                }
                if (Request.Form["isSlack"] != null)
                {
                    Manager.isSlack = 1;
                }

                //Image
                Directory.CreateDirectory(Server.MapPath("~") + "Content\\Images\\Managers\\" + logedinuser.Id);
                string ext = null;
                var fileName = "";
                string path = "Content\\Images\\Managers\\" + logedinuser.Id;

                var file = Request.Files[0];
                fileName = Path.GetFileName(file.FileName);
                ext = Path.GetExtension(fileName);

                BlobManager BlobManagerObj = new BlobManager();
                if (file != null && file.ContentLength > 0)
                {
                    if (ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".jpeg"))
                    {
                        path = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);

                        string FileAbsoluteUri = BlobManagerObj.UploadFile(file, path);

                        string userPicture = string.Empty;
                        try
                        {
                            userPicture = FileAbsoluteUri.Split('/').Last();
                            if (userPicture.Contains('?'))
                            {
                                userPicture = userPicture.Split('?').First();
                            }
                        }
                        catch (Exception e)
                        {
                            userPicture = string.Empty;
                        }


                        Manager.Image = userPicture != string.Empty ? BlobManager.GetBlobSasUri(userPicture, userman.CompanyID.ToString()) : "";
                      //  Manager.Image = FileAbsoluteUri;
                    }
                    else
                    {
                        if (way == "Structure")
                        {
                            return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(Manager.Id), division = isDivision, message = "Profile picture not updated successfully. Please select an image file to upload.", way = way });
                        }
                        else
                        {
                            return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(Manager.Id), message = "Profile picture not updated successfully. Please select an image file to upload.", way = way });
                        }
                    }

                }
                else
                {
                    if (RemoveFile == "1")
                    {
                        Manager.Image = null;

                    }
                    else
                    {
                        Manager.Image = Manager.Image;

                    }
                }
                if (Convert.ToInt32(logedinuser.Role) == 2 || Convert.ToInt32(logedinuser.Role) == 4)
                {
                    //  logedinuser.Image = Manager.Image;
                }
                Log.Info("Canvas Login Mail updating Manager's info to Database: " + Manager.CanvasLoginId);

                List<User_Worktype> wtlist = new User_WorktypeBL().getUser_WorktypesList().Where(x => x.UserId == Manager.Id).ToList();
                foreach (User_Worktype w in wtlist)
                {
                    //DatabaseEntities dc = new DatabaseEntities();
                    using (DatabaseEntities db = new DatabaseEntities())
                    {
                        db.User_Worktype.Remove(db.User_Worktype.FirstOrDefault(x => x.Id == w.Id));
                        db.SaveChanges();
                    }
                }

                if (Worktypes != null)
                {
                    foreach (int wt in Worktypes)
                    {
                        User_Worktype user_wt = new User_Worktype()
                        {
                            UserId = Manager.Id,
                            IsActive = 1,
                            WorktypeId = wt,


                        };
                        new User_WorktypeBL().AddUser_Worktypes(user_wt);
                    }
                }

                DatabaseEntities dc = new DatabaseEntities();
                User mainuser = new UserBL().getUsersById(Manager.Id, dc);
                mainuser.FirstName = Manager.FirstName;
                mainuser.LastName = Manager.LastName;
                mainuser.Password = Manager.Password;
                mainuser.PhoneNumber = Manager.PhoneNumber;
                mainuser.HomeNumber = Manager.HomeNumber;
                mainuser.SlackAddress = Manager.SlackAddress;
                mainuser.Notes = Manager.Notes;
                mainuser.isMail = Manager.isMail;
                mainuser.isSMS = Manager.isSMS;
                mainuser.isSlack = Manager.isSlack;
                mainuser.Image = Manager.Image;
                mainuser.DivisionId = Manager.DivisionId;
                mainuser.OutlookToken = userman.OutlookToken;

                new UserBL().UpdateUsers(mainuser, dc);


                if (isDivision != "")
                {
                    if (way == "Structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = "User updated successfully" });
                    }
                    else
                    {
                        return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(Manager.DivisionId.Value), message = "Manager has been updated.", page1 = "Division" });
                    }
                }
                if (logedinuser.Id == Manager.Id)
                {
                    return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(logedinuser.Id), message = "Your account has been updated successfully.", way = way });
                }

                if (logedinuser.Role == 1 || logedinuser.IsMasterAdmin == 1)
                {
                    return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(Manager.Id), message = "Manager's account has been updated successfully.", way = way });
                }
                else
                    return RedirectToAction("Index", "Auth");

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult AddManagerAction(User manager, string dId = null, string CanvasLoginId = "",string DivisionID="", List<int> Worktypes=null, string way ="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                Log.Info("Manger's Canvas Login Mail while adding Manager to DataBase: " + CanvasLoginId);

                // manager.Image = null;
                manager.IsActive = 2;
                manager.CompanyID = Convert.ToInt32(logedinuser.Company);
                manager.Password = "--";
                manager.Role = 2;
                if (CanvasLoginId != "")
                {
                    manager.CanvasLoginId = CanvasLoginId;
                }
                if (dId != null && dId != "")
                {
                    manager.DivisionId = General_Purpose.DecryptId(dId);

                    Log.Info("Manager's Canvas Login Mail before adding into Database: " + manager.CanvasLoginId);

                    new UserBL().AddUsers(manager);
                    new MainMailClass().inviteUser(manager.Email, manager.Id);
                    return RedirectToAction("displayDivisionDetails", new { sid = dId, message = "An email has been sent to manager's email. When the manager will confirm the email then user will be added in the system." });
                }
                else
                {
                    if (DivisionID == "")
                    {
                        if(way == "structure")
                        {
                            return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Divisioin name must be selected." });
                        }
                        else
                        {
                            return RedirectToAction("DisplayManager", new { message = "Divisioin name must be selected." });
                        }
                    }

                    if (way == "structure")
                    {
                        manager.DivisionId = Convert.ToInt32(DivisionID);
                    }
                    else
                    {
                        manager.DivisionId = General_Purpose.DecryptId(DivisionID);
                    }

                    Log.Info("Manager's Canvas Login Mail before adding into Database: " + manager.CanvasLoginId);
                    
                    new UserBL().AddUsers(manager);

                    if (Worktypes != null)
                    {
                        foreach (int wt in Worktypes)
                        {
                            User_Worktype user_wt = new User_Worktype()
                            {
                                UserId = manager.Id,
                                IsActive = 1,
                                WorktypeId = wt,


                            };
                            new User_WorktypeBL().AddUser_Worktypes(user_wt);
                        }
                    }

                    new MainMailClass().inviteUser(manager.Email, manager.Id);

                    if (way == "structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = "An email has been sent to manager's mail. When the manager will confirm the email then user will be added in the system." });
                    }
                    else
                    {
                        return RedirectToAction("DisplayManager", new { message = "An email has been sent to manager's mail. When the manager will confirm the email then user will be added in the system." });
                    }

                }
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        #region Add Users Through CSV

        private static Row GetRow(Worksheet worksheet, uint rowIndex)
        {
            return worksheet.GetFirstChild<SheetData>().
              Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }

        private static Cell GetCell(Worksheet worksheet, string columnName, uint rowIndex)
        {
            try
            {
                Row row = GetRow(worksheet, rowIndex);

                if (row == null)
                    return null;

                return row.Elements<Cell>().Where(c => string.Compare
                       (c.CellReference.Value, columnName +
                       rowIndex, true) == 0).First();
            }
            catch
            {
                return null;
            }
        }

        public ActionResult PostAddManagerFromExcel(HttpPostedFileBase File, string DivisionID = "", string Page = "")
        {
            try
            {
                if (DivisionID == "")
                {
                    return RedirectToAction("Error");
                }

                if (Page == "Structure")
                {
                    DivisionID = General_Purpose.Encrypt(Convert.ToInt32(DivisionID));
                }

                if (File == null)
                {
                    if (Page == "list")
                    {
                        return RedirectToAction("DisplayManager", new { message = "Empty field not allowed, Please Select an Excel File." });
                    }
                    else if (Page == "Structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Empty field not allowed, Please Select an Excel File." });
                    }
                    else
                    {
                        return RedirectToAction("displayDivisionDetails", new { sid = DivisionID, message = "Empty field not allowed, Please Select an Excel File." });
                    }
                    //return RedirectToAction("Users/AddManager", new { message = "Please Select an Excel File", divisionId = DivisionID, page = Page });
                }

                if (File.ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(File.FileName);
                    if (extension != ".xlsx")
                    {
                        if (Page == "list")
                        {
                            return RedirectToAction("DisplayManager", new { message = "Only .xlsx Format Allowed." });
                        }
                        else if (Page == "Structure")
                        {
                            return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Only .xlsx Format Allowed." });
                        }
                        else
                        {
                            return RedirectToAction("displayDivisionDetails", new { sid = DivisionID, message = "Only .xlsx Format Allowed." });
                        }
                        //return RedirectToAction("Users/AddManager", new { message = "Only .xlsx Format Allowed", divisionId = DivisionID, page = Page });
                    }

                    string path = "Uploaded_Managers" + DateTime.Now.Ticks.ToString() + extension;
                    path = Server.MapPath("~") + "/Content/Backup/" + path;
                    File.SaveAs(path);

                    string check = AddManagerViaExcel(path, DivisionID);

                    if (Page == "list")
                    {
                        return RedirectToAction("DisplayManager", new { message = check });
                    }
                    else if (Page == "Structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = check });
                    }
                    else
                    {
                        return RedirectToAction("displayDivisionDetails", new { sid = DivisionID, message = check });
                    }
                }
                else
                {
                    if (Page == "list")
                    {
                        return RedirectToAction("DisplayManager", new { message = "Empty field not allowed, Please Select an Excel File." });
                    }
                    else if (Page == "Structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Empty field not allowed, Please Select an Excel File" });
                    }
                    else
                    {
                        return RedirectToAction("displayDivisionDetails", new { sid = DivisionID, message = "Empty field not allowed, Please Select an Excel File." });
                    }
                    //return RedirectToAction("Users/AddManager", "Utilities", new { message = "Please Select an Excel File", divisionId = DivisionID, page = Page });
                }
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        //row wise insertion
        public string AddManagerViaExcel(string path, string DivisionID)
        {
            try
            {
                string errorMsg = "";
                //List<string> errorMsg = new List<string>();
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false))
                    {
                        WorkbookPart workbookPart = doc.WorkbookPart;
                        SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                        SharedStringTable sst = sstpart.SharedStringTable;
                        WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                        Worksheet sheet = worksheetPart.Worksheet;
                        var cells = sheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Cell>();
                        var rows = sheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>();
                        Console.WriteLine("Row count = {0}", rows.LongCount());
                        Console.WriteLine("Cell count = {0}", cells.LongCount());
                        int rcount = rows.Count();
                        int ccount = cells.Count();
                        

                        int i = 0;

                        char cellref = 'A';
                        int cellcount = 2;

                        foreach (Row row in rows)
                        {
                            User u = new User();

                            if (i == 9999)
                            {
                                break;
                            }


                            if (i > 0)
                            {
                                for (int j = 0; j <= 6; j++)
                                {
                                    string str;
                                    if (j == 7)
                                    {
                                        break;
                                    }

                                    Cell cell = GetCell(sheet, cellref.ToString(), (uint)cellcount);
                                    if (cell != null)
                                    {
                                        if (cell.InnerText == "")
                                        {
                                            str = null;
                                        }
                                        else
                                        {
                                            float f;

                                            if (cell.DataType != null)
                                            {
                                                if (cell.DataType == CellValues.SharedString)
                                                {
                                                    int ssid = Convert.ToInt32(cell.CellValue.Text);
                                                    str = sst.ChildElements[ssid].InnerText;
                                                }
                                                else
                                                {
                                                    str = cell.CellValue.Text;
                                                }
                                            }
                                            else if (float.TryParse(cell.InnerText, out f))
                                            {
                                                str = cell.InnerText;
                                            }
                                            else
                                            {
                                                str = null;
                                            }


                                        }
                                    }
                                    else
                                    {
                                        str = null;
                                    }

                                    switch (j)
                                    {
                                        case 0:
                                            u.FirstName = str;
                                            break;
                                        case 1:
                                            u.LastName = str;
                                            break;
                                        case 2:
                                            u.Email = str;
                                            break;
                                        case 3:
                                            u.PhoneNumber = str;
                                            break;
                                        case 4:
                                            u.HomeNumber = str;
                                            break;
                                        case 5:
                                            u.SlackAddress = str;
                                            break;
                                        case 6:
                                            u.Notes = str;
                                            break;
                                        
                                    }

                                    cellref++;

                                }



                                User obj = new User()
                                {
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    Email = u.Email,
                                    PhoneNumber = u.PhoneNumber,
                                    HomeNumber = u.HomeNumber,
                                    SlackAddress = u.SlackAddress,
                                    Notes = u.Notes,
                                    Password = "--",
                                    IsActive = 2,
                                    CompanyID = Convert.ToInt32(logedinuser.Company),
                                    DivisionId = General_Purpose.DecryptId(DivisionID),
                                    Role = 2
                                };




                                if (obj.FirstName != null && obj.LastName != null && obj.Email != null && obj.PhoneNumber != null)
                                {
                                    bool chkEmail = Regex.IsMatch(obj.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                                    if (chkEmail)
                                    {
                                        User chk = new UserBL().getAllUsersList().Where(x => x.Email.ToLower() == obj.Email.ToLower() && (x.IsActive == 1 || x.IsActive == 2)).FirstOrDefault();
                                        if (chk == null)
                                        {
                                            new UserBL().AddUsers(obj);
                                            new MainMailClass().inviteUser(obj.Email, obj.Id);
                                        }
                                        else
                                        {
                                            int rowcount = i + 1;
                                            errorMsg += obj.Email + " Already Exist in System At Row " + rowcount + "/ ";
                                        }
                                    }
                                    else
                                    {
                                        int rowcount = i + 1;
                                        errorMsg += obj.Email + ", This Email Has Invalid Format At Row" + rowcount + "/ ";
                                    }
                                }
                                else
                                {
                                    int rowcount = i+1;
                                    if (rows.Count() != rowcount)
                                    {
                                        errorMsg += "Required Field is Empty At Row " + rowcount + "/ ";
                                    }
                                    //errorMsg.Add("Required Field is Empty At Row " + rowcount);
                                }

                                cellref = 'A';
                                cellcount++;
                            }



                            i++;
                        }
                    }
                }

                if (errorMsg== "")
                {
                    return "Uers Inserted Successfully";
                }
                else
                {
                    
                    //MemoryStream ms = new MemoryStream();
                    //TextWriter tw = new StreamWriter(ms);
                    //tw.WriteLine("Following Errors Occurred While Inserting Record");
                    //tw.WriteLine(" ");
                    //foreach (string item in errorMsg)
                    //{
                    //    tw.WriteLine(item);
                    //}
                    //tw.Flush();
                    //byte[] bytes = ms.ToArray();
                    //ms.Close();

                    //Response.Clear();
                    //Response.ContentType = "application/force-download";
                    //Response.AddHeader("content-disposition", "attachment;    filename=Error_Message.txt");
                    //Response.BinaryWrite(bytes);
                    //Response.End();


                    
                    //ExportReportController ec = new ExportReportController();
                    //ec.ExportAddUserErrorMsg(errorMsg);
                    //string path2 = Server.MapPath("~") + "\\Content\\Backup\\Error_Report (" + DateTime.Now.ToString("MM-dd-yyyy") + ").xlsx";

                    //ExcelManagement.GenerateAddUserErrorExcel(path2, errorMsg);

                    //return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Weekly EntryTime Report (" + DateTime.Now.ToString("MM-dd-yyyy") + ").xlsx");

                    //ExcelManagement.GenerateAddUserErrorExcel();
                    //return "Record updated with some errors. Please check Error Report";
                    return errorMsg;
                }
            }
            catch
            {
                return "Somethings' Wrong";
            }
        }



        
        #endregion

        public ActionResult sendMailInviteManager(int id, string em, string fn, bool isUtilityPage = false, string sid = null, string way="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                new MainMailClass().inviteUser(em, id);

                if (isUtilityPage)
                    return RedirectToAction("DisplayManager", new { message = "An inivitation has been sent again." });
                else if (sid != null)
                {
                    return RedirectToAction("displayDivisionDetails", new { sid = sid, message = "An inivitation has been sent again." });
                }
                else if(way == "structure")
                {
                    return RedirectToAction("ViewStructure", "OrgStructure", new { message = "An inivitation has been sent again." });
                }


                return RedirectToAction("UpdateManager", new { id = id });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        #endregion Manager

        #region Employee

        public ActionResult addEmployee(string departmentId = null, string way = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                int adminid = logedinuser.Role;
                List<Department> department;
                if (departmentId != null)
                {
                    ViewBag.dep = General_Purpose.DecryptId(departmentId);
                }
                else
                    ViewBag.dep = null;
                if (adminid == (int)Enums.Role.Admin)
                {
                    ViewBag.department = department = new DepartmentBL().getDepartmentsList();
                    ViewBag.Id = departmentId;
                    ViewBag.way = way;
                    return View("Users/AddEmployee");
                }
                else if (adminid == (int)Enums.Role.Manager || adminid == (int)Enums.Role.Cordinator)
                {
                    int id = logedinuser.Id;
                    User manager = new UserBL().getUsersList().Where(mg => (mg.Role == 2 || mg.Role == 4) && mg.Id == id).FirstOrDefault();
                    department = new DepartmentBL().getDepartmentsList();

                    if (manager.DivisionId != null)
                        department = department.Where(x => x.DivisionID == manager.DivisionId).ToList();


                    ViewBag.Id = departmentId;
                    ViewBag.department = department;
                    ViewBag.way = way;
                    return View("Users/AddEmployee");
                }
                return Content("UnAuthorized Action");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult DisplayEmployee(string fname = "", string lname = "", string departname = "", string divisionName = "", string message = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                if (departname == "")
                {
                    departname = "-1";
                }
               
                List<Department> departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();
                List<User> trainees = new List<User>();
                int role = logedinuser.Role;

                if (role == (int)Enums.Role.Admin)
                {
                    trainees = new UserBL().getUsersList().Where(x => x.Role == 3).OrderBy(s => s.FirstName).ToList();
                }
                else if (role == (int)Enums.Role.Manager || role == (int)Enums.Role.Cordinator)
                {
                    int id = logedinuser.Id;
                    User manager = new UserBL().getUsersById(id, new DatabaseEntities());
                    if (manager.DivisionId.HasValue)
                    {
                        int managerDivisionId = manager.DivisionId.Value;

                        List<User> divisiontrainees = new List<User>();

                        foreach (Department d in manager.Division.Departments)
                        {
                            divisiontrainees = new UserBL().getActiveandInvitedUser().Where(x => x.Role == 3 && x.DepartmentId == d.Id && (x.IsActive == 1 || x.IsActive == 2)).OrderBy(s => s.FirstName).ToList();

                            foreach (User t in divisiontrainees)
                            {
                                trainees.Add(t);
                            }
                        }
                    }
                    if (manager.DivisionId != null)
                        departments = departments.Where(x => x.DivisionID == manager.DivisionId).OrderBy(s => s.Name).ToList();
                }

                if (fname != "")
                {
                    trainees = trainees.Where(x => x.FirstName.ToUpper().Contains(fname.ToUpper())).OrderBy(s => s.FirstName).ToList();
                }
                if (lname != "")
                {
                    trainees = trainees.Where(x => x.LastName.ToUpper().Contains(lname.ToUpper())).OrderBy(s => s.FirstName).ToList();
                }
                if (divisionName != "")
                {
                    trainees = trainees.Where(x => x.Department != null && (x.Department.Division.Name.ToUpper().Contains(divisionName.ToUpper()))).OrderBy(s => s.FirstName).ToList();
                }
                if(departname!= "AllEmpkck")
                if (departname != "")
                {
                    trainees = trainees.Where(x => x.Department != null && (x.Department.Name.ToUpper().Contains(departname.ToUpper()))).OrderBy(s => s.FirstName).ToList();
                }

              //  List<Course> canvascourses = General_Purpose.GetCanvasCourse();
                User luser = new UserBL().getUsersById(logedinuser.Id);
                ViewBag.managers = new UserBL().getUsersList().Where(x => x.DivisionId == luser.DivisionId || x.Role == 1 && x.IsActive == 1).ToList();
               // ViewBag.managers = new UserBL().getUsersList().Where(x => x.Role == 2 || x.Role == 4 || x.Role == 1).OrderBy(s => s.FirstName).ToList();
                ViewBag.fname = fname;
                ViewBag.lname = lname;
                ViewBag.departname = departname;
                ViewBag.divisionName = divisionName;
                ViewBag.Employees = trainees;
                ViewBag.departmentList = departments;
                ViewBag.message = message;
              //  ViewBag.canvascourses = canvascourses;
                ViewBag.Divisionlist = new DivisionBL().getDivisionsList().Where(x => x.IsActive == 1 && x.CompanyID == Convert.ToInt32(logedinuser.Company));


                return View("Users/DisplayEmployee");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult AddEmployeeAction(User employee, int depId = -1, string CanvasLoginId = "",List<int> Worktypes=null, string way="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                employee.IsActive = 2;
                //employee.ManagerID = logedinuser.Id;
                employee.Password = "--";
                employee.isMail = 1;
                employee.isSlack = 1;
                employee.isSMS = 1;
                employee.DepartmentId = depId;
                employee.Role = 3;
                employee.CompanyID = Convert.ToInt32(logedinuser.Company);

                Log.Info("Canvas Login Mail while adding an Employee :" + CanvasLoginId);

                if (CanvasLoginId != "")
                {
                    employee.CanvasLoginId = CanvasLoginId;
                }
                new UserBL().AddUsers(employee);
                if (Worktypes != null)
                {
                    foreach (int wt in Worktypes)
                    {
                        User_Worktype user_wt = new User_Worktype()
                        {
                            UserId = employee.Id,
                            IsActive = 1,
                            WorktypeId = wt,


                        };
                        new User_WorktypeBL().AddUser_Worktypes(user_wt);
                    }
                }
                new MainMailClass().inviteUser(employee.Email, employee.Id);
                //if (logedinuser.Role != 2 && logedinuser.Role != 4)
                //{
                if (depId != -1)
                {
                    if (way == "Structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Employee invitation email has been sent." });
                    }
                    else
                    {
                        return RedirectToAction("DisplayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(depId), message = "Employee invitation email has been sent.", way = way });
                    }
                }
                //}
                return RedirectToAction("DisplayEmployee", "Utilities", new { message = "An employee has been added." });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        #region Add User Through CSV
        public ActionResult PostAddEmployeeFromExcel(HttpPostedFileBase File, int depId = -1, string way = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                if (depId == -1)
                {
                    return RedirectToAction("Error");
                }
                if (File == null)
                {
                    if (way == "Manager")
                    {
                        return RedirectToAction("DisplayEmployee", "Utilities", new { message = "Empty field not allowed, Please Select an Excel File." });
                    }
                    else if (way == "Structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Empty field not allowed, Please Select an Excel File." });
                    }
                    else
                    {
                        return RedirectToAction("DisplayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(depId), message = "Empty field not allowed, Please Select an Excel File.", way = way });
                    }
                }

                if (File.ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(File.FileName);
                    if (extension != ".xlsx")
                    {
                        if (way == "Manager")
                        {
                            return RedirectToAction("DisplayEmployee", "Utilities", new { message = "Only .xlsx Format Allowed." });
                        }
                        else if (way == "Structure")
                        {
                            return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Only .xlsx Format Allowed." });
                        }
                        else
                        {
                            return RedirectToAction("DisplayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(depId), message = "Only .xlsx Format Allowed.", way = way });
                        }
                    }
                    string path = "Uploaded_Managers" + DateTime.Now.Ticks.ToString() + extension;
                    path = Server.MapPath("~") + "/Content/Backup/" + path;
                    File.SaveAs(path);

                    string msg = AddEmployeeViaExcel(path, depId);

                    if (way == "Manager")
                    {
                        return RedirectToAction("DisplayEmployee", "Utilities", new { message = msg });
                    }
                    else if (way == "Structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = msg });
                    }
                    else
                    {
                        return RedirectToAction("DisplayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(depId), message = msg, way = way });
                    }
                }
                else
                {
                    if (way == "Manager")
                    {
                        return RedirectToAction("DisplayEmployee", "Utilities", new { message = "Empty field not allowed, Please Select an Excel File." });
                    }
                    else if (way == "Structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Empty field not allowed, Please Select an Excel File." });
                    }
                    else
                    {
                        return RedirectToAction("DisplayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(depId), message = "Empty field not allowed, Please Select an Excel File.", way = way });
                    }
                }
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        //row wise insertion
        public string AddEmployeeViaExcel(string path, int depId)
        {
            try
            {
                string errorMsg = "";
                //List<string> errorMsg = new List<string>();
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false))
                    {
                        WorkbookPart workbookPart = doc.WorkbookPart;
                        SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                        SharedStringTable sst = sstpart.SharedStringTable;
                        WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                        Worksheet sheet = worksheetPart.Worksheet;
                        var cells = sheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Cell>();
                        var rows = sheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>();
                        Console.WriteLine("Row count = {0}", rows.LongCount());
                        Console.WriteLine("Cell count = {0}", cells.LongCount());
                        


                        int i = 0;

                        char cellref = 'A';
                        int cellcount = 2;

                        foreach (Row row in rows)
                        {
                            User u = new User();

                            if (i == 9999)
                            {
                                break;
                            }


                            if (i > 0)
                            {
                                for (int j = 0; j <= 6; j++)
                                {
                                    string str;
                                    if (j == 7)
                                    {
                                        break;
                                    }

                                    Cell cell = GetCell(sheet, cellref.ToString(), (uint)cellcount);
                                    if (cell != null)
                                    {
                                        if (cell.InnerText == "")
                                        {
                                            str = null;
                                        }
                                        else
                                        {
                                            float f;

                                            if (cell.DataType != null)
                                            {
                                                if (cell.DataType == CellValues.SharedString)
                                                {
                                                    int ssid = Convert.ToInt32(cell.CellValue.Text);
                                                    str = sst.ChildElements[ssid].InnerText;
                                                }
                                                else
                                                {
                                                    str = cell.CellValue.Text;
                                                }
                                            }
                                            else if (float.TryParse(cell.InnerText, out f))
                                            {
                                                str = cell.InnerText;
                                            }
                                            else
                                            {
                                                str = null;
                                            }


                                        }
                                    }
                                    else
                                    {
                                        str = null;
                                    }

                                    switch (j)
                                    {
                                        case 0:
                                            u.FirstName = str;
                                            break;
                                        case 1:
                                            u.LastName = str;
                                            break;
                                        case 2:
                                            u.Email = str;
                                            break;
                                        case 3:
                                            u.PhoneNumber = str;
                                            break;
                                        case 4:
                                            u.HomeNumber = str;
                                            break;
                                        case 5:
                                            u.SlackAddress = str;
                                            break;
                                        case 6:
                                            u.Notes = str;
                                            break;

                                    }

                                    cellref++;

                                }



                                User obj = new User()
                                {
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    Email = u.Email,
                                    PhoneNumber = u.PhoneNumber,
                                    HomeNumber = u.HomeNumber,
                                    SlackAddress = u.SlackAddress,
                                    Notes = u.Notes,
                                    Password = "--",
                                    IsActive = 2,
                                    isMail = 1,
                                    isSlack = 1,
                                    isSMS = 1,
                                    DepartmentId = depId,
                                    Role = 3,
                                    CompanyID = Convert.ToInt32(logedinuser.Company)
                            };




                                if (obj.FirstName != null && obj.LastName != null && obj.Email != null && obj.PhoneNumber != null)
                                {
                                    bool chkEmail = Regex.IsMatch(obj.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                                    if (chkEmail)
                                    {
                                        User chk = new UserBL().getAllUsersList().Where(x => x.Email.ToLower() == obj.Email.ToLower() && (x.IsActive == 1 || x.IsActive == 2)).FirstOrDefault();
                                        if (chk == null)
                                        {
                                            new UserBL().AddUsers(obj);
                                            new MainMailClass().inviteUser(obj.Email, obj.Id);
                                        }
                                        else
                                        {
                                            int rowcount = i + 1;
                                            errorMsg += obj.Email + " Already Exist in System At Row " + rowcount + "/ ";
                                        }
                                    }
                                    else
                                    {
                                        int rowcount = i + 1;
                                        
                                        errorMsg += obj.Email + ", This Email Has Invalid Format At Row " + rowcount + "/ ";
                                    }
                                }
                                else
                                {
                                    int rowcount = i + 1;
                                    errorMsg += "Required Field is Empty At Row " + rowcount + "/ ";
                                    //errorMsg.Add("Required Field is Empty At Row " + rowcount);
                                }

                                cellref = 'A';
                                cellcount++;
                            }



                            i++;
                        }
                    }
                }

                if (errorMsg == "")
                {
                    return "Uers Inserted Successfully";
                }
                else
                {
                    return errorMsg;
                }
            }
            catch
            {
                return "Somethings' Wrong";
            }
        }
        #endregion

        public ActionResult UpdateEmployee(string sid, string department = null, string message = "", string way = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                Log.Info("Update Employee Page...");
                ViewBag.dep = department;
                int tem = General_Purpose.DecryptId(sid);


                int did = -1;
                if (tem == 0)
                {
                    sid = HttpUtility.UrlEncode(sid);
                    did = General_Purpose.DecryptId(sid);
                }
                else
                {
                    did = General_Purpose.DecryptId(sid);

                }

                User trainee = new UserBL().getUsersById(did);

                ViewBag.UserWorktypes = new User_WorktypeBL().getUser_WorktypesList().Where(x => x.UserId == trainee.Id).ToList();
                List<Worktype> wt = new WorktypeBL().getWorktypesList().Where(x => x.DivisionID == trainee.Department.DivisionID).ToList(); ;
                ViewBag.Worktype = wt;
                trainee.Password = StringCipher.Decrypt(trainee.Password, "zuptu");
                ViewBag.message = message;
                ViewBag.depId = trainee.DepartmentId;
                ViewBag.way = way;
                ViewBag.sid = sid;
                ViewBag.department = department;

                Log.Info("Employees's Canvas Login Mail Retreived from Database: " + trainee.CanvasLoginId);

                return View("Users/UpdateEmployee", trainee);
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult GetUpdateEmployeeTask(string sid,string Archived="")
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

                int did = -1;
               int tem= General_Purpose.DecryptId(sid);
                if (tem == 0)
                {
                    sid = HttpUtility.UrlEncode(sid);
                    did = General_Purpose.DecryptId(sid);
                    //User trainee = new UserBL().getUsersById(did);
                }
                else
                {
                    did = General_Purpose.DecryptId(sid);

                }
                User trainee = new User();
                //int uId = General_Purpose.Decrypt(HttpUtility.UrlDecode(sid));
                if (Archived != "")
                {
                     trainee = new UserBL().getInActiveUserById(did);
                }
                else
                {
                     trainee = new UserBL().getUsersById(did);
                }
                List<TrainingTracker.Models.User_Task> tasks = new List<TrainingTracker.Models.User_Task>();
                if (Archived != "")
                {
                    tasks = trainee.User_Task1.Where(x => x.IsActive == 0 && x.IsPrivate == 0).ToList();

                }
                else
                {
                    tasks = trainee.User_Task1.Where(x => x.IsActive == 1 && x.IsPrivate == 0).ToList();

                }


                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                //List<Department> departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();
                // List<User> managerlist = new UserBL().getActiveandInvitedUser().Where(x => x.CompanyID == (int)logedinuser.Company && (x.Role == 2 || x.Role == 4) && (x.IsActive == 1 || x.IsActive == 2)).OrderBy(s => s.FirstName).ToList();


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

                List<EmployeeDTO> mnglist = new List<EmployeeDTO>();

                string priority = "";
                string status = "";
                string AssignedBy = "";
                int temp = 0;

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
                            priority = "<span class='label label-warning'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                        }
                        else if (General_Purpose.getPriorityValue(x.Priority.Value) == "Low")
                        {
                            priority = "<span class='label label-success'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                        }
                        else
                        {
                            priority = "<span class='label label-danger'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                        }

                    }
                    else
                        priority = "";

                    if (x.CreatedID == null)
                    {
                        AssignedBy = "Self Assigned";
                        temp = 1;
                    }
                    else
                    {
                        AssignedBy = x.User.FirstName + " " + x.User.LastName;
                    }

                    EmployeeDTO obj = new EmployeeDTO()
                    {

                        //IsActive=(int)x.IsActive,
                        Task = x.Task.Name,
                        StartDate = x.StartDate.Value.ToString("MM/dd/yyyy"),
                        EndtDate = x.EndDate.Value.ToString("MM/dd/yyyy"),
                        Name = AssignedBy,
                        Priority = priority,
                        WorkStatus = status,
                        IsActive = temp,

                        Id = x.Id,
                        EncryptedId = General_Purpose.EncryptId(x.Id)

                    };
                    mnglist.Add(obj);
                    temp = 0;


                }

                return Json(new { data = mnglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult GetUpdateEmployeeDocuments(string sid, string Archived = "")
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

                int did = -1;
                int tem = General_Purpose.DecryptId(sid);
                if (tem == 0)
                {
                    sid = HttpUtility.UrlEncode(sid);
                    did = General_Purpose.DecryptId(sid);
                }
                else
                {
                    did = General_Purpose.DecryptId(sid);

                }
                User trainee = new User();

                trainee = new UserBL().getUsersById(did);


                List<Certificate> tasks = trainee.Certificates.OrderByDescending(c => c.CreatedAt).Where(ss => ss.IsActive == 1).ToList();





                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];



                int totalrows = tasks.Count();
                //filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    tasks = tasks.Where(x => x.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                int totalrowsafterfilterinig = tasks.Count();
                //sorting

                // pagination
                tasks = tasks.Skip(start).Take(length).ToList();

                List<CertificateDTO> mnglist = new List<CertificateDTO>();


                int count = 1;
                string date = "";
                foreach (Certificate x1 in tasks)
                {
                    if(x1.CreatedAt==null)
                    {
                        date = "";
                    }
                    else
                    {
                        date = x1.CreatedAt.Value.ToString("MM/dd/yyyy");
                    }

                    CertificateDTO obj = new CertificateDTO()
                    {

                        //IsActive=(int)x.IsActive,
                        Id=x1.Id,
                        Name = x1.Name,
                        CreatedAt = date,
                        Sr = count,
                        Path=x1.Path,
                       
                        

                    };
                    mnglist.Add(obj);
                    //  temp = 0;

                    count++;
                }

                return Json(new { data = mnglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult UpdateEmployeeAction(User trainee, List<int> Worktypes, string dep = null, string repeat_password = "", string way = "", string CanvasLoginId = "",string RemoveFile="")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                if (logedinuser.Id == trainee.Id)
                {
                    var input = trainee.Password;
                    //  Regex r = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
                    var hasNumber = new Regex(@"[0-9]+");
                    var hasUpperChar = new Regex(@"[A-Z]+");
                    var hasLowerChar = new Regex(@"[a-z]+");
                    var hasspecialChar = new Regex(@"[$!%*?@/]+");
                    var hasMinimum8Chars = new Regex(@".{8,}");

                    var isValidated = hasNumber.IsMatch(input) && hasUpperChar.IsMatch(input) && hasLowerChar.IsMatch(input) && hasspecialChar.IsMatch(input) && hasMinimum8Chars.IsMatch(input);
                    if (isValidated == false)
                    {
                        if (dep != null && dep != "")
                            return RedirectToAction("DisplayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(trainee.DepartmentId.Value), message = "Password pattern doesn't match.", way = way });
                        else
                            return RedirectToAction("UpdateEmployee", "Utilities", new { sid = General_Purpose.EncryptId(logedinuser.Id), message = "Password pattern doesn't match.", way = way });


                    }
                    Models.User uss = new UserBL().getUsersById(trainee.Id);
                    if (trainee.Password != StringCipher.Decrypt(uss.Password, "zuptu"))
                    {

                        OldPassword olpass = new OldPassword()
                        {
                            UserId = uss.Id,
                            Password = uss.Password,
                            CreatedAt = DateTime.Now,
                            IsActive = 1,
                            CompanyId= uss.CompanyID
                        };
                        new OldPasswordBL().AddOldPassword(olpass);

                        OldPassword p = new OldPasswordBL().getOldPasswordsList().Where(x => StringCipher.Decrypt(x.Password, "zuptu") == trainee.Password && x.UserId == uss.Id).FirstOrDefault();
                        if (p != null)
                        {
                            int days = DateTime.Now.Subtract(Convert.ToDateTime(p.CreatedAt)).Days;

                            if (dep != null && dep != "")
                                return RedirectToAction("DisplayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(trainee.DepartmentId.Value), message = "You cannot use your old password.", way = way });
                            else
                                return RedirectToAction("UpdateEmployee", "Utilities", new { sid = General_Purpose.EncryptId(logedinuser.Id), message = "You cannot use your old password.", way = way });


                        }
                    }


                    if (dep == null)
                        if (trainee.Password != repeat_password)
                        {
                            if (dep != null && dep != "")
                                return RedirectToAction("DisplayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(trainee.DepartmentId.Value), message = "Password and Repeat Password doesn't match.", way = way });
                            else
                                return RedirectToAction("UpdateEmployee", "Utilities", new { sid = General_Purpose.EncryptId(logedinuser.Id), message = "Password and Repeat Password doesn't match.", way = way });
                        }
                }
                Log.Info("UpdateEmployeeAction function starting with Canvas Login mail:" + CanvasLoginId);

                if (Request.Form["isSMS"] != null)
                {
                    trainee.isSMS = 1;
                }
                if (Request.Form["isMail"] != null)
                {
                    trainee.isMail = 1;
                }
                if (Request.Form["isSlack"] != null)
                {
                    trainee.isSlack = 1;
                }
                
                User useremp = new UserBL().getUsersById(trainee.Id);

                if (useremp != null)
                {
                    trainee.CanvasLoginId = useremp.CanvasLoginId;
                    trainee.Email = useremp.Email;
                }

               // }

                //Image
                Directory.CreateDirectory(Server.MapPath("~") + "Content\\Images\\Employees\\" + logedinuser.Id);
                string ext = null;
                var fileName = "";
                string path = "Content\\Images\\Employees\\" + logedinuser.Id;

                var file = Request.Files[0];
                fileName = Path.GetFileName(file.FileName);
                ext = Path.GetExtension(fileName);
                BlobManager BlobManagerObj = new BlobManager();
                if (file != null && file.ContentLength > 0)
                {
                    if (ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".jpeg"))
                    {
                        path = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);

                        string FileAbsoluteUri = BlobManagerObj.UploadFile(file, path);
                        string userPicture = string.Empty;
                        try
                        {
                            userPicture = FileAbsoluteUri.Split('/').Last();
                            if (userPicture.Contains('?'))
                            {
                                userPicture = userPicture.Split('?').First();
                            }
                        }
                        catch (Exception e)
                        {
                            userPicture = string.Empty;
                        }


                        trainee.Image = userPicture != string.Empty ? BlobManager.GetBlobSasUri(userPicture, useremp.CompanyID.ToString()) : "";
                      //  trainee.Image = FileAbsoluteUri;
                    }
                    else
                    {
                        if(way == "Structure")
                        {
                            return RedirectToAction("UpdateEmployee", "Utilities", new { sid = General_Purpose.EncryptId(trainee.Id), department = dep, message = "Profile picture not updated successfully. Please select an image file to upload.", way = way });
                        }
                        else
                        {
                            return RedirectToAction("UpdateEmployee", "Utilities", new { sid = General_Purpose.EncryptId(trainee.Id), message = "Profile picture not updated successfully. Please select an image file to upload.", way = way });
                        }
                    }

                }
                else
                {

                    if (RemoveFile == "1")
                    {
                        trainee.Image = null;

                    }
                    else
                    {
                        trainee.Image = trainee.Image;

                    }
                }


                trainee.Password = StringCipher.Encrypt(trainee.Password, "zuptu");

                Log.Info("Canvas Login Mail before updating Employee's info to Database :" + trainee.CanvasLoginId);

                List<User_Worktype> wtlist = new User_WorktypeBL().getUser_WorktypesList().Where(x => x.UserId == trainee.Id).ToList();
                foreach (User_Worktype w in wtlist)
                {
                    //DatabaseEntities dc = new DatabaseEntities();
                    using (DatabaseEntities db = new DatabaseEntities())
                    {
                        db.User_Worktype.Remove(db.User_Worktype.FirstOrDefault(x => x.Id == w.Id));
                        db.SaveChanges();
                    }
                }

                if (Worktypes != null)
                {
                    foreach (int wt in Worktypes)
                    {
                        User_Worktype user_wt = new User_Worktype()
                        {
                            UserId = trainee.Id,
                            IsActive = 1,
                            WorktypeId = wt,


                        };
                        new User_WorktypeBL().AddUser_Worktypes(user_wt);
                    }
                }


                DatabaseEntities dc = new DatabaseEntities();
                User mainuser = new UserBL().getUsersById(trainee.Id, dc);
                mainuser.FirstName = trainee.FirstName;
                mainuser.LastName = trainee.LastName;
                mainuser.Password = trainee.Password;
                mainuser.PhoneNumber = trainee.PhoneNumber;
                mainuser.HomeNumber = trainee.HomeNumber;
                mainuser.SlackAddress = trainee.SlackAddress;
                mainuser.Notes = trainee.Notes;
                mainuser.isMail = trainee.isMail;
                mainuser.isSMS = trainee.isSMS;
                mainuser.isSlack = trainee.isSlack;
                mainuser.Image = trainee.Image;
                mainuser.OutlookToken = useremp.OutlookToken;

                new UserBL().UpdateUsers(mainuser, dc);


              //  new UserBL().UpdateUsers(trainee);

                if (logedinuser.Role == 2 || logedinuser.Role == 4)
                {
                    return RedirectToAction("UpdateEmployee", "Utilities", new { sid = General_Purpose.EncryptId(trainee.Id), message = "User updated successfully", way = way });

                }
                else if (logedinuser.Role == (int)Enums.Role.Trainee && logedinuser.Id == trainee.Id)
                {

                    return RedirectToAction("UpdateEmployee", "Utilities", new { sid = General_Purpose.EncryptId(logedinuser.Id), message = "Your account has been updated successfully.", way = way });

                }
                else if(logedinuser.Role==1)
                {
                    if (dep != null && dep != "")
                    {
                        if (way == "Structure")
                        {
                            return RedirectToAction("ViewStructure", "OrgStructure", new { message = "User updated successfully" });
                        }
                        else
                        {
                            return RedirectToAction("DisplayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(trainee.DepartmentId.Value), message = "Employee's account has been updated successfully.", way = way });
                        }
                    }
                    else
                        return RedirectToAction("UpdateEmployee", "Utilities", new { sid = General_Purpose.EncryptId(trainee.Id), message = "Employee's account has been updated successfully.", way = way });



                }
                return RedirectToAction("Index", "Auth");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }



        public ActionResult sendMailInviteEmployee(int id, string em, string fn, bool isUtilityPage = false, string sid = null, string way ="")
        {
            try
            {
                new MainMailClass().inviteUser(em, id);
                if (isUtilityPage)
                    return RedirectToAction("DisplayEmployee", new { message = "An inivitation has been sent again." });
                else if (sid != null)
                {
                    return RedirectToAction("displayDepartmentDetails", new { sid = sid, message = "An inivitation has been sent again." });
                }
                else if (way == "structure")
                {
                    return RedirectToAction("ViewStructure", "OrgStructure", new { message = "An inivitation has been sent again." });
                }

                return RedirectToAction("UpdateEmployee", new { id = id, message = "An inivitation has been sent again." });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        #endregion Employee

        #region Admin
        public ActionResult SignUp(string msg = "",string k3y="")
        {
            try
            {
                ViewBag.msg = msg;
                if (k3y == "1/2k%3d")
                {
                    return View();
                }

                return RedirectToAction("login", "Auth");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }

        }


        public ActionResult AddAdmin(string message = "")
        {
            try
            {
                if (logedinuser.Role == 1)
                {
                    ViewBag.message = message;
                    return View();
                }
                else
                {
                    return RedirectToAction("login", "Auth");
                }
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        [HttpPost]
        public ActionResult addAdmin(User admin, string password="", string password2="", string CompanyName="", string admintype = "", string CanvasLoginId = "", string way ="")
        {
            try
            {

              
                if (admin.Email == null)
                {
                    if (admintype == "")
                    {
                        return RedirectToAction("Signup", "Utilities", new { msg = "Email not be null." });
                    }
                    else
                    {
                        if(way == "structure")
                        {
                            return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Email not be null" });
                        }
                        else
                        {
                            return RedirectToAction("AddAdmin", "Utilities", new { message = "Email not be null" });
                        }
                    }

                }
                string _data = admin.Email.Trim();

                if(admintype == "")
                {
                    if (/*password == "" || password2 == "" ||*/ CompanyName == "")
                    {
                        return RedirectToAction("AddAdmin", "Utilities", new { message = "Required fields must be filled." });
                    }
                }
               

                int value = 0;
                // _data = _data.Trim();

                // int value1 = 0;
                value = new CompanyBL().getAllCompanyList().Where(x => x.Name.ToUpper() == CompanyName.ToUpper()).ToList().Count();
                if (value > 0)
                {
                    return RedirectToAction("AddAdmin", "Utilities", new { message = "Company Name Already Exists." });
                }
                if (admintype == "")
                {
                    value = new UserBL().getAllUsersList().Where(x => x.Email.ToUpper() == _data.ToUpper()).ToList().Count();

                }
                else
                {
                    value = new UserBL().getUsersList().Where(x => x.Email == _data).ToList().Count();
                }
                //value = new UserBL().getAllUsersList().Where(x => x.Email.ToUpper() == _data.ToUpper()).ToList().Count();

                if (value > 0)
                {
                    if (admintype == "")
                    {
                        return RedirectToAction("Signup", "Utilities", new { msg = "Email already exists in the system." });

                    }
                    else
                    {
                        if (way == "structure")
                        {
                            return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Email Already Exists." });
                        }
                        else
                        {
                            return RedirectToAction("AddAdmin", "Utilities", new { message = "Email Already Exists." });
                        }


                    }

                }


                var input = password;
                //  Regex r = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasspecialChar = new Regex(@"[$!%*?@/]+");
                var hasMinimum8Chars = new Regex(@".{8,}");

                var isValidated = hasNumber.IsMatch(input) && hasUpperChar.IsMatch(input) && hasLowerChar.IsMatch(input) && hasspecialChar.IsMatch(input) && hasMinimum8Chars.IsMatch(input);
                if (isValidated == false)
                {
                    if (way == "structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Password pattern doesn't match." });
                    }
                    else
                    {
                        return RedirectToAction("AddAdmin", "Utilities", new { message = "Password pattern doesn't match." });

                    }
                }

                if (password != password2)
                {
                    if (way == "structure")
                    {
                        return RedirectToAction("ViewStructure", "OrgStructure", new { message = "Password and Confirm Password doesn't match." });
                    }
                    else
                    {
                        return RedirectToAction("AddAdmin", "Utilities", new { message = "Password and Confirm Password doesn't match." });
                    }
                }
                else
                {
                    User masteradmin = new UserBL().getAllUsersList().Where(x => x.IsActive == 1 && x.IsMasterAdmin == 1 && x.CompanyID == Convert.ToInt32(logedinuser.Company)).FirstOrDefault();

                    if (admintype == "")
                    {
                        admin.Password = "--";
                    }
                    else
                    {
                        admin.Password = StringCipher.Encrypt(password, "zuptu");

                    }
                    

                   
                    admin.isSlack = 1;
                    admin.isSMS = 1;
                    admin.isMail = 1;
                    admin.Role = 1;

                    User loggedInUser = new UserBL().getUsersById(logedinuser.Id);

                    // admin.Image = "~/Content/Images/ig.png";
                    if (CanvasLoginId != "")
                    {
                        admin.CanvasLoginId = CanvasLoginId;
                    }


                    if (admintype == "")
                    {
                        admin.IsMasterAdmin = 1;
                        admin.IsActive = 2;
                        Company company = new Company() { Name = CompanyName, isActive = 1, ManagerNumber = 5, EmployeeNumber = 5 };
                        int id = new CompanyBL().AddCompany(company);
                        if (id == -1)
                        {
                            return Content("Please contact customer support");
                        }

                        admin.CompanyID = id;
                    }
                    else
                    {
                        //admin.IsMasterAdmin = 0;

                        if (loggedInUser.IsPrimary == 1)
                        {
                            if (admin.IsMasterAdmin == null)
                            {
                                admin.IsMasterAdmin = 0;
                            }
                            else
                            {
                                admin.IsMasterAdmin = 1;
                            }
                        }
                        else
                        {
                            admin.IsMasterAdmin = 0;
                        }

                        admin.IsActive = 1;
                        admin.CompanyID = masteradmin.CompanyID;
                    }


                    new UserBL().AddUsers(admin);
                    if (admintype == "")
                    {
                        new MainMailClass().inviteUser(admin.Email, admin.Id,Convert.ToInt32(admin.CompanyID));

                    }

                    if (admin.IsMasterAdmin == 1 && loggedInUser.IsPrimary != 1)
                    {
                        return RedirectToAction("Signup", "Utilities", new { msg = "Verification mail has been sent to your email address." });

                    }
                    else
                    {
                        if (way == "structure")
                        {
                            return RedirectToAction("ViewStructure", "OrgStructure", new { message = "New admin has been added successfully." });
                        }
                        else
                        {
                            return RedirectToAction("DisplayAdmin", "Utilities", new { message = "New admin has been added successfully." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
         }

        public ActionResult DisplayAdmin(string message = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
               
                ViewBag.message = message;
                ViewBag.Divisions = new DivisionBL().getDivisionsList();
                
                return View("Users/DisplayAdmin");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult GetDisplayAdmin()
        {
            try
            {
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                int adminid = logedinuser.Role;

                if (adminid == (int)Enums.Role.Admin)
                {
                    //List<User> AdminList = new UserBL().getUsersList().Where(x => x.Role == 1 && x.IsMasterAdmin == 0).OrderBy(s => s.FirstName).ToList();
                    List<User> AdminList = new UserBL().getUsersList().Where(x => x.Role == 1 && x.IsPrimary == null && x.Id != logedinuser.Id && (x.IsMasterAdmin == 0 || x.IsMasterAdmin == 1)).OrderBy(s => s.FirstName).ToList();

                    int totalrows = AdminList.Count();
                    //filter
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        AdminList = AdminList.Where(x => x.FirstName.ToLower().Contains(searchValue.ToLower())).ToList();
                    }

                    int totalrowsafterfilterinig = AdminList.Count();
                    //sorting

                    // pagination
                    AdminList = AdminList.Skip(start).Take(length).ToList();

                    List<User> userslist = new List<User>();
                    int c = 0;
                    foreach (User x in AdminList)
                    {
                        User obj = new User()
                        {

                            CompanyID = c + 1, // It just shows Srerial Number
                            DepartmentId = logedinuser.IsMasterAdmin,
                            FirstName = x.FirstName + " " + x.LastName,
                            Id = x.Id
                        };
                        userslist.Add(obj);
                        c++;
                    }

                    return Json(new { data = userslist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = 0, draw = Request["draw"], recordsTotal = 0, recordsFiltered = 0 }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public string UpdateCanvasLoginId(string Id, string CanvasMail = "") 
        {

            MainMailClass mail = new MainMailClass();
            bool result = mail.UpdtaeCanvasMail(Id, CanvasMail);
            string x = result.ToString();

            return JsonConvert.SerializeObject(x, Newtonsoft.Json.Formatting.Indented,
             new JsonSerializerSettings()
             {
                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
             });

        }

        public string RemoveCanvasLoginId()
        {

            //MainMailClass mail = new MainMailClass();
            //bool result = mail.UpdtaeCanvasMail(Id, CanvasMail);
            DatabaseEntities de = new DatabaseEntities();
            User u = new UserBL().getUsersById(logedinuser.Id, de);
            u.CanvasLoginId = null;
            bool result=  new UserBL().UpdateUsers(u, de);

            string x = result.ToString();

            return JsonConvert.SerializeObject(x, Newtonsoft.Json.Formatting.Indented,
             new JsonSerializerSettings()
             {
                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
             });

        }

        public ActionResult UpdateCanvasLoginIdAction(string val, string t, string User = "")
        {
            try
            {
                long ticks = Convert.ToInt64(StringCipher.Base64Decode(t));
                DateTime dt = new DateTime(ticks);
                if (dt < DateTime.Now)
                {
                    return RedirectToAction("ExpireLink", "Auth");

                }
                int uId;
                string str = "";
                uId = General_Purpose.DecryptId(User);
                string canvasmail = StringCipher.Base64Decode(val);
                if (uId == 0)
                {
                    str = HttpUtility.UrlEncode(User);
                    uId = General_Purpose.DecryptId(str);
                }
                DatabaseEntities de = new DatabaseEntities();
                User u = new UserBL().getUsersById(uId, de);
                if (u == null)
                {
                    return RedirectToAction("ExpireLink", "Auth");
                }
                else if (u.CanvasLoginId == canvasmail)
                {
                    return RedirectToAction("ExpireLink", "Auth");
                }

                u.CanvasLoginId = canvasmail;
                new UserBL().UpdateUsers(u, de);
                if (u.Role == 3)
                {
                    return RedirectToAction("UpdateEmployee", new { sid = User });
                }
                else if (u.Role == 2 || u.Role == 4)
                {
                    return RedirectToAction("UpdateManager", new { sid = User });
                }


                return RedirectToAction("UpdateAdmin", new { sid = User });



            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        public ActionResult UpdateAdmin(string sid, string message)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                Log.Info("Update Admin Page...");

                int uId = General_Purpose.DecryptId(sid);
                User Admin = new UserBL().getUsersById(uId);
                int adminid = logedinuser.Role;
                ViewBag.message = message;
                ViewBag.Id = sid;
                ViewBag.slackwebhook = Admin.Company.SlackWebhook;
                ViewBag.companyName = new CompanyBL().getCompanyById(Convert.ToInt32(logedinuser.Company)).Name;
                Admin.Password = StringCipher.Decrypt(Admin.Password, "zuptu");
                if (adminid == (int)Enums.Role.Admin)
                {
                    Log.Info("Canvas Login Mail Retreived from Database: " + Admin.CanvasLoginId);

                    return View("Users/UpdateAdmin", Admin);
                }
                return Content("Unauthorised Action");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult UpdateAdminAction(User Admin, string repeat_password, string slackwebhook, string companyName, string CanvasLoginId = "",string RemoveFile="",string RemoveSignature="",HttpPostedFileBase signedFile = null, string SignedImageUrl = "")
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                Log.Info("UpdateAdminActin Function Starting with Canvas Login Mail:" + CanvasLoginId);

                CompanyBL cbl = new CompanyBL();
                DatabaseEntities de = new DatabaseEntities();
                Company c = cbl.getCompanyById(Convert.ToInt32(logedinuser.Company), de);
                User useradmin = new UserBL().getUsersById(Admin.Id);
                Company company = new Company()
                {
                    Id = c.Id,
                    Name = useradmin.Company.Name,
                    Address = c.Address,
                    Phone_Number = c.Phone_Number,
                    isActive = c.isActive,
                    SlackWebhook = slackwebhook,
                    ManagerNumber = c.ManagerNumber,
                    EmployeeNumber = c.EmployeeNumber,
                    IsFavReport = c.IsFavReport,
                    IsDocManager = c.IsDocManager,
                    IsLMS = c.IsLMS,
                    CreatedAt = c.CreatedAt


                };

                // COmpany LOGO
                string path2 = "";
                var file2 = Request.Files["Logo"];
              
                BlobManager BlobManagerObj = new BlobManager();
                if (file2 != null && file2.ContentLength > 0)
                {
                    string fileName2 = Path.GetFileName(file2.FileName);
                    string ext2 = Path.GetExtension(fileName2);
                    if (ext2.ToLower().Equals(".png") || ext2.ToLower().Equals(".jpg") || ext2.ToLower().Equals(".jpeg"))
                    {
                        path2 = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file2.FileName);

                        string FileAbsoluteUri = BlobManagerObj.UploadFile(file2, path2);

                        string userPicture = string.Empty;
                        try
                        {
                            userPicture = FileAbsoluteUri.Split('/').Last();
                            if (userPicture.Contains('?'))
                            {
                                userPicture = userPicture.Split('?').First();
                            }
                        }
                        catch (Exception e)
                        {
                            userPicture = string.Empty;
                        }


                        company.Logo = userPicture;
                    }
                    else
                    {
                        return RedirectToAction("UpdateAdmin", new { sid = General_Purpose.EncryptId(Admin.Id), message = "Company Logo not updated successfully. Please select an image file to upload." });
                    }
                }
                else
                {
                    company.Logo = c.Logo;
                }
               

                cbl.UpdateCompany(company, de);

                var input =Admin.Password;
                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasspecialChar = new Regex(@"[$!%*?@/]+");
                var hasMinimum8Chars = new Regex(@".{8,}");
                bool a = hasNumber.IsMatch(input);
                bool b = hasUpperChar.IsMatch(input);
                bool sc = hasLowerChar.IsMatch(input);
                bool d = hasspecialChar.IsMatch(input);
                bool ee = hasMinimum8Chars.IsMatch(input);
                var isValidated = hasNumber.IsMatch(input) && hasUpperChar.IsMatch(input) && hasLowerChar.IsMatch(input) && hasspecialChar.IsMatch(input) && hasMinimum8Chars.IsMatch(input);
                if (isValidated == false)
                {
                    return RedirectToAction("UpdateAdmin", new { sid = General_Purpose.EncryptId(Admin.Id), message = "Password pattern dosen't match." });

                }
                
                //Models.User uss = new UserBL().getUsersById(Admin.Id);
                if (Admin.Password != StringCipher.Decrypt(useradmin.Password, "zuptu"))
                {

                    OldPassword olpass = new OldPassword()
                    {
                        UserId = useradmin.Id,
                        Password = useradmin.Password,
                        CreatedAt = DateTime.Now,
                        IsActive = 1,
                        CompanyId = useradmin.CompanyID
                    };
                    new OldPasswordBL().AddOldPassword(olpass);

                    OldPassword p = new OldPasswordBL().getOldPasswordsList().Where(x => StringCipher.Decrypt(x.Password, "zuptu") == Admin.Password && x.UserId == useradmin.Id).FirstOrDefault();
                    if (p != null)
                    {
                        int days = DateTime.Now.Subtract(Convert.ToDateTime(p.CreatedAt)).Days;
                        return RedirectToAction("UpdateAdmin", new { sid = General_Purpose.EncryptId(Admin.Id), message = "You cannot use your old password." });


                    }
                }



                if (repeat_password == Admin.Password)
                {
                    Admin.Password = StringCipher.Encrypt(Admin.Password, "zuptu");
                    if (Request.Form["isSMS"] != null)
                    {
                        Admin.isSMS = 1;
                    }
                    if (Request.Form["isMail"] != null)
                    {
                        Admin.isMail = 1;
                    }
                    if (Request.Form["isSlack"] != null)
                    {
                        Admin.isSlack = 1;
                    }

                    Admin.Role = 1;
                    //Canvas Login ID
                    // if (CanvasLoginId != "")
                    //{
                    
                    if (useradmin != null)
                    {
                        Admin.CanvasLoginId =  useradmin.CanvasLoginId;
                        Admin.Email = useradmin.Email;
                    }
                        
                   // }

                    //Image
                    Directory.CreateDirectory(Server.MapPath("~") + "Content\\Images\\Admins\\" + logedinuser.Id);
                    string ext = null;
                    var fileName = "";
                    string path = "Content\\Images\\Admins\\" + logedinuser.Id;

                    var file = Request.Files["File"];
                    fileName = Path.GetFileName(file.FileName);
                    ext = Path.GetExtension(fileName);
                   // BlobManager BlobManagerObj = new BlobManager();
                    if (file != null && file.ContentLength > 0)
                    {
                        if (ext.ToLower().Equals(".png") || ext.ToLower().Equals(".jpg") || ext.ToLower().Equals(".jpeg"))
                        {
                            path = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);

                            string FileAbsoluteUri = BlobManagerObj.UploadFile(file, path);
                            
                            string userPicture = string.Empty;
                            try
                            {
                                userPicture = FileAbsoluteUri.Split('/').Last();
                                if (userPicture.Contains('?'))
                                {
                                    userPicture = userPicture.Split('?').First();
                                }
                            }
                            catch (Exception e)
                            {
                                userPicture = string.Empty;
                            }


                            Admin.Image = userPicture != string.Empty ? BlobManager.GetBlobSasUri(userPicture, c.Id.ToString()) : "";
                        }
                        else
                        {
                            return RedirectToAction("UpdateAdmin", new { sid = General_Purpose.EncryptId(Admin.Id), message = "Profile picture not updated successfully. Please select an image file to upload." });
                        }
                    }
                    else
                    {
                        if (RemoveFile == "1")
                        {
                            Admin.Image = null;

                        }
                        else
                        {
                            Admin.Image = Admin.Image;

                        }
                    }

                    //Signature Image
                    string fileLocalPath = "";
                    string defaultpath = @"~/Content/AllFiles";
                  //  System.IO.Directory.CreateDirectory("/Content/AllFiles");

                    if (SignedImageUrl!="")
                    {
                            byte[] imageBytes = Convert.FromBase64String(SignedImageUrl.Replace("data:image/png;base64,", String.Empty));
                            Image image;


                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                image = Image.FromStream(ms);
                                string fileName3 = logedinuser.Id + "-" + "Signature-" + DateTime.Now.ToString("yymmddfff") + ".png";
                                string path3 = Path.Combine(Server.MapPath("~/Content/AllFiles"), fileName3);
                                fileLocalPath = "/Content/AllFiles/" + fileName3;
                                // file is uploaded
                                image.Save(path3, System.Drawing.Imaging.ImageFormat.Png);
                                // releasing resources
                                image.Dispose();
                            }
                        Admin.SignatureImage = fileLocalPath;

                    }
                    else if(signedFile!=null)
                    {
                        
                            string ext3 = System.IO.Path.GetExtension(System.IO.Path.GetFileName(signedFile.FileName));
                            if (ext3.ToLower() == ".jpg" || ext3.ToLower() == ".jpeg" || ext3.ToLower() == ".png")
                            {
                                string fileName3 = logedinuser.Id + "-" + "Signature-" + DateTime.Now.ToString("yymmddfff") + ext3;
                                string path3 = Path.Combine(Server.MapPath("~/Content/AllFiles"), fileName3);
                                fileLocalPath = "/Content/AllFiles/" + fileName3;
                                // file is uploaded
                                signedFile.SaveAs(path3);
                            }
                            else
                            {
                                return RedirectToAction("UpdateAdmin", new { sid = General_Purpose.EncryptId(Admin.Id), message = "Sign must be an image file!" });

                            }
                        Admin.SignatureImage = fileLocalPath;
                    }

                    if (SignedImageUrl == "" && signedFile==null)
                    {
                        if (string.IsNullOrEmpty(useradmin.SignatureImage))
                        {
                           // if(useradmin.IsPrimary==1 && useradmin.Role==1)
                            //return RedirectToAction("UpdateAdmin", new { sid = General_Purpose.EncryptId(Admin.Id), message = "Signature must be uploaded" });

                        }
                        Admin.SignatureImage = useradmin.SignatureImage;
                    }

                    

                    //   logedinuser.Image = Admin.Image;

                    Log.Info("Canvas Login Mail Before updating Admin's info  to DataBase: " + Admin.CanvasLoginId);
                    DatabaseEntities db = new DatabaseEntities();
                    User mainuser = new UserBL().getUsersById(Admin.Id,db);
                    mainuser.FirstName = Admin.FirstName;
                    mainuser.LastName = Admin.LastName;
                    mainuser.Password = Admin.Password;
                    mainuser.PhoneNumber = Admin.PhoneNumber;
                    mainuser.HomeNumber = Admin.HomeNumber;
                    mainuser.SlackAddress = Admin.SlackAddress;
                    mainuser.Notes = Admin.Notes;
                    mainuser.isMail = Admin.isMail;
                    mainuser.isSMS = Admin.isSMS;
                    mainuser.isSlack = Admin.isSlack;
                    mainuser.Image = Admin.Image;
                    mainuser.OutlookToken = useradmin.OutlookToken;
                    mainuser.SignatureImage = Admin.SignatureImage;

                    new UserBL().UpdateUsers(mainuser,db);
                    return RedirectToAction("UpdateAdmin", new { sid = General_Purpose.EncryptId(Admin.Id), message = "Your account has been updated." });
                }
                else
                {
                    return RedirectToAction("UpdateAdmin", new { sid = General_Purpose.EncryptId(Admin.Id), message = "Password and Repeat Password doesn't match." });
                }

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        #endregion Admin

        public FileResult DownloadPDFReport(string path, string v, string name)
        {
            return File(path, v, name);
        }
        public ActionResult deleteUser(int id, int accessLevel, int divisionId = -1, int deptId = -1, string way = "" )
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                if (accessLevel == (int)Enums.Role.Manager || accessLevel == (int)Enums.Role.Cordinator)
                {

                    new UserBL().DeleteUsers(id);

                    if (deptId == -1 && divisionId == -1)
                    {
                        return RedirectToAction("DisplayManager", new { message = "A manager's account deleted." });

                    }
                    return RedirectToAction("displayDivisionDetails", new { sid = General_Purpose.EncryptId(divisionId), message = "A manager's account deleted." });
                }
                else if (accessLevel == (int)Enums.Role.Admin)
                {
                    new UserBL().DeleteUsers(id);
                    return RedirectToAction("displayAdmin", new { message = "You have deleted the admin." });
                }
                else if (accessLevel == (int)Enums.Role.Trainee)
                {
                    new UserBL().DeleteUsers(id);
                    if (deptId != -1)
                        return RedirectToAction("displayDepartmentDetails", new { sid = General_Purpose.EncryptId(deptId), message = "An employee's account deleted.", way = way });

                    return RedirectToAction("DisplayEmployee", new { message = "You have deleted the Employee" });
                }
                return Content("Un Authorized Action");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        #region AJAXValidations

        [HttpPost]
        public ActionResult GetUserListToChangeOver(int Id, int Way, int UserType = -1)
        {
            User u = new UserBL().getUsersById(Id);

            List<User> userList = new List<User>();

            if (Way == 1)
            {
                userList = new UserBL().getAllActiveUsersList().Where(x => x.Id != u.Id && x.Role == 1 && x.CompanyID == u.CompanyID).ToList();
            }
            else if(Way == 2)
            {
                if(UserType == 1)
                {
                    userList = new UserBL().getAllActiveUsersList().Where(x => x.Id != u.Id && x.Role == 1 && x.CompanyID == u.CompanyID).ToList();
                }
                else
                {
                    userList = new UserBL().getAllActiveUsersList().Where(x => x.Id != u.Id && x.Role == 2 && x.CompanyID == u.CompanyID && x.DivisionId == u.DivisionId).ToList();
                }
            }
            else if(Way == 3)
            {
                if (UserType == 1)
                {
                    userList = new UserBL().getAllActiveUsersList().Where(x => x.Id != u.Id && x.Role == 1 && x.CompanyID == u.CompanyID).ToList();
                }
                else if (UserType == 2)
                {
                    userList = new UserBL().getAllActiveUsersList().Where(x => x.Id != u.Id && x.Role == 2 && x.CompanyID == u.CompanyID && x.DivisionId == u.Department.DivisionID).ToList();
                }
                else
                {
                    userList = new UserBL().getAllActiveUsersList().Where(x => x.Id != u.Id && x.Role == 3 && x.CompanyID == u.CompanyID && x.DepartmentId == u.DepartmentId).ToList();
                }
            }

            List<UserDTO> udto = new List<UserDTO>();

            foreach(User i in userList)
            {
                UserDTO obj = new UserDTO()
                {
                    Id = i.Id,
                    Name = i.FirstName + " " + i.LastName
                };

                udto.Add(obj);
            }

            return Json(udto, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult divisionNameValidation(string _data,string way, int ID=-1)
        {
            int value = 0;
            if (way != "")
            {
                _data = _data.Trim();

                value = Convert.ToInt32((new DivisionBL().getDivisionsList().Where(x => x.Name.ToUpper().Equals(_data.ToUpper())).Count() > 0));

            }
            else
            {
                if (ID != -1)
                {
                    Division div = new DivisionBL().getDivisionsById(ID);
                   
                    _data = _data.Trim();
                    if (div.Name.ToUpper() != _data.ToUpper())
                    {
                        value = Convert.ToInt32((new DivisionBL().getDivisionsList().Where(x => x.Name.ToUpper().Equals(_data.ToUpper())).Count() > 0));
                    }

                        
                }
               
            }

            return Json(value);
        }

        [HttpPost]
        public ActionResult TaskTypeNameValidation(string _data)
        {
            _data = _data.Trim();

            int value = Convert.ToInt32((new TaskTypeBL().getTaskTypesList().Where(x => x.Name.ToUpper().Equals(_data.ToUpper())).Count() > 0));

            return Json(value);
        }
        [HttpPost]
        public ActionResult WorkTypeNameValidation(string _data="", int Id=-1,int DivId=-1)
        {
            _data = _data.Trim();
            int value = 0;
            
            if (Id != -1)
            {
                Worktype wt = new WorktypeBL().getWorktypesById(Id);
                if (wt.Name.ToUpper() == _data.ToUpper() && wt.DivisionID==DivId)
                {

                }
                else
                {
                    value = Convert.ToInt32(new WorktypeBL().getWorktypesList().Where(x => x.Name.ToUpper().Equals(_data.ToUpper()) && x.DivisionID == DivId).Count());

                }
            }
            else
            {
                 value = Convert.ToInt32((new WorktypeBL().getWorktypesList().Where(x => x.Name.ToUpper().Equals(_data.ToUpper()) && x.DivisionID == DivId).Count() > 0));

            }


            return Json(value);
        }

        [HttpPost]
        public ActionResult departmentNameValidation(string _data, int divId,string way,int deptID=-1)
        {
            int value = -1;
            _data = _data.Trim();
            if (way != "")
            {
                

                value = Convert.ToInt32((new DepartmentBL().getDepartmentsList().Where(x => x.DivisionID == divId && x.Name.ToUpper().Equals(_data.ToUpper())).Count() > 0));

            }
            else
            {
                if (deptID != -1)
                {
                    Department dep = new DepartmentBL().getDepartmentsById(deptID);
                    if (dep.Name.ToUpper() != _data.ToUpper())
                    {
                        

                        value = Convert.ToInt32((new DepartmentBL().getDepartmentsList().Where(x => x.DivisionID == divId && x.Name.ToUpper().Equals(_data.ToUpper())).Count() > 0));

                    }
                }
            }

            return Json(value);
        }

        [HttpPost]
        public ActionResult tagNameValidation(string _data, int divId)
        {
            _data = _data.Trim();

            int value = Convert.ToInt32((new TagBL().getTagsList().Where(x => x.DivisionId == divId && x.Name.ToUpper().Equals(_data.ToUpper())).Count() > 0));

            return Json(value);
        }

        [HttpPost]
        public ActionResult groupTaskNameValidation(string _data, int divId, string way, int grptaskID=-1)
        {
            _data = _data.Trim();
            GroupTask gt = new GroupTaskBL().getGroupTasksById(grptaskID);
            
            int value = 0;
            if (way != "")
            {
                if(gt!=null)
                    if(gt.Name.ToLower()!=_data.ToLower())
                 value = Convert.ToInt32((new GroupTaskBL().getGroupTasksList().Where(x => x.User.DivisionId == divId && x.Name.ToUpper().Trim().Equals(_data.ToUpper())).Count() > 0));

            }
            else
            {
                if (grptaskID != -1)
                {
                   // GroupTask grp = new GroupTaskBL().getGroupTasksById(grptaskID);
                    if (gt.Name.ToUpper() != _data.ToUpper())
                    {
                        value = Convert.ToInt32((new GroupTaskBL().getGroupTasksList().Where(x => x.User.DivisionId == divId && x.Name.ToUpper().Trim().Equals(_data.ToUpper())).Count() > 0));

                    }
                }

            }
           
            return Json(value);
        }
        [HttpPost]
        public ActionResult SignUpemailValidation(string _data)
        {
            _data = _data.Trim();

            int value = 0;

            value = new UserBL().getAllUsersList().Where(x => x.Email.ToUpper() == _data.ToUpper() && x.IsActive==1).ToList().Count();
            // value = Convert.ToInt32((new UserBL().getUsersList().Where(x => x.Email.Equals(_data)).Count() > 0));

            return Json(value);
        }

        [HttpPost]
        public ActionResult SignUpCompanyValidation(string _data)
        {
            _data = _data.Trim();

            int value = 0;
            value = new CompanyBL().getAllCompanyList().Where(x => x.Name.ToUpper() == _data.ToUpper() && x.isActive==1).ToList().Count();


            return Json(value);
        }
        public ActionResult emailValidation(string _data)
        {
            _data = _data.Trim();

            int value = 0;
            value = new UserBL().getUsersList().Where(x => x.Email.ToUpper() == _data.ToUpper()).ToList().Count();


            return Json(value);
        }
        public ActionResult ActivateUserValidation(int _data)
        {
            DatabaseEntities de = new DatabaseEntities();
            User u = new UserBL().getDeletedUserById(_data, de);
            

        //    _data = _data.Trim();

            int value = 0;
            if (u != null)
            {
                value = new UserBL().getUsersList().Where(x => x.Email.ToUpper() == u.Email.ToUpper()).ToList().Count();

            }


            return Json(value);
        }
        #endregion AJAXValidations


        #region Async Excel Report

        public FileResult AsyncdownloadBackup(int manager1 = -1, int employee1 = -1, int managerTask1 = -1, int employeeTask1 = -1, int task1 = -1, int groupTask1 = -1)
        {

            bool current = false;
            bool manager = false;
            bool employee = false;
            bool managerTask = false;
            bool employeeTask = false;
            bool task = false;
            bool groupTask = false;


            if (manager1 == 1)
            {
                manager = true;
            }
            if (employee1 == 1)
            {
                employee = true;
            }
            if (managerTask1 == 1)
            {
                managerTask = true;
            }
            if (employeeTask1 == 1)
            {
                employeeTask = true;
            }
            if (task1 == 1)
            {
                task = true;
            }
            if (groupTask1 == 1)
            {
                groupTask = true;
            }


            string reportName = "Backup Report";

            MainMailClass mail = new MainMailClass();
            List<string> local;
            new System.Threading.Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                local = downloadBackup(current, manager, employee, managerTask, employeeTask, task, groupTask);
                mail.DownloadReport(local, MediaTypeNames.Text.Plain, reportName);
            }).Start();
            return null;
        }


        #endregion
        public ActionResult Backup()
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


        public List<string> downloadBackup(bool current = false, bool manager = false, bool employee = false, bool managerTask = false, bool employeeTask = false, bool task = false, bool groupTask = false)
        {

            Log.Info("Excel generation method called");

            List<string> objlist = new List<string>();
            objlist.Add(logedinuser.Id.ToString());
            User loggedinmanager = new UserBL().getUsersById(logedinuser.Id);

            if (AuthenticateUser() == false)
            {
                return null;
            }
            int compId = Convert.ToInt32(logedinuser.Company);
            



            Log.Info("Excel generation method search completed");

            string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
            ExcelManagement.generateExcelFile(path, current, manager, employee, task, employeeTask, managerTask, groupTask);

            List<string> objlist2 = new List<string>();

            objlist2 = General_Purpose.GetBlobFilePath(path, loggedinmanager.Id);
            objlist.AddRange(objlist2);

            return objlist;

            //return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");
        }

        //public ActionResult downloadBackup()
        //{
        //    try
        //    {
        //        if (AuthenticateUser() == false)
        //        {
        //            return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
        //        }
        //        int compId = Convert.ToInt32(logedinuser.Company);
        //        bool current = false;
        //        bool manager = false;
        //        bool employee = false;
        //        bool managerTask = false;
        //        bool employeeTask = false;
        //        bool task = false;
        //        bool groupTask = false;


        //        if (Request.Form["manager"] != null)
        //        {
        //            manager = true;
        //        }
        //        if (Request.Form["employee"] != null)
        //        {
        //            employee = true;
        //        }
        //        if (Request.Form["managerTask"] != null)
        //        {
        //            managerTask = true;
        //        }
        //        if (Request.Form["employeeTaskFalse"] != null)
        //        {
        //            employeeTask = true;
        //        }
        //        if (Request.Form["task"] != null)
        //        {
        //            task = true;
        //        }
        //        if (Request.Form["groupTask"] != null)
        //        {
        //            groupTask = true;
        //        }

        //        string path = Server.MapPath("~") + ProjectVaraiables.BACKUP_FOLDER_LOCATION + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx";
        //        ExcelManagement.generateExcelFile(path, current, manager, employee, task, employeeTask, managerTask, groupTask);
        //        return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".xlsx");
        //    }
        //    catch (Exception ex)
        //    {

        //        errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
        //        return RedirectToAction("Error");

        //    }
        //}

        #region googleCalendars
        public ActionResult AttachGoogle(CancellationToken cancellationToken)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });

                }
                var redirectUri = GoogleCalendarAsyncer.GetOauthTokenUri(this);
                if (redirectUri == null)
                    return RedirectToAction("Index", "Auth");
                return Redirect(redirectUri);
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


       public ActionResult GoogleDeattach(int userid = -1)
       {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });

                }
                Models.User user = null;
                user = new UserBL().getUsersById(userid);

                Models.User us = new User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    HomeNumber = user.HomeNumber,
                    Notes = user.Notes,
                    IsActive = user.IsActive,
                    AcessLevel = user.AcessLevel,
                    Image = user.Image,
                    Password = user.Password,
                    isSlack = user.isSlack,
                    isSMS = user.isSMS,
                    isMail = user.isMail,
                    SlackAddress = user.SlackAddress,
                    GoogleKeyLength = null,
                    GoggleTaskColor = user.GoggleTaskColor,
                    ShowTasks = user.ShowTasks,
                    ShowUrgent = user.ShowUrgent,
                    CompanyID = user.CompanyID,
                    DivisionId = user.DivisionId,
                    DepartmentId = user.DepartmentId,
                    Role = user.Role,
                    IsMasterAdmin = user.IsMasterAdmin,
                    OutlookToken = user.OutlookToken,
                    CanvasLoginId = user.CanvasLoginId,
                    CreatedAt = user.CreatedAt,
                    SearchByDepartment = user.SearchByDepartment,
                    SearchByDivision = user.SearchByDivision,
                    SearchByPriority = user.SearchByPriority,
                    SearchByPrivate = user.SearchByPrivate,
                    JoiningDate = user.JoiningDate,
                    DeletionDate = user.DeletionDate,
                    Player_Id = user.Player_Id,
                    HighPriorityColor = user.HighPriorityColor,
                    MediumPriorityColor = user.MediumPriorityColor,
                    LowPriorityColor = user.LowPriorityColor,
                    SearchByUserType = user.SearchByUserType,
                    IsPrimary = user.IsPrimary,
                    IsDelegate = user.IsDelegate,
                    IsZuptuSuperAdminUser = user.IsZuptuSuperAdminUser,
                    SignatureImage = user.SignatureImage



                };
                new UserBL().UpdateUsers(us);

                return RedirectToAction("Index", "Auth");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
       public void AttachOutlook()
       {
            // Outside of Web Forms page class, use HttpContext.Current.
            var response = HttpContext.Response;

            string Zuptu_Id = string.Empty;
           // var response = HttpContext.Response;
            System.Web.HttpCookie userInfo = new System.Web.HttpCookie("Zuptu_Id");
            System.Web.HttpCookie userInfo2 = new System.Web.HttpCookie("Zuptu_Id2");
            try
            {
                userInfo.Expires.AddDays(-1);
                userInfo2.Expires.AddDays(-1);
            }
            catch (Exception)
            {

            }
           

            response.Cookies.Add(userInfo);
            response.Cookies.Add(userInfo2);

           // System.Web.HttpCookie userInfo = new System.Web.HttpCookie("Zuptu_Id");
            userInfo.Value = logedinuser.Id.ToString();
            userInfo.Expires.AddDays(1);
           // System.Web.HttpCookie userInfo2 = new System.Web.HttpCookie("Zuptu_Id2");
            userInfo2.Value = logedinuser.Id.ToString();
            userInfo2.Expires.AddDays(1);

            // userInfo["UserColor"] = "Black";
            //userInfo.Expires.Add(new TimeSpan(0, 1, 0));
            response.Cookies.Add(userInfo);
            response.Cookies.Add(userInfo2);

            // Zuptu_Id = response.Cookies["ZuptuId"].Value;
            Zuptu_Id = response.Cookies["Zuptu_Id"].Value;

            // Signal OWIN to send an authorization request to Azure
            HttpContext.GetOwinContext().Authentication.Challenge(
              new AuthenticationProperties { RedirectUri = "/auth/index" },
              OpenIdConnectAuthenticationDefaults.AuthenticationType);

        }
       public int LoginedUserId()
       {
           // CheckAuthenticationDTO logedinukser = General_Purpose.CheckAuthentication();

            int id = logedinuser.Id;
            return id;
        }
       public ActionResult OutlookDeattach(int outlookuserid = -1)
       {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });

                }
                var response = HttpContext.Response;
                System.Web.HttpCookie userInfo = new System.Web.HttpCookie("Zuptu_Id");
                System.Web.HttpCookie userInfo2 = new System.Web.HttpCookie("Zuptu_Id2");
                try
                {
                    userInfo.Expires.AddDays(-1);
                    userInfo2.Expires.AddDays(-1);
                }
                catch (Exception)
                {

                }

                response.Cookies.Add(userInfo);
                response.Cookies.Add(userInfo2);

                Models.User user = null;
                user = new UserBL().getUsersById(outlookuserid);

                Models.User us = new User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    HomeNumber = user.HomeNumber,
                    Notes = user.Notes,
                    IsActive = user.IsActive,
                    AcessLevel = user.AcessLevel,
                    Image = user.Image,
                    Password = user.Password,
                    isSlack = user.isSlack,
                    isSMS = user.isSMS,
                    isMail = user.isMail,
                    SlackAddress = user.SlackAddress,
                    GoogleKeyLength = user.GoogleKeyLength,
                    GoggleTaskColor = user.GoggleTaskColor,
                    ShowTasks = user.ShowTasks,
                    ShowUrgent = user.ShowUrgent,
                    CompanyID = user.CompanyID,
                    DivisionId = user.DivisionId,
                    DepartmentId = user.DepartmentId,
                    Role = user.Role,
                    IsMasterAdmin = user.IsMasterAdmin,
                    OutlookToken = null,
                    CanvasLoginId = user.CanvasLoginId,
                    CreatedAt=user.CreatedAt,
                    SearchByDepartment=user.SearchByDepartment,
                    SearchByDivision=user.SearchByDivision,
                    SearchByPriority=user.SearchByPriority,
                    SearchByPrivate=user.SearchByPrivate,
                    JoiningDate=user.JoiningDate,
                    DeletionDate=user.DeletionDate,
                    Player_Id=user.Player_Id,
                    HighPriorityColor=user.HighPriorityColor,
                    MediumPriorityColor=user.MediumPriorityColor,
                    LowPriorityColor=user.LowPriorityColor,
                    SearchByUserType=user.SearchByUserType,
                    IsPrimary = user.IsPrimary,
                    IsDelegate = user.IsDelegate,
                    IsZuptuSuperAdminUser = user.IsZuptuSuperAdminUser,
                    SignatureImage = user.SignatureImage
                };
                new UserBL().UpdateUsers(us);

                return RedirectToAction("Index", "Auth");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
       #endregion


        #region Admin Section
        public ActionResult displayDivisionDetails(string sid, string message = "", string page1 = "")
        {
            try
            {
                if (logedinuser.Name == null || logedinuser.Role == 3 || logedinuser.Role == 2 || logedinuser.Role == 4)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                int id = General_Purpose.DecryptId(sid);
                Division division = new DivisionBL().getDivisionsById(id);
              //  List<Course> canvascourses = General_Purpose.GetCanvasCourse();

                ViewBag.managers = new UserBL().getUsersList().Where(x => x.DivisionId == division.Id || x.Role == 1 && x.IsActive == 1).ToList();

                ViewBag.message = message;
                ViewBag.Id = sid;
                ViewBag.page = page1;
              //  ViewBag.canvascourses = canvascourses;
                return View(division);
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult displayDepartmentDetails(string sid, string message = "", string way = "")
        {
            try
            {
                //if (logedinuser.Name == null || logedinuser.Role == 3 || logedinuser.Role == 2 || logedinuser.Role == 4)
                if (logedinuser.Name == null || logedinuser.Role == 3 )
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                int id;
                string str = "";
                id = General_Purpose.DecryptId(sid);

                if (id == 0)
                {
                    str = HttpUtility.UrlEncode(sid);
                    id = General_Purpose.DecryptId(str);
                }

                Department department = new DepartmentBL().getDepartmentsById(id);

                List<User> us = new List<User>();

                us = department.Users.Where(x => x.IsActive == 1 || x.IsActive == 2).ToList();
              //  List<Course> canvascourses = General_Purpose.GetCanvasCourse();

                ViewBag.managers = new UserBL().getUsersList().Where(x => x.DivisionId == department.DivisionID || x.Role == 1 && x.IsActive == 1).ToList();
                ViewBag.message = message;
                ViewBag.way = way;
                ViewBag.Id = sid;
                // ViewBag.canvascourses = canvascourses;
                return View(department);
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult DepartmentDetails(string sid, string message = "")
        {
            try
            {
                if (logedinuser.Name == null || logedinuser.Role == 3 || logedinuser.Role == 2 || logedinuser.Role == 4)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                int id;
                string str = "";
                id = General_Purpose.DecryptId(sid);

                if (id == 0)
                {
                    str = HttpUtility.UrlEncode(sid);
                    id = General_Purpose.DecryptId(str);
                }

                Department department = new DepartmentBL().getDepartmentsById(id);

                List<User> us = new List<User>();

                us = department.Users.Where(x => x.IsActive == 1 || x.IsActive == 2).ToList();
                //  List<Course> canvascourses = General_Purpose.GetCanvasCourse();

                ViewBag.managers = new UserBL().getUsersList().Where(x => x.DivisionId == department.DivisionID || x.Role == 1 && x.IsActive == 1).ToList();
                ViewBag.message = message;
                ViewBag.Id = sid;
                // ViewBag.canvascourses = canvascourses;
                return View(department);
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        public ActionResult GetEmpInDepatment(string sid = "")
        {
            try
            {
                int id = -1;
                string str = "";
                id = General_Purpose.DecryptId(sid);
                if (id == 0)
                {
                    str = HttpUtility.UrlEncode(sid);
                    id = General_Purpose.DecryptId(str);
                }


                Department department = new DepartmentBL().getDepartmentsById(id);

                List<User> Emp = new List<User>();


                //department.Tasks.Where(x => x.IsActive == 1)
                Emp = department.Users.Where(x => x.IsActive == 1 || x.IsActive == 2).ToList();

                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                //List<Department> departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();
                // List<User> managerlist = new UserBL().getActiveandInvitedUser().Where(x => x.CompanyID == (int)logedinuser.Company && (x.Role == 2 || x.Role == 4) && (x.IsActive == 1 || x.IsActive == 2)).OrderBy(s => s.FirstName).ToList();


                int totalrows = Emp.Count();
                //filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Emp = Emp.Where(x => x.FirstName.ToLower().Contains(searchValue.ToLower()) || x.LastName.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                int totalrowsafterfilterinig = Emp.Count();
                //sorting
                //users = users.OrderBy(sortColumnName + " " + sortDirection).ToList();

                // pagination
                Emp = Emp.Skip(start).Take(length).ToList();

                //List<User> mnglist = new List<User>();
                List<ManagerDTO> mnglist = new List<ManagerDTO>();

                string priority = "";
                string status = "";
                string input = "";
                foreach (User x in Emp)
                {
                    if (x.IsActive == 1)
                    {
                        input = "<input type = 'checkbox' class='messageCheckbox' id='check " + x.Id + "'value='" + x.Id + "' name='" + x.FirstName + "' />";
                    }
                    else
                    {
                        input = "";
                    }



                    //<input type = "checkbox" class="messageCheckbox" id="check @item.Id" value="@item.Id" name="@item.FirstName" />

                    //@Url.Action("taskDisplay", "Task", new { v = StringCipher.Base64Encode(item.Id.ToString()) })
                    //   x.Name = "<a href='../Task/taskDisplay?v=" + StringCipher.Base64Encode(x.Id.ToString()) + "'>" + x.Name + "</a>";


                    ManagerDTO obj = new ManagerDTO()
                    {
                        // HttpUtility.UrlEncode(General_Purpose.Encrypt(item.Id ))

                        Name = input, // check box
                        FirstName = x.FirstName + "" + x.LastName,
                        IsActive = (int)x.IsActive,
                        Email = x.Email,
                        deptId = department.Id,
                        Role2 = (int)x.Role,
                        Id = x.Id,
                        EncryptedEmpId = General_Purpose.EncryptId(x.Id)

                    };
                    mnglist.Add(obj);



                }

                return Json(new { data = mnglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }
        #endregion
        public ActionResult GetEmpTaskInDepatment(string sid = "")
        {
            try
            {
                int id = -1;
                string str = "";
                id = General_Purpose.DecryptId(sid);
                if (id == 0)
                {
                    str = HttpUtility.UrlEncode(sid);
                    id = General_Purpose.DecryptId(str);
                }

                Department department = new DepartmentBL().getDepartmentsById(id);

                /// List<User> Emp = new List<User>();


                List<Models.Task> Emptasks = new List<Models.Task>();

                Emptasks = department.Tasks.Where(x => x.IsActive == 1).ToList();

                //Emp = department.Users.Where(x => x.IsActive == 1 || x.IsActive == 2).ToList();

                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                //List<Department> departments = new DepartmentBL().getDepartmentsList().OrderBy(s => s.Name).ToList();
                // List<User> managerlist = new UserBL().getActiveandInvitedUser().Where(x => x.CompanyID == (int)logedinuser.Company && (x.Role == 2 || x.Role == 4) && (x.IsActive == 1 || x.IsActive == 2)).OrderBy(s => s.FirstName).ToList();


                int totalrows = Emptasks.Count();
                //filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Emptasks = Emptasks.Where(x => x.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                int totalrowsafterfilterinig = Emptasks.Count();
                //sorting
                //users = users.OrderBy(sortColumnName + " " + sortDirection).ToList();

                // pagination
                Emptasks = Emptasks.Skip(start).Take(length).ToList();

                //List<User> mnglist = new List<User>();
                List<ManagerDTO> mnglist = new List<ManagerDTO>();

                string priority = "";
                string status = "";
                string input = "";
                foreach (Models.Task x in Emptasks.OrderByDescending(x=>x.Id))
                {


                    if (x.Cost_ == null)
                    {
                        x.Cost_ = 0;
                    }
                    if (x.CEU == null)
                    {
                        x.CEU = 0;
                    }
                    if (x.Hours == null)
                    {
                        x.Hours = 0;
                    }
                    ManagerDTO obj = new ManagerDTO()
                    {
                        // HttpUtility.UrlEncode(General_Purpose.Encrypt(item.Id ))

                        Name = x.Name, // check box
                        Cost = (double)x.Cost_,
                        CEU = (double)x.CEU,
                        Hours = (double)x.Hours,

                        EncryptedDeptId = General_Purpose.EncryptId(department.Id),

                        Id = x.Id,
                        EncryptedEmpId = General_Purpose.EncryptId(x.Id)

                    };
                    mnglist.Add(obj);



                }

                return Json(new { data = mnglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString()+"  \n Site Trace:: "+ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult AjaxAction()
        {
            try
            {
                return RedirectToAction("Logout", "Auth");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }


    }
}