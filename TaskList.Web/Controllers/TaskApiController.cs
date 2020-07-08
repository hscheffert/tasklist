using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskList.Business.Helpers;
using TaskList.Core.DTOs;

namespace TaskList.Web.Controllers
{
    [Authorize]
    [Route("api/tasks")]
    [ApiController]
    public class TaskApiController : ControllerBase
    {
        // GET: api/tasks
        [HttpGet]
        public IEnumerable<TaskDTO> GetAll()
        {
            return Tasks.GetAll();
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
        public Guid? Post([FromBody] TaskDetailsDTO dto)
        {
            return Tasks.Save(dto);
        }

        // PUT api/tasks/toggle/5
        [HttpPut]
        [Route("toggle/{id:guid}")]
        public void Toggle(Guid id)
        {
            Tasks.Toggle(id);
        }
    }
}
