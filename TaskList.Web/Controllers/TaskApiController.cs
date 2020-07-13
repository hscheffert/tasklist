using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Azure;
using TaskList.Business.Helpers;
using TaskList.Core.DTOs;
using TaskList.Data.Model;

namespace TaskList.Web.Controllers
{
    [Authorize]
    [Route("api/tasks")]
    [ApiController]
    public class TaskApiController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaskApiController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/tasks
        [HttpGet]
        public IEnumerable<TaskDTO> GetAll()
        {
            return Tasks.GetAll();
        }

        // GET: api/tasks/getAllUserTasks/5
        [HttpGet]
        [Route("getAllUserTasks/{id:guid?}")]
        public IEnumerable<TaskDTO> GetAllUserTasks(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return Tasks.GetAllTasks();
            }

            return Tasks.GetAllUsersTasks((Guid)id);
        }

        // GET api/tasks/5
        [HttpGet]
        [Route("{id:guid}")]
        public TaskDTO Get(Guid id)
        {
            return Tasks.GetByID(id);
        }

        // GET api/tasks/getWithDetails/5
        [HttpGet]
        [Route("getWithDetails/{id:guid}")]
        public TaskDetailsDTO GetWithDetails(Guid id)
        {
            return Tasks.GetByIDWithDetails(id);
        }

        // POST api/tasks
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public Guid? Post([FromBody] TaskDetailsDTO dto)
        {
            var email = _httpContextAccessor.HttpContext.User.GetCurrentUserEmail();

            return Tasks.Save(dto, email);
        }

        // PUT api/tasks/toggle/5
        [HttpPut]
        [Authorize(Policy = "Admin")]
        [Route("toggle/{id:guid}")]
        public void Toggle(Guid id)
        {
            Tasks.Toggle(id);
        }

        //[HttpGet]
        //[Route("export")]
        //public IActionResult Export()
        //{
        //    var tasks = Tasks.GetTasksForExport();
        //    var builder = new StringBuilder();
        //    //PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(TaskExportDTO));
        //    //foreach (PropertyDescriptor prop in props)
        //    //{
        //    //    builder.Append(prop.DisplayName);
        //    //    builder.Append(",");
        //    //}
        //    //builder.AppendLine();

        //    //foreach (TaskExportDTO item in tasks)
        //    //{
        //    //    foreach (PropertyDescriptor prop in props)
        //    //    {
        //    //        var value = prop.GetValue(item);

        //    //        builder.Append(prop.Converter.ConvertToString(value));
        //    //        builder.Append(",");
        //    //    }
        //    //    builder.AppendLine();
        //    //}

        //    // https://forums.asp.net/t/2147783.aspx?Download+csv+file+from+WEB+API+
        //    var staffTypeNames = StaffTypes.GetAll(true).Select(x => x.Name).ToList();

        //    var columnNames = new List<string>()
        //    {
        //        "Name", "Area", "Sub Area", "Frequency", "Notes", "Policy Tech", "Procedure File Name"
        //    };
        //    staffTypeNames.ForEach(x => columnNames.Add(x));
        //    builder.AppendLine(columnNames.Join(","));

        //    foreach (TaskExportDTO task in tasks)
        //    {
        //        var policyTechYesNo = task.IsInPolicyTech ? "Yes" : "No";
        //        var cleanedName = task.Name.Replace(",", "\",\"");

        //        builder.Append($"{cleanedName},{task.AreaName},{task.SubAreaName},{task.FrequencyName},{task.Notes},{policyTechYesNo},{task.ProcedureFileName}");
        //        builder.Append(",");

        //        var staffGroups = task.Staff.GroupBy(s => s.StaffTypeName,
        //            (group, staff) => new StaffGroupDTO
        //            {
        //                StaffTypeName = group,
        //                StaffNames = staff.Select(s => s.Name)
        //            });

        //        foreach (StaffGroupDTO s in staffGroups)
        //        {

        //            builder.Append($"{s.StaffNames.Join(" ")} ({s.StaffTypeName})");
        //            builder.Append(",");
        //        }

        //        //foreach (StaffGroupDTO s in task.StaffGroups)
        //        //{
        //        //    builder.Append($"{s.StaffNames.Join(", ")} ({s.StaffTypeName})");
        //        //    builder.Append(",");
        //        //}

        //        builder.AppendLine();
        //    }

        //    var fileAsString = builder.ToString();
        //    var bytes = Encoding.UTF8.GetBytes(fileAsString);
        //    var fileName = "tasks.csv"; // $"Tasks-{DateTime.Now}";

        //    System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
        //    {
        //        FileName = fileName,
        //        Inline = false
        //    };
        //    Response.Headers.Add("Content-Disposition", cd.ToString());

        //    var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; //"text/csv";
        //    return File(bytes, contentType, fileName);
        //}

        [HttpGet]
        [Route("export")]
        public IActionResult Export()
        {
            byte[] content = TaskExport.GetTaskExportFile();
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "tasks.xlsx"; // $"Tasks-{DateTime.Now}";

            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = false
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());

            return File(content, contentType, fileName);
        }
    }
}