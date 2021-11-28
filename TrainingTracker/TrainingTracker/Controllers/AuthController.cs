using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Windows.Forms;
using TrainingTracker.BL;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;
using TrainingTracker.HelpingClasses.Logging;
using TrainingTracker.HelpingClasses.Outlook;
using TrainingTracker.Models;


namespace TrainingTracker.Controllers
{

    public class AuthController : Controller
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

        public ActionResult ActivateDelegateAccess(int delegateaccessuserid = -1)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                if (delegateaccessuserid == -1)
                {
                    return RedirectToAction("ArchivedUsers", "Task", new { message = "Some error has occured!" });
                }

                string DocMUrl = "Auth/AllowDelegateAccess?v=" 
                    + General_Purpose.EncryptId(delegateaccessuserid) 
                    + "&p=" + General_Purpose.EncryptId(logedinuser.Id);

                //return Redirect("https://localhost:44308/" + DocMUrl);
                return Redirect(ProjectVaraiables.DocM_URL + DocMUrl);
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        // public AuthController()
        //{
        //    //  Logout();
        //    Console.WriteLine("Constructor");
        //}

        [HandleError]
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult TaskCalendar()
        {
            return View();
        }
        // GET: Admin

        public ActionResult CreateCompany(string error="")
        {
            ViewBag.errorMsg = error;
            return View();
        }
        
        public ActionResult Index(string s1X = "", string message = "" ,string Lvtx="",string DxM="",string uty="")
        {
            try
            {
                CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
                
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                

                //string Zuptu_Id = HttpContext.Response.Cookies["Zuptu_Id"].Value;
                //if (!string.IsNullOrEmpty(Zuptu_Id))
                //{
                //    RefreshCookies(Zuptu_Id);
                //}



                //if (uty != "")
                //{
                //    Models.User use = new UserBL().getUsersById(logedinuser.Id);
                //      string EncryptID = HttpUtility.UrlEncode(General_Purpose.Encrypt(use.Id));
                //    return Redirect(ProjectVaraiables.LMS_Auth_URL+EncryptID+"&Lvtx="+Lvtx+"&DxM="+DxM);
                //}

                //////////Stress Testing

                //for (int i = 0; i < 1000; i++) 
                //{
                //    DateTime dt = DateTime.Now.AddDays(-200).Date;
                //    Random random = new Random();
                //    int days= random.Next(1, 180);

                //    DateTime st = dt.AddDays(days);
                //    DateTime et = st.AddDays(1).AddMinutes(-1);

                //    DateTime stticket = st;
                //    DateTime edticket = st.AddMinutes(15);



                //    User_Task tmm = new User_Task()
                //    {
                //        CEU = 2,
                //        Cost=2,
                //        CreatedID = 61,
                //        StartDate = st,
                //        EndDate = et,
                //        Hours = 1.5,
                //        IsActive = 1,
                //        IsPrivate = 0,
                //        Notes = "Stress Testing Notes2",
                //        Priority = 2,
                //        Status = 0,
                //        TaskID=212,
                //        UserID=390

                //    };
                //    tmm = new User_TaskBL().AddUser_Tasks(tmm);
                //    for(int j = 0; j < 3; j++)
                //    {
                //        TaskComment cmt = new TaskComment() 
                //        { 

                //            Comment="Stress Testing Comment2",
                //            Date=DateTime.Now.ToString(),
                //            File="637185116719249706.pdf",
                //            FileName="DivisionReport.pdf",
                //            IsActive=1,
                //            TaskId=tmm.Id,
                //            UserId=61,

                //        };
                //        new TaskCommentBL().AddTaskComments(cmt);
                //    }


                //    for(int k=0; k < 3; k++)
                //    {
                //        Task_Ticket ttc = new Task_Ticket() 
                //        {
                //            CreatedBy=61,
                //            CreationDatetime=DateTime.Now,
                //            StartDate= stticket,
                //            EndDate= edticket,
                //            IsActive=1,
                //            Name="Stress Testing Ticket2",
                //            UserTask_Id=tmm.Id
                //        };
                //        new Task_TicketBL().AddTask_Tickets(ttc);
                //    }

                //}

                //////// 


                ViewBag.message = message;

                #region Canvas

             //   List<Course> canvascourses = General_Purpose.GetCanvasCourse();
                DateTime dtt = new DateTime(Convert.ToInt64(logedinuser.PicSignatureTime));
                if (DateTime.Now >= dtt)
                {
                    DatabaseEntities de = new DatabaseEntities();
                    Models.User Admin = new UserBL().getUsersById(logedinuser.Id, de);

                    string userPicture = string.Empty;
                    try
                    {
                        userPicture = Admin.Image.Split('/')[4];
                        if (userPicture.Contains('?'))
                        {
                            userPicture = userPicture.Split('?').First();
                        }
                    }
                    catch (Exception e)
                    {
                        userPicture = string.Empty;
                    }


                    Admin.Image = userPicture != string.Empty ? BlobManager.GetBlobSasUri(userPicture, Admin.Company.Id.ToString()) : "";
                    new UserBL().UpdateUsers(Admin, de);
                }

          

               
                #endregion





                if (s1X != "")
                {
                    ViewBag.FirstTimeLogin = s1X;
                    Log.Info(logedinuser.Name+ " Loged Into Zuptu");

                }
                else
                {
                    ViewBag.FirstTimeLogin = null;
                }



              

                int id = logedinuser.Id;
                int role = logedinuser.Role;


                string googleKey = string.Empty;
                Models.User user = null;
                user = new UserBL().getUsersById(id);

                ///////////////////////////////////
                List<User_Task> tm;
                DateTime customdate = DateTime.Now;
                tm = new User_TaskBL().getUser_TasksList().Where(x => x.UserID == id && x.CompletionDate == null).ToList();
                // tm = tm.Where(x => (Convert.ToDateTime(x.StartDate).Date == customdate.Date) || (Convert.ToDateTime(x.EndDate).Date == customdate.Date) || (Convert.ToDateTime(x.StartDate).Date < customdate.Date && Convert.ToDateTime(x.EndDate).Date > customdate.Date)).ToList();
                tm = tm.Where(x => Convert.ToDateTime(x.EndDate).Date <= customdate.Date).ToList();

                List<DueDateReminderDTO> list = new List<DueDateReminderDTO>();
                int late = 0;
                foreach (User_Task u in tm)
                {
                    if (u.EndDate < DateTime.Now.AddDays(30))
                    {
                        if (u.EndDate < DateTime.Now)
                        {
                            late = 1;
                        }
                        else
                            late = 0;
                        DueDateReminderDTO dt = new DueDateReminderDTO()
                        {
                            Id = HttpUtility.UrlEncode(General_Purpose.Encrypt(u.Id)),
                            Task = u.Task.Name,
                            DueDate = (DateTime)u.EndDate,
                            Late = late,
                            Role = (int)u.User1.Role

                        };

                        list.Add(dt);
                    }
                }
                ViewBag.DueDateTasks = list;
                if (list.Count() != 0)
                {
                    ViewBag.DDTask = 1;
                }
                else
                {
                    ViewBag.DDTask = 0;
                }

                

                googleKey = user.GoogleKeyLength;
                //  Session["isGoogle"] = user.GoogleKeyLength != null ? 1 : 0;
                // ViewBag.google = (int)Session["isGoogle"];
                ViewBag.google = user.GoogleKeyLength != null ? 1 : 0; //owin
                ViewBag.outlook = user.OutlookToken != null ? 1 : 0;
                ViewBag.loggedinuser = user;
                ViewBag.divisions = new DivisionBL().getDivisionsList().ToList();
                ViewBag.departments = new DepartmentBL().getAllDepartmentsList().ToList();
                ViewBag.UserId = user.Id;
                //   ViewBag.canvascourses = canvascourses;
                ViewBag.Course = new CourseBL().getCourseList().Where(x => x.IsActive == 1).ToList();



                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        [HttpPost]
        public ActionResult SearchSetting(string LowPriorityColor, string MediumPriorityColor, string HighPriorityColor, string SearchByPriority, string SearchByUserType, string SearchByPrivate)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });

                }

                Models.User u = new UserBL().getUsersById(logedinuser.Id);

                Models.User user = new Models.User()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    HomeNumber = u.HomeNumber,
                    Notes = u.Notes,
                    IsActive = u.IsActive,
                    AcessLevel = u.AcessLevel,
                    Image = u.Image,
                    Password = u.Password,
                    isSlack = u.isSlack,
                    isSMS = u.isSMS,
                    isMail = u.isMail,
                    SlackAddress = u.SlackAddress,
                    GoogleKeyLength = u.GoogleKeyLength,
                    GoggleTaskColor = u.GoggleTaskColor,
                    ShowTasks = u.ShowTasks,
                    ShowUrgent = u.ShowUrgent,
                    CompanyID = u.CompanyID,
                    DivisionId = u.DivisionId,
                    DepartmentId = u.DepartmentId,
                    IsMasterAdmin = u.IsMasterAdmin,
                    OutlookToken = u.OutlookToken,
                    SearchByUserType = SearchByUserType,
                    SearchByPrivate = SearchByPrivate,
                    SearchByPriority = SearchByPriority,
                    Role = u.Role,
                    LowPriorityColor = LowPriorityColor,
                    MediumPriorityColor = MediumPriorityColor,
                    HighPriorityColor = HighPriorityColor,
                    CanvasLoginId = u.CanvasLoginId,
                    JoiningDate= u.JoiningDate,
                    DeletionDate =u.DeletionDate,
                    Player_Id = u.Player_Id,
                    CreatedAt = u.CreatedAt,
                    IsDelegate= u.IsDelegate,
                    IsZuptuSuperAdminUser= u.IsZuptuSuperAdminUser,
                    IsPrimary =u.IsPrimary,
                    SearchByDepartment=u.SearchByDepartment,
                    SearchByDivision=u.SearchByDivision,
                    SignatureImage=u.SignatureImage
                };

                user.IsPrimary = u.IsPrimary;
                new UserBL().UpdateUsers(user);

                return RedirectToAction("Index", "Auth");

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult Assignments()
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

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        public ActionResult EditAssignment()
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

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult ManagerForm()
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

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult EditManagerForm()
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

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult Task()
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

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult EditTask()
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

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult Trainee()
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

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult EditTrainee()
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

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult Divisions()
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

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult LogoutZuptuWeb(string Lvtx="",string DxM="")
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut("ApplicationCookie");

          //  Log.Info(logedinuser.Name +" Logedout from Zuptu");

            if (Lvtx != "")
            {
                return Redirect(ProjectVaraiables.LMS_URL);


            }
            else if (DxM != "")
            {
                return Redirect(ProjectVaraiables.DocM_URL);

            }

            return RedirectToAction("Login");
            //  return RedirectToAction("Login");
        }

        public ActionResult Logout(string Lvtx="",string DxM="")
        {
            
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut("ApplicationCookie");

            Log.Info(logedinuser.Name +" Logedout from Zuptu");


          return Redirect(ProjectVaraiables.LMS_URL + "/Auth/LogoutLMS?Lvtx=" + Lvtx + "&DxM=" + DxM);
           //   return RedirectToAction("Login");
        }
        

        public ActionResult AuthenticateUser(string v="",string Lvtx="", string DxM="")
        {
            try
            {
                int id = General_Purpose.DecryptId(v);
                Models.User user = new UserBL().getUsersById(id);


                Company company = user.Company;

                string userPicture = string.Empty;
                try
                {
                    userPicture = user.Image.Split('/')[4];
                    // userPicture = Admin.Image.Split('/')[4];
                    if (userPicture.Contains('?'))
                    {
                        userPicture = userPicture.Split('?').First();
                    }
                }
                catch (Exception e)
                {
                    userPicture = string.Empty;
                }

                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim("SlackAddress", company.SlackWebhook!=null? company.SlackWebhook:""),
                    new Claim("Company", company.Id.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("PicSignatureTime", DateTime.Now.AddHours(24).Ticks.ToString()),


                    new Claim("Image", userPicture != string.Empty? BlobManager.GetBlobSasUri(userPicture,company.Id.ToString()):""),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim("isGoogle", user.GoogleKeyLength != null ? "1" : "0"),
                    new Claim("IsMasterAdmin", user.IsMasterAdmin != null?user.IsMasterAdmin.ToString():"0"),

                   }, "ApplicationCookie");
                var claimsPrincipal = new ClaimsPrincipal(identity);
                // Set current principal
                Thread.CurrentPrincipal = claimsPrincipal;
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

                string EncryptID = General_Purpose.EncryptId(user.Id);

                // return Redirect(ProjectVaraiables.LMS_Auth_URL + EncryptID + "&Lvtx=" + Lvtx + "&DxM=" + DxM);
                if (Lvtx != "")
                {
                    if (company.IsLMS == 1)
                        return Redirect(ProjectVaraiables.LMS_URL + "/Auth/authenticateLMS?v=" + EncryptID);
                    else
                        return RedirectToAction("Index", new { s1X = "2c" });


                }
                else if (DxM != "")
                {
                    if (company.IsDocManager == 1)
                        return Redirect(ProjectVaraiables.DocM_URL + "/Auth/authenticateDocM?v=" + EncryptID);
                    else
                        return RedirectToAction("Index", new { s1X = "2c" });

                }
                return RedirectToAction("Index", new { s1X = "2c" });
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult Login(string error = "",string company = "",string email="",string Lvtx="",string DxM="",string v="")
        {
            try
            {
                CheckAuthenticationDTO ln = General_Purpose.CheckAuthentication();

                if (AuthenticateUser() == true)
                {
                    Models.User u = new UserBL().getUsersById(ln.Id);
                    Company com = new CompanyBL().getCompanyById((int)u.CompanyID);
                    string EncryptID = General_Purpose.EncryptId(u.Id);
                    if (Lvtx != "")
                    {
                        if(com.IsLMS==1)
                        return Redirect(ProjectVaraiables.LMS_URL + "/Auth/authenticateLMS?v="+EncryptID);
                        else
                            return RedirectToAction("Index", "Auth");


                       
                    }
                    else if (DxM != "")
                    {
                        if(com.IsDocManager==1)
                        return Redirect(ProjectVaraiables.DocM_URL + "/Auth/authenticateDocM?v="+EncryptID);
                        else
                            return RedirectToAction("Index", "Auth");


                    }



                    return RedirectToAction("Index", "Auth");
                }
                if (v!="")
                {
                    return RedirectToAction("AuthenticateUser", new { v,Lvtx,DxM });
                }
                ViewBag.email = email;
                ViewBag.company = company;
                ViewBag.errorMsg = error;
                ViewBag.LMS = Lvtx;
                ViewBag.DocM = DxM;
                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        
              
        public ActionResult validateUserCreateCompany(string email="", string password="")
        {
            try
            {
                //Company com = new CompanyBL().getCompanyList().Where(x => x.Name.ToUpper().Equals(companyname.ToUpper())).FirstOrDefault();
                if(email=="" || password == "")
                {
                    return RedirectToAction("CreateCompany", new { error = "Incorrect credentials." });
                }
                string mail = "trevor@zuptu.com";
                string pwd = "19c2k15";
                int user = 0;
                if(email.ToUpper()==mail.ToUpper() && password.ToUpper()== pwd.ToUpper())
                {
                    user = 1;
                }
                else
                {
                    user = 0;
                }
               // Models.User user = new UserBL().getAllUsersList().Where(x => x.Email.Equals(email) && x.Password.Equals(password) && x.CompanyID==com.Id && x.IsActive == 1).FirstOrDefault();

                if (user != 0)
                {

                    return RedirectToAction("signup","Utilities", new { k3y = "1/2k%3d" });

                }
                return RedirectToAction("CreateCompany", new { error = "Incorrect credentials." });



            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult validateUser(string email="", string password="", string companyname="",string Lvtx="",string DxM = "")
         {
            try
            {
                if (companyname == "")
                {
                    return RedirectToAction("Login", new { error = "Company Name field must have value",company = companyname, email = email });

                }
                if (email == "")
                {
                    return RedirectToAction("Login", new { error = "Email field must have value", company = companyname, email = email });

                }
                if (password == "")
                {
                    return RedirectToAction("Login", new { error = "Password field must have value", company = companyname, email = email });

                }
                
                companyname = companyname.Trim();
                Company com = new CompanyBL().getCompanyList().Where(x => x.Name.ToUpper().Equals(companyname.ToUpper())).FirstOrDefault();
                
                if (com == null)
                {
                    return RedirectToAction("Login", new { error = "Company Name not valid", company = companyname, email = email });
                }
                email = email.Trim();
               // string c = StringCipher.Decrypt("q Aq0nRXYSbGF ZeDa /HluTf8MgSRe4b3CxUYKmruAKgwDQm3cGFxSM0fgRcdwcFvsdS10PLi1wLcO3 qxjVT5C9HWIAj06qZOnem4bCin5WI1rtMR4DJ4jKSBQ1 ja","zuptu");
                //Models.User uuu = new UserBL().getAllUsersForLogin().Where(x => x.Id == 408).FirstOrDefault();
                Models.User user = new UserBL().getAllUsersForLogin().Where(x => x.Email.ToUpper().Equals(email.ToUpper()) && StringCipher.Decrypt(x.Password, "zuptu").Equals(password) && x.CompanyID == com.Id && x.IsActive == 1).FirstOrDefault();
                // Models.User user = new UserBL().getAllUsersList().Where(x => x.Email.Equals(email) && x.Password.Equals(password) && x.CompanyID==com.Id && x.IsActive == 1).FirstOrDefault();

                if (user != null)
                {

                    Company company = user.Company;

                    string userPicture = string.Empty;
                    try
                    {
                        userPicture = user.Image.Split('/')[4];
                        // userPicture = Admin.Image.Split('/')[4];
                        if (userPicture.Contains('?'))
                        {
                            userPicture = userPicture.Split('?').First();
                        }
                    }
                    catch (Exception e)
                    {
                        userPicture = string.Empty;
                    }
                    //Session["Role"] = user.Role;
                    //Session["SlackAddress"] = company.SlackWebhook!=null? company.SlackWebhook:null;
                    //Session["Company"] = company.Id;
                    //Session["Id"] = user.Id;
                    //Session["Image"] = user.Image;
                    //Session["Name"] = user.FirstName;
                    //Session["isGoogle"] = user.GoogleKeyLength != null ? 1 : 0;
                    //Session["IsMasterAdmin"] = user.IsMasterAdmin != null?user.IsMasterAdmin:0;

                    var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim("SlackAddress", company.SlackWebhook!=null? company.SlackWebhook:""),
                    new Claim("Company", company.Id.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("PicSignatureTime", DateTime.Now.AddHours(24).Ticks.ToString()),


                    new Claim("Image", userPicture != string.Empty? BlobManager.GetBlobSasUri(userPicture,company.Id.ToString()):""),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim("isGoogle", user.GoogleKeyLength != null ? "1" : "0"),
                    new Claim("IsMasterAdmin", user.IsMasterAdmin != null?user.IsMasterAdmin.ToString():"0"),

                   }, "ApplicationCookie");
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    // Set current principal
                    Thread.CurrentPrincipal = claimsPrincipal;
                    var ctx = Request.GetOwinContext();
                    var authManager = ctx.Authentication;
                    authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

                    DatabaseEntities de = new DatabaseEntities();
                    Models.User Admin = new UserBL().getUsersById(user.Id, de);

                    Admin.Image = userPicture != string.Empty ? BlobManager.GetBlobSasUri(userPicture, Admin.Company.Id.ToString()) : "";
                    new UserBL().UpdateUsers(Admin, de);

               

                   string EncryptID = HttpUtility.UrlEncode(General_Purpose.Encrypt(user.Id));

                    if(company.IsLMS==1  || company.IsDocManager == 1)
                    {
                        return Redirect(ProjectVaraiables.LMS_URL + "/Auth/authenticateUser?v=" + EncryptID + "&Lvtx=" + Lvtx + "&DxM=" + DxM);
                    }

                  
                     return RedirectToAction("Index", new { s1X = "2c"});


                }
                return RedirectToAction("Login", new { error = "Incorrect credentials.",company=companyname,email=email,Lvtx=Lvtx,DxM=DxM });



            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public void RefreshCookies(string id)
        {
           Models.User user = new UserBL().getUsersById(Convert.ToInt32(id));
            Company company = new CompanyBL().getCompanyById((int)user.CompanyID);
            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim("SlackAddress", company.SlackWebhook!=null? company.SlackWebhook:""),
                    new Claim("Company", company.Id.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("PicSignatureTime", DateTime.Now.AddHours(24).Ticks.ToString()),


                   // new Claim("Image", userPicture != string.Empty? BlobManager.GetBlobSasUri(userPicture,company.Id.ToString()):""),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim("isGoogle", user.GoogleKeyLength != null ? "1" : "0"),
                    new Claim("IsMasterAdmin", user.IsMasterAdmin != null?user.IsMasterAdmin.ToString():"0"),

                   }, "ApplicationCookie");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            // Set current principal
            Thread.CurrentPrincipal = claimsPrincipal;
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

            //var identity2 = (ClaimsPrincipal)Thread.CurrentPrincipal;
            //CheckAuthenticationDTO auth = new CheckAuthenticationDTO();

            //auth.Role = Convert.ToInt32(identity2.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault());


            //auth.SlackAddress = identity2.Claims.Where(c => c.Type == "SlackAddress").Select(c => c.Value).FirstOrDefault();
            //auth.Company = identity2.Claims.Where(c => c.Type == "Company").Select(c => c.Value).FirstOrDefault();
            //auth.Id = Convert.ToInt32(identity2.Claims.Where(c => c.Type == "Id").Select(c => c.Value).FirstOrDefault());
            //auth.Image = identity2.Claims.Where(c => c.Type == "Image").Select(c => c.Value).FirstOrDefault();
            //auth.Name = identity2.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).FirstOrDefault();
            //auth.isGoogle = Convert.ToInt32(identity2.Claims.Where(c => c.Type == "isGoogle").Select(c => c.Value).FirstOrDefault());
            //auth.IsMasterAdmin = Convert.ToInt32(identity2.Claims.Where(c => c.Type == "IsMasterAdmin").Select(c => c.Value).FirstOrDefault());
            //auth.PicSignatureTime = Convert.ToDouble(identity2.Claims.Where(c => c.Type == "PicSignatureTime").Select(c => c.Value).FirstOrDefault());

            //return auth;
        }
        public ActionResult Reset(string msg = "",string email="",string company="")
        {
            ViewBag.message = msg;
            ViewBag.email = email;
            ViewBag.company = company;
            return View();
        }

        public ActionResult sendResetMail(string email="", string companyname = "")
        {
            try
            {
                if (email == "")
                {
                    return RedirectToAction("Reset", new { msg = "Email field must have value", email = email, company = companyname });
                }
                if ( companyname=="")
                {
                    return RedirectToAction("Reset", new { msg = "Company field must have value", email = email, company = companyname });
                }
                
                //bool check = false;
                string updatedEmail = email.Trim().ToUpper();
                string updatedCom = companyname.Trim().ToUpper();
                Company com = new CompanyBL().getCompanyList().Where(x => x.Name.ToUpper().Equals(updatedCom)).FirstOrDefault();
                if (com == null)
                {
                    //check = true;
                   
                    return RedirectToAction("Reset", new { msg = "Email or Company doesn't exist in the system.", email = email, company = companyname });
                }
                //User temp = null;
                Models.User user = new UserBL().getFogetPwdUsersList().Where(x => x.Email.ToUpper().Equals(updatedEmail) && x.CompanyID == com.Id && x.IsActive == 1).FirstOrDefault();

                if (user != null)
                {
                    //check = true;
                    new MainMailClass().sendForgetMail(email, user.Id);
                    return RedirectToAction("Reset", new { msg = "An email has been sent to your Email" });
                }
                //Hello World
                //Helloo Wolrd 2
                //Hello World 3
                   


                return RedirectToAction("Reset", new { msg = "Email or Company doesn't exist in the system.",email=email,company=companyname });

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult updatePassword(string password, string confirmpassword, string value, string time, string id, int role, string IsUser = "")
        {
            try
            {
                var input = password;
                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasspecialChar = new Regex(@"[$!%*?@/]+");
                var hasMinimum8Chars = new Regex(@".{8,}");

                var isValidated = hasNumber.IsMatch(input) && hasUpperChar.IsMatch(input) && hasLowerChar.IsMatch(input) && hasspecialChar.IsMatch(input) && hasMinimum8Chars.IsMatch(input);
                if (isValidated == false)
                {

                    if (IsUser != "")
                    {
                        if (password != confirmpassword)
                        {
                            return RedirectToAction("isotaskingUpdate", "Auth", new { val = value, t = time, i = id, msg = "Password pattern doesn't match.", IsUser = IsUser });
                        }
                    }
                    else
                    {
                        return RedirectToAction("resetpassword", "Auth", new { val = value, t = time, i = id, msg = "Password pattern doesn't match." });

                    }





                }

                if (IsUser != "")
                {
                    int uId = Convert.ToInt32(StringCipher.Base64Decode(id));
                    Models.User us = new UserBL().getUserByIdInactive(uId);
                    int val = 0;
                    if (us != null)
                    {

                        val = new UserBL().getAllActiveUsersList().Where(x => x.Email == us.Email && x.CompanyID == us.CompanyID).ToList().Count();
                    }

                    if (val != 0)
                        return RedirectToAction("isotaskingUpdate", "Auth", new { val = value, t = time, i = id, msg = "You are already exists in System.", IsUser = IsUser });

                }
                int userId = Convert.ToInt32(StringCipher.Base64Decode(id));

                if (IsUser == "")
                {
                    Models.User uss = new UserBL().getUsersById(userId);

                    OldPassword olpass = new OldPassword()
                    {
                        UserId = uss.Id,
                        Password = uss.Password,
                        CreatedAt = DateTime.Now,
                        IsActive = 1,
                        CompanyId = uss.CompanyID
                    };
                    new OldPasswordBL().AddOldPassword(olpass);

                    OldPassword p = new OldPasswordBL().getOldPasswordsList().Where(x => StringCipher.Decrypt(x.Password, "zuptu") == password && x.UserId == uss.Id).FirstOrDefault();
                    if (p != null)
                    {
                        int days = DateTime.Now.Subtract(Convert.ToDateTime(p.CreatedAt)).Days;

                        return RedirectToAction("resetpassword", "Auth", new { val = value, t = time, i = id, msg = "You cannot set your old password." });


                    }
                    if (password != confirmpassword)
                    {
                        return RedirectToAction("resetpassword", "Auth", new { val = value, t = time, i = id, msg = "Password and confirm password doesn't match." });
                    }

                }
                if(IsUser!="")
                    if (password != confirmpassword)
                    {
                        return RedirectToAction("isotaskingUpdate", "Auth", new { val = value, t = time, i = id, msg = "Password and confirm password doesn't match.", IsUser = IsUser });
                    }






                DatabaseEntities de = new DatabaseEntities();

                Models.User user;
                if (IsUser == "yes")
                {
                    user = new UserBL().getUserByIdInactive(userId, de);
                    user.Password = StringCipher.Encrypt(password, "zuptu");
                    user.IsActive = 1;
                    new UserBL().UpdateUsers(user, de);
                }
                else
                {
                    user = new UserBL().getAllActiveUsersList().Where(x => x.Id == userId).FirstOrDefault();
                    user.Password = StringCipher.Encrypt(password, "zuptu");

                    Models.User newUser = new Models.User()
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
                        isMail = user.isMail,
                        isSlack = user.isSlack,
                        isSMS = user.isSMS,
                        SlackAddress = user.SlackAddress,
                        GoogleKeyLength = user.GoogleKeyLength,
                        GoggleTaskColor = user.GoggleTaskColor,
                        ShowTasks = user.ShowTasks,
                        ShowUrgent = user.ShowUrgent,
                        CompanyID = user.CompanyID,
                        DivisionId = user.DivisionId,
                        SearchByDepartment = user.SearchByDepartment,
                        SearchByDivision = user.SearchByDivision,
                        SearchByPriority = user.SearchByPriority,
                        SearchByPrivate = user.SearchByPrivate,
                        SearchByUserType = user.SearchByUserType,
                        LowPriorityColor = user.LowPriorityColor,
                        HighPriorityColor = user.HighPriorityColor,
                        MediumPriorityColor = user.MediumPriorityColor,
                        Role = user.Role,
                        IsMasterAdmin = user.IsMasterAdmin,
                        OutlookToken = user.OutlookToken,
                        DepartmentId = user.DepartmentId,
                        CanvasLoginId = user.CanvasLoginId,
                        JoiningDate = user.JoiningDate,
                        DeletionDate = user.DeletionDate,
                        Player_Id = user.Player_Id,
                        CreatedAt = user.CreatedAt,
                        IsDelegate = user.IsDelegate,
                        IsZuptuSuperAdminUser = user.IsZuptuSuperAdminUser,
                        IsPrimary = user.IsPrimary,
                        SignatureImage=user.SignatureImage
                    };

                    newUser.IsPrimary = user.IsPrimary;
                    new UserBL().UpdateUsers(newUser);
                }
                if (user != null)
                {

                    return RedirectToAction("login", "Auth");

                }
                return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult isotaskingUpdate(string val, string t, string i, string msg = "", string IsUser = "")
        {
            try
            {
                ViewBag.val = val;
                ViewBag.t = t;
                ViewBag.msg = msg;

                long ticks = Convert.ToInt64(StringCipher.Base64Decode(t));
                DateTime dt = new DateTime(ticks);
                if (dt < DateTime.Now)
                {
                    ViewBag.expire = "yes";
                    return View("ChangePassword");
                }
                string content = StringCipher.Base64Decode(val);
                string UserID = StringCipher.Base64Decode(i);

                Models.User u = new UserBL().getActiveandInvitedUser().Where(x => x.Id == Convert.ToInt32(UserID)).FirstOrDefault();
              
              
                if (u == null)
                {
                    ViewBag.expire = "yes";
                    return View("ChangePassword");

                }
                else if (u.IsActive == 1)
                {
                    ViewBag.expire = "yes";
                    return View("ChangePassword");
                }



                Models.User user = new UserBL().getActiveandInvitedUser().Where(x => x.Email.Equals(content) && x.CompanyID == u.CompanyID && x.IsActive == 2).FirstOrDefault();

                int vall = new UserBL().getAllActiveUsersList().Where(x => x.Email == user.Email && x.CompanyID == u.CompanyID).ToList().Count();
                string res = "";
                if (vall != 0)
                {
                    res = "Active";
                }

                //int role = -1;
                if (user != null)
                {

                    ViewBag.id = StringCipher.Base64Encode(user.Id.ToString());
                    ViewBag.role = user.Role;
                    ViewBag.IsUser = IsUser;
                    ViewBag.Active = res;

                    return View("ChangePassword");

                }
                return RedirectToAction("Login", new { error = "Couldn't find the User." });


            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult ExpireLink()
        {
            try
            {
                ViewBag.expire = "yes";
                return View("ChangePassword");
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }
        public ActionResult resetpassword(string val, string t, string i, string msg = "")
        {
            try
            {
                ViewBag.val = val;
                ViewBag.t = t;
                ViewBag.msg = msg;

                long ticks = Convert.ToInt64(StringCipher.Base64Decode(t));
                DateTime dt = new DateTime(ticks);
                if (dt < DateTime.Now)
                {
                    return RedirectToAction("Login", new { err = "Link has been Expired" });
                }
                string content = StringCipher.Base64Decode(val);
                string UserID = StringCipher.Base64Decode(HttpUtility.UrlDecode(i));

                Models.User u = new UserBL().getUsersById(Convert.ToInt32(UserID));

                Models.User user = new UserBL().getFogetPwdUsersList().Where(x => x.Email.Equals(content) && x.CompanyID == u.CompanyID && x.IsActive == 1).FirstOrDefault();


                if (user != null)
                {
                    ViewBag.id = StringCipher.Base64Encode(Convert.ToString(user.Id));
                    ViewBag.role = user.Role;
                    return View("ChangePassword");
                }
                return RedirectToAction("Login", new { error = "Couldn't find the User." });

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name+": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        [HttpPost]
        public ActionResult checkSession()
        {
            int value = 0;

            if (AuthenticateUser() == false)
            {
                value = 1;
            }
            else
            {
                value = 0;
            }
            
            return Json(value);
        }

    }
}