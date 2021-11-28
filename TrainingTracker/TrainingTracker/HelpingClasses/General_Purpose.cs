using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using RestSharp;
using RusticiSoftware.HostedEngine.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using TrainingTracker.BL;
using TrainingTracker.DataHub;
using TrainingTracker.Models;

namespace TrainingTracker.HelpingClasses
{
    public class General_Purpose
    {
         // CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

        public static bool AuthenticateUser(CheckAuthenticationDTO logUser)
        {
            if (logUser.Name == null)
            {
                return false;
            }

            User user = new UserBL().getUsersById(logUser.Id);

            if (user == null)
            {
                return false;
            }

            if (logUser.Role != user.Role)
            {
                return false;
            }

            return true;
        }

        #region SignalR Notification
        public static void SendExtensionRequestNotification()
        {
            CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.broadcastExtensionNotification("1", Convert.ToInt32(logedinuser.Company));
        }

        public static void SendReportDownloadNotification(int userId,string ReportName,string DownloadLink,string datetime)
        {
            
            //Notifiy User
            string datetime2 = datetime;
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.broadcastReportNotification("1", userId,DownloadLink,ReportName, datetime2);
        }
        #endregion
        //public static List<ExtensionRequest> GetActiveExtensionRequests()
        //{
        //    List<ExtensionRequest> newlist = new List<ExtensionRequest>();
        //    List<ExtensionRequest> listex = new ExtensionRequestBL().getExtensionRequestsList().Where(x => x.Status == 0 && x.isActive == 1 && x.Displayed == 0).ToList();
        //    listex.Select(x => x.User_TaskId).Distinct();
        //    listex.Select(x => x.GroupTaskDetails_Id).Distinct();
        //    List<int> ulist = new List<int>();
        //    List<int> gtlist = new List<int>();
        //    foreach (var item in listex)
        //    {
        //        if (item.User_TaskId != null)
        //        {
        //            if (ulist.Contains((int)item.User_TaskId))
        //            {
        //                continue;
        //            }
        //            ulist.Add((int)item.User_TaskId);
        //        }
        //        else
        //        {
        //            if (gtlist.Contains((int)item.GroupTaskDetails_Id))
        //            {
        //                continue;
        //            }
        //            gtlist.Add((int)item.GroupTaskDetails_Id);
        //        }

        //        newlist.Add(item);
        //    }
        //    return newlist;
        //}
        public static List<string> GetBlobFilePath(string path,int id)
        {
            User u = new UserBL().getUsersById((int)id);
            BlobManager BlobManagerObj = new BlobManager(ProjectVaraiables.COMPANY_ALPHABET + Convert.ToInt32(u.CompanyID));
            string contentType = MimeMapping.GetMimeMapping(path);
            FileStream fs = System.IO.File.Open(path, FileMode.Open);
            string fileName = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(path);
            string FileAbsoluteUri = BlobManagerObj.UploadFileFromStream(fs, fileName, contentType);
            //string fileurl = BlobManager.GetBlobSasUri(fileName, logedinuser.Company.ToString());
            List<string> objlist = new List<string>();
            objlist.Add(fileName);
            objlist.Add(Path.GetFileName(path));
            return objlist;
        }

        public static List<ExtensionRequest> GetActiveExtensionRequests()
        {
            List<ExtensionRequest> newlist = new List<ExtensionRequest>();
            List<ExtensionRequest> listex = new ExtensionRequestBL().getExtensionRequestsList().Where(x => x.isActive == 1 && x.Displayed == 0).ToList();
            listex.Select(x => x.User_TaskId).Distinct();
            listex.Select(x => x.GroupTaskDetails_Id).Distinct();
            List<int> ulist = new List<int>();
            List<int> gtlist = new List<int>();
            foreach (var item in listex.OrderByDescending(x => x.Id))
            {
                if (item.User_TaskId != null)
                {
                    if (ulist.Contains((int)item.User_TaskId))
                    {
                        continue;
                    }
                    ulist.Add((int)item.User_TaskId);
                }
                else
                {
                    if (gtlist.Contains((int)item.GroupTaskDetails_Id))
                    {
                        continue;
                    }
                    gtlist.Add((int)item.GroupTaskDetails_Id);
                }
                newlist.Add(item);
            }
            return newlist.Where(x => x.Status == 0 && x.Displayed == 0).ToList();
        }


        public static string generateoptionScript(int value)
        {
            string text = "<option value='1' selected > " + getStatusValue(value) + "</option>";

            for (int i = 0; i < 5; i++)
            {
                if (i != value)
                {
                    text = text + "<option value='1'  > " + getStatusValue(i) + "</option>";

                }
            }
            return text;
        }


        public static string getStatusValue(int id)
        {
            switch (id)
            {
                case 0:
                    return "Started";
                case 1:

                    return "25%";

                case 2:
                    return "50%";
                case 3:
                    return "75%";
                case 4:
                    return "100%";

                default:
                    return "Started";

            }
        }
        public static string getPriorityValue(int value)
        {

            switch (value)
            {
                case 0:
                    return "Low";
                case 1:

                    return "Medium";

                case 2:
                    return "High";

                default:
                    return "Not Selected";

            }
        }





        public void addEmployeViaExcel(string path, int department, int managerId)
        {
            UserBL traineeBL = new UserBL();
            MainMailClass mmc = new MainMailClass();

            List<User> users = new UserBL().getUsersList();
            try
            {
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
                        // Or... via each row
                        foreach (Row row in rows)
                        {
                            User trainee = new User();
                            if (i == 9999) { break; }
                            if (i > 0)
                            {
                                int ii = 0;

                                foreach (Cell c in row.Elements<Cell>())
                                {

                                    string str;
                                    if (ii == 11) { break; }
                                    if (ii == 3)
                                    {
                                        str = c.InnerText;
                                        str = str.Replace("-", "");
                                        trainee.PhoneNumber = str;
                                    }
                                    else if (ii == 4)
                                    {
                                        str = c.InnerText;
                                        str = str.Replace("-", "");
                                        trainee.HomeNumber = str;

                                    }
                                    else if (c.InnerText == "")
                                    {
                                        str = null;
                                    }
                                    else
                                    {
                                        if (c.DataType == CellValues.SharedString)
                                        {

                                            int ssid = Convert.ToInt32(c.CellValue.Text);
                                            str = sst.ChildElements[ssid].InnerText;

                                        }
                                        else
                                            str = c.CellValue.Text;
                                    }
                                    if (ii == 0)
                                    {
                                        if (str == null)
                                        {
                                            break;
                                        }
                                        trainee.Email = str;
                                    }
                                    if (ii == 1)
                                    {
                                        if (str == null)
                                        {
                                            break;
                                        }
                                        trainee.FirstName = str;
                                    }
                                    if (ii == 2)
                                    {
                                        if (str == null)
                                        {
                                            break;
                                        }
                                        trainee.LastName = str;
                                    }


                                    if (ii == 5)
                                    {
                                        trainee.Notes = str;
                                    }
                                    if (ii == 6)
                                    {

                                    }
                                    if (ii == 7)
                                    {

                                    }
                                    if (ii == 8)
                                    {

                                    }
                                    if (ii == 9)
                                    {

                                    }


                                    ii++;
                                }
                                if ((users.Where(x => x.Email.ToUpper().Equals(trainee.Email.ToUpper())).Count() == 0))
                                {
                                    CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
                                    trainee.IsActive = 1;

                                    trainee.DepartmentId = department;

                                    trainee.Role = 3;
                                    trainee.Image = @"\TrainingTracker\Content\Images\avt.png";
                                    trainee.IsActive = 2;
                                    //employee.ManagerID = (int)Session["Id"];
                                    trainee.Password = "--";
                                    trainee.isMail = 1;
                                    trainee.isSlack = 1;
                                    trainee.isSMS = 1;
                                    trainee.DepartmentId = department;
                                    trainee.Role = 3;
                                    trainee.CompanyID = Convert.ToInt32(logedinuser.Company);


                                    traineeBL.AddUsers(trainee);
                                    mmc.inviteUser(trainee.Email, trainee.Id);



                                }

                            }
                            i++;
                        }
                    }
                }
            }



            catch (Exception e)
            { }


        }
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        public static string SizeSuffix(long value, int decimalPlaces = 0)
        {
            if (value < 0)
            {
                throw new ArgumentException("Bytes should not be negative", "value");
            }
            var mag = (int)Math.Max(0, Math.Log(value, 1024));
            var adjustedSize = Math.Round(value / Math.Pow(1024, mag), decimalPlaces);
            return String.Format("{0} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        public static string Encrypt(int id)
        {
            string stringToEncrypt = id.ToString();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            byte[] rgbIV = { 0x21, 0x43, 0x56, 0x87, 0x10, 0xfd, 0xea, 0x1c };
            byte[] key = { };
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes("A0D1nX0Q");
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, rgbIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static int Decrypt(string EncryptedText)
        {
            byte[] inputByteArray = new byte[EncryptedText.Length + 1];
            byte[] rgbIV = { 0x21, 0x43, 0x56, 0x87, 0x10, 0xfd, 0xea, 0x1c };
            byte[] key = { };

            try
            {
                key = System.Text.Encoding.UTF8.GetBytes("A0D1nX0Q");
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(EncryptedText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, rgbIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return Convert.ToInt32(encoding.GetString(ms.ToArray()));
            }
            catch (Exception e)
            {
                return 0;
            }
        }


        public static string getTaskColor(int value = -1)
        {

            switch (value)
            {
                case 0:
                    return "#2E7D32";
                case 1:
                    return "#D84315";
                case 2:
                    return "#0072C6";
                default:
                    return "#C62828";

            }




        }
        public static string generateUrl(int id, int manager = 0, int individual = 0,int TaskorGT=0,int UserRole=0)
        {
            CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
            User_Task ut = new User_TaskBL().getUser_TasksById(id);
            string encryptedId = General_Purpose.EncryptId(id);
            string url = "#z=" + id + "&individual=" + individual + "&manager=" + manager;
           
            if (TaskorGT == 1)
            {
               // if ((DepartmentID != "" || DivisionID != "") && logedinuser.Role == 1)
                    if ((!string.IsNullOrEmpty(ut.DepartmentID) || !string.IsNullOrEmpty(ut.DivisionID)) && logedinuser.Role == 1)
                {
                    string i = "";
                    if (!string.IsNullOrEmpty(ut.DepartmentID))
                    {
                        i = ut.DepartmentID;
                    }
                    else
                    {
                        i = ut.DivisionID;
                    }
                    url = ProjectVaraiables.SYSTEM_URL + "/Task/assignments?DivDepID=" + i + "&isModal=01";
                }
                else
                {

                    if (UserRole == 3)
                    {
                        url = ProjectVaraiables.SYSTEM_URL + "/Task/taskdetails?v=" + encryptedId + "&isModal=01";
                    }
                    else
                    {
                        url = ProjectVaraiables.SYSTEM_URL + "/Manager/taskdetails?v=" + encryptedId + "&isModal=01";

                    }
                }
            }else if (TaskorGT == 2)
            {
                url = ProjectVaraiables.SYSTEM_URL + "/Task/grouptaskdetails?v=" + encryptedId + "&isModal=01";
            }
            //string url = ProjectVaraiables.SYSTEM_URL + "/task/taskLoaderCalendar?z=" + id + "&individual=" + individual + "&manager=" + manager;
            
            return url;
        }

        public static string generateOldUrl(int id, int manager = 0, int individual = 0)
        {
           // CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
         //   User_Task ut = new User_TaskBL().getUser_TasksById(id);
           // string encryptedId = General_Purpose.EncryptId(id);
            string url = "#z=" + id + "&individual=" + individual + "&manager=" + manager;

          
            return url;
        }


        public static string getTaskDisplay(int value = 1)
        {
            switch (value)
            {
                case 1:
                    return "My Tasks";
                case 2:
                    return "My Created Tasks";
                case 3:
                    return "All Tasks";
                default:
                    return "All Tasks";

            }




        }



        public static CheckAuthenticationDTO CheckAuthentication()
        {
          
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            CheckAuthenticationDTO auth = new CheckAuthenticationDTO();

            auth.Role = Convert.ToInt32(identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault());

            
            auth.SlackAddress = identity.Claims.Where(c => c.Type == "SlackAddress").Select(c => c.Value).FirstOrDefault();
            auth.Company = identity.Claims.Where(c => c.Type == "Company").Select(c => c.Value).FirstOrDefault();
            auth.Id = Convert.ToInt32(identity.Claims.Where(c => c.Type == "Id").Select(c => c.Value).FirstOrDefault());
            if (auth.Id != 0)
            {
               // var response = HttpContext.Current.Response;

               // string Zuptu_Id = string.Empty;
               // Zuptu_Id = response.Cookies["Zuptu_Id"].Value;
               // response.Cookies.Remove("Zuptu_Id");

               //     System.Web.HttpCookie userInfo = new System.Web.HttpCookie("Zuptu_Id");
               //     userInfo.Value = auth.Id.ToString();
               //     // userInfo["UserColor"] = "Black";
               //     //userInfo.Expires.Add(new TimeSpan(0, 1, 0));
               //     response.Cookies.Add(userInfo);
                
               //// Zuptu_Id = response.Cookies["ZuptuId"].Value;
               // Zuptu_Id = response.Cookies["Zuptu_Id"].Value;
            }
            auth.Image = identity.Claims.Where(c => c.Type == "Image").Select(c => c.Value).FirstOrDefault();
            auth.Name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).FirstOrDefault();
            auth.isGoogle = Convert.ToInt32(identity.Claims.Where(c => c.Type == "isGoogle").Select(c => c.Value).FirstOrDefault());
            auth.IsMasterAdmin = Convert.ToInt32(identity.Claims.Where(c => c.Type == "IsMasterAdmin").Select(c => c.Value).FirstOrDefault());
            auth.PicSignatureTime = Convert.ToDouble(identity.Claims.Where(c => c.Type == "PicSignatureTime").Select(c => c.Value).FirstOrDefault());



            return auth;
        }

     
        
        //public static string CanvasAccesToken()
        //{
        //    // string accessToken = "7~SWvsFuDcmNBY3eCBcP7EJDVivdgE8b3MW2EdEsOXF7slxj5ytjdfOiGmJ5yv3BKq";
        //    string accessToken = ProjectVaraiables.CanvasAccessToken;
        //    return accessToken;
        //}
        //public static List<Course> GetCanvasCourse()
        //{
        //    try
        //    {
        //        string accessToken = General_Purpose.CanvasAccesToken();

        //        var client = new RestClient(ProjectVaraiables.CanvasRestClient);
        //        var getCoursesRequest = new RestRequest("/api/v1/courses?access_token=" + accessToken, Method.GET);
        //        IRestResponse getCoursesResponse = client.Execute(getCoursesRequest);

        //        List<Course> courses = JsonConvert.DeserializeObject<List<Course>>(getCoursesResponse.Content).Where(x => x.workflow_state == "available").ToList();

        //        return courses;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}
        //public static List<Course> GetCanvasUserCourses()
        //{
        //    try
        //    {
        //        string accessToken = General_Purpose.CanvasAccesToken();

        //        var client = new RestClient(ProjectVaraiables.CanvasRestClient);



        //        var getCoursesRequest = new RestRequest("/api/v1/courses?access_token=" + accessToken, Method.GET);
        //        IRestResponse getCoursesResponse = client.Execute(getCoursesRequest);
        //        if (getCoursesResponse.Content == "")
        //            return null;
        //        List<Course> courses = JsonConvert.DeserializeObject<List<Course>>(getCoursesResponse.Content).Where(x => x.workflow_state == "available").ToList();
        //        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        //        List<Course> usercourse = new List<Course>();
        //        User us = new UserBL().getUsersById(logedinuser.Id);
        //        if (us.CanvasLoginId != null)
        //        {
        //            if (courses != null)
        //            {
        //                foreach (Course c in courses)
        //                {
        //                    var getUsersRequest = new RestRequest("/api/v1/courses/" + c.id + "/users?access_token=" + accessToken, Method.GET);
        //                    IRestResponse getUsersResponse = client.Execute(getUsersRequest);

        //                    List<CanvasUser> uss = JsonConvert.DeserializeObject<List<CanvasUser>>(getUsersResponse.Content).Where(x => x.email != null).ToList();

        //                    int users = uss.Where(x => x.email.ToUpper() == us.CanvasLoginId.ToUpper()).Count();

        //                    if (users > 0)
        //                    {
        //                        Course cr = new Course()
        //                        {
        //                            name = c.name,
        //                            id = c.id
        //                        };
        //                        usercourse.Add(cr);

        //                    }
        //                }
        //            }

        //        }






        //        return usercourse;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}
        //public static List<Course> GetCanvasUserCoursesById(int id)
        //{
        //    string accessToken = General_Purpose.CanvasAccesToken();

        //    var client = new RestClient(ProjectVaraiables.CanvasRestClient);



        //    var getCoursesRequest = new RestRequest("/api/v1/courses?access_token=" + accessToken, Method.GET);
        //    IRestResponse getCoursesResponse = client.Execute(getCoursesRequest);

        //    List<Course> courses = JsonConvert.DeserializeObject<List<Course>>(getCoursesResponse.Content).Where(x => x.workflow_state == "available").ToList();
        //    //  CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        //    List<Course> usercourse = new List<Course>();
        //    User us = new UserBL().getUsersById(id);
        //    if (us.CanvasLoginId != null)
        //    {
        //        foreach (Course c in courses)
        //        {
        //            var getUsersRequest = new RestRequest("/api/v1/courses/" + c.id + "/users?access_token=" + accessToken, Method.GET);
        //            IRestResponse getUsersResponse = client.Execute(getUsersRequest);

        //            List<CanvasUser> uss = JsonConvert.DeserializeObject<List<CanvasUser>>(getUsersResponse.Content).Where(x => x.email != null).ToList();

        //            int users = uss.Where(x => x.email.ToUpper() == us.CanvasLoginId.ToUpper()).Count();

        //            if (users > 0)
        //            {
        //                Course cr = new Course()
        //                {
        //                    name = c.name,
        //                    id = c.id
        //                };
        //                usercourse.Add(cr);

        //            }
        //        }
        //    }






        //    return usercourse;
        //}
        //public static List<Quiz> GetCanvasQuizes(int CourseId)
        //{
        //    string accessToken = General_Purpose.CanvasAccesToken();
        //    var client = new RestClient(ProjectVaraiables.CanvasRestClient);


        //    var getQuizRequest = new RestRequest("/api/v1/courses/" + CourseId + "/quizzes?access_token=" + accessToken, Method.GET);
        //    IRestResponse getQuizResponse = client.Execute(getQuizRequest);
        //    var ac = getQuizResponse.Content;


        //    List<Quiz> quizes = JsonConvert.DeserializeObject<List<Quiz>>(getQuizResponse.Content).Where(x => x.question_count > 0 && x.published == true && x.points_possible >= 0 && x.assignment_id != null && x.assignment_group_id != null).ToList();
        //    return quizes;
        //}
        //public static List<Assignment> GetCanvasAssignments(int CourseId)
        //{
        //    string accessToken = General_Purpose.CanvasAccesToken();
        //    var client = new RestClient(ProjectVaraiables.CanvasRestClient);

        //    var getAssignmentRequest = new RestRequest("/api/v1/courses/" + CourseId + "/assignments?access_token=" + accessToken, Method.GET);
        //    IRestResponse getAssignmentResponse = client.Execute(getAssignmentRequest);

        //    List<Assignment> assignments = JsonConvert.DeserializeObject<List<Assignment>>(getAssignmentResponse.Content).Where(x => x.published == true).ToList();

        //    return assignments;

        //}

        //public static Enrollment GetResult(int userId, int courseId)
        //{
        //    try
        //    {
        //        string accessToken = General_Purpose.CanvasAccesToken();

        //        var client = new RestClient(ProjectVaraiables.CanvasRestClient);

        //        Course course = General_Purpose.GetCanvasCourse().Where(x => x.id == courseId).FirstOrDefault();

        //        User us = new UserBL().getUsersById(userId);
        //        //Canvas Users
        //        var getUsersRequest = new RestRequest("/api/v1/courses/" + courseId + "/users?access_token=" + accessToken, Method.GET);
        //        IRestResponse getUsersResponse = client.Execute(getUsersRequest);

        //        List<CanvasUser> uss = JsonConvert.DeserializeObject<List<CanvasUser>>(getUsersResponse.Content).Where(x => x.email != null).ToList();

        //        CanvasUser user = uss.Where(x => x.email.ToUpper() == us.CanvasLoginId.ToUpper()).FirstOrDefault();
        //        //  string aa = "7~bcvTxshxrsEZ5hB21LG0zAZpjDaEqRv7sd4V5GVO2zaLV75QGTuLewaxAfZT1BPh";
        //        var getUserInCourseRequest = new RestRequest("/api/v1/users/" + user.id + "/courses?include[]=total_scores&include[]=current_grading_period_scores&include[]=syllabus_body&access_token=" + accessToken, Method.GET);
        //        IRestResponse getUserInCourseResponse = client.Execute(getUserInCourseRequest);

        //        Course userincours = JsonConvert.DeserializeObject<List<Course>>(getUserInCourseResponse.Content).Where(x => x.course_code == course.course_code).FirstOrDefault();
        //        Enrollment a = userincours.enrollments.Where(x => x.type == "student").FirstOrDefault();


        //        return a;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }

        //}

        //public static List<User> GetUsersInCourse(int courseId)
        //{

        //    string accessToken = General_Purpose.CanvasAccesToken();

        //    var client = new RestClient(ProjectVaraiables.CanvasRestClient);

        //    var getUsersRequest = new RestRequest("/api/v1/courses/" + courseId + "/users?access_token=" + accessToken, Method.GET);
        //    IRestResponse getUsersResponse = client.Execute(getUsersRequest);

        //    List<CanvasUser> uss = JsonConvert.DeserializeObject<List<CanvasUser>>(getUsersResponse.Content).Where(x => x.email != null).ToList();

        //    List<User_Task> usertasks = new User_TaskBL().getAllUser_TasksList().Where(x => x.CanvasCourseId != null).ToList();
        //    List<User_Task> usertask2 = usertasks.Where(x => x.CanvasCourseId == courseId && x.IsActive == 1).ToList();

        //    List<User> canvasuser = new List<User>();
        //    foreach (CanvasUser user in uss)
        //    {
        //        foreach (User_Task u in usertask2)
        //        {
        //            if (u.User1.CanvasLoginId != null)
        //            {
        //                if (u.User1.CanvasLoginId.ToUpper() == user.email.ToUpper())
        //                {
        //                    int ss = canvasuser.Where(x => x.Id == u.User1.Id).Count();
        //                    if (ss <= 0)
        //                    {
        //                        canvasuser.Add(u.User1);
        //                    }


        //                }
        //            }
        //        }
        //    }




        //    return canvasuser;
        //}


        //public static CanvasUser IsUserInCanvasSystem(int userId, List<Course> courses)
        //{
        //    string accessToken = General_Purpose.CanvasAccesToken();
        //    CanvasUser dummyuser = new CanvasUser();
        //    var client = new RestClient(ProjectVaraiables.CanvasRestClient);
        //    foreach (Course cours in courses)
        //    {
        //        Course course = General_Purpose.GetCanvasCourse().Where(x => x.id == cours.id).FirstOrDefault();

        //        User us = new UserBL().getUsersById(userId);
        //        //Canvas Users
        //        var getUsersRequest = new RestRequest("/api/v1/courses/" + cours.id + "/users?access_token=" + accessToken, Method.GET);
        //        IRestResponse getUsersResponse = client.Execute(getUsersRequest);

        //        List<CanvasUser> uss = JsonConvert.DeserializeObject<List<CanvasUser>>(getUsersResponse.Content).Where(x => x.email != null).ToList();

        //        CanvasUser user = uss.Where(x => x.email.ToUpper() == us.CanvasLoginId.ToUpper()).FirstOrDefault();
        //        if (user != null)
        //        {
        //            return user;
        //        }

        //    }


        //    return null;

        //}

        //public static string UserEnrollmentInCanvas(int UserId = -1, int courseId = -1)
        //{
        //    string response = "";
        //    string accessToken = General_Purpose.CanvasAccesToken();

        //    var client = new RestClient(ProjectVaraiables.CanvasRestClient);

        //    if (UserId != -1 && courseId != -1)
        //    {
        //        var request = new RestRequest("/api/v1/courses/{id}/enrollments?access_token=" + accessToken, Method.POST);
        //        request.AddParameter("enrollment[user_id]", UserId); // adds to POST or URL querystring based on Method
        //        request.AddParameter("enrollment[type]", "StudentEnrollment"); // adds to POST or URL querystring based on Method
        //        request.AddParameter("enrollment[enrollment_state]", "active"); // adds to POST or URL querystring based on Method
        //        request.AddUrlSegment("id", courseId); // replaces matching token in request.Resource
        //        IRestResponse PostCourseEnrollmentResponse = client.Execute(request);

        //        response = PostCourseEnrollmentResponse.StatusCode.ToString();
        //        return response;

        //    }

        //    return response;

        //}


        //Send Push Notification To Mobile App

        public static string EncryptId(int Id)
        {

            return HttpUtility.UrlEncode(General_Purpose.Encrypt(Id));
        }
        public static int DecryptId(string val)
        {
            //string c = "WRohnSD wxg=";
            //if (c.Contains(' '))
            //{
            //    c = c.Replace(' ', '+');
            //}
            if (val.Contains(' '))
            {
                val = val.Replace(' ', '+');
            }
            int id = -1;
            id = General_Purpose.Decrypt(HttpUtility.UrlDecode(val));
            string str = "";
            if (id == 0)
            {
                str = HttpUtility.UrlEncode(val);
                id = General_Purpose.Decrypt(HttpUtility.UrlDecode(str));
            }
            return id;
        }
        public static bool SendPushNotification(string Player_Id = "", string Content = "", string Heading = "Message from Zuptu", PushDataDTO data = null, string userId = "")
        {

            string response = "";
            //   var client = new RestClient("https://devapi.zuptu.systems");
            var client = new RestClient(ProjectVaraiables.PushNotification);

            var request = new RestRequest("/api/Task/SendWebNotification", Method.GET);
            request.AddParameter("val", Player_Id); // adds to POST or URL querystring based on Method
            request.AddParameter("Content", Content); // adds to POST or URL querystring based on Method
            request.AddParameter("heading", Heading); // adds to POST or URL querystring based on Method
            request.AddParameter("TaskId", data.TaskId); // adds to POST or URL querystring based on Method
            request.AddParameter("GroupTaskDetailId", data.GroupTaskDetailsId); // adds to POST or URL querystring based on Method
            request.AddParameter("userId", userId); // adds to POST or URL querystring based on Methodax

            IRestResponse PostCourseEnrollmentResponse = client.Execute(request);

            response = PostCourseEnrollmentResponse.StatusCode.ToString();

            if (response == "OK")
                return true;
            return false;
        }

        public static string GetTaskLink(int Id, int Role)
        {
            string EncryptedTaskId = HttpUtility.UrlEncode(General_Purpose.Encrypt(Id));
            string link = "";
            if (Role == 3)
            {
                link = ProjectVaraiables.SYSTEM_URL + "/Task/taskdetails?v=" + EncryptedTaskId + "&isModal=01";

            }
            else
            {
                link = ProjectVaraiables.SYSTEM_URL + "/Manager/taskdetails?v=" + EncryptedTaskId + "&isModal=01";

            }
            return link;
        }

        public static string GetGroupTaskLink(int Id)
        {
            string EncryptedTaskId = HttpUtility.UrlEncode(General_Purpose.Encrypt(Id));
            string link = "";

            link = ProjectVaraiables.SYSTEM_URL + "/Task/grouptaskdetails?v=" + EncryptedTaskId + "&isModal=01";


            return link;
        }

        public static Course ReturnCourse(Course c)
        {
            Course obj = new Course()
            {
                Id = c.Id,
                Name = c.Name,
                IsPublished = c.IsPublished,
                Description = c.Description,
                CompanyID = c.CompanyID,
                NoFile = c.NoFile,
                FileSize = c.FileSize,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                TitleImage=c.TitleImage,
                CreateCertificate=c.CreateCertificate,
                IsScorm=c.IsScorm,
                ScormCourseFile=c.ScormCourseFile,
                SignatureImage=c.SignatureImage,
                Citation= c.Citation,
                CertificateValidity=c.CertificateValidity,
                SignatureText=c.SignatureText
            };

            return obj;
        }

        public static Course_UserTask ReturnCourse_UserTask(Course_UserTask c)
        {
            Course_UserTask obj = new Course_UserTask()
            {
                Id = c.Id,
                IsActive = c.IsActive,
                CourseID = c.CourseID,
                User_TaskID = c.User_TaskID,
                CreatedAt = c.CreatedAt
            };

            return obj;
        }
        public static bool AssignLMSCourse(int UserTaskId=-1, int CourseId=-1)
        {
            if(UserTaskId!=-1 && CourseId != -1)
            {
                Course cou = new CourseBL().getCourseById(CourseId);
                
                User_Task ut = new User_TaskBL().getUser_TasksById(UserTaskId);
                Course_UserTask obj = new Course_UserTask()
                {
                    IsActive = 1,
                    CourseID = CourseId,
                    User_TaskID = UserTaskId,
                    CreatedAt = DateTime.Now,
                    UserId=ut.UserID,
                    CompanyID=cou.CompanyID
                    
                };
                obj = new Course_UserTaskBL().AddCourse_UserTask(obj);

                if (cou.IsScorm == 1)
                {
                    User user = new UserBL().getUsersById((int)ut.UserID);
                    ScormCloud.Configuration = new RusticiSoftware.HostedEngine.Client.Configuration(
                    "https://cloud.scorm.com/EngineWebServices",
                    ProjectVaraiables.ScormAppId,
                    ProjectVaraiables.ScormSecretKey,
                       ProjectVaraiables.ScormOrigin);
                    try
                    {
                        ScormCloud.RegistrationService.CreateRegistration(obj.Id.ToString(), cou.Id.ToString(), ut.Id.ToString(), user.FirstName, user.LastName);
                    }
                    catch (Exception ex)
                    {

                    }

                }
                return true;
            }
            return false;
           
        }

        public static string RefreshImageSignature(int userId)
        {
            Models.User Admin = new UserBL().getUsersById(userId);

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

            try
            {

                userPicture = userPicture != string.Empty ? BlobManager.GetBlobSasUri(userPicture, Admin.Company.Id.ToString()) : "";

            }
            catch (Exception ex)
            {
                userPicture = string.Empty;
            }


            return userPicture;

        }

        public static string CustomTemplate3(Course Course, string CompletedBy = "", string CompletionDate = "", string AssignedBy = "", User user = null)
        {
            CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
            string url = ProjectVaraiables.LMS_URL + Course.SignatureImage;
            Company company = new CompanyBL().getCompanyById((int)user.CompanyID);
            string CompanyName = company.Name;
            string LOGO = "/Content/landing_page/images/ig.PNG";
            if (!string.IsNullOrEmpty(company.Logo))
            {
                LOGO = BlobManager.GetBlobSasUri(company.Logo, logedinuser.Company.ToString());

            }
           string template = "<div style='width:; height:; padding:10px; text-align:center; border: 50px solid #22456b'>" +

"<div style = 'width:; height:; padding:10px; text-align:center; border: 5px solid #0150A1'>" +

          "<div style = ' height:810px; padding:10px; text-align:center; border: 3px solid #C1C1C1'>" +

         "<div style = 'float: left; width: 100%;background-color: '>" +

              "<center style = 'padding: 0px;margin-left:'>" +


                "<h1 style = 'padding: 0px; color: #008940; font-size: 48px;'> CERTIFICATE of TRAINING</h1>" +

                     "<h3 style = 'font-size: 18px; margin-top: 30px'> THIS ACKNOWLEDGES THAT</h3>" +

                          "<h1 style = 'margin-top: 0px; font-size: 40px'>"+CompletedBy+"</h1>" +

                               "<h3 style = 'font-size: 18px; margin-top: 10px'>HAS SUCCESSFULLY COMPLETED</h3>" +




                                        "</center>" +

                                   "</div>" +


                                   "<div style = 'float: left; width: 100%;background-color: ;min-height: 130px;'>" +


                                            "<h3 style = 'font-size: 33px; margin-top: 10px; text-align: center;margin-left: 5px'>"+Course.Name+ "</h3>" +


                                       "</div>" +

                                        "<div style = 'float: right; width: 100%;'>" +


                                        "<div style = 'float: left; width: 33%;background-color: ;min-height:300px'>" +



                                                     "<div style = 'width: 80%;  border: 1px solid;box-sizing: border-box; padding: 10px'>" +

                                                          "<div style = 'width: 100%; min-height: 145px'>" +

                                                               "<center>" +

                                                                 "<h3> "+CompletedBy+"'s Certification</h3>" +
                                                                 "<span>"+Course.Citation+"</span>" +

                    "</center>" +

                       "</div>" +

                       "<hr>" +

                       "<div style = 'width: 100%; min-height: 40px'>" +

                            "<center>" +

                               "<h3 style='margin-bottom:3px'>Employer's Signature </h3>" +

                         "<span> Certificate valid for " +Course.CertificateValidity+


                            "</span>" +


                    "</center>" +

                           "</div>" +

                           "<hr>" +

                       "<div style = 'width: 100%; min-height: 90px'>" +

                            "<center>" +

                                "<img src = '" + user.SignatureImage + "'  style = 'position: relative; height:100px;width: 300px;margin-top:20'>" +

                    "</center>" +

                           "</div>" +


                       "</div>" +



           "</div>" +

           "<div style = 'float: left;width: 33%; margin-top: 130px'>" +

                "<center style = 'margin-top: 0px ; margin-left:'>" +

                         "<img src = '" +url+ "'  style = 'position: relative; height:190px;width: 100%'>" +

                             "</center>" +

                    "<center style = 'margin-top: 20px'>" +

                    "<b> " + user.FirstName + " " + user.LastName + ", " + CompanyName + "</b>" +


                                     "<br>" +

                                     "<b>" + CompletionDate + "</b></center>" +

                           "<hr>" +

                       "</div>" +

                       "<div style = 'float: left;width: 33%; margin-top: 290px'>" +

                    "<img src='"+LOGO+"' style ='position: relative; height: 91px; width:100px; margin-left: 120px'>" +


                           "</div>" +


                           "</div>" +





                      "</div>" +
                    "</div>" +
                    "</div>";

            return template;
        }

        public static bool CreateGroupStudy(int GroupTaskDetailsId, string GroupStudyName,int PrimeLeadId,int CourseId,int TaskId,DatabaseEntities de=null)
        {
            try
            {

            CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
            Course cou = new CourseBL().getCourseById(CourseId);
          //  DatabaseEntities de = new DatabaseEntities();

            GroupTasks_Details gtd = new GroupTasks_DetailsBL().getGroupTasks_DetailssByIdWrapepr(GroupTaskDetailsId, de);

                User_Task ut = new User_Task()
                {
                    UserID = PrimeLeadId,
                    CEU = gtd.CEU,
                    Cost = gtd.Cost,
                    Hours = gtd.Hours,
                    CreatedAt = DateTime.Now,
                    CompanyId = Convert.ToInt32(logedinuser.Company),
                    CreatedID = logedinuser.Id,
                    IsActive = 1,
                    StartDate = gtd.StartDate,
                    EndDate = gtd.EndDate,
                    TaskID = TaskId,
                    Priority = 0,
                    GroupTaskDetailId = GroupTaskDetailsId,
                    Grad = gtd.Grade,
                    IsPrivate = 0,
                    Status=0


            };
           ut= new User_TaskBL().AddUser_Tasks(ut, de);

            CourseGroupStudy Cgst = new CourseGroupStudy()
            {
                CourseID = CourseId,
                IsActive = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = logedinuser.Id,
                LeadBy = ut.UserID,
                Description = "",
                Name = GroupStudyName,
                Status = 0,
                CompanyId=Convert.ToInt32(logedinuser.Company),
                User_TaskId =ut.Id
                
            };
            int GroupStudyid = new CourseGroupStudyBL().AddCourseGroupStudy(Cgst,de).Id;

            Course_UserTask obj = new Course_UserTask()
            {
                IsActive = 1,
                CourseID = CourseId,
                User_TaskID = ut.Id,
                CreatedAt = DateTime.Now,
                UserId = ut.UserID,
                CompanyID = Convert.ToInt32(logedinuser.Company),
                IsLead = 1,
                CourseGroupStudyId = GroupStudyid

            };
            obj = new Course_UserTaskBL().AddCourse_UserTask(obj);

            if (cou.IsScorm == 1)
            {
                User user = new UserBL().getUsersById((int)ut.UserID);
                ScormCloud.Configuration = new RusticiSoftware.HostedEngine.Client.Configuration(
                "https://cloud.scorm.com/EngineWebServices",
                ProjectVaraiables.ScormAppId,
                ProjectVaraiables.ScormSecretKey,
                   ProjectVaraiables.ScormOrigin);
                try
                {
                    ScormCloud.RegistrationService.CreateRegistration(obj.Id.ToString(), cou.Id.ToString(), ut.Id.ToString(), user.FirstName, user.LastName);
                }
                catch (Exception ex)
                {

                }

            }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

    }
}