using ClosedXML.Excel;
using System.Collections.Generic;
using System.Linq;
using TaskList.Core.DTOs;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;

namespace TaskList.Business.Helpers
{
    public static class TaskExport
    {
        public static byte[] GetTaskExportFile()
        {
            var tasks = Tasks.GetAllTasks();
            var staffTypes = StaffTypes.GetStaffTypesForExport();

            // Loop through tasks to determine how many of each staff type there are
            foreach (TaskDTO task in tasks)
            {
                var staffTypeLookup = Staffs.GetSecondaryTaskStaffLookup(task.TaskId);

                foreach (StaffTypeDTO staffType in staffTypes)
                {
                    var taskStaffTypeUserNames = staffTypeLookup[staffType.Name];
                    var taskStaffTypeUserCount = taskStaffTypeUserNames.Count();

                    if (taskStaffTypeUserCount > staffType.Max)
                    {
                        staffType.Max = taskStaffTypeUserCount;
                    }
                }
            }

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Tasks");
                worksheet.ColumnWidth = 20;
                worksheet.Column(1).Width = 5;
                worksheet.Column(7).Width = 10;
                worksheet.Column(2).Width = 40;
                worksheet.Style.Alignment.WrapText = true;

                AddHeaderRow(worksheet, staffTypes);

                var row = 2;
                foreach (TaskDTO task in tasks)
                {
                    AddTaskRow(worksheet, task, row, staffTypes);
                    row += 1;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }

        private static void AddHeaderRow(IXLWorksheet worksheet, IEnumerable<StaffTypeDTO> staffTypes)
        {
            var columnNames = new List<string>()
            {
                "Display Order", "Name", "Area", "Sub Area", "Frequency", "Notes", "Policy Tech", "Procedure File Name", "Primary Staff"
            };

            foreach (var staffType in staffTypes)
            {
                if(staffType.Max == 1)
                {
                    columnNames.Add(staffType.Name);
                }
                else
                {
                    for (var i = 0; i < staffType.Max; i++)
                    {
                        columnNames.Add($"{staffType.Name} {i + 1}");
                    }
                }
            }

            var column = 1;
            foreach(var columnName in columnNames)
            {
                worksheet.Cell(1, column).Value = columnName;
                column++;
            }

            // Header is bold and has thick bottom border.
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Row(1).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            worksheet.Row(1).Style.Alignment.WrapText = false;
        }

        private static void AddTaskRow(IXLWorksheet worksheet, TaskDTO task, int row, IEnumerable<StaffTypeDTO> staffTypes)
        {
            worksheet.Cell(row, 1).Value = task.DisplayOrder;
            worksheet.Cell(row, 2).Value = task.Name;
            worksheet.Cell(row, 3).Value = task.AreaName;
            worksheet.Cell(row, 4).Value = task.SubAreaName;
            worksheet.Cell(row, 5).Value = task.FrequencyName;
            worksheet.Cell(row, 6).Value = task.Notes;
            worksheet.Cell(row, 7).Value = task.IsInPolicyTech ? "Yes" : "No";
            worksheet.Cell(row, 8).Value = task.ProcedureFileName;
            worksheet.Cell(row, 9).Value = task.PrimaryStaffName;

            var staffTypeLookup = Staffs.GetSecondaryTaskStaffLookup(task.TaskId);
            var column = 10;

            // Secondary Staff
            foreach (StaffTypeDTO staffType in staffTypes)
            {
                var taskStaffTypeUserNames = staffTypeLookup[staffType.Name];
                var taskStaffTypeUserCount = taskStaffTypeUserNames.Count();

                if(taskStaffTypeUserCount == 0)
                {
                    worksheet.Cell(row, column).Value = "";                    
                } 
                else
                {
                    var tempColumnStart = column;

                    foreach (var name in taskStaffTypeUserNames)
                    {
                        worksheet.Cell(row, tempColumnStart).Value = name;
                        tempColumnStart += 1;
                    }
                }

                // Move to the next set of staff type columns
                column += staffType.Max;
            }
        }
    }
}
