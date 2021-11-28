using IronPdf;
using TrainingTracker.HelpingClasses.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using static IronPdf.PdfPrintOptions;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text;
using System.IO;
using iTextSharp.tool.xml.pipeline;
using TrainingTracker.BL;

namespace TrainingTracker.HelpingClasses
{
    public class PDF_Export
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

        public void generate_groupTaskDetails_pdf(GroupTasks_Details gtd, string path)
        {
            IronPdf.License.LicenseKey = ProjectVaraiables.IRONPDF_License;
            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            string gtds = HTML_PDF_Varaiables.HTML_GroupTaskDetails;
            string dates = HTML_PDF_Varaiables.HTML_GroupTask_DateDetails;
            gtds = gtds.Replace(HTML_PDF_Varaiables.HTML_Desciption, gtd.Notes);
            gtds = gtds.Replace(HTML_PDF_Varaiables.HTML_Cost, gtd.Cost.HasValue ? gtd.Cost.Value.ToString() : "NA");
            gtds = gtds.Replace(HTML_PDF_Varaiables.HTML_Task_Name, gtd.GroupTask.Name);
            gtds = gtds.Replace(HTML_PDF_Varaiables.HTML_WorkStatus, gtd.Status.HasValue ? General_Purpose.getStatusValue(gtd.Status.Value) : "NA");
            // gtds = gtds.Replace(HTML_PDF_Varaiables.HTML_Division, gtd.GroupTask_Manager.Count>0? gtd.GroupTask_Manager.FirstOrDefault().Manager.Division.Name:"NA");
            //gtds = gtds.Replace(HTML_PDF_Varaiables.HTML_Revenue, "NA");
            gtds = gtds.Replace(HTML_PDF_Varaiables.HTML_hrs, gtd.Hours.HasValue ? gtd.Hours.Value.ToString() : "NA");

            if (gtd.CreatedBy.HasValue)
            {
                User u = new UserBL().getUsersById((int)gtd.CreatedBy);

                gtds = gtds.Replace(HTML_PDF_Varaiables.HTML_AssignedBy, gtd.CreatedBy.HasValue ? u.FirstName.ToString() + " " + u.LastName.ToString() : "NA");

            }
            else
            {
                gtds = gtds.Replace(HTML_PDF_Varaiables.HTML_AssignedBy, "NA");

            }


            dates = dates.Replace(HTML_PDF_Varaiables.HTML_assignDate, gtd.CreatedAt.HasValue ? gtd.CreatedAt.Value.ToString("MM/dd/yyyy") : gtd.StartDate.Value.ToString("MM/dd/yyyy"));
            dates = dates.Replace(HTML_PDF_Varaiables.HTML_startDate, gtd.StartDate.HasValue ? gtd.StartDate.Value.ToString("MM/dd/yyyy") : "NA");
            dates = dates.Replace(HTML_PDF_Varaiables.HTML_endDate, gtd.EndDate.HasValue ? gtd.EndDate.Value.ToString("MM/dd/yyyy") : "NA");
            dates = dates.Replace(HTML_PDF_Varaiables.HTML_completionDate, gtd.CompletionDate.HasValue ? gtd.CompletionDate.Value.ToString("MM/dd/yyyy") : "Not Completed Yet");
            dates = dates.Replace(HTML_PDF_Varaiables.HTML_priority, General_Purpose.getPriorityValue(gtd.Priority.HasValue ? gtd.Priority.Value : 1));
            dates = dates.Replace(HTML_PDF_Varaiables.HTML_WorkStatus, gtd.Status.HasValue ? General_Purpose.getStatusValue(gtd.Status.Value) : "NA");
            List<GroupTask_User> managers = gtd.GroupTask_User.OrderBy(o => o.LeadRole).ToList();
            gtds = gtds + "</br> </br>      <hr>";

            foreach (var item in managers)
            {
                string content = string.Empty;
                if (item.LeadRole == (int)Enums.GroupTaskLead.Primary)
                    content = HTML_PDF_Varaiables.HTML_GroupTask_Prime;
                else if (item.LeadRole == (int)Enums.GroupTaskLead.Secondary)
                    content = HTML_PDF_Varaiables.HTML_GroupTask_Second;
                else
                {
                    content = HTML_PDF_Varaiables.HTML_GroupTask_Employee;
                }

                content = content.Replace(HTML_PDF_Varaiables.HTML_Name, item.User.FirstName + " " + item.User.LastName);
                content = content.Replace(HTML_PDF_Varaiables.HTML_Number, item.User.PhoneNumber);
                content = content.Replace(HTML_PDF_Varaiables.HTML_Department, item.User.DivisionId.HasValue ? new BL.DivisionBL().getDivisionsById(item.User.DivisionId.Value).Name : item.User.DepartmentId.HasValue ? new BL.DepartmentBL().getDepartmentsById(item.User.DepartmentId.Value).Name : "Admin");
                content = content.Replace(HTML_PDF_Varaiables.HTML_Email, item.User.Email);
                //content = content.Replace(HTML_PDF_Varaiables.HTML_Job, "NA");
                gtds = gtds + content + "     <hr>";
            }



            gtds = gtds + dates;
            string time = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToString("MMMM dd,yyyy");
            Renderer.PrintOptions.Footer = new HtmlHeaderFooter()
            {
                FontFamily = "Helvetica,Arial",
                Height = 15,
                HtmlFragment = "<footer style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\"><table style=\"width:100%;\"><tr><td style=\"text-align:left;width:20%;\">" + time + "</td><td style=\"text-align:center;width:60%;\"><img src=\"\\Content\\landing_page\\images\\logo.png\" width=\"260px\" height=\"55\"></td><td style=\"text-align:right;width:20%;\">page {page}</td></tr></table></footer>"
                //DrawDividerLine = true
            };
            Renderer.PrintOptions.Header.FontFamily = "Helvetica,Arial";
            Renderer.PrintOptions.PaperSize = PdfPaperSize.A4Rotated;
            // Build a header using an image asset
            // Note the use of BaseUrl to set a relative path to the assets
            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {

                Height = 30,
                HtmlFragment = "<div id=\"customers\" style=\"width:100%; font-size:20px;\">    <div style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\">    <h2 >Group Task Report</h2></div></div>"
                //DrawDividerLine = true
                //BaseUrl = new Uri(@"C:\assets\images\").AbsoluteUri
            };
            Renderer.RenderHtmlAsPdf((gtds + "</div>"))
               .SaveAs(path);

        }

        public void generate_employeeTask_pdf(List<User_Task> tasks, string path)
        {

            IronPdf.License.LicenseKey = ProjectVaraiables.IRONPDF_License;
            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            string totalPdf = string.Empty;

            foreach (var item in tasks)
            {
                string epdf = HTML_PDF_Varaiables.Trainee_Task_Pdf;
                string lowercontent = HTML_PDF_Varaiables.Trainee_task_DateDetails;
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_UserName, item.User1.FirstName + " " + item.User1.LastName);
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_Email, item.User1.Email);
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_Department, item.User1.DepartmentId.HasValue ? new BL.DepartmentBL().getDepartmentsById(item.User1.DepartmentId.Value).Name : "NA");
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_Number, item.User1.PhoneNumber);
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_Task_Name, item.Task.Name);
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_Task_Type, item.Task.TaskType != null ? item.Task.TaskType.Name : "");
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_ceu, item.CEU.HasValue ? item.CEU.Value.ToString() : "NA");
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_Desciption, item.Notes != null ? item.Notes : "");

                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_startDate, item.StartDate.HasValue ? item.StartDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_endDate, item.EndDate.HasValue ? item.EndDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_completionDate, item.CompletionDate.HasValue ? item.CompletionDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Grade, item.Grad.HasValue ? item.Grad.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Frequency, item.RepeatTime.HasValue ? item.RepeatTime.ToString() + " days" : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Completion_Status, item.CompletionDate.HasValue ? "Completed" : item.EndDate.Value < DateTime.Now ? "Late" : "In process");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_priority, General_Purpose.getPriorityValue(item.Priority.HasValue ? item.Priority.Value : 1));
                totalPdf = totalPdf + epdf + lowercontent;
                totalPdf = totalPdf + "</br> </br>";
            }

            string time = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToString("MMMM dd,yyyy");
            Renderer.PrintOptions.Footer = new HtmlHeaderFooter()
            {
                FontFamily = "Helvetica,Arial",
                Height = 15,
                HtmlFragment = "<footer style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\"><table style=\"width:100%;\"><tr><td style=\"text-align:left;width:20%;\">" + time + "</td><td style=\"text-align:center;width:60%;\"><img src=\"\\Content\\landing_page\\images\\logo.png\" width=\"260px\" height=\"55\"></td><td style=\"text-align:right;width:20%;\">page {page}</td></tr></table></footer>"
                //DrawDividerLine = true
            };
            Renderer.PrintOptions.Header.FontFamily = "Helvetica,Arial";
            Renderer.PrintOptions.PaperSize = PdfPaperSize.A4Rotated;
            // Build a header using an image asset
            // Note the use of BaseUrl to set a relative path to the assets
            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {

                Height = 30,
                HtmlFragment = "<div id=\"customers\" style=\"width:100%; font-size:20px;\">    <div style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\">    <h2 >Employee Report</h2></div></div>"
                //DrawDividerLine = true
                //BaseUrl = new Uri(@"C:\assets\images\").AbsoluteUri
            };


            Renderer.RenderHtmlAsPdf((totalPdf + "</div>"))
              .SaveAs(path);


        }

        public void generate_ManagerTask_pdf(List<User_Task> tasks, string path, string name = "Manager")
        {

            IronPdf.License.LicenseKey = ProjectVaraiables.IRONPDF_License;
            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            string totalPdf = string.Empty;

            foreach (var item in tasks)
            {
                string epdf = HTML_PDF_Varaiables.Trainee_Task_Pdf;
                string lowercontent = HTML_PDF_Varaiables.Trainee_task_DateDetails;
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_UserName, item.User1.FirstName + " " + item.User1.LastName);
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_Email, item.User1.Email);
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_Department, item.User1.DepartmentId.HasValue ? new BL.DepartmentBL().getDepartmentsById(item.User1.DepartmentId.Value).Name : "NA");
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_Number, item.User1.PhoneNumber);
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_Task_Name, item.Task.Name);
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_Task_Type, item.Task.TaskType.Name);
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_ceu, item.CEU.HasValue ? item.CEU.Value.ToString() : "NA");
                epdf = epdf.Replace(HTML_PDF_Varaiables.HTML_Desciption, item.Notes != null ? item.Notes : "");

                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_startDate, item.StartDate.HasValue ? item.StartDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_endDate, item.EndDate.HasValue ? item.EndDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_completionDate, item.CompletionDate.HasValue ? item.CompletionDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Grade, item.Grad.HasValue ? item.Grad.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Frequency, item.RepeatTime.HasValue ? item.RepeatTime.ToString() + " days" : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Completion_Status, item.CompletionDate.HasValue ? "Completed" : item.EndDate.Value < DateTime.Now ? "Late" : "In process");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_priority, General_Purpose.getPriorityValue(item.Priority.HasValue ? item.Priority.Value : 1));

                totalPdf = totalPdf + epdf + lowercontent;
                totalPdf = totalPdf + "</br> </br>";
            }
            string time = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToString("MMMM dd,yyyy");
            Renderer.PrintOptions.Footer = new HtmlHeaderFooter()
            {
                FontFamily = "Helvetica,Arial",
                Height = 15,

                HtmlFragment = "<footer style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\"><table style=\"width:100%;\"><tr><td style=\"text-align:left;width:20%;\">" + time + "</td><td style=\"text-align:center;width:60%;\"><img src=\"\\Content\\landing_page\\images\\logo.png\" width=\"260px\" height=\"55\"></td><td style=\"text-align:right;width:20%;\">page {page}</td></tr></table></footer>"
                //DrawDividerLine = true
            };
            Renderer.PrintOptions.Header.FontFamily = "Helvetica,Arial";
            Renderer.PrintOptions.PaperSize = PdfPaperSize.A4Rotated;
            // Build a header using an image asset
            // Note the use of BaseUrl to set a relative path to the assets
            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {

                Height = 30,
                HtmlFragment = "<div id=\"customers\" style=\"width:100%; font-size:20px;\">    <div style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\">    <h2 >" + name + " Task Report</h2></div></div>"
                //DrawDividerLine = true
                //BaseUrl = new Uri(@"C:\assets\images\").AbsoluteUri
            };


            Renderer.RenderHtmlAsPdf((totalPdf + "</div>"))
              .SaveAs(path);


        }

        public string generate_employee_assignments(List<User_Task> tasks, string path)
        {

            Log.Info("Pdf generation inner method 0 called");
            //IronPdf.License.LicenseKey = ProjectVaraiables.IRONPDF_License;
            Log.Info("Pdf generation inner method called 1");
            //IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            Log.Info("Pdf generation inner method called 2");
            string totalPdf = HTML_PDF_Varaiables.employee_assignment_detail;


            Log.Info("Pdf generation inner method 3 level called");
            foreach (var item in tasks)
            {

                string lowercontent = HTML_PDF_Varaiables.empContent;
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_UserName, item.User1.FirstName + " " + item.User1.LastName);

                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Task_Name, item.Task.Name);


                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_startDate, item.StartDate.HasValue ? item.StartDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_endDate, item.EndDate.HasValue ? item.EndDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_completionDate, item.CompletionDate.HasValue ? item.CompletionDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Completion_Status, item.CompletionDate.HasValue ? "Completed" : item.EndDate.Value < DateTime.Now ? "Late" : "In process");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_priority, General_Purpose.getPriorityValue(item.Priority.HasValue ? item.Priority.Value : 1));
                totalPdf = totalPdf + lowercontent;
                // totalPdf = totalPdf + "</br> </br>";
            }


            totalPdf = totalPdf + HTML_PDF_Varaiables.empEnd;
            string time = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToString("MMMM dd,yyyy");
            //Renderer.PrintOptions.Footer = new HtmlHeaderFooter()
            //{
            //    FontFamily = "Helvetica,Arial",
            //    Height = 15,
            //    HtmlFragment = "<footer style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\"><table style=\"width:100%;\"><tr><td style=\"text-align:left;width:20%;\">" + time + "</td><td style=\"text-align:center;width:60%;\"><img src=\"\\Content\\landing_page\\images\\logo.png\" width=\"165px\" height=\"70\"></td><td style=\"text-align:right;width:20%;\">page {page}</td></tr></table></footer>"
            //    //DrawDividerLine = true
            //};
            //Renderer.PrintOptions.Header.FontFamily = "Helvetica,Arial";
            //Renderer.PrintOptions.PaperSize = PdfPaperSize.A4Rotated;
            //// Build a header using an image asset
            //// Note the use of BaseUrl to set a relative path to the assets
            //Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            //{

            //    Height = 30,
            //    HtmlFragment = "<div id=\"customers\" style=\"width:100%; font-size:20px;\">    <div style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\">    <h2 >Employee Assignment Report</h2></div></div>"
            //    //DrawDividerLine = true
            //    //BaseUrl = new Uri(@"C:\assets\images\").AbsoluteUri
            //};


            //Log.Info("Pdf generation inner method completed");
            //Log.Info(path);
            //Renderer.RenderHtmlAsPdf((totalPdf + "</div>"))
            //  .SaveAs(path);

            return (totalPdf);


        }

        public void generate_manager_assignments(List<User_Task> tasks, string path, string name = "Manager", int role = -1)
        {
            Log.Info("Pdf generation inner method 0 called");
            IronPdf.License.LicenseKey = ProjectVaraiables.IRONPDF_License;
            Log.Info("Pdf generation inner method 1 called");
            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            Log.Info("Pdf generation inner method 2 called");
            string totalPdf = "";
            if (role == 3)
                totalPdf = HTML_PDF_Varaiables.employee_assignment_detail2;
            else if (role == 1)
                totalPdf = HTML_PDF_Varaiables.admin_assignment_detail;
            else
                totalPdf = HTML_PDF_Varaiables.manager_assignment_detail;
            totalPdf = totalPdf + "</br>" + "</br>";

            foreach (var item in tasks.OrderByDescending(x => x.Id))
            {

                string lowercontent = HTML_PDF_Varaiables.empContent;
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_UserName, item.User1.FirstName + " " + item.User1.LastName);

                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Task_Name, item.Task.Name);


                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_startDate, item.StartDate.HasValue ? item.StartDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_endDate, item.EndDate.HasValue ? item.EndDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_completionDate, item.CompletionDate.HasValue ? item.CompletionDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Completion_Status, item.CompletionDate.HasValue ? "Completed" : item.EndDate.Value < DateTime.Now ? "Late" : "In process");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_priority, General_Purpose.getPriorityValue(item.Priority.HasValue ? item.Priority.Value : 1));
                totalPdf = totalPdf + lowercontent;
                //totalPdf = totalPdf+"</br>";
            }


            totalPdf = totalPdf + HTML_PDF_Varaiables.empEnd;
            string time = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToString("MMMM dd,yyyy");
            Renderer.PrintOptions.Footer = new HtmlHeaderFooter()
            {
                FontFamily = "Helvetica,Arial",
                Height = 15,
                HtmlFragment = "<footer style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\"><table style=\"width:100%;\"><tr><td style=\"text-align:left;width:20%;\">" + time + "</td><td style=\"text-align:center;width:60%;\"><img src=\"\\Content\\landing_page\\images\\logo.png\" width=\"260px\" height=\"55\"></td><td style=\"text-align:right;width:20%;\">page {page}</td></tr></table></footer>"
                //DrawDividerLine = true
            };
            Renderer.PrintOptions.Header.FontFamily = "Helvetica,Arial";
            Renderer.PrintOptions.PaperSize = PdfPaperSize.A4Rotated;
            // Build a header using an image asset
            // Note the use of BaseUrl to set a relative path to the assets
            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {

                Height = 20,
                HtmlFragment = "<div id=\"customers\" style=\"width:100%; font-size:20px;\">    <div style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\">    <h2 >" + name + " Assignment Report</h2></div></div>"
                //DrawDividerLine = true
                //BaseUrl = new Uri(@"C:\assets\images\").AbsoluteUri
            };
            Log.Info(path);

            Renderer.RenderHtmlAsPdf((totalPdf + "</div>"))
              .SaveAs(path);

        }
        public void generate_division_report(List<DivisionReportDTO> tasks, string path, string title, int isEmpReport = 0)
        {
            Log.Info("Pdf generation inner method 0 called");
            IronPdf.License.LicenseKey = ProjectVaraiables.IRONPDF_License;
            Log.Info("Pdf generation inner method 1 called");
            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            Log.Info("Pdf generation inner method 2 called");
            
            string totalPdf = "";

            if (isEmpReport == 1)
            {
                totalPdf = HTML_PDF_Varaiables.divisionReportHeaderEmp;
                totalPdf = totalPdf+ "</br>" + "</br>";

            }
            else
            {
                totalPdf = HTML_PDF_Varaiables.divisionReportHeader;
                totalPdf = totalPdf + "</br>" + "</br>";
            }

            foreach (var item in tasks)
            {

                if (isEmpReport == 1)
                {
                    string lowercontent = HTML_PDF_Varaiables.empDivisionReportEmp;
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_UserName, item.DepartmentName != null ? item.DepartmentName : "NA");

                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Task_Type, item.TaskType != null ? item.TaskType : "NA");
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Task_Name, item.TaskName);
                    //lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Assigned__By, item.AssignedBy);
                    //lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Assigned__To, item.AssignedTo);
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_priority, item.Priority != null ? item.Priority : "NA");


                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_startDate, item.StartDate != null ? item.StartDate : "NA");
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_endDate, item.DueDate != null ? item.DueDate : "NA");
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_completionDate, item.CompletionDate != null ? item.CompletionDate : "NA");
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Completion_Status, item.CompletionStatus);
                    totalPdf = totalPdf + lowercontent;
                    //totalPdf = totalPdf+"</br>";
                }
                else
                {
                    string lowercontent = HTML_PDF_Varaiables.empDivisionReport;
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_UserName, item.DepartmentName != null ? item.DepartmentName : "NA");

                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Task_Type, item.TaskType != null ? item.TaskType : "NA");
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Task_Name, item.TaskName);
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Assigned__By, item.AssignedBy);
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Assigned__To, item.AssignedTo);
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_priority, item.Priority != null ? item.Priority : "NA");


                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_startDate, item.StartDate != null ? item.StartDate : "NA");
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_endDate, item.DueDate != null ? item.DueDate : "NA");
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_completionDate, item.CompletionDate != null ? item.CompletionDate : "NA");
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Completion_Status, item.CompletionStatus);
                    totalPdf = totalPdf + lowercontent;
                    //totalPdf = totalPdf+"</br>";
                }
            }


            totalPdf = totalPdf + HTML_PDF_Varaiables.empEnd;
            string time = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToString("MMMM dd,yyyy");
            Renderer.PrintOptions.Footer = new HtmlHeaderFooter()
            {
                FontFamily = "Helvetica,Arial",
                Height = 15,
                HtmlFragment = "<footer style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\"><table style=\"width:100%;\"><tr><td style=\"text-align:left;width:20%;\">" + time + "</td><td style=\"text-align:center;width:60%;\"><img src=\"\\Content\\landing_page\\images\\logo.png\" width=\"260px\" height=\"55\"></td><td style=\"text-align:right;width:20%;\">page {page}</td></tr></table></footer>"
                //DrawDividerLine = true
            };
            Renderer.PrintOptions.Header.FontFamily = "Helvetica,Arial";
            Renderer.PrintOptions.PaperSize = PdfPaperSize.A4Rotated;
            // Build a header using an image asset
            // Note the use of BaseUrl to set a relative path to the assets
            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {

                Height = 20,
                HtmlFragment = "<div id=\"customers\" style=\"width:100%; font-size:20px;\">    <div style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\">    <h2 >" + title + "</h2></div></div>"
                //DrawDividerLine = true
                //BaseUrl = new Uri(@"C:\assets\images\").AbsoluteUri
            };
            Log.Info(path);

            Renderer.RenderHtmlAsPdf((totalPdf + "</div>"))
              .SaveAs(path);

        }
        public void generate_department_report(List<DepartmentReportDTO> tasks, string path, string reportTitile)
        {
            Log.Info("Pdf generation inner method 0 called");
            IronPdf.License.LicenseKey = ProjectVaraiables.IRONPDF_License;
            Log.Info("Pdf generation inner method 1 called");
            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            Log.Info("Pdf generation inner method 2 called");
            string totalPdf = HTML_PDF_Varaiables.department_division_ReportDetail;
            totalPdf = totalPdf + "</br>" + "</br>";

            foreach (var item in tasks)
            {

                string lowercontent = HTML_PDF_Varaiables.empDepartmentTaskReport;
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_UserName, item.EmployeeName);

                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Task_Type, item.TaskType);
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Task_Name, item.TaskName);
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Assigned__By, item.AssignedBy);
              //  lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Assigned__To, item.AssignedTo);
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_priority, item.Priority);


                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_startDate, item.StartDate);
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_endDate, item.DueDate);
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_completionDate, item.CompletionDate);
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Completion_Status, item.CompletionStatus);
                totalPdf = totalPdf + lowercontent;
                //totalPdf = totalPdf+"</br>";
            }


            totalPdf = totalPdf + HTML_PDF_Varaiables.empEnd;
            string time = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToString("MMMM dd,yyyy");
            Renderer.PrintOptions.Footer = new HtmlHeaderFooter()
            {
                FontFamily = "Helvetica,Arial",
                Height = 15,
                HtmlFragment = "<footer style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\"><table style=\"width:100%;\"><tr><td style=\"text-align:left;width:20%;\">" + time + "</td><td style=\"text-align:center;width:60%;\"><img src=\"\\Content\\landing_page\\images\\logo.png\" width=\"260px\" height=\"55\"></td><td style=\"text-align:right;width:20%;\">page {page}</td></tr></table></footer>"
                //DrawDividerLine = true
            };
            Renderer.PrintOptions.Header.FontFamily = "Helvetica,Arial";
            Renderer.PrintOptions.PaperSize = PdfPaperSize.A4Rotated;
            // Build a header using an image asset
            // Note the use of BaseUrl to set a relative path to the assets
            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {

                Height = 20,
                HtmlFragment = "<div id=\"customers\" style=\"width:100%; font-size:20px;\">    <div style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\">    <h2 >" + reportTitile + "</h2></div></div>"
                //DrawDividerLine = true
                //BaseUrl = new Uri(@"C:\assets\images\").AbsoluteUri
            };
            Log.Info(path);

            Renderer.RenderHtmlAsPdf((totalPdf + "</div>"))
              .SaveAs(path);
        }


        public void generate_groupTaskDetailsassignment_pdf(List<GroupTasks_Details> gtd, string path, int isGroupStudy)
        {

            IronPdf.License.LicenseKey = ProjectVaraiables.IRONPDF_License;
            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            string totalPdf = HTML_PDF_Varaiables.group_assignment_detail;


            foreach (var item in gtd)
            {
                User manager;
                string lowercontent = HTML_PDF_Varaiables.grpContent;
                if (item.GroupTask_User.Count > 0)
                {
                    GroupTask_User gtu = item.GroupTask_User.Where(x => x.LeadRole == 1).FirstOrDefault();
                    if (gtu != null)
                    {
                        manager = new UserBL().getUsersById((int)gtu.UserId);
                        if (manager == null)
                        {
                            lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_UserName, "NA");
                        }
                        else
                        { 
                            lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_UserName, manager.FirstName + " " + manager.LastName); 
                        }
                    }
                    else
                    {
                        lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_UserName, "NA");
                    }

                }
                else
                {
                    lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_UserName, "NA");

                }
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Task_Name, item.GroupTask.Name);


                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_startDate, item.StartDate.HasValue ? item.StartDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_endDate, item.EndDate.HasValue ? item.EndDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_completionDate, item.CompletionDate.HasValue ? item.CompletionDate.Value.ToString() : "NA");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Completion_Status, item.CompletionDate.HasValue ? "Completed" : item.EndDate.Value < DateTime.Now ? "Late" : "In process");
                lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_priority, General_Purpose.getPriorityValue(item.Priority.HasValue ? item.Priority.Value : 1));
                totalPdf = totalPdf + lowercontent;
                //  totalPdf = totalPdf + "</br> </br>";
            }


            totalPdf = totalPdf + HTML_PDF_Varaiables.empEnd;


            string tempHeader = "";
            if(isGroupStudy != -1)
            {
                tempHeader = "Group Study Report";
            }
            else
            {
                tempHeader = "Group Task Report";
            }
            string time = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToString("MMMM dd,yyyy");
            Renderer.PrintOptions.Footer = new HtmlHeaderFooter()
            {
                FontFamily = "Helvetica,Arial",
                Height = 15,
                HtmlFragment = "<footer style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\"><table style=\"width:100%;\"><tr><td style=\"text-align:left;width:20%;\">" + time + "</td><td style=\"text-align:center;width:60%;\"><img src=\"\\Content\\landing_page\\images\\logo.png\" width=\"260px\" height=\"55\"></td><td style=\"text-align:right;width:20%;\">page {page}</td></tr></table></footer>"
                //DrawDividerLine = true
            };
            Renderer.PrintOptions.Header.FontFamily = "Helvetica,Arial";
            Renderer.PrintOptions.PaperSize = PdfPaperSize.A4Rotated;
            // Build a header using an image asset
            // Note the use of BaseUrl to set a relative path to the assets
            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {

                Height = 30,
                HtmlFragment = "<div id=\"customers\" style=\"width:100%; font-size:20px;\">    <div style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\">    <h2 >"+tempHeader+"</h2></div></div>"
                //DrawDividerLine = true
                //BaseUrl = new Uri(@"C:\assets\images\").AbsoluteUri
            };
            Renderer.RenderHtmlAsPdf((totalPdf + "</div>"))
               .SaveAs(path);

        }

        public void generate_CourseCertificate_PDF(List<Course_UserTask> CUTask, string path, string name = "Course Completion Certificate")
        {
            //   Log.Info("Pdf generation inner method 0 called");
            IronPdf.License.LicenseKey = ProjectVaraiables.IRONPDF_License;
            //     Log.Info("Pdf generation inner method 1 called");
            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            //   Log.Info("Pdf generation inner method 2 called");
            string totalPdf = "";


            // totalPdf = HTML_PDF_Varaiables.Course_result_Detail;
            // totalPdf = totalPdf + "</br>" + "</br>";

            foreach (var item in CUTask)
            {
                Course course = new CourseBL().getCourseById((int)item.CourseID);
                User_Task ut = new User_TaskBL().getUser_TasksById((int)item.User_TaskID);
                Task task = new TaskBL().getTasksById((int)ut.TaskID);
                User user1 = new User();

                User us = new User();
                string name1 = "", status = "", completiondate = "", totalMarks = "";
                if (item.CourseGroupStudyId != null && item.IsLead != 1)
                {
                    user1 = new UserBL().getUsersById((int)item.UserId);
                }
                else
                {
                    user1 = new UserBL().getUsersById((int)ut.UserID);

                }

                if (ut.CreatedID.HasValue)
                {
                    us = new UserBL().getUsersById((int)ut.CreatedID);
                    name1 = us.FirstName + " " + us.LastName;
                }
                else
                {
                    us = new UserBL().getUsersById((int)user1.Id);
                    name1 = us.FirstName + " " + us.LastName;
                }
                if (item.ResultStatus != null)
                {
                    if (item.ResultStatus == 1)
                        status = "Pass";
                    else
                        status = "Fail";
                }
                if (item.CompletedAt != null)
                {
                    completiondate = item.CompletedAt.Value.ToString("MMMM dd, yyyy");
                }
                else
                {
                    completiondate = DateTime.Now.ToString("MMMM dd, yyyy");

                }
                if (item.TotalMarks != null)
                {
                    totalMarks = item.TotalMarks;
                }

                //string lowercontent = HTML_PDF_Varaiables.CourseCompletionCertificate;
                //lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_CourseName, course.Name);
                //lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Task_Name, task.Name);
                //lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_CommpletedBy, user1.FirstName +" "+user1.LastName);
                //lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_AuthorizingOfficer,name1);
                //lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_completionDate, completiondate);
                //lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_MarksInPercentage, totalMarks);
                //lowercontent = lowercontent.Replace(HTML_PDF_Varaiables.HTML_Status, status);
                //totalPdf = totalPdf + lowercontent;


                //string lowercontent = General_Purpose.CustomCourseCompletionCertificate(course.Name,user1.FirstName+" "+user1.LastName,completiondate,name1,course.SignatureImage);
                //string lowercontent = General_Purpose.CustomCertificateTemplate(course.Name,user1.FirstName+" "+user1.LastName,completiondate,name1,course.SignatureImage, us);
                string lowercontent = General_Purpose.CustomTemplate3(course, user1.FirstName + " " + user1.LastName, completiondate, name1, us);

                totalPdf = lowercontent;
            }


            totalPdf = totalPdf + HTML_PDF_Varaiables.empEnd;
            string time = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToString("MMMM dd,yyyy");
            // Renderer.PrintOptions.Footer = new HtmlHeaderFooter()
            //{
            //    FontFamily = "Helvetica,Arial",
            //    Height = 15,
            //    HtmlFragment = "<footer style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\"><table style=\"width:100%;\"><tr><td style=\"text-align:left;width:20%;\">" + time + "</td><td style=\"text-align:center;width:60%;\"><img src=\"\\Content\\landing_page\\images\\logo.png\" width=\"260px\" height=\"55\"></td><td style=\"text-align:right;width:20%;\">page {page}</td></tr></table></footer>"
            //    //DrawDividerLine = true
            //};
            //Renderer.PrintOptions.Header.FontFamily = "Helvetica,Arial";
            // Renderer.PrintOptions.PaperSize = PdfPaperSize.A4Rotated;
            Renderer.PrintOptions.PaperSize = PdfPaperSize.A4Rotated;
            Renderer.PrintOptions.MarginLeft = 0;
            Renderer.PrintOptions.MarginRight = 0;
            Renderer.PrintOptions.MarginTop = 0;
            Renderer.PrintOptions.MarginBottom = 0;

            // Build a header using an image asset
            // Note the use of BaseUrl to set a relative path to the assets
            //Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            //{

            //    Height = 20,
            //    HtmlFragment = "<div id=\"customers\" style=\"width:100%; font-size:20px;\">    <div style=\" text-align: center;border-top:2px solid black; border-bottom:2px solid black;width: 100%;\">    <h2 >" + name + " Report</h2></div></div>"
            //    //DrawDividerLine = true
            //    //BaseUrl = new Uri(@"C:\assets\images\").AbsoluteUri
            //};
            //  Log.Info(path);

            Renderer.RenderHtmlAsPdf((totalPdf))
              .SaveAs(path);

        }

    }

    public class HtmlPageEventHelper : PdfPageEventHelper
    {
        public HtmlPageEventHelper(string html)
        {
            this.html = html;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            ColumnText ct = new ColumnText(writer.DirectContent);
            XMLWorkerHelper.GetInstance().ParseXHtml(new ColumnTextElementHandler(ct), new StringReader(html));
            ct.SetSimpleColumn(document.Left, document.Top, document.Right, document.GetTop(-20), 10, Element.ALIGN_MIDDLE);
            ct.Go();
        }

        string html = null;
    }

    public class HtmlPageEventHelperFooter : PdfPageEventHelper
    {
        int page;
        public HtmlPageEventHelperFooter(string html)
        {
            page = 0;
            this.html = html;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            ColumnText ct = new ColumnText(writer.DirectContent);
            XMLWorkerHelper.GetInstance().ParseXHtml(new ColumnTextElementHandler(ct), new StringReader(html));
            ct.SetSimpleColumn(document.Left, document.Bottom, document.Right, document.GetBottom(-20), 10, Element.ALIGN_MIDDLE);
            ct.Go();
        }

        string html = null;
    }

    public class ColumnTextElementHandler : IElementHandler
    {
        public ColumnTextElementHandler(ColumnText ct)
        {
            this.ct = ct;
        }

        ColumnText ct = null;

        public void Add(IWritable w)
        {
            if (w is WritableElement)
            {
                foreach (IElement e in ((WritableElement)w).Elements())
                {
                    ct.AddElement(e);
                }
            }
        }
    }





}