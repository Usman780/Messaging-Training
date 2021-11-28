using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;
using TrainingTracker.BL;
using TrainingTracker.Helper_Classes;

namespace TrainingTracker.Controllers
{
    public class FavoriteReportController : Controller
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        MainMailClass errormail = new MainMailClass();

        public ActionResult Error()
        {
            return View();
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

        [HttpPost]
        public ActionResult SaveStatusReport(string ReportName, string DivisionID, string DepartmentID, string WorkerTypeID, string TaskNameID, string TaskTypeID, string StatusID, string PriorityID, string StartDateID, string EndDateID, string FilterDate, string UserTypeId, string EmployeeID, int ReportType)
        {
            try
            {
                FavoriteReport fr = new FavoriteReport();

                if(ReportName != "" && ReportName != null)
                {
                    fr.Name = ReportName; 
                }

                if (DivisionID != "-1" && DivisionID != "" && DivisionID != null)
                {
                    fr.Division = DivisionID;
                }

                if (DepartmentID != "" && DepartmentID != null)
                {
                    fr.Department = DepartmentID;
                }

                if (WorkerTypeID != "" && WorkerTypeID != null)
                {
                    fr.WorkerType = WorkerTypeID;
                }

                if (TaskNameID != "" && TaskNameID != null)
                {
                    fr.TaskName = TaskNameID;
                }

                if (TaskNameID != "" && TaskNameID != null)
                {
                    fr.TaskName = TaskNameID;
                }

                if (TaskTypeID != "" && TaskTypeID != null)
                {
                    fr.TaskType = TaskTypeID;
                }

                if (StatusID != "-1" && StatusID != "" && StatusID != null)
                {
                    fr.Status = StatusID;                    
                }

                if (PriorityID != "" && PriorityID != null)
                {
                    fr.Priority = PriorityID;
                }

                if (StartDateID != "" && StartDateID != null)
                {
                    fr.StartingDate = StartDateID;
                }

                if (EndDateID != "" && EndDateID != null)
                {
                    fr.EndingDate = EndDateID;
                }

                if (FilterDate != "" && FilterDate != null)
                {
                    fr.FilterDate = FilterDate;
                }

                if (UserTypeId != "" && UserTypeId != null)
                {
                    fr.UserType = UserTypeId;
                }

                if (EmployeeID != "" && EmployeeID != null)
                {
                    fr.Employee = EmployeeID;
                }
                                
                fr.IsActive = 1;
                fr.IsStatic = 1;
                fr.User_Id = logedinuser.Id;
                fr.ReportType = ReportType;

                new FavoriteReportBL().AddFavoriteReport(fr);

                if (ReportType == 1)
                {
                    return RedirectToAction("StatusReportDivision", "Report", new { message = "Report is saved in your favorites successfully." });
                }

                else if (ReportType == 2)
                {
                    return RedirectToAction("StatusReportDepartment", "Report", new { message = "Report is saved in your favorites successfully." });
                }

                else if (ReportType == 3)
                {
                    return RedirectToAction("StatusReportEmployee", "Report", new { message = "Report is saved in your favorites successfully." });
                }

                else if (ReportType == 4)
                {
                    return RedirectToAction("UserTaskReport", "Report", new { message = "Report is saved in your favorites successfully." });
                }
                else
                {
                    return RedirectToAction("Index" , "Auth");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("StatusReportDivision", "Report", new { message = "Report is not saved because of an unexpected error." });
            }            
            
        }


        [HttpPost]
        public ActionResult UpdateStatusReport(string encodeID, string ReportName, string DivisionID, string DepartmentID, string FilterDate, string WorkerTypeID, string TaskNameID, string TaskTypeID, string StatusID, string PriorityID, string StartDateID, string EndDateID, string StartID, string EndID, string CompleteID, string UserTypeId, string EmployeeID, int ReportType)
        {
            try
            {

                FavoriteReport fr = new FavoriteReport();

                fr.Id = Convert.ToInt32(StringCipher.Base64Decode(encodeID));

                if (ReportName != "" && ReportName != null)
                {
                    fr.Name = ReportName;
                }

                if (DivisionID != "-1" && DivisionID != "" && DivisionID != null)
                {
                    fr.Division = DivisionID;
                }

                if (DepartmentID != "" && DepartmentID != null)
                {
                    fr.Department = DepartmentID;
                }

                if (WorkerTypeID != "" && WorkerTypeID != null)
                {
                    fr.WorkerType = WorkerTypeID;
                }

                if (TaskNameID != "" && TaskNameID != null)
                {
                    fr.TaskName = TaskNameID;
                }

                if (TaskTypeID != "" && TaskTypeID != null)
                {
                    fr.TaskType = TaskTypeID;
                }

                if (StatusID != "-1" && StatusID != "" && StatusID != null)
                {
                    fr.Status = StatusID;
                }

                if (PriorityID != "" && PriorityID != null)
                {
                    fr.Priority = PriorityID;
                }

                if (StartDateID != "" && StartDateID != null)
                {
                    fr.StartingDate = StartDateID;
                }

                if (EndDateID != "" && EndDateID != null)
                {
                    fr.EndingDate = EndDateID;
                }

                if (FilterDate != "" && FilterDate != null)
                {
                    fr.FilterDate = FilterDate;
                }

                if (UserTypeId != "" && UserTypeId != null)
                {
                    fr.UserType = UserTypeId;
                }

                if (EmployeeID != "" && EmployeeID != null)
                {
                    fr.Employee = EmployeeID;
                }

                fr.IsActive = 1;
                fr.IsStatic = 1;
                fr.User_Id = logedinuser.Id;
                fr.ReportType = ReportType;
                fr.CompanyId = Convert.ToInt32(logedinuser.Company);

                new FavoriteReportBL().UpdateFavoriteReport(fr);

                if (ReportType == 1)
                {
                    return RedirectToAction("StatusReportDivision", "Report", new { frId = StringCipher.Base64Encode(fr.Id.ToString()), message = "Report is updated in your favorites successfully." });
                }

                else if (ReportType == 2)
                {
                    return RedirectToAction("StatusReportDepartment", "Report", new { frId = StringCipher.Base64Encode(fr.Id.ToString()), message = "Report is updated in your favorites successfully." });
                }

                else if (ReportType == 3)
                {
                    return RedirectToAction("StatusReportEmployee", "Report", new { frId = StringCipher.Base64Encode(fr.Id.ToString()), message = "Report is updated in your favorites successfully." });
                }

                else if (ReportType == 4)
                {
                    return RedirectToAction("UserTaskReport", "Report", new { frId = StringCipher.Base64Encode(fr.Id.ToString()), message = "Report is updated in your favorites successfully." });
                }
                else
                {
                    return RedirectToAction("Index", "Auth");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("StatusReportDivision", "Report", new { message = "Report is not updated because of an unexpected error." });
            }

        }

        [HttpPost]
        public ActionResult ShareStatusReport(int ReportType, string encodeID,List<int> selectedUsers,string ReportDescription="")
        {
            try
            {


                int frId = Convert.ToInt32(StringCipher.Base64Decode(encodeID));
                DatabaseEntities de = new DatabaseEntities();

                FavoriteReport fReport = new FavoriteReportBL().getFavoriteReportById(frId, de);
                foreach(int item in selectedUsers)
                {
                    FavoriteReport obj = new FavoriteReport()
                    {
                        CompleteDate = fReport.CompleteDate,
                        Created_At = fReport.Created_At,
                        Department = fReport.Department,
                        Division = fReport.Division,
                        IsActive = fReport.IsActive,
                        Employee = fReport.Employee,
                        EndDate = fReport.EndDate,
                        EndingDate = fReport.EndingDate,
                        FilterDate = fReport.FilterDate,
                        IsStatic = fReport.IsStatic,
                        Name = fReport.Name,
                        WorkerType = fReport.WorkerType,
                        Priority = fReport.Priority,
                        ReportType = fReport.ReportType,
                        StartDate = fReport.StartDate,
                        StartingDate = fReport.StartingDate,
                        Status = fReport.Status,
                        TaskName= fReport.TaskName,
                        TaskType= fReport.TaskType,
                        UserType= fReport.UserType,
                        User_Id= item,
                        IsShared = 1,
                        SharedBy = logedinuser.Id,
                        SharedDescription = ReportDescription
                    };
                    
                   

                    new FavoriteReportBL().AddFavoriteReport(obj);

                }



                if (ReportType == 1)
                {
                    return RedirectToAction("StatusReportDivision", "Report", new { frId = StringCipher.Base64Encode(frId.ToString()), message = "Report has been shared successfully" });
                }

                else if (ReportType == 2)
                {
                    return RedirectToAction("StatusReportDepartment", "Report", new { frId = StringCipher.Base64Encode(frId.ToString()), message = "Report has been shared successfully" });
                }

                else if (ReportType == 3)
                {
                    return RedirectToAction("StatusReportEmployee", "Report", new { frId = StringCipher.Base64Encode(frId.ToString()), message = "Report has been shared successfully" });
                }

                else if (ReportType == 4)
                {
                    return RedirectToAction("UserTaskReport", "Report", new { frId = StringCipher.Base64Encode(frId.ToString()), message = "Report has been shared successfully." });
                }
                else
                {
                    return RedirectToAction("Index", "Auth");
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("StatusReportDivision", "Report", new { message = "Report is not shared because of an unexpected error." });
            }

        }


        [HttpPost]
        public ActionResult DeleteReport(string EncodedID)
        {
            try
            {
                DatabaseEntities de = new DatabaseEntities();
                FavoriteReport freport = new FavoriteReportBL().getFavoriteReportById(Convert.ToInt32(StringCipher.Base64Decode(EncodedID)),de);
                freport.IsActive = 0;
                //FavoriteReport fr = new FavoriteReport()
                //{
                //    Id = freport.Id,
                //    Name = freport.Name,
                //    Division = freport.Division,
                //    Department = freport.Department,
                //    TaskName = freport.TaskName,
                //    TaskType = freport.TaskType,
                //    Status = freport.Status,
                //    Priority = freport.Priority,
                //    StartingDate = freport.StartingDate,
                //    EndingDate = freport.EndingDate,
                //    StartDate = freport.StartDate,
                //    EndDate = freport.EndDate,
                //    CompleteDate = freport.CompleteDate,
                //    UserType = freport.UserType,
                //    Employee = freport.Employee,
                //    FilterDate = freport.FilterDate,
                //    IsActive = 0,
                //    User_Id = freport.User_Id,
                //    IsStatic = freport.IsStatic,
                //    ReportType = freport.ReportType,
                //    Created_At = freport.Created_At
                //};

                new FavoriteReportBL().UpdateFavoriteReport(freport,de);

                if (freport.ReportType == 1)
                {
                    return RedirectToAction("StatusReportDivision", "Report", new { message = "Report is deleted from your favorites successfully." });
                }

                else if (freport.ReportType == 2)
                {
                    return RedirectToAction("StatusReportDepartment", "Report", new { message = "Report is deleted from your favorites successfully." });
                }

                else if (freport.ReportType == 3)
                {
                    return RedirectToAction("StatusReportEmployee", "Report", new { message = "Report is deleted from your favorites successfully." });
                }

                else if (freport.ReportType == 4)
                {
                    return RedirectToAction("UserTaskReport", "Report", new { message = "Report is deleted from your favorites successfully." });
                }
                else
                {
                    return RedirectToAction("Index", "Auth");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("StatusReportDivision", "Report", new { message = "Report is not deleted because of an unexpected error." });
            }
        }
    }
}