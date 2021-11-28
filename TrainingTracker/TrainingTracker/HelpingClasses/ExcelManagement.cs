using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.BL;
using TrainingTracker.Models;


namespace TrainingTracker.HelpingClasses
{
    public class ExcelManagement
    {

        //public static void CreateSpreadsheetWorkbook(string filepath)
        //{
        //    sdd(filepath);
        //    return;
        //    List<Manager> managers = new ManagerBL().getAllManagersList();
        //    // Create a spreadsheet document by supplying the filepath.
        //    // By default, AutoSave = true, Editable = true, and Type = xlsx.
        //    SpreadsheetDocument ssDoc = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook);

        //    WorkbookPart workbookpart = ssDoc.AddWorkbookPart();
        //    workbookpart.Workbook = new Workbook();

        //    Sheets sheets = ssDoc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
        //    // Add a WorkbookPart to the document.


        //    // Add a WorksheetPart to the WorkbookPart.
        //    WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
        //    worksheetPart.Worksheet = new Worksheet(new SheetData());

        //    // Add Sheets to the Workbook.


        //    // Append a new worksheet and associate it with the workbook.
        //    Sheet sheet = new Sheet() { Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Managers" };
        //    sheets.Append(sheet);

        //    // Get the sheetData cell table.
        //    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

        //    // Add a row to the cell table.
        //    Row row;


        //    // In the new row, find the column location to insert a cell in A1.



        //     row = new Row();

        //    row.Append(
        //        new Cell() { CellValue = new CellValue("Id"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //        new Cell() { CellValue = new CellValue("First Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //          new Cell() { CellValue = new CellValue("Last Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //            new Cell() { CellValue = new CellValue("Email"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //              new Cell() { CellValue = new CellValue("Phone Number"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //                new Cell() { CellValue = new CellValue("Home Number"), DataType = new EnumValue<CellValues>(CellValues.String) },                        
        //                    new Cell() { CellValue = new CellValue("Designation"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //                      new Cell() { CellValue = new CellValue("Employee Id"), DataType = new EnumValue<CellValues>(CellValues.String),},
        //                      new Cell() { CellValue = new CellValue("Status"), DataType = new EnumValue<CellValues>(CellValues.String) }
        //                );


        //    // Insert the header row to the Sheet Data
        //    sheetData.AppendChild(row);
        //    int managersStartingnumber = 101;
        //    // Set the cell value to be a numeric value of 100.
        //    foreach (var item in managers)
        //    {
        //        row = new Row();
        //        row.Append(new Cell() { CellValue = new CellValue((item.Id+managersStartingnumber).ToString()), DataType = new EnumValue<CellValues>(CellValues.Number) },
        //             new Cell() { CellValue = new CellValue(item.FirstName), DataType = new EnumValue<CellValues>(CellValues.String) },
        //          new Cell() { CellValue = new CellValue(item.LastName), DataType = new EnumValue<CellValues>(CellValues.String) },
        //            new Cell() { CellValue = new CellValue(item.Email), DataType = new EnumValue<CellValues>(CellValues.String) },
        //              new Cell() { CellValue = new CellValue(item.PhoneNumber), DataType = new EnumValue<CellValues>(CellValues.String) },
        //                new Cell() { CellValue = new CellValue(item.HomeNumber), DataType = new EnumValue<CellValues>(CellValues.String) },
        //                    new Cell() { CellValue = new CellValue(item.Designation), DataType = new EnumValue<CellValues>(CellValues.String) },
        //                      new Cell() { CellValue = new CellValue(item.EmployeeID), DataType = new EnumValue<CellValues>(CellValues.String), },
        //                      new Cell() { CellValue = new CellValue(item.IsActive==1?"Active" : "Inactive"), DataType = new EnumValue<CellValues>(CellValues.String) }
        //         );
        //        sheetData.AppendChild(row);
        //    }


        //    // Add a WorksheetPart to the WorkbookPart.
        //    WorksheetPart worksheetPart2 = workbookpart.AddNewPart<WorksheetPart>();
        //    worksheetPart.Worksheet = new Worksheet(new SheetData());

        //    // Append a new worksheet and associate it with the workbook.
        //    Sheet sheet1 = new Sheet() { Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart2), SheetId = 1, Name = "Managers" };
        //    sheets.Append(sheet1);

        //    // Get the sheetData cell table.
        //    SheetData sheetData1 = worksheetPart.Worksheet.GetFirstChild<SheetData>();

        //    // Add a row to the cell table.



        //    // In the new row, find the column location to insert a cell in A1.



        //    row = new Row();

        //    row.Append(
        //        new Cell() { CellValue = new CellValue("Id"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //        new Cell() { CellValue = new CellValue("First Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //          new Cell() { CellValue = new CellValue("Last Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //            new Cell() { CellValue = new CellValue("Email"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //              new Cell() { CellValue = new CellValue("Phone Number"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //                new Cell() { CellValue = new CellValue("Home Number"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //                    new Cell() { CellValue = new CellValue("Designation"), DataType = new EnumValue<CellValues>(CellValues.String) },
        //                      new Cell() { CellValue = new CellValue("Employee Id"), DataType = new EnumValue<CellValues>(CellValues.String), },
        //                      new Cell() { CellValue = new CellValue("Status"), DataType = new EnumValue<CellValues>(CellValues.String) }
        //                );


        //    // Insert the header row to the Sheet Data
        //    sheetData.AppendChild(row);

        //    // Set the cell value to be a numeric value of 100.
        //    foreach (var item in managers)
        //    {
        //        row = new Row();
        //        row.Append(new Cell() { CellValue = new CellValue((item.Id + managersStartingnumber).ToString()), DataType = new EnumValue<CellValues>(CellValues.Number) },
        //             new Cell() { CellValue = new CellValue(item.FirstName), DataType = new EnumValue<CellValues>(CellValues.String) },
        //          new Cell() { CellValue = new CellValue(item.LastName), DataType = new EnumValue<CellValues>(CellValues.String) },
        //            new Cell() { CellValue = new CellValue(item.Email), DataType = new EnumValue<CellValues>(CellValues.String) },
        //              new Cell() { CellValue = new CellValue(item.PhoneNumber), DataType = new EnumValue<CellValues>(CellValues.String) },
        //                new Cell() { CellValue = new CellValue(item.HomeNumber), DataType = new EnumValue<CellValues>(CellValues.String) },
        //                    new Cell() { CellValue = new CellValue(item.Designation), DataType = new EnumValue<CellValues>(CellValues.String) },
        //                      new Cell() { CellValue = new CellValue(item.EmployeeID), DataType = new EnumValue<CellValues>(CellValues.String), },
        //                      new Cell() { CellValue = new CellValue(item.IsActive == 1 ? "Active" : "Inactive"), DataType = new EnumValue<CellValues>(CellValues.String) }
        //         );
        //        sheetData.AppendChild(row);
        //    }

        //    ssDoc.Close();
        //}


        public static void generateExcelFile(string saveFile, bool isDeleteInclude, bool managerInclude, bool employeeInclude, bool taskInclude, bool employeeTask, bool managerTask, bool groupTask)
        {
            SpreadsheetDocument ssDoc = SpreadsheetDocument.Create(saveFile,
            SpreadsheetDocumentType.Workbook);
            int sheetId = 1;
            WorkbookPart workbookPart = ssDoc.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            Sheets sheets = ssDoc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            if (managerInclude)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getManagersSheet(true, worksheetPart);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Managers"
                };
                sheets.Append(sheet);
                sheetId++;

            }
            if (employeeInclude)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getEmployeesSheet(true, worksheetPart);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Employees"
                };
                sheets.Append(sheet);
                sheetId++;
            }
            if (taskInclude)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getTasksData(true, worksheetPart);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Tasks"
                };
                sheets.Append(sheet);
                sheetId++;
            }
            if (employeeTask)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getEmployeesTaskData(true, worksheetPart);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Employee Task"
                };
                sheets.Append(sheet);
                sheetId++;
                WorksheetPart worksheetPart2 = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getEmployeesTaskComment(true, worksheetPart2);
                Sheet sheet2 = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart2),
                    SheetId = (uint)sheetId,
                    Name = "Employee Task Comments"
                };
                sheets.Append(sheet2);
                sheetId++;
            }
            if (managerTask)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getManagerTask(true, worksheetPart);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Manager Task"
                };
                sheets.Append(sheet);
                sheetId++;
                WorksheetPart worksheetPart2 = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getManagerTaskComment(true, worksheetPart2);
                Sheet sheet2 = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart2),
                    SheetId = (uint)sheetId,
                    Name = "Manager Task Comments"
                };
                sheets.Append(sheet2);
                sheetId++;
            }
            if (groupTask)
            {
                List<GroupTasks_Details> gtlist = new List<GroupTasks_Details>();//Emplty
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getGroupTask(true, worksheetPart);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Group Tasks"
                };
                sheets.Append(sheet);
                sheetId++;
                WorksheetPart worksheetPart2 = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getGroupTaskComment(true, worksheetPart2,gtlist);
                Sheet sheet2 = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart2),
                    SheetId = (uint)sheetId,
                    Name = "Group Task Comments"
                };
                sheets.Append(sheet2);
                sheetId++;
            }

            // Begin: Code block for Excel sheet 1

            // End: Code block for Excel sheet 2

            ssDoc.Close();
        }
        /// <summary>
        /// This method is used to download the excel sheets for specific queries on the employees, managers, tasks and grouptasks pages
        /// </summary>
        /// <param name="saveFile">Path where file is supposed to download</param>
        /// <param name="trainees"></param>
        /// <param name="managers"></param>
        /// <param name="tasks"></param>
        /// <param name="groupTasks_Details"></param>
        public static void generateGenericExcelFile(string saveFile, List<User> trainees = null, List<User> managers = null, List<Task> tasks = null, List<GroupTasks_Details> groupTasks_Details = null, List<User_Task> trainee_Tasks = null, List<User_Task> taskManger = null, List<User_Task> taskadmin = null)
        {
            SpreadsheetDocument ssDoc = SpreadsheetDocument.Create(saveFile,
           SpreadsheetDocumentType.Workbook);
            int sheetId = 1;
            WorkbookPart workbookPart = ssDoc.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            Sheets sheets = ssDoc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
            if (managers != null)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getManagersSheet(true, worksheetPart, managers);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Managers"
                };
                sheets.Append(sheet);
                sheetId++;

            }
            if (trainees != null)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getEmployeesSheet(true, worksheetPart, trainees);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Employees"
                };
                sheets.Append(sheet);
                sheetId++;
            }
            if (tasks != null)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getTasksData(true, worksheetPart, tasks);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Tasks"
                };
                sheets.Append(sheet);
                sheetId++;
            }
            if (trainee_Tasks != null)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getEmployeesTaskData(true, worksheetPart, trainee_Tasks);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Employee Task"
                };
                sheets.Append(sheet);
                sheetId++;

            }
            if (taskManger != null)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getManagerTask(true, worksheetPart, taskManger);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Manager Task"
                };
                sheets.Append(sheet);
                sheetId++;

            }
            if (taskadmin != null)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getManagerTask(true, worksheetPart, taskadmin);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Admin Task"
                };
                sheets.Append(sheet);
                sheetId++;

            }

            if (groupTasks_Details != null)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getGroupTask(true, worksheetPart, groupTasks_Details);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Group Tasks"
                };
                sheets.Append(sheet);
                sheetId++;
                WorksheetPart worksheetPart2 = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = getGroupTaskComment(true, worksheetPart2, groupTasks_Details);
                Sheet sheet2 = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart2),
                    SheetId = (uint)sheetId,
                    Name = "Group Task Comments"
                };
                sheets.Append(sheet2);
                sheetId++;
            }

            // Begin: Code block for Excel sheet 1

            // End: Code block for Excel sheet 2

            ssDoc.Close();



        }
        /// <summary>
        /// /Managers start from 100
        /// employees start from 1000
        /// tasks start from 5000
        /// grouptasks from 10000
        /// trainee tasks 15000
        /// manager tasks 20000
        /// 
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        /// 
        public static void generateDepartmentExcel(string saveFile, List<DepartmentReportDTO> trainees = null, string title="")
        {
            SpreadsheetDocument ssDoc = SpreadsheetDocument.Create(saveFile,
           SpreadsheetDocumentType.Workbook);
            int sheetId = 1;
            WorkbookPart workbookPart = ssDoc.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            Sheets sheets = ssDoc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());


            if (trainees != null)
            {
               
           
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
             

                Worksheet workSheet2 = new Worksheet();
                SheetData sheetData2 = new SheetData();
                Row row = new Row();
                //row.Append(new Cell { CellValue = new CellValue(title), DataType = new EnumValue<CellValues>(CellValues.String) });
               // sheetData2.AppendChild(row);
                Columns columns = new Columns();

                columns.Append(new Column() { Min = 1, Max = 1, Width = 40, CustomWidth = true });
                columns.Append(new Column() { Min = 2, Max = 8, Width = 15, CustomWidth = true });
                workSheet2.Append(columns);

                Workbook workbook = new Workbook();

                



                row.Append(
                            new Cell() { CellValue = new CellValue(title), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue(""), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue(""), DataType = new EnumValue<CellValues>(CellValues.String) },
                                  new Cell() { CellValue = new CellValue(""), DataType = new EnumValue<CellValues>(CellValues.String) },
                                    new Cell() { CellValue = new CellValue(""), DataType = new EnumValue<CellValues>(CellValues.String) },
                                           new Cell() { CellValue = new CellValue(""), DataType = new EnumValue<CellValues>(CellValues.String), },
                                           new Cell() { CellValue = new CellValue(""), DataType = new EnumValue<CellValues>(CellValues.String), },
                                           new Cell() { CellValue = new CellValue(""), DataType = new EnumValue<CellValues>(CellValues.String), },
                                           new Cell() { CellValue = new CellValue(""), DataType = new EnumValue<CellValues>(CellValues.String), }
                                      );

                  sheetData2.AppendChild(row);



                //  row = new Row();


                //row.Append(
                //            new Cell() { CellValue = new CellValue("Employee"), DataType = new EnumValue<CellValues>(CellValues.String) },
                //              new Cell() { CellValue = new CellValue("Task Type"), DataType = new EnumValue<CellValues>(CellValues.String) },
                //                new Cell() { CellValue = new CellValue("Task Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                //                  new Cell() { CellValue = new CellValue("Priority"), DataType = new EnumValue<CellValues>(CellValues.String) },
                //                    new Cell() { CellValue = new CellValue("Start Date"), DataType = new EnumValue<CellValues>(CellValues.String) },
                //                           new Cell() { CellValue = new CellValue("Due Date"), DataType = new EnumValue<CellValues>(CellValues.String), },
                //                           new Cell() { CellValue = new CellValue("Completion Date"), DataType = new EnumValue<CellValues>(CellValues.String), },
                //                           new Cell() { CellValue = new CellValue("Completion Status"), DataType = new EnumValue<CellValues>(CellValues.String), }
                //                      );



                //sheetData2.AppendChild(row);

                // Set the cell value to be a numeric value of 100.
                int flag = 0;
                foreach (var item in trainees)
                {
                    if (flag == 0)
                    {
                        row = new Row();
                        row.Append(new Cell() { CellValue = new CellValue("Employee"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                      new Cell() { CellValue = new CellValue("Task Type"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                        new Cell() { CellValue = new CellValue("Task Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                        new Cell() { CellValue = new CellValue("Assigned By"), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue("Priority"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue("Start Date"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                       new Cell() { CellValue = new CellValue("Due Date"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                       new Cell() { CellValue = new CellValue("Completion Date"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                       new Cell() { CellValue = new CellValue("Completion Status"), DataType = new EnumValue<CellValues>(CellValues.String), }
                            );
                        sheetData2.AppendChild(row);
                        flag++;
                        row = new Row();
                        row.Append(new Cell() { CellValue = new CellValue(item.EmployeeName), DataType = new EnumValue<CellValues>(CellValues.String) },
                                      new Cell() { CellValue = new CellValue(item.TaskType != null ? item.TaskType : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                        new Cell() { CellValue = new CellValue(item.TaskName != null ? item.TaskName : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                        new Cell() { CellValue = new CellValue(item.AssignedBy != null ? item.AssignedBy : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue(item.Priority), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue(item.StartDate != null ? item.StartDate : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                       new Cell() { CellValue = new CellValue(item.DueDate != null ? item.DueDate : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                       new Cell() { CellValue = new CellValue(item.CompletionDate != null ? item.CompletionDate : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                       new Cell() { CellValue = new CellValue(item.CompletionStatus != null ? item.CompletionStatus : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), }
                            );
                        sheetData2.AppendChild(row);
                    }
                    else
                    {
                        row = new Row();
                        row.Append(new Cell() { CellValue = new CellValue(item.EmployeeName), DataType = new EnumValue<CellValues>(CellValues.String) },
                                      new Cell() { CellValue = new CellValue(item.TaskType != null ? item.TaskType : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                        new Cell() { CellValue = new CellValue(item.TaskName != null ? item.TaskName : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                            new Cell() { CellValue = new CellValue(item.AssignedBy != null ? item.AssignedBy : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                        new Cell() { CellValue = new CellValue(item.Priority), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue(item.StartDate != null ? item.StartDate : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                       new Cell() { CellValue = new CellValue(item.DueDate != null ? item.DueDate : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                       new Cell() { CellValue = new CellValue(item.CompletionDate != null ? item.CompletionDate : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                       new Cell() { CellValue = new CellValue(item.CompletionStatus != null ? item.CompletionStatus : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), }
                            );
                        sheetData2.AppendChild(row);
                    }
             
                }








                workSheet2.AppendChild(sheetData2);
                 worksheetPart.Worksheet = workSheet2;


                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Department Data"
                };
                sheets.Append(sheet);
                sheetId++;
               





            }
            ssDoc.Close();



        }
        public static void generateDivisionExcel(string saveFile, List<DivisionReportDTO> trainees = null, string title = "")
        {
            SpreadsheetDocument ssDoc = SpreadsheetDocument.Create(saveFile,
           SpreadsheetDocumentType.Workbook);
            int sheetId = 1;
            WorkbookPart workbookPart = ssDoc.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            Sheets sheets = ssDoc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());


            if (trainees != null)
            {


                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();


                Worksheet workSheet2 = new Worksheet();
                SheetData sheetData2 = new SheetData();
                Row rowTitle = new Row();
                rowTitle.Append(new Cell { CellValue = new CellValue(title), DataType = new EnumValue<CellValues>(CellValues.String) });
                sheetData2.AppendChild(rowTitle);
                Columns columns = new Columns();

                columns.Append(new Column() { Min = 1, Max = 1, Width = 40, CustomWidth = true });
                columns.Append(new Column() { Min = 2, Max = 8, Width = 15, CustomWidth = true });
                workSheet2.Append(columns);





                Row row = new Row();


                row.Append(
                            new Cell() { CellValue = new CellValue("Department"), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue("Task Type"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue("Task Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue("Assigned By"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue("Assigned To"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                  new Cell() { CellValue = new CellValue("Priority"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                    new Cell() { CellValue = new CellValue("Start Date"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                           new Cell() { CellValue = new CellValue("Due Date"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                           new Cell() { CellValue = new CellValue("Completion Date"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                           new Cell() { CellValue = new CellValue("Completion Status"), DataType = new EnumValue<CellValues>(CellValues.String), }
                                      );




                sheetData2.AppendChild(row);
                // Set the cell value to be a numeric value of 100.
                foreach (var item in trainees)
                {
                  

                    row = new Row();
                    row.Append(new Cell() { CellValue = new CellValue(item.DepartmentName), DataType = new EnumValue<CellValues>(CellValues.String) },
                                  new Cell() { CellValue = new CellValue(item.TaskType != null ? item.TaskType : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                    new Cell() { CellValue = new CellValue(item.TaskName != null ? item.TaskName : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                    new Cell() { CellValue = new CellValue(item.AssignedBy != null ? item.AssignedBy : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                    new Cell() { CellValue = new CellValue(item.AssignedTo != null ? item.AssignedTo : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                          new Cell() { CellValue = new CellValue(item.Priority), DataType = new EnumValue<CellValues>(CellValues.String) },
                            new Cell() { CellValue = new CellValue(item.StartDate != null ? item.StartDate : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                   new Cell() { CellValue = new CellValue(item.DueDate != null ? item.DueDate : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                   new Cell() { CellValue = new CellValue(item.CompletionDate != null ? item.CompletionDate : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                   new Cell() { CellValue = new CellValue(item.CompletionStatus != null ? item.CompletionStatus : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), }
                        );
                    sheetData2.AppendChild(row);
                }








                workSheet2.AppendChild(sheetData2);
                worksheetPart.Worksheet = workSheet2;


                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Department Data"
                };
                sheets.Append(sheet);
                sheetId++;






            }
            ssDoc.Close();



        }
        public static void generateEmployeeExcel(string saveFile, List<DivisionReportDTO> trainees = null, string title = "")
        {
            SpreadsheetDocument ssDoc = SpreadsheetDocument.Create(saveFile,
           SpreadsheetDocumentType.Workbook);
            int sheetId = 1;
            WorkbookPart workbookPart = ssDoc.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            Sheets sheets = ssDoc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());


            if (trainees != null)
            {


                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();


                Worksheet workSheet2 = new Worksheet();
                SheetData sheetData2 = new SheetData();
                Row rowTitle = new Row();
                rowTitle.Append(new Cell { CellValue = new CellValue(title), DataType = new EnumValue<CellValues>(CellValues.String) });
                sheetData2.AppendChild(rowTitle);
                Columns columns = new Columns();

                columns.Append(new Column() { Min = 1, Max = 1, Width = 40, CustomWidth = true });
                columns.Append(new Column() { Min = 2, Max = 8, Width = 15, CustomWidth = true });
                workSheet2.Append(columns);





                Row row = new Row();


                row.Append(
                            new Cell() { CellValue = new CellValue("Department"), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue("Task Type"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue("Task Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                  new Cell() { CellValue = new CellValue("Priority"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                    new Cell() { CellValue = new CellValue("Start Date"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                           new Cell() { CellValue = new CellValue("Due Date"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                           new Cell() { CellValue = new CellValue("Completion Date"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                           new Cell() { CellValue = new CellValue("Completion Status"), DataType = new EnumValue<CellValues>(CellValues.String), }
                                      );


                sheetData2.AppendChild(row);


                // Set the cell value to be a numeric value of 100.
                foreach (var item in trainees)
                {
                    row = new Row();
                    row.Append(new Cell() { CellValue = new CellValue(item.DepartmentName), DataType = new EnumValue<CellValues>(CellValues.String) },
                                  new Cell() { CellValue = new CellValue(item.TaskType != null ? item.TaskType : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                    new Cell() { CellValue = new CellValue(item.TaskName != null ? item.TaskName : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                          new Cell() { CellValue = new CellValue(item.Priority), DataType = new EnumValue<CellValues>(CellValues.String) },
                            new Cell() { CellValue = new CellValue(item.StartDate != null ? item.StartDate : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                   new Cell() { CellValue = new CellValue(item.DueDate != null ? item.DueDate : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                   new Cell() { CellValue = new CellValue(item.CompletionDate != null ? item.CompletionDate : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                   new Cell() { CellValue = new CellValue(item.CompletionStatus != null ? item.CompletionStatus : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), }
                        );
                    sheetData2.AppendChild(row);
                }








                workSheet2.AppendChild(sheetData2);
                worksheetPart.Worksheet = workSheet2;


                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Department Data"
                };
                sheets.Append(sheet);
                sheetId++;






            }
            ssDoc.Close();



        }



        public static WorksheetPart getManagersSheet(bool includeDeleteDate, WorksheetPart ws, List<User> man = null)
        {

            List<User> managers;
            if (man == null)
            {
                managers = new UserBL().getManagerList().Where(x => x.IsActive == 1).ToList();
                //managers = new UserBL().getAllUsersList().Where(x =>x.Role==2|| x.Role==4 &&  x.IsActive == 1 ).ToList();
                //if (!includeDeleteDate)
                //{
                //    managers = managers.Where(x => x.IsActive == 1).ToList();
                //}

            }
            else
                managers = man;

            Worksheet workSheet2 = new Worksheet();
            SheetData sheetData2 = new SheetData();

            // the data for sheet 2
            Row row = new Row();


            row.Append(
               // new Cell() { CellValue = new CellValue("Id"), DataType = new EnumValue<CellValues>(CellValues.String) },
                new Cell() { CellValue = new CellValue("First Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                  new Cell() { CellValue = new CellValue("Last Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                    new Cell() { CellValue = new CellValue("Email"), DataType = new EnumValue<CellValues>(CellValues.String) },
                      new Cell() { CellValue = new CellValue("Phone Number"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue("Home Number"), DataType = new EnumValue<CellValues>(CellValues.String) }
                         //new Cell() { CellValue = new CellValue("Employee Id"), DataType = new EnumValue<CellValues>(CellValues.String), }
                        //new Cell() { CellValue = new CellValue("Status"), DataType = new EnumValue<CellValues>(CellValues.String) }
                        );


            // Insert the header row to the Sheet Data
            sheetData2.AppendChild(row);

            // Set the cell value to be a numeric value of 100.
            foreach (var item in managers)
            {
                row = new Row();
                row.Append(new Cell() { CellValue = new CellValue(item.FirstName), DataType = new EnumValue<CellValues>(CellValues.String) },
                  new Cell() { CellValue = new CellValue(item.LastName), DataType = new EnumValue<CellValues>(CellValues.String) },
                    new Cell() { CellValue = new CellValue(item.Email), DataType = new EnumValue<CellValues>(CellValues.String) },
                      new Cell() { CellValue = new CellValue(item.PhoneNumber), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue(item.HomeNumber), DataType = new EnumValue<CellValues>(CellValues.String) }
                   //new Cell() { CellValue = new CellValue(item.EmployeeID), DataType = new EnumValue<CellValues>(CellValues.String), }
                 //new Cell() { CellValue = new CellValue(item.IsActive == 1 ? "Active" : "Inactive"), DataType = new EnumValue<CellValues>(CellValues.String) }
                 );
                sheetData2.AppendChild(row);
            }








            workSheet2.AppendChild(sheetData2);
            ws.Worksheet = workSheet2;
            return ws;
        }

        public static WorksheetPart getEmployeesSheet(bool includeDeleteDate, WorksheetPart ws, List<User> train = null)
        {
            List<User> trainees;
            if (train == null)
            {
                trainees = new UserBL().getTraineesList().Where(x => x.IsActive == 1).ToList();
                //if (!includeDeleteDate)
                //{
                //    trainees = trainees.Where(x => x.IsActive == 1).ToList();
                //}
            }
            else
                trainees = train;

            Worksheet workSheet2 = new Worksheet();
            SheetData sheetData2 = new SheetData();

            // the data for sheet 2
            Row row = new Row();


            row.Append(
                //new Cell() { CellValue = new CellValue("Id"), DataType = new EnumValue<CellValues>(CellValues.String) },
                new Cell() { CellValue = new CellValue("First Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                  new Cell() { CellValue = new CellValue("Last Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                    new Cell() { CellValue = new CellValue("Email"), DataType = new EnumValue<CellValues>(CellValues.String) },
                      new Cell() { CellValue = new CellValue("Phone Number"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue("Home Number"), DataType = new EnumValue<CellValues>(CellValues.String) }
                        //new Cell() { CellValue = new CellValue("Employee Id"), DataType = new EnumValue<CellValues>(CellValues.String), }
                        //new Cell() { CellValue = new CellValue("Status"), DataType = new EnumValue<CellValues>(CellValues.String) }
                        );


            // Insert the header row to the Sheet Data
            sheetData2.AppendChild(row);

            // Set the cell value to be a numeric value of 100.
            foreach (var item in trainees)
            {
                row = new Row();
                row.Append(new Cell() { CellValue = new CellValue(item.FirstName), DataType = new EnumValue<CellValues>(CellValues.String) },
                  new Cell() { CellValue = new CellValue(item.LastName), DataType = new EnumValue<CellValues>(CellValues.String) },
                    new Cell() { CellValue = new CellValue(item.Email), DataType = new EnumValue<CellValues>(CellValues.String) },
                      new Cell() { CellValue = new CellValue(item.PhoneNumber), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue(item.HomeNumber), DataType = new EnumValue<CellValues>(CellValues.String) }
                 //new Cell() { CellValue = new CellValue(item.EmployeeID), DataType = new EnumValue<CellValues>(CellValues.String), }
                 //new Cell() { CellValue = new CellValue(item.IsActive == 1 ? "Active" : "Inactive"), DataType = new EnumValue<CellValues>(CellValues.String) }
                 );
                sheetData2.AppendChild(row);
            }








            workSheet2.AppendChild(sheetData2);
            ws.Worksheet = workSheet2;
            return ws;
        }

        public static WorksheetPart getTasksData(bool includeDeleteDate, WorksheetPart ws, List<Task> ta = null)
        {

            List<Task> trainees;
            if (ta == null)
            {
                trainees = new TaskBL().getAllTasksList().Where(x => x.IsActive == 1).ToList();
                //if (!includeDeleteDate)
                //{
                //    trainees = trainees.Where(x => x.IsActive == 1).ToList();
                //}
            }
            else
                trainees = ta;

            Worksheet workSheet2 = new Worksheet();
            SheetData sheetData2 = new SheetData();

            // the data for sheet 2
            Row row = new Row();


            row.Append(
                       // new Cell() { CellValue = new CellValue("Id"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue("Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                          new Cell() { CellValue = new CellValue("Task Type"), DataType = new EnumValue<CellValues>(CellValues.String) },
                            new Cell() { CellValue = new CellValue("Division"), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue("Description"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue("Cost"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                       new Cell() { CellValue = new CellValue("CEU"), DataType = new EnumValue<CellValues>(CellValues.String), }
                                       //  new Cell() { CellValue = new CellValue("Manager Id"), DataType = new EnumValue<CellValues>(CellValues.String), }
                                //new Cell() { CellValue = new CellValue("Status"), DataType = new EnumValue<CellValues>(CellValues.String) }
                                );


            // Insert the header row to the Sheet Data
            sheetData2.AppendChild(row);

            // Set the cell value to be a numeric value of 100.
            foreach (var item in trainees)
            {
                row = new Row();
                row.Append(new Cell() { CellValue = new CellValue(item.Name), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue(item.TaskType != null ? item.TaskType.Name : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue(item.Division != null ? item.Division.Name : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                      new Cell() { CellValue = new CellValue(item.Description), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue(item.Cost_.HasValue ? item.Cost_.Value.ToString() : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.CEU.HasValue ? item.CEU.Value.ToString() : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), }
                             // new Cell() { CellValue = new CellValue((item.UserId + 101).ToString()), DataType = new EnumValue<CellValues>(CellValues.String), }

                 //new Cell() { CellValue = new CellValue(item.IsActive == 1 ? "Active" : "Inactive"), DataType = new EnumValue<CellValues>(CellValues.String) }
                 );
                sheetData2.AppendChild(row);
            }








            workSheet2.AppendChild(sheetData2);
            ws.Worksheet = workSheet2;
            return ws;
        }

        public static WorksheetPart getEmployeesTaskData(bool includeDeleteDate, WorksheetPart ws, List<User_Task> tt = null)
        {
            List<User_Task> traineesTask;
            if (tt == null)
            {
                traineesTask = new User_TaskBL().getUser_TasksList().Where(x =>x.User1.Role==3 && x.IsActive == 1).ToList();
                //if (!includeDeleteDate)
                //{
                //    traineesTask = traineesTask.Where(x => x.IsActive == 1).ToList();
                //}
            }
            else
            {
                traineesTask = tt;
            }
            Worksheet workSheet2 = new Worksheet();
            SheetData sheetData2 = new SheetData();

            // the data for sheet 2
            Row row = new Row();


            row.Append(
                       // new Cell() { CellValue = new CellValue("Id"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue("Employee Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue("Employee Task Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                          new Cell() { CellValue = new CellValue("Start Date"), DataType = new EnumValue<CellValues>(CellValues.String) },
                            new Cell() { CellValue = new CellValue("End Date"), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue("Cost"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue("CEU"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                       new Cell() { CellValue = new CellValue("Hours"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                         new Cell() { CellValue = new CellValue("Assigned By"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                         new Cell() { CellValue = new CellValue("Priority"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                           new Cell() { CellValue = new CellValue("Task Status"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                        new Cell() { CellValue = new CellValue("Notes"), DataType = new EnumValue<CellValues>(CellValues.String), }

                                //new Cell() { CellValue = new CellValue("Status"), DataType = new EnumValue<CellValues>(CellValues.String) }
                                );


            // Insert the header row to the Sheet Data
            sheetData2.AppendChild(row);
          
            // Set the cell value to be a numeric value of 100.
            foreach (var item in traineesTask)
            {
                
                row = new Row();
                row.Append(new Cell() { CellValue = new CellValue(item.User1.FirstName +" "+item.User1.LastName) , DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue(item.Task.Name ), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue(item.StartDate.Value.ToString("MM/dd/yyyy")), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.EndDate.Value.ToString("MM/dd/yyyy")), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.Cost.HasValue ? item.Cost.Value.ToString() : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.CEU.HasValue ? item.CEU.Value.ToString() : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), },
                               new Cell() { CellValue = new CellValue(item.Hours.HasValue ? item.Hours.Value.ToString() : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.CreatedID != null ? item.User.FirstName +" "+item.User.LastName : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), },
                               new Cell() { CellValue = new CellValue(item.Priority.HasValue ? General_Purpose.getPriorityValue(item.Priority.Value) : "Not Applicable"), DataType = new EnumValue<CellValues>(CellValues.String), },
                               new Cell() { CellValue = new CellValue(item.Status.HasValue ? General_Purpose.getStatusValue(item.Status.Value) : "Not Applicable"), DataType = new EnumValue<CellValues>(CellValues.String), },
                               new Cell() { CellValue = new CellValue(item.Notes), DataType = new EnumValue<CellValues>(CellValues.String), }
                 //new Cell() { CellValue = new CellValue(item.IsActive == 1 ? "Active" : "Inactive"), DataType = new EnumValue<CellValues>(CellValues.String) }
                 );
                sheetData2.AppendChild(row);
            }
            workSheet2.AppendChild(sheetData2);
            ws.Worksheet = workSheet2;
            return ws;
        }

        public static WorksheetPart getManagerTask(bool includeDeleteDate, WorksheetPart ws, List<User_Task> tm = null)
        {
            List<User_Task> taskManagers;
            if (tm == null)
            {
                taskManagers = new User_TaskBL().getUser_TasksList().Where(x =>x.User1.Role==2 || x.User1.Role==4 && x.IsActive == 1).ToList();
                //if (!includeDeleteDate)
                //{
                //    taskManagers = taskManagers.Where(x => x.IsActive == 1).ToList();
                //}
            }
            else
            {
                taskManagers = tm;
            }
            Worksheet workSheet2 = new Worksheet();
            SheetData sheetData2 = new SheetData();

            // the data for sheet 2
            Row row = new Row();


            row.Append(
                       // new Cell() { CellValue = new CellValue("Id"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue("Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue("Task Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                          new Cell() { CellValue = new CellValue("Start Date"), DataType = new EnumValue<CellValues>(CellValues.String) },
                            new Cell() { CellValue = new CellValue("End Date"), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue("Cost"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue("CEU"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                       new Cell() { CellValue = new CellValue("Hours"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                          new Cell() { CellValue = new CellValue("Priority"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                           new Cell() { CellValue = new CellValue("Task Status"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                        new Cell() { CellValue = new CellValue("Notes"), DataType = new EnumValue<CellValues>(CellValues.String), }

                                //new Cell() { CellValue = new CellValue("Status"), DataType = new EnumValue<CellValues>(CellValues.String) }
                                );


            // Insert the header row to the Sheet Data
            sheetData2.AppendChild(row);
            int tasksValue = 5000;
            // Set the cell value to be a numeric value of 100.
            foreach (var item in taskManagers)
            {

                row = new Row();
                row.Append(new Cell() { CellValue = new CellValue(item.User1.FirstName + " "+item.User1.LastName), DataType = new EnumValue<CellValues>(CellValues.String), },
                new Cell() { CellValue = new CellValue(item.Task.Name), DataType = new EnumValue<CellValues>(CellValues.String), },
                               new Cell() { CellValue = new CellValue(item.StartDate.Value.ToString("MM/dd/yyyy")), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.EndDate.Value.ToString("MM/dd/yyyy")), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.Cost.HasValue ? item.Cost.Value.ToString() : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.CEU.HasValue ? item.CEU.Value.ToString() : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), },
                               new Cell() { CellValue = new CellValue(item.Hours.HasValue ? item.Hours.Value.ToString() : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.Priority.HasValue ? General_Purpose.getPriorityValue(item.Priority.Value) : "Not Applicable"), DataType = new EnumValue<CellValues>(CellValues.String), },
                               new Cell() { CellValue = new CellValue(item.Status.HasValue ? General_Purpose.getStatusValue(item.Status.Value) : "Not Applicable"), DataType = new EnumValue<CellValues>(CellValues.String), },
                               new Cell() { CellValue = new CellValue(item.Notes), DataType = new EnumValue<CellValues>(CellValues.String), }
                 //new Cell() { CellValue = new CellValue(item.IsActive == 1 ? "Active" : "Inactive"), DataType = new EnumValue<CellValues>(CellValues.String) }
                 );
                sheetData2.AppendChild(row);
            }
            workSheet2.AppendChild(sheetData2);
            ws.Worksheet = workSheet2;
            return ws;
        }

        public static WorksheetPart getEmployeesTaskComment(bool includeDeleteDate, WorksheetPart ws)
        {
            List<TaskComment> traineesTask = new TaskCommentBL().getTaskCommentsList().Where(x => x.IsActive == 1 && x.User_Task.IsActive==1).ToList();
            traineesTask = traineesTask.Where(x => x.User_Task.User1.Role == 3).ToList();
            
            Worksheet workSheet2 = new Worksheet();
            SheetData sheetData2 = new SheetData();

            // the data for sheet 2
            Row row = new Row();


            row.Append(
                       // new Cell() { CellValue = new CellValue("Id"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue("Task Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                          new Cell() { CellValue = new CellValue("Employee Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                            new Cell() { CellValue = new CellValue("Comment"), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue("File"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                      new Cell() { CellValue = new CellValue("Date"), DataType = new EnumValue<CellValues>(CellValues.String) }
                           );


            // Insert the header row to the Sheet Data
            sheetData2.AppendChild(row);

            // Set the cell value to be a numeric value of 100.
            foreach (var item in traineesTask)
            {

                row = new Row();
                row.Append(new Cell() { CellValue = new CellValue(item.User_Task.Task.Name), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue(item.User_Task.User1.FirstName + " " + item.User_Task.User1.LastName), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.Comment), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.File != null ? "Yes" : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.Date.ToString()), DataType = new EnumValue<CellValues>(CellValues.String), }
                 );
                sheetData2.AppendChild(row);
            }








            workSheet2.AppendChild(sheetData2);
            ws.Worksheet = workSheet2;
            return ws;
        }

        public static WorksheetPart getManagerTaskComment(bool includeDeleteDate, WorksheetPart ws)
        {
            List<TaskComment> traineesTask = new TaskCommentBL().getTaskCommentsList().Where(x =>x.User_Task.User1.Role==2 || x.User_Task.User1.Role==4 ).ToList();
            traineesTask = traineesTask.Where(x => x.IsActive == 1 && x.User_Task.IsActive == 1).ToList();
            Worksheet workSheet2 = new Worksheet();
            SheetData sheetData2 = new SheetData();

            // the data for sheet 2
            Row row = new Row();


            row.Append(
                        //new Cell() { CellValue = new CellValue("Id"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue("Task Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                          new Cell() { CellValue = new CellValue("Manager Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                            new Cell() { CellValue = new CellValue("Comment"), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue("File"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                      new Cell() { CellValue = new CellValue("Date"), DataType = new EnumValue<CellValues>(CellValues.String) }
                           );


            // Insert the header row to the Sheet Data
            sheetData2.AppendChild(row);

            // Set the cell value to be a numeric value of 100.
            foreach (var item in traineesTask)
            {

                row = new Row();
                row.Append(new Cell() { CellValue = new CellValue(item.User_Task.Task.Name.ToString()), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue(item.User_Task.User1.FirstName + " " + item.User_Task.User1.LastName), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.Comment), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.File != null ? "Yes" : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.Date.ToString()), DataType = new EnumValue<CellValues>(CellValues.String), }
                 );
                sheetData2.AppendChild(row);
            }








            workSheet2.AppendChild(sheetData2);
            ws.Worksheet = workSheet2;
            return ws;
        }

        public static WorksheetPart getGroupTask(bool includeDeleteDate, WorksheetPart ws, List<GroupTasks_Details> gd = null)
        {
            List<GroupTasks_Details> groupTasks;
            if (gd == null)
            {
                groupTasks = new GroupTasks_DetailsBL().getGroupTasks_DetailssList().Where(x => x.IsActive == 1).ToList();
                //if (!includeDeleteDate)
                //{
                //    groupTasks = groupTasks.Where(x => x.IsActive == 1).ToList();
                //}
            }
            else
                groupTasks = gd;
            Worksheet workSheet2 = new Worksheet();
            SheetData sheetData2 = new SheetData();

            // the data for sheet 2
            Row row = new Row();


            row.Append(
                        //new Cell() { CellValue = new CellValue("Id"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue("Prime Lead Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue("Secondary Lead Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                         new Cell() { CellValue = new CellValue("Employees"), DataType = new EnumValue<CellValues>(CellValues.String) },
                         new Cell() { CellValue = new CellValue("Task Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                      new Cell() { CellValue = new CellValue("Start Date"), DataType = new EnumValue<CellValues>(CellValues.String) },
                            new Cell() { CellValue = new CellValue("End Date"), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue("Cost"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                new Cell() { CellValue = new CellValue("CEU"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                       new Cell() { CellValue = new CellValue("Hours"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                             new Cell() { CellValue = new CellValue("Task Status"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                        new Cell() { CellValue = new CellValue("Notes"), DataType = new EnumValue<CellValues>(CellValues.String), }

                                //new Cell() { CellValue = new CellValue("Status"), DataType = new EnumValue<CellValues>(CellValues.String) }
                                );


            // Insert the header row to the Sheet Data
            sheetData2.AppendChild(row);

            // Set the cell value to be a numeric value of 100.
            foreach (var item in groupTasks)
            {
                string employees = "";
                string tasks = "";

                string prime = string.Empty;
                string secondary = string.Empty;
                if (item.GroupTask_User.Where(x => x.LeadRole == 1).Count() != 0)
                {
                    User m = item.GroupTask_User.Where(x => x.LeadRole == 1).FirstOrDefault().User;
                    prime = m.FirstName + " "+m.LastName;
                }
                else
                    prime = "NA";
                if (item.GroupTask_User.Where(x => x.LeadRole == 2).Count() != 0)
                {
                    User m = item.GroupTask_User.Where(x => x.LeadRole == 2).FirstOrDefault().User;

                    secondary = m.FirstName + " "+m.LastName;
                }
                else
                    secondary = "NA";
                foreach (var emp in item.GroupTask_User.Where(x=>x.IsActive==1 &&x.LeadRole!=1 && x.LeadRole!=2).ToList())
                {
                    employees = employees + emp.User.FirstName + " "+emp.User.LastName;
                }
                if (item.GroupTask != null)
                    foreach (var tas in item.GroupTask.GroupTask_Task)
                    {
                        tasks = tasks + tas.Task.Name ;
                    }

                row = new Row();
                row.Append(new Cell() { CellValue = new CellValue(prime), DataType = new EnumValue<CellValues>(CellValues.String), },
                               new Cell() { CellValue = new CellValue(secondary), DataType = new EnumValue<CellValues>(CellValues.String), },
                               new Cell() { CellValue = new CellValue(employees), DataType = new EnumValue<CellValues>(CellValues.String), },
                              new Cell() { CellValue = new CellValue(item.GroupTask.Name), DataType = new EnumValue<CellValues>(CellValues.String), },
                              new Cell() { CellValue = new CellValue(item.StartDate.Value.ToString("MM/dd/yyyy")), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.EndDate.Value.ToString("MM/dd/yyyy")), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.Cost.HasValue ? item.Cost.Value.ToString() : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.CEU.HasValue ? item.CEU.Value.ToString() : "NA"), DataType = new EnumValue<CellValues>(CellValues.String), },
                               new Cell() { CellValue = new CellValue(item.Hours.HasValue ? item.Hours.Value.ToString() : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.Status.HasValue ? General_Purpose.getStatusValue(item.Status.Value) : "Not Applicable"), DataType = new EnumValue<CellValues>(CellValues.String), },
                                 new Cell() { CellValue = new CellValue(item.Notes), DataType = new EnumValue<CellValues>(CellValues.String), }
                 //new Cell() { CellValue = new CellValue(item.IsActive == 1 ? "Active" : "Inactive"), DataType = new EnumValue<CellValues>(CellValues.String) }
                 );
                sheetData2.AppendChild(row);
            }
            workSheet2.AppendChild(sheetData2);
            ws.Worksheet = workSheet2;
            return ws;
        }

        public static WorksheetPart getGroupTaskComment(bool includeDeleteDate, WorksheetPart ws,List<GroupTasks_Details> groupTasks_DetailsList)
        {
            List<GroupTaskComment> traineesTask = new List<GroupTaskComment>();

            foreach (GroupTasks_Details val in groupTasks_DetailsList)
            {
                List<GroupTaskComment> temp = new GroupTaskCommentBL().getGroupTaskCommentsList().Where(x => x.IsActive == 1 && x.GroupTaskDetailsId == val.Id).ToList();
                foreach (GroupTaskComment item in temp)
                {
                    GroupTasks_Details gtd = new GroupTasks_DetailsBL().getGroupTasks_DetailssById((int)item.GroupTaskDetailsId);
                    if (gtd.IsActive == 1)
                    {
                        traineesTask.Add(item);
                    }
                }
            }
         
            
            Worksheet workSheet2 = new Worksheet();
            SheetData sheetData2 = new SheetData();

            // the data for sheet 2
            Row row = new Row();


            row.Append(
                       // new Cell() { CellValue = new CellValue("Id"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell() { CellValue = new CellValue("Group Task Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                          new Cell() { CellValue = new CellValue("User Name"), DataType = new EnumValue<CellValues>(CellValues.String) },
                            new Cell() { CellValue = new CellValue("Comment"), DataType = new EnumValue<CellValues>(CellValues.String) },
                              new Cell() { CellValue = new CellValue("File"), DataType = new EnumValue<CellValues>(CellValues.String) },
                                      new Cell() { CellValue = new CellValue("Date"), DataType = new EnumValue<CellValues>(CellValues.String) }
                           );


            // Insert the header row to the Sheet Data
            sheetData2.AppendChild(row);

            // Set the cell value to be a numeric value of 100.
            foreach (var item in traineesTask)
            {

                row = new Row();
                row.Append(new Cell() { CellValue = new CellValue(item.GroupTasks_Details.GroupTask.Name), DataType = new EnumValue<CellValues>(CellValues.String) },
                new Cell() { CellValue = new CellValue(item.UserId != null? item.User.FirstName+" "+item.User.LastName: item.GroupTask_User.User.FirstName + " " + item.GroupTask_User.User.LastName), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.Comment), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.File != null ? "Yes" : "NA"), DataType = new EnumValue<CellValues>(CellValues.String) },
                               new Cell() { CellValue = new CellValue(item.Date.ToString()), DataType = new EnumValue<CellValues>(CellValues.String), }
                 ); ;
                sheetData2.AppendChild(row);
            }

           






            workSheet2.AppendChild(sheetData2);
            ws.Worksheet = workSheet2;
            return ws;
        }


        #region Generating Error Message reports

        public static void GenerateAddUserErrorExcel(string saveFile, List<string> ErrorMsg = null)
        {
            SpreadsheetDocument ssDoc = SpreadsheetDocument.Create(saveFile,
            SpreadsheetDocumentType.Workbook);

            int sheetId = 1;
            WorkbookPart workbookPart = ssDoc.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            Sheets sheets = ssDoc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            if (ErrorMsg != null)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart = GenerateAddUserErrorExcelSheet(true, worksheetPart, ErrorMsg);
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)sheetId,
                    Name = "Error report"
                };
                sheets.Append(sheet);
                sheetId++;
            }

            ssDoc.Close();
        }

        public static WorksheetPart GenerateAddUserErrorExcelSheet(bool includeDeleteDate, WorksheetPart ws, List<string> ErrorMsg = null)
        {


            Worksheet workSheet2 = new Worksheet();
            SheetData sheetData2 = new SheetData();


            Row row = new Row();
            row.Append(
                new Cell() { CellValue = new CellValue("Following Errors Occurred While Inserting Record"), DataType = new EnumValue<CellValues>(CellValues.String) }
                      );
            sheetData2.AppendChild(row);


            foreach (string item in ErrorMsg)
            {
                row = new Row();
                row.Append(
                    new Cell() { CellValue = new CellValue(item), DataType = new EnumValue<CellValues>(CellValues.String) }
                        );
                sheetData2.AppendChild(row);
            }

            
            workSheet2.AppendChild(sheetData2);
            ws.Worksheet = workSheet2;
            return ws;
        }


        #endregion
    }
}