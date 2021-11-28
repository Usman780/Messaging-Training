using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TrainingTracker.HelpingClasses
{
    public class ProjectVaraiables
    {
        public static string DEFAULTPATH = ConfigurationManager.AppSettings["DEFAULTPATH"];
        public static string IMAGEPATH = ConfigurationManager.AppSettings["IMAGEPATH"];
        public static string PDFPATH = ConfigurationManager.AppSettings["PDFPATH"];
        public static string ErrorFolder = ConfigurationManager.AppSettings["ErrorFolder"];
        public static string EMAIL = ConfigurationManager.AppSettings["EMAIL"];
        public static string PASSWORD = ConfigurationManager.AppSettings["PASSWORD"];
        public static string WEBHOST = ConfigurationManager.AppSettings["WEBHOST"];
        public static string TWILIO_ACCOUNTSID = ConfigurationManager.AppSettings["TWILIO_ACCOUNTSID"];
        public static string TWILIO_AUTHENTTOKEN = ConfigurationManager.AppSettings["TWILIO_AUTHENTTOKEN"];
        public static string TWILIO_SENDERNUMBER = ConfigurationManager.AppSettings["TWILIO_SENDERNUMBER"];
        public static string BACKUP_FOLDER_LOCATION = ConfigurationManager.AppSettings["BACKUP_FOLDER_LOCATION"];
        public static string GOOGLE_APP_Name = ConfigurationManager.AppSettings["GOOGLE_APP_Name"];
        public static string GOOGLE_REFRESH_TOKEN = ConfigurationManager.AppSettings["GOOGLE_REFRESH_TOKEN"];
        public static string GOOGLE_CLIENT_ID = ConfigurationManager.AppSettings["GOOGLE_CLIENT_ID"];
        public static string GOOGLE_SECRET_KEY = ConfigurationManager.AppSettings["GOOGLE_SECRET_KEY"];
        public static string IRONPDF_License = ConfigurationManager.AppSettings["IRONPDF_License"];
        //public const string SYSTEM_URL = @"http://localhost:60126";
        public static string SYSTEM_URL = ConfigurationManager.AppSettings["SYSTEM_URL"];
        //public const string SYSTEM_URL = @"https://zuptu.nodlays.com/";
        public static string APPID = ConfigurationManager.AppSettings["APPID"];
        public static string AppPassword = ConfigurationManager.AppSettings["AppPassword"];
       // public const string RedirectUri = "http://localhost:60126/";
       public static string RedirectUri = ConfigurationManager.AppSettings["RedirectUri"];
        //public const string RedirectUri = "https://zuptu.nodlays.com/";
        public static string AppScopes = ConfigurationManager.AppSettings["AppScopes"];
        public static string AZURE_CLOUD_STORAGE = ConfigurationManager.AppSettings["AZURE_CLOUD_STORAGE"];
        public static string COMPANY_ALPHABET = ConfigurationManager.AppSettings["COMPANY_ALPHABET"];
        // Canvas Azure 
        public static string CanvasRestClient = ConfigurationManager.AppSettings["CanvasRestClient"];
        public static string CanvasAccessToken = ConfigurationManager.AppSettings["CanvasAccessToken"];
        public static string DeveloperMail = ConfigurationManager.AppSettings["DeveloperMail"];
        public static string InviteCanvasMail = ConfigurationManager.AppSettings["InviteCanvasMail"];
        public static string ResetPasswordUrl = ConfigurationManager.AppSettings["ResetPasswordUrl"];
        public static string NormalContactMails = ConfigurationManager.AppSettings["NormalContactMails"];
        public static string ModerateContactMails = ConfigurationManager.AppSettings["ModerateContactMails"];
        public static string SevereContactMails = ConfigurationManager.AppSettings["SevereContactMails"];
        public static string InviteUser = ConfigurationManager.AppSettings["InviteUser"];
        public static string PushNotification = ConfigurationManager.AppSettings["PushNotification"];
        public static string LMS_URL = ConfigurationManager.AppSettings["LMS_URL"];
        public static string LMS_Auth_URL = ConfigurationManager.AppSettings["LMS_Auth_URL"];
        public static string DocM_URL = ConfigurationManager.AppSettings["DocM_URL"];
        public static string ScormAppId = ConfigurationManager.AppSettings["ScormAppId"];
        public static string ScormSecretKey = ConfigurationManager.AppSettings["ScormSecretKey"];
        public static string ScormOrigin = ConfigurationManager.AppSettings["ScormOrigin"];

        public static string ChatSupportURL = ConfigurationManager.AppSettings["ChatSupportURL"];
        //public static string DocM_URL = ConfigurationManager.AppSettings["DocM_URL"];


        // Canvas Local
        //public const string CanvasRestClient = "https://canvas.instructure.com";
        // public const string CanvasAccessToken = "7~SWvsFuDcmNBY3eCBcP7EJDVivdgE8b3MW2EdEsOXF7slxj5ytjdfOiGmJ5yv3BKq";


        //These are html to pdf variables

    }
    public static class HTML_PDF_Varaiables
    {
        
        //public static string HTML_GroupTaskDetails = "<div id=\"customers\" style=\"width:100%; font-size:20px;\"><table style=\" text-align: left;\">            <tbody>                                                           <tr>                    <th>Task Name: </th>                    <td>_tn_</td>                </tr>                <tr>                    <th>Work Status: </th>                    <td>_ws_</td>                </tr>            <tr>                    <th>Hours: </th>                    <td>_hr_</td>                </tr>            <tr>                    <th>Cost/ Resources: </th>                    <td>_cr_</td>                </tr>                <tr>                    <th>Revenue: </th>                    <td>_rev_</td>                </tr>                 <th>Description: </th>                    <td>_desc_</td>                </tr>            </tbody>        </table>";
        //public static string HTML_GroupTask_Prime = "<table style=\"width:100%;\">                <thead>                    <tr style=\"text-align:left;\">                    <th>Prime's Name</th>                    <th>Job Title</th>                    <th>Division </th>                    <th>Phone Number</th>                    <th>Email</th>                </tr>                </thead>                <tbody>                    <tr style=\"text-align:left; margin-bottom: 1px solid black;\">                        <td>_Name_</td>                        <td>_job_</td>                        <td>_dept_</td>                        <td>_number_</td>                        <td>_email_</td>                    </tr>                </tbody>                    </table>";
        //public static string HTML_GroupTask_Second = "<table style=\"width:100%;\">                <thead>                    <tr style=\"text-align:left;\">                    <th>Second's Name</th>                    <th>Job Title</th>                    <th>Division </th>                    <th>Phone Number</th>                    <th>Email</th>                </tr>                </thead>                <tbody>                    <tr style=\"text-align:left; margin-bottom: 1px solid black;\">                        <td>_Name_</td>                        <td>_job_</td>                        <td>_dept_</td>                        <td>_number_</td>                        <td>_email_</td>                    </tr>                </tbody>                    </table>";
        //public static string HTML_GroupTask_Employee = "<table style=\"width:100%;\">                <thead>                    <tr style=\"text-align:left;\">                    <th>Employee's Name</th>                    <th>Job Title</th>                    <th>Department </th>                    <th>Phone Number</th>                    <th>Email</th>                </tr>                </thead>                <tbody>                    <tr style=\"text-align:left; margin-bottom: 1px solid black;\">                        <td>_Name_</td>                        <td>_job_</td>                        <td>_dept_</td>                        <td>_number_</td>                        <td>_email_</td>                    </tr>                </tbody>                    </table>";
        //public static string HTML_GroupTask_DateDetails = "<table style=\"width:70%;\"><thead>    <tr style=\"text-align:left;\">    <td>Assigned Date</td>    <td>Due Date</td>    <td>Completion Date</td>        <td>Completion Status</td>    <td>Priority</td></tr></thead><tbody>    <tr style=\"text-align:left;\">        <td style=\"border-bottom:1px solid black;\">_sd_</td>        <td style=\"border-bottom:1px solid black;\">_ed_</td>        <td style=\"border-bottom:1px solid black;\">_cd_</td>        <td style=\"border-bottom:1px solid black;\">_ws_</td>        <td style=\"border-bottom:1px solid black;\">_prior_</td>            </tr></tbody>    </table>";
        //public static string Trainee_task_DateDetails = "<table style=\"width:100%;\"><thead>    <tr style=\"text-align:left;\">   " +
        //    " <td>Assigned Date</td> " +
        //    "   <td>Due Date</td>  " +
        //    "  <td>Completion Date</td>  " +

        //    "      <td>Grade</td>" +
        //    "      <td>Frequency</td>" +
        //       "      <td>Completion Status</td>" +
        //    "    <td>Priority<" +
        //    "/td></tr></thead><tbody>    <tr style=\"text-align:left;\">      " +
        //    "  <td style=\"border-bottom:1px solid black;\">_sd_</td>  " +
        //    "      <td style=\"border-bottom:1px solid black;\">_ed_</td>    " +
        //    "    <td style=\"border-bottom:1px solid black;\">_cd_</td>     " +

        //    "   <td style=\"border-bottom:1px solid black;\">_grade_</td>    " +
        //    "   <td style=\"border-bottom:1px solid black;\">_freq_</td>    " +
        //       "   <td style=\"border-bottom:1px solid black;\">_cs_</td>    " +
        //    "    <td style=\"border-bottom:1px solid black;\">_prior_</td>  " +

        //    "          </tr></tbody>    </table>";


        public static string HTML_GroupTaskDetails = "<div id=\"customers\" style=\"width:100%; font-size:20px;\"><table style=\" text-align: left;\">            <tbody>                                                           <tr>                    <th style='width:20%'>Task Name: </th>                    <td>_tn_</td>                </tr>                <tr>                    <th style='width:20%'>Work Status: </th>                    <td>_ws_</td>                </tr>            <tr>                    <th style='width:20%'>Hours: </th>                    <td>_hr_</td>                </tr>            <tr>                    <th style='width:20%'>Cost/ Resources: </th>                    <td>_cr_</td>                </tr> <tr>          <th style='width:20%'>Description: </th>                    <td>_desc_</td>                </tr>   <tr>                    <th style='width:20%'>Assigned By: </th>                    <td>_agnby_</td>                </tr>           </tbody>        </table>";
        public static string HTML_GroupTask_Prime = "<table style=\"width:100%;\">                <thead>                    <tr style=\"text-align:left;\">                    <th style='width:25%'>Primary Lead</th>                    <th style='width:25%'>Division </th>                    <th style='width:25%'>Phone Number</th>                    <th style='width:25%'>Email</th>                </tr>                </thead>                <tbody>                    <tr style=\"text-align:left; margin-bottom: 1px solid black;\">                        <td style='width:25%'>_Name_</td>                                               <td style='width:25%'>_dept_</td>                        <td style='width:25%'>_number_</td>                        <td style='width:25%'>_email_</td>                    </tr>                </tbody>                    </table>";
        public static string HTML_GroupTask_Second = "<table style=\"width:100%;\">                <thead>                    <tr style=\"text-align:left;\">                    <th style='width:25%'>Secondary Lead</th>                   <th style='width:25%'>Division </th>                    <th style='width:25%'>Phone Number</th>                    <th style='width:25%'>Email</th>                </tr>                </thead>                <tbody>                    <tr style=\"text-align:left; margin-bottom: 1px solid black;\">                        <td style='width:25%'>_Name_</td>                                               <td style='width:25%'>_dept_</td>                        <td style='width:25%'>_number_</td>                        <td style='width:25%'>_email_</td>                    </tr>                </tbody>                    </table>";
        public static string HTML_GroupTask_Employee = "<table style=\"width:100%;\">                <thead>                    <tr style=\"text-align:left;\">                    <th style='width:25%'>Employee's Name</th>                   <th style='width:25%'>Department </th>                    <th style='width:25%'>Phone Number</th>                    <th style='width:25%'>Email</th>                </tr>                </thead>                <tbody>                    <tr style=\"text-align:left; margin-bottom: 1px solid black;\">                        <td style='width:25%'>_Name_</td>                                              <td style='width:25%'>_dept_</td>                        <td style='width:25%'>_number_</td>                        <td style='width:25%'>_email_</td>                    </tr>                </tbody>                    </table>";
        public static string HTML_GroupTask_DateDetails = "<table style=\"width:70%;\"><thead>    <tr style=\"text-align:left;\">  <td style='width:20%'>Assigned Date</td>  <td style='width:20%'>Start Date</td>    <td style='width:20%'>Due Date</td>    <td style='width:20%'>Completion Date</td>        <td style='width:20%'>Completion Status</td>    <td style='width:20%'>Priority</td></tr></thead><tbody>    <tr style=\"text-align:left;\">    <td style=\"border-bottom:1px solid black;\">_assindate_</td>    <td style=\"border-bottom:1px solid black;\">_sd_</td>        <td style=\"border-bottom:1px solid black;\">_ed_</td>        <td style=\"border-bottom:1px solid black;\">_cd_</td>        <td style=\"border-bottom:1px solid black;\">_ws_</td>        <td style=\"border-bottom:1px solid black;\">_prior_</td>            </tr></tbody>    </table>";
        public static string Trainee_task_DateDetails = "<table style=\"width:100%;\"><thead>    <tr style=\"text-align:left;\">   " +
            " <td>Start Date</td> " +
            "   <td>Due Date</td>  " +
            "  <td>Completion Date</td>  " +

            "      <td>Grade</td>" +
            "      <td>Frequency</td>" +
               "      <td>Completion Status</td>" +
            "    <td>Priority<" +
            "/td></tr></thead><tbody>    <tr style=\"text-align:left;\">      " +
            "  <td style=\"border-bottom:1px solid black;\">_sd_</td>  " +
            "      <td style=\"border-bottom:1px solid black;\">_ed_</td>    " +
            "    <td style=\"border-bottom:1px solid black;\">_cd_</td>     " +

            "   <td style=\"border-bottom:1px solid black;\">_grade_</td>    " +
            "   <td style=\"border-bottom:1px solid black;\">_freq_</td>    " +
               "   <td style=\"border-bottom:1px solid black;\">_cs_</td>    " +
            "    <td style=\"border-bottom:1px solid black;\">_prior_</td>  " +

            "          </tr></tbody>    </table>";

        public static string employee_assignment_detail = "<style>  table{ border: 1px solid black; margin-top:-2%; } td, th { border: 1px solid black;} th{font-size:15px;}table { border-collapse: collapse; width: 100%;}td { height: 50px; vertical-align: bottom;}</style><table style=\"width:100%;\"><thead>    <tr style=\"text-align:left;\">   " +
            " <td> <strong>Employee Name </strong></td> " +
            "   <td> <strong>Task Name</strong></td>  " +
            "  <td> <strong>Start Date</strong></td>  " +

            "      <td> <strong>End Date</strong></td>" +
            "      <td> <strong>Completion Date</strong></td>" +
            "      <td> <strong>Work Status</strong></td>" +

            "    <td> <strong>Priority</strong><" +
            "/td></tr></thead><tbody> ";


        public static string group_assignment_detail = "<style>  table{ border: 1px solid black; } td, th { border: 1px solid black;} th{font-size:15px;}table { border-collapse: collapse; width: 100%;}td { height: 50px; vertical-align: bottom;}</style><table style=\"width:100%;\"><thead>    <tr style=\"text-align:left;\">   " +

            "   <td> <strong>Task Name</strong></td>  " +
            "  <td> <strong>Start Date</strong></td>  " +

            "      <td> <strong>End Date</strong></td>" +

            "      <td> <strong>Completion Date</strong></td>" +
                  " <td> <strong>Lead Name </strong></td> " +
            "      <td> <strong>Work Status</strong></td>" +

            "    <td> <strong>Priority</strong><" +
            "/td></tr></thead><tbody> ";

        public static string manager_assignment_detail = "<style> thead { display: table-header-group }tfoot { display: table-row-group }tr { page-break-inside: avoid }  table{ border: 1px solid black;  } td, th { border: 1px solid black;} th{font-size:15px;}table { border-collapse: collapse; width: 100%;}td { height: 50px; vertical-align: bottom;}</style><table style=\"width:100%;\"><thead>    <tr style=\"text-align:left;\">   " +
            " <td> <strong>Manager Name </strong></td> " +
            "   <td> <strong>Task Name</strong></td>  " +
            "  <td> <strong>Start Date</strong></td>  " +

            "      <td> <strong>End Date</strong></td>" +
            "      <td> <strong>Completion Date</strong></td>" +
            "      <td> <strong>Work Status</strong></td>" +

            "    <td> <strong>Priority</strong><" +
            "/td></tr></thead><tbody> ";

        public static string admin_assignment_detail = "<style> thead { display: table-header-group }tfoot { display: table-row-group }tr { page-break-inside: avoid }  table{ border: 1px solid black;  } td, th { border: 1px solid black;} th{font-size:15px;}table { border-collapse: collapse; width: 100%;}td { height: 50px; vertical-align: bottom;}</style><table style=\"width:100%;\"><thead>    <tr style=\"text-align:left;\">   " +
          " <td> <strong>Admin Name </strong></td> " +
          "   <td> <strong>Task Name</strong></td>  " +
          "  <td> <strong>Start Date</strong></td>  " +

          "      <td> <strong>End Date</strong></td>" +
          "      <td> <strong>Completion Date</strong></td>" +
          "      <td> <strong>Work Status</strong></td>" +

          "    <td> <strong>Priority</strong><" +
          "/td></tr></thead><tbody> ";

        public static string employee_assignment_detail2 = "<style> thead { display: table-header-group }tfoot { display: table-row-group }tr { page-break-inside: avoid }  table{ border: 1px solid black;  } td, th { border: 1px solid black;} th{font-size:15px;}table { border-collapse: collapse; width: 100%;}td { height: 50px; vertical-align: bottom;}</style><table style=\"width:100%;\"><thead>    <tr style=\"text-align:left;\">   " +
            " <td> <strong>Employee Name </strong></td> " +
            "   <td> <strong>Task Name</strong></td>  " +
            "  <td> <strong>Start Date</strong></td>  " +

            "      <td> <strong>End Date</strong></td>" +
            "      <td> <strong>Completion Date</strong></td>" +
            "      <td> <strong>Work Status</strong></td>" +

            "    <td> <strong>Priority</strong><" +
            "/td></tr></thead><tbody> ";
        public static string department_division_ReportDetail = "<style> thead { display: table-header-group }tfoot { display: table-row-group }tr { page-break-inside: avoid }  table{ border: 1px solid black;  } td, th { border: 1px solid black;} th{font-size:15px;}table { border-collapse: collapse; width: 100%;}td { height: 50px; vertical-align: bottom;}</style><table style=\"width:100%;\"><thead>    <tr style=\"text-align:left;\">   " +
            " <td> <strong>Employee <span>(Admin/Manager/Employee)</span> </strong></td> " +
            "   <td> <strong>Task Type</strong></td>  " +
            "   <td> <strong>Task Name</strong></td>  " +
            "   <td> <strong>Assigned By</strong></td>  " +
              "    <td> <strong>Priority</strong>" +
            "  <td> <strong>Start Date</strong></td>  " +

            "      <td> <strong>End Date</strong></td>" +
            "      <td> <strong>Completion Date</strong></td>" +
            "      <td> <strong>Work Status</strong></td>" +


            "<td></tr></thead><tbody> ";
        public static string grpContent = "<tr>      " +

          "      <td>_tn_</td>    " +
          "    <td>_sd_</td>     " +

          "   <td>_ed_</td>    " +
             "   <td>_cd_</td>    " +
            "  <td>_name_</td>  " +

             "   <td>_cs_</td>    " +
          "    <td>_prior_</td>  " +

          "          </tr>";

        public static string empContent = "<tr>      " +
            "  <td>_name_</td>  " +
            "      <td>_tn_</td>    " +
            "    <td>_sd_</td>     " +

            "   <td>_ed_</td>    " +
               "   <td>_cd_</td>    " +

               "   <td>_cs_</td>    " +
            "    <td>_prior_</td>  " +

            "          </tr>";

        public static string divisionReportHeader = "<style> thead { display: table-header-group }tfoot { display: table-row-group }tr { page-break-inside: avoid }  table{ border: 1px solid black;  } td, th { border: 1px solid black;} th{font-size:15px;}table { border-collapse: collapse; width: 100%;}td { height: 50px; vertical-align: bottom;}</style><table style=\"width:100%;\"><thead>    <tr style=\"text-align:left;\">   " +
            " <td> <strong>Department </strong></td> " +
            "   <td> <strong>Task Type</strong></td>  " +
            "   <td> <strong>Task Name</strong></td>  " +
            "   <td> <strong>Assigned By</strong></td>  " +
            "   <td> <strong>Assigned To</strong></td>  " +
               "    <td> <strong>Priority</strong>" +
            "  <td> <strong>Start Date</strong></td>  " +

            "      <td> <strong>End Date</strong></td>" +
            "      <td> <strong>Completion Date</strong></td>" +
            "      <td> <strong>Completion Status</strong></td>" +


            "</td></tr></thead><tbody> ";


        //work done by Waqas, Wajeeh
        public static string divisionReportHeaderEmp = "<style> thead { display: table-header-group }tfoot { display: table-row-group }tr { page-break-inside: avoid }  table{ border: 1px solid black;  } td, th { border: 1px solid black;} th{font-size:15px;}table { border-collapse: collapse; width: 100%;}td { height: 50px; vertical-align: bottom;}</style><table style=\"width:100%;\"><thead>    <tr style=\"text-align:left;\">   " +
            " <td> <strong>Department </strong></td> " +
            "   <td> <strong>Task Type</strong></td>  " +
            "   <td> <strong>Task Name</strong></td>  " +
            //"   <td> <strong>Assigned By</strong></td>  " +
            //"   <td> <strong>Assigned To</strong></td>  " +
               "    <td> <strong>Priority</strong>" +
            "  <td> <strong>Start Date</strong></td>  " +

            "      <td> <strong>End Date</strong></td>" +
            "      <td> <strong>Completion Date</strong></td>" +
            "      <td> <strong>Completion Status</strong></td>" +


            "</td></tr></thead><tbody> ";

        public static string empDivisionReportEmp = "<tr>      " +
           "  <td>_name_</td>  " +
          "      <td>_tt_</td>    " +
           "    <td>_tn_</td>     " +
           //"    <td>_aby_</td>     " +
           //"    <td>_ato_</td>     " +
               "    <td>_prior_</td>     " +
           "    <td>_sd_</td>     " +

           "   <td>_ed_</td>    " +
              "   <td>_cd_</td>    " +

              "   <td>_cs_</td>    " +


           "          </tr>";
        //work done by Waqas, Wajeeh



        public static string empEnd = "</tbody>    </table>";


        public static string Trainee_Task_Pdf = "<div id=\"customers\" style=\"width:100%; font-size:20px;\"><table style=\" text-align: left;\">            <tbody>   " +
            "<tr>                    <th> Name: </th>                    <td>_name_</td>                </tr>         " +
            "<tr><th>Email: </th><td>_email_</td></tr>" +
            "<tr><th>Department: </th><td>_dept_</td></tr>" +
            "<tr><th>Phone Number: </th><td>_number_</td></tr>" +
            "<tr><th>Task Name:</th><td>_tn_</td></tr>" +
            "<tr><th>Task Type: </th><td>_tt_</td></tr>" +
             "<th>CEU:</th><td>_ceu_</td></tr>" +
            "<tr><th>Description:</th><td>_desc_</td></tr>" +
            "</tbody></table>";
        public static string empDepartmentTaskReport = "<tr>      " +
        "  <td>_name_</td>  " +
        "      <td>_tt_</td>    " +
        "    <td>_tn_</td>     " +
        "    <td>_aby_</td>     " +
        "    <td>_prior_</td>     " +

        "   <td>_sd_</td>    " +
        "   <td>_ed_</td>    " +
           "   <td>_cd_</td>    " +

           "   <td>_cs_</td>    " +


        "          </tr>";

        public static string empDivisionReport = "<tr>      " +
           "  <td>_name_</td>  " +
          "      <td>_tt_</td>    " +
           "    <td>_tn_</td>     " +
           "    <td>_aby_</td>     " +
           "    <td>_ato_</td>     " +
               "    <td>_prior_</td>     " +
           "    <td>_sd_</td>     " +

           "   <td>_ed_</td>    " +
              "   <td>_cd_</td>    " +

              "   <td>_cs_</td>    " +


           "          </tr>";

        public static string CourseCompletionCertificate = "<center><h1 style='color:green'>Course Completion Certificate</h1></center>" +
       "<div style='align-left'>" +
       "<h3>Course Name: <span>_cname_</span></h3>" +
       "<h3>Task Name: <span>_tn_</span></h3>" +
       "<h3>Completed By: <span>_CB</span></h3>" +
       "<h3>Authorizing Officer: <span>_AO</span></h3>" +
       "<h3>Completion Date: <span>_cd_</span></h3>" +
       "<h3>Percentage: <span>_mip_</span></h3>" +
       "<h3>Result Status: <span>_status_</span></h3>" +
       "</div>";
        //Course Completion

        public const string HTML_AuthorizingOfficer = "_AO";
        public const string HTML_CommpletedBy = "_CB";
        public const string HTML_CourseName = "_cname_";
        public const string HTML_MarksInPercentage = "_mip_";
        public const string HTML_Status = "_status_";


        public const string HTML_UserName = "_name_";
        public const string HTML_Task_Type = "_tt_";
        public const string HTML_Task_Name = "_tn_";
        public const string HTML_Assigned__By = "_aby_";
        public const string HTML_Assigned__To = "_ato_";
        public const string HTML_Cost = "_cr_";
        //public const string HTML_Revenue = "_rev_";
        public const string HTML_WorkStatus = "_ws_";
        public const string HTML_Desciption = "_desc_";
        public const string HTML_Division = "_division_";

        public const string HTML_startDate = "_sd_";
        public const string HTML_assignDate = "_assindate_";
        public const string HTML_endDate = "_ed_";
        public const string HTML_completionDate = "_cd_";
        public const string HTML_priority = "_prior_";
        public const string HTML_ceu = "_ceu_";
        public const string HTML_hrs = "_hr_";
        public const string HTML_Completion_Status = "_cs_";
        public const string HTML_Frequency = "_freq_";
        public const string HTML_Grade = "_grade_";
        public const string HTML_AssignedBy = "_agnby_";

        public const string HTML_Name = "_Name_";
        //public const string HTML_Job = "_job_";
        public const string HTML_Department = "_dept_";
        public const string HTML_Number = "_number_";
        public const string HTML_Email = "_email_";
    }


}