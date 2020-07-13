using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskList.Core.DTOs;
using Microsoft.EntityFrameworkCore.Internal;
using TaskList.Data.Model;
using System.IO;

namespace TaskList.Business.Helpers
{
    public static class TaskExport
    {
        public static byte[] GetTaskExportFile()
        {
            var tasks = Tasks.GetTasksForExport();
            var staffTypes = StaffTypes.GetAll(true);
            var staffTypeNames = staffTypes.Select(x => x.Name).ToList();

            var columnNames = new List<string>()
            {
                "Name", "Area", "Sub Area", "Frequency", "Notes", "Policy Tech", "Procedure File Name"
            };
            staffTypeNames.ForEach(x => columnNames.Add(x));

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Tasks");
                worksheet.ColumnWidth = 20;
                worksheet.Style.Alignment.WrapText = true;

                // Header
                for (var i = 0; i < columnNames.Count; i++)
                {
                    var column = i + 1;
                    worksheet.Cell(1, column).Value = columnNames[i];
                    worksheet.Cell(1, column).Style.Font.Bold = true;
                    worksheet.Cell(1, column).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                }

                var row = 2;
                foreach (TaskExportDTO task in tasks)
                {
                    var policyTechYesNo = task.IsInPolicyTech ? "Yes" : "No";

                    worksheet.Cell(row, 1).Value = task.Name;
                    worksheet.Cell(row, 2).Value = task.AreaName;
                    worksheet.Cell(row, 3).Value = task.SubAreaName;
                    worksheet.Cell(row, 4).Value = task.FrequencyName;
                    worksheet.Cell(row, 5).Value = task.Notes;
                    worksheet.Cell(row, 6).Value = policyTechYesNo;
                    worksheet.Cell(row, 7).Value = task.ProcedureFileName;

                    var staffGroups = task.Staff.ToLookup(s => s.StaffTypeName,
                         s => s.Name);

                    var column = 8;

                    foreach (StaffTypeDTO type in staffTypes)
                    {
                        var names = staffGroups[type.Name];
                        worksheet.Cell(row, column).Value = names.ToList().Join("\n"); ;
                        column += 1;
                    }

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
    }
}
