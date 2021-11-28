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
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.Controllers
{
    public class TraineeController : Controller
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
        // GET: Trainee
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

        public ActionResult updateProfile(string way = "")
        {
            try
            {


                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                int role = logedinuser.Role;
                int theid = logedinuser.Id;
                if (role == (int)Enums.Role.Trainee)
                {
                    return RedirectToAction("UpdateEmployee", "utilities", new { sid = General_Purpose.EncryptId(theid), way = way });
                }
                else if (role == (int)Enums.Role.Admin)
                {
                    return RedirectToAction("UpdateAdmin", "utilities", new { sid = General_Purpose.EncryptId(theid) });
                }
                else if (role == (int)Enums.Role.Cordinator)
                {
                    return RedirectToAction("UpdateManager", "utilities", new { sid = General_Purpose.EncryptId(theid), way = "self" });
                }
                else if (role == (int)Enums.Role.Manager)
                {
                    return RedirectToAction("UpdateManager", "utilities", new { sid = General_Purpose.EncryptId(theid), way = "self" });
                }
                return null;
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult downloadFile(int id)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                Certificate certificate = new CertificateBL().getCertificateById(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~") + ProjectVaraiables.IMAGEPATH + certificate.Path);
                string fileneame = certificate.Path;

                return File(Server.MapPath("~") + ProjectVaraiables.IMAGEPATH + certificate.Path, MediaTypeNames.Text.Plain, certificate.Path);
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult download(string filePath, string name)
        {
            try
            {
                BlobManager BlobManagerObj = new BlobManager();
                CloudBlob cbb = BlobManagerObj.getCloudBlockBlob(filePath);

                Stream blobStream = cbb.OpenRead();

                if (name != null && name != "")
                {
                    name = name + "." + filePath.Split('.')[1];
                    return File(blobStream, cbb.Properties.ContentType, name);

                }
                return File(blobStream, cbb.Properties.ContentType, filePath);
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult DownloadBlobFile(string filePath, string name,string u,string fid="")
        {
            try
            { 
            if (fid != "")
            {
                int Id = General_Purpose.DecryptId(fid);
                DatabaseEntities db = new DatabaseEntities();
                FileDownloadLink fdl = new FileDownloadLinkBL().getFileDownloadLinkById(Id, db);
                fdl.IsDownloaded = 1;
                new FileDownloadLinkBL().UpdateFileDownloadLink(fdl, db);

               General_Purpose.SendReportDownloadNotification((int)fdl.UserId, fdl.ReportTitle, fdl.ReportLink, fdl.CreatedAt.Value.ToString("MM/dd/yyyy hh:mm tt"));//Temporary Commented
            }
           // Models.FileVersion file = new FileVersionBL().getFileVersionById(General_Purpose.DecryptId(v));
            BlobManager BlobManagerObj = new BlobManager();
            //AzureBlobDownloadInfo cb = BlobManagerObj.getCloudBlockBlob(file.FilePath);
            if (name != null && name != "")
            {
                name = name + "." + filePath.Split('.')[1];
               // return File(blobStream, cbb.Properties.ContentType, name);

            }
            AzureBlobDownloadInfo cb = BlobManagerObj.getCloudBlockBlob(filePath,name,u);
            DateTime dt = DateTime.Now;
            //File Log entry (Download case could be change in future)
           
                return Redirect(cb.RedirectUri);

                //  return Content("Error in downloading file!");
                //return Redirect(cb.RedirectUri);
                //Stream blobStream = cb.OpenRead();
                //return File(blobStream, cb.Properties.ContentType, file.Name);
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }

        public ActionResult downloadmailfile(string filePath, string name, int companyid)
        {
            try
            {
                BlobManager BlobManagerObj = new BlobManager(null, companyid);
                CloudBlob cbb = BlobManagerObj.getCloudBlockBlob(filePath);

                Stream blobStream = cbb.OpenRead();

                return File(blobStream, cbb.Properties.ContentType, filePath);
            }
            catch (Exception ex)
            {
                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Trace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");
            }
        }



        public ActionResult uploadFile(string name, int userId, string way = "")
        {
            try
            {

                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                {
                    int count = Request.Files.Count;
                    var file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        Certificate c = new Certificate();
                        int id = logedinuser.Id;
                        BlobManager BlobManagerObj = new BlobManager();
                        c.Name = name;
                        c.Path = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                        string FileAbsoluteUri = BlobManagerObj.UploadFile(file, c.Path);



                        User trainee = new UserBL().getUsersById(id);
                        c.IsActive = 1;
                        c.UserID = userId;
                        c.CreatedAt = DateTime.Now;
                        string path = trainee.Id + file.FileName;
                        new CertificateBL().AddCertificate(c);
                    }
                    if (logedinuser.Id != userId)
                    {

                        User currentUser = new UserBL().getUsersById(userId);
                        if (currentUser.Role == 3)
                        {
                            string v = "";
                            if(way == "setupDep" || way == "Department" || way == "Structure")
                            {
                                v = way;
                            }
                            else
                            {
                                v = "Manager";
                            }
                            return RedirectToAction("UpdateEmployee", "Utilities", new { sid = General_Purpose.EncryptId(userId), way = v });

                        }
                        else if (currentUser.Role != 1)
                        {
                            if (way == "Structure")
                            {
                                return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(userId), division="yes", message = "Document Uploaded Successfully", way = way });
                            }
                            else
                            {
                                return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(userId), way = "DisplayManager" });
                            }
                        }
                        else
                        {
                            return RedirectToAction("updateProfile", new { way = way });
                        }

                    }


                    return RedirectToAction("updateProfile", new { way = way });
                }

            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult deleteFile(int id, string way, int userId)
        {
            try
            { 
                DatabaseEntities de = new DatabaseEntities();
                CertificateBL cbl = new CertificateBL();
                cbl.DeleteCertificate(id, de);
                if (logedinuser.Id != userId)
                {

                    User currentUser = new UserBL().getUsersById(userId);
                    if (currentUser.Role == 3)
                    {
                        string v = "";
                        if (way == "setupDep" || way == "Department" || way == "Structure")
                        {
                            v = way;
                        }
                        else
                        {
                            v = "Manager";
                        }
                        return RedirectToAction("UpdateEmployee", "Utilities", new { sid = General_Purpose.EncryptId(userId), way = v });

                    }
                    else if (currentUser.Role != 1)
                    {
                        if (way == "Structure")
                        {
                            return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(userId), division = "yes", message = "Document Deleted Successfully", way = way });
                        }
                        else
                        {
                            return RedirectToAction("UpdateManager", "Utilities", new { sid = General_Purpose.EncryptId(userId), way = "DisplayManager" });
                        }
                    }
                    else
                    {
                        return RedirectToAction("updateProfile", new { way = way });
                    }

                }

                return RedirectToAction("updateProfile", new { way = way });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }


        }


        public ActionResult displayEmployeeTask(string startDate = "", string endDate = "", string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1)
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
                List<User_Task> tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == logedinuser.Id).ToList();
                if (Request.Form["start"] != null)
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
                if (Request.Form["end"] != null)
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
                if (Request.Form["complete"] != null)
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

                if (status != -1)
                {
                    tasks = tasks.Where(x => x.Status == status).ToList();
                }
                if (priority != -1)
                {
                    tasks = tasks.Where(x => x.Priority == priority).ToList();
                }
                ViewBag.tasks = tasks;
                ViewBag.startDate = startDate;
                ViewBag.endDate = endDate;
                ViewBag.status = status;

                ViewBag.taskName = name;
                ViewBag.taskType = new TaskTypeBL().getTaskTypesById(taskType);
                ViewBag.taskTypes = new TaskTypeBL().getTaskTypesList().OrderBy(s => s.Name).ToList();
                ViewBag.statuses = new List<int>() { 0, 1, 2, 3, 4 }.Where(x => x != status);
                ViewBag.priority = priority;
                ViewBag.priorities = new List<int>() { 0, 1, 2 }.Where(x => x != priority);
                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult GetdisplayEmployeeTask(string startDate = "", string endDate = "", string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int start = 0, int end = 0, int complete = 0, int isCompleted = -1)
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
                List<User_Task> tasks = new List<User_Task>();

                if(isCompleted != -1)
                {
                    tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == logedinuser.Id && x.CompletionDate != null).ToList();
                }
                else
                {
                    tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == logedinuser.Id && x.CompletionDate == null).ToList();
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

                if (status != -1)
                {
                    tasks = tasks.Where(x => x.Status == status).ToList();
                }
                if (priority != -1)
                {
                    tasks = tasks.Where(x => x.Priority == priority).ToList();
                }
                int start1 = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];



                int totalrows = tasks.Count();
                //filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    tasks = tasks.Where(x => x.Task.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                int totalrowsafterfilterinig = tasks.Count();
                //sorting

                // pagination
                tasks = tasks.Skip(start1).Take(length).ToList();

                //List<User> mnglist = new List<User>();
                List<EmployeeDTO> mnglist = new List<EmployeeDTO>();

                string priority1 = "";
                string status1 = "";
                string AssignedBy = "";
                int temp = 0;

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
                            priority1 = "<span class='mediumPriorityButton'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                        }
                        else if (General_Purpose.getPriorityValue(x.Priority.Value) == "Low")
                        {
                            priority1 = "<span class='lowPriorityButton'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                        }
                        else
                        {
                            priority1 = "<span class='highPriorityButton'>" + General_Purpose.getPriorityValue(x.Priority.Value) + "</span>";
                        }

                    }
                    else
                        priority1 = "";

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
                        Priority = priority1,
                        WorkStatus = status1,
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

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public JsonResult GetdisplayEmpTaskBySearch(string startDate = "", string endDate = "", string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1, int start = 0, int end = 0, int complete = 0)
        {
            int id = logedinuser.Id;
            List<User_Task> tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == logedinuser.Id).ToList();
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

            if (status != -1)
            {
                tasks = tasks.Where(x => x.Status == status).ToList();
            }
            if (priority != -1)
            {
                tasks = tasks.Where(x => x.Priority == priority).ToList();
            }

            List<EmployeeDTO> mnglist = new List<EmployeeDTO>();

            string priority1 = "";
            string status1 = "";
            string AssignedBy = "";
            int temp = 0;

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
                    Priority = priority1,
                    WorkStatus = status1,
                    IsActive = temp,
                    Id = x.Id,
                    EncryptedId = General_Purpose.EncryptId(x.Id)

                };
                mnglist.Add(obj);
                temp = 0;


            }


            return Json(mnglist, JsonRequestBehavior.AllowGet);

        }

        public ActionResult displayEmployeeGroupTask(string startDate = "", string endDate = "", string name = "", int status = -1, string startcompletionDate = "", int taskType = 0)
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
                List<GroupTask_User> gTasks = new GroupTask_UserBL().getGroupTask_UsersList().Where(x => x.UserId == id).ToList();
                gTasks = gTasks.Where(x => x.GroupTasks_Details.IsActive == 1).ToList();

                if (Request.Form["start"] != null)
                {
                    if (startDate != "")
                    {
                        gTasks = gTasks.Where(x => x.GroupTasks_Details.StartDate != null && (x.GroupTasks_Details.StartDate > DateTime.Parse(startDate))).ToList();
                    }

                    if (endDate != "")
                    {
                        gTasks = gTasks.Where(x => x.GroupTasks_Details.StartDate != null && (x.GroupTasks_Details.StartDate < DateTime.Parse(endDate))).ToList();
                    }

                    ViewBag.sd = 1;
                }
                if (Request.Form["end"] != null)
                {
                    if (startDate != "")
                    {
                        gTasks = gTasks.Where(x => x.GroupTasks_Details.EndDate != null && (x.GroupTasks_Details.EndDate > DateTime.Parse(startDate))).ToList();
                    }

                    if (endDate != "")
                    {
                        gTasks = gTasks.Where(x => x.GroupTasks_Details.EndDate != null && (x.GroupTasks_Details.EndDate < DateTime.Parse(endDate))).ToList();
                    }
                    ViewBag.ed = 1;
                }
                if (Request.Form["complete"] != null)
                {
                    if (startDate != "")
                    {
                        gTasks = gTasks.Where(x => x.GroupTasks_Details.CompletionDate != null && (x.GroupTasks_Details.CompletionDate > DateTime.Parse(startDate))).ToList();
                    }

                    if (endDate != "")
                    {
                        gTasks = gTasks.Where(x => x.GroupTasks_Details.CompletionDate != null && (x.GroupTasks_Details.CompletionDate < DateTime.Parse(endDate))).ToList();
                    }
                    ViewBag.cd = 1;
                }

                if (name != "")
                {
                    gTasks = gTasks.Where(x => x.GroupTasks_Details.GroupTask.Name.ToUpper().Contains(name.ToUpper())).ToList();
                }

                if (status != -1)
                {
                    gTasks = gTasks.Where(x => x.GroupTasks_Details.Status == status).ToList();
                }

                ViewBag.startDate = startDate;
                ViewBag.endDate = endDate;
                ViewBag.status = status;
                ViewBag.managers = new UserBL().getUsersList().Where(x => x.Role == 2 || x.Role == 4).OrderBy(s => s.FirstName).ToList();

                ViewBag.taskName = name;
                ViewBag.taskType = new TaskTypeBL().getTaskTypesById(taskType);
                ViewBag.taskTypes = new TaskTypeBL().getTaskTypesList().OrderBy(s => s.Name).ToList();
                ViewBag.statuses = new List<int>() { 0, 1, 2, 3, 4 }.Where(x => x != status);
                ViewBag.gtasks = gTasks;
                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult uploadBulkEmployee(int department, int isDepartpage = 0)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                else if (logedinuser.Role == (int)(Enums.Role.Manager) || logedinuser.Role == (int)(Enums.Role.Admin) || logedinuser.Role == (int)(Enums.Role.Cordinator))
                {
                    int count = Request.Files.Count;
                    var file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        int id = logedinuser.Id;

                        string path = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);

                        path = Server.MapPath("~") + ProjectVaraiables.IMAGEPATH + path;
                        file.SaveAs(path);
                        new General_Purpose().addEmployeViaExcel(path, department, logedinuser.Id);
                    }

                    if (isDepartpage == 1)
                    {
                        return RedirectToAction("displayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(department), message = "Upload Successfull!" });
                    }

                    return RedirectToAction("DisplayEmployee", "Utilities");
                }
                return Content("Invalid Access");
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult downloadSampleFile()
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~") + @"\Content\SmapleFile\sampleFile.xlsx");
                string fileneame = "sampleFile.xlsx";

                return File(Server.MapPath("~") + @"\Content\SmapleFile\sampleFile.xlsx", MediaTypeNames.Text.Plain, fileneame);
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult addTraineTask(string Time, int taskId, int priority, int? cost, int? freqDays, float? ceu, float? hours, int? Grad, int? CanvasCourseId, DateTime? startDate, DateTime? endDate, int days = -1, string notes = null, int traineeId = -1, int departmentId = -1, string FeqEndingDate = "", string way = "",int DReminder=-1)
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
                    UserID = traineeId,
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
                if (id != traineeId)
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
                        return RedirectToAction("displayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(departmentId), message = "Task couldn't be assigned to Employee because End date was not entered.", way = way });

                    }
                    tm.EndDate = endDate.Value;
                    DateTime dateTime = DateTime.Now;
                    DateTime timeValue = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
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
                        return RedirectToAction("displayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(departmentId), message = "Task has not been assigned because you have not set frequency ending date.", way = way });

                    }
                    DateTime FEndingDate = Convert.ToDateTime(FeqEndingDate);

                    if (FEndingDate < Convert.ToDateTime(tm.StartDate))
                    {
                        return RedirectToAction("displayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(departmentId), message = "Task has not been assigned because task's start date is less than frequency ending date.", way = way });

                    }
                    if (DReminder != -1)
                    {
                        tm.DailyReminder = DReminder;
                    }
                    tm = new User_TaskBL().AddUser_Tasks(tm);
                    if (CanvasCourseId != null)
                    {
                        General_Purpose.AssignLMSCourse(tm.Id, (int)CanvasCourseId);
                    }
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

                return RedirectToAction("displayDepartmentDetails", "Utilities", new { sid = General_Purpose.EncryptId(departmentId), message = "Task has been assigned to Employee.", way = way });
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        public ActionResult deleteTraineeTask(int id, int isCompleted = -1)
        {
            try
            {
                if (AuthenticateUser() == false)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                new User_TaskBL().DeleteUser_Tasks(id);

                if (isCompleted != -1)
                {
                    return RedirectToAction("DisplayCompletedEmployeeTask");
                }
                else
                {
                    return RedirectToAction("displayEmployeeTask");
                }
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }

        [HttpPost]
        public string EmployeeDepartmentTask()
        {

            User loggedinuser = new UserBL().getUsersById(logedinuser.Id);
            List<Task> alltasks = new TaskBL().getAllTasksList().Where(x => x.DepartmentID == loggedinuser.DepartmentId && x.IsPrivate == 0 && x.IsActive == 1).ToList();
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

        [HttpPost]
        public string getEmployeeAssignTaskDTO(string taskId)
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

        public ActionResult assigntaskEmployee(string employeetimelineDate, string Time, int? priority, int? cost, int? freqDays, float? ceu, float? hours, int? Grad, int? CanvasCourseId, int taskId = -1, int days = -1, string notes = null, string managerId = null, string employeeId = null, int divisionId = -1, string newselftaskname = "", int newselftasktype = -1, string FeqEndingDate = "", string EndDate = "", string StartDate = "", int DReminder=-1)
        {
            try
            {
                if (logedinuser.Name == null && logedinuser.Id != 3)
                {
                    return RedirectToAction("login", "Auth", new { error = "Timeout! Please Login Again" });
                }
                if (taskId == -1)
                {
                    return RedirectToAction("Index", "Auth", new { message = "Task not be null.Task didn't add successfully." });

                }


                if (taskId == 0)
                {

                    User us = new UserBL().getUsersById(logedinuser.Id);



                    Department dept = new DepartmentBL().getDepartmentsById((int)us.DepartmentId);

                    if (newselftasktype == -1)
                    {
                        return RedirectToAction("Index", "Auth", new { message = "Task Type not be null.Task didn't add successfully." });
                    }
                    int taskcount = 0;
                    if (newselftaskname != "")
                    {
                        taskcount = dept.Tasks.Where(x => x.IsActive == 1 && x.Name.ToUpper().Equals(newselftaskname.ToUpper())).Count();
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
                            DepartmentID = (int)us.DepartmentId,
                            TaskTypeID = newselftasktype,
                            Name = newselftaskname,
                            CEU = ceu,
                            Cost_ = cost,
                            Hours = hours,
                            Description = notes,
                           // CourseId = CanvasCourseId
                        };


                        taskId = new TaskBL().AddTasks(obj);
                        //int count = Request.Files.Count;
                        // BlobManager BlobManagerObj = new BlobManager();

                        //for (int i = 0; i < count; i++)
                        //{
                        //    var file = Request.Files[i];
                        //    if (file.ContentLength > 0)
                        //    {



                        //        string path = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);

                        //        string FileAbsoluteUri = BlobManagerObj.UploadFile(file, path);



                        //        new TaskFileBL().AddTaskFile(
                        //            new TaskFile()
                        //            {
                        //                Name = file.FileName,
                        //                Path = path,
                        //                IsActive = 1,
                        //                TaskId = taskId
                        //            });
                        //    }
                        //}





                        //  return RedirectToAction("Index", "Auth", new {message = "Task has been added." });


                    }


                }

                ///////////////////////////////
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

                //DateTime TimelineDate = Convert.ToDateTime(employeetimelineDate);
                DateTime STimelineDate = Convert.ToDateTime(StartDate);//Custom Date
                DateTime ETimelineDate = Convert.ToDateTime(EndDate).AddDays(1);//Custom End Date
                ETimelineDate = ETimelineDate.AddMinutes(-1);
                DateTime dateTime = DateTime.Now;
                DateTime timeValue = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0); ;
                if (Time != null)
                {
                    timeValue = Convert.ToDateTime(Time);
                    DateTime drt = new DateTime(ETimelineDate.Year, ETimelineDate.Month, ETimelineDate.Day, timeValue.Hour, timeValue.Minute, timeValue.Second);
                    ETimelineDate = drt;
                }

               

                User_Task tm = new User_Task()
                {
                    UserID = logedinuser.Id,
                    TaskID = taskId,
                    Priority = priority,
                    Cost = cost,
                    CEU = ceu,
                    Hours = hours,
                    Grad = Grad,
                    Notes = notes,
                    StartDate = STimelineDate,
                    //EndDate = ETimelineDate,
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
                    tm.EndDate = STimelineDate;
                    tm.RepeatTime = days;
                }
                else
                {
                    tm.EndDate = ETimelineDate;
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
                    tm.CreatedAt = DateTime.Now;
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
                            ParentID = tm.Id,
                            CreatedAt= DateTime.Now

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
                     //   dt = dt.AddDays(days);


                    }

                }




                tm.User1 = User;
                //Canvas Course Assinging mail
                //if (CanvasCourseId != null)
                //{
                //    User admin = new UserBL().getUsersById(id);
                //    Course cr = General_Purpose.GetCanvasCourse().Where(x => x.id == CanvasCourseId).FirstOrDefault();
                //    string rle = "";

                //    if (User.Role == 2 || User.Role == 4)
                //        rle = "Manager";
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

                //    string content1 = "Dear " + admin.FirstName + " " + admin.LastName + ",\n You assigned Canvas Course " + '"' + cr.name + '"' + " to " + rle + " " + '(' + User.FirstName + " " + User.LastName + ')' + " please ask your Admin to send you a Course assigning invitation from his Canvas Account on this Email " +
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

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        #region Completed Assignments

        public ActionResult DisplayCompletedEmployeeTask(string startDate = "", string endDate = "", string name = "", int status = -1, string startcompletionDate = "", int taskType = 0, int priority = -1)
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
                List<User_Task> tasks = new User_TaskBL().getUser_TasksList().Where(x => x.User1.Id == logedinuser.Id && x.CompletionDate != null).ToList();
                if (Request.Form["start"] != null)
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
                if (Request.Form["end"] != null)
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
                if (Request.Form["complete"] != null)
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

                if (status != -1)
                {
                    tasks = tasks.Where(x => x.Status == status).ToList();
                }
                if (priority != -1)
                {
                    tasks = tasks.Where(x => x.Priority == priority).ToList();
                }
                ViewBag.tasks = tasks;
                ViewBag.startDate = startDate;
                ViewBag.endDate = endDate;
                ViewBag.status = status;

                ViewBag.taskName = name;
                ViewBag.taskType = new TaskTypeBL().getTaskTypesById(taskType);
                ViewBag.taskTypes = new TaskTypeBL().getTaskTypesList().OrderBy(s => s.Name).ToList();
                ViewBag.statuses = new List<int>() { 0, 1, 2, 3, 4 }.Where(x => x != status);
                ViewBag.priority = priority;
                ViewBag.priorities = new List<int>() { 0, 1, 2 }.Where(x => x != priority);
                return View();
            }
            catch (Exception ex)
            {

                errormail.Remindermail(ProjectVaraiables.DeveloperMail, ex.Message.ToString() + "  \n Site Tace:: " + ex.StackTrace.ToString(), logedinuser.Name + ": Error in Zuptu");
                return RedirectToAction("Error");

            }
        }


        #endregion
    }
}