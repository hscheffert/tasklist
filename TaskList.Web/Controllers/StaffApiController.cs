using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskList.Business.Helpers;
using TaskList.Core.DTOs;
using TaskList.Core.Exceptions;

namespace TaskList.Web.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffApiController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StaffApiController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/staff
        [HttpGet]
        public IEnumerable<StaffDTO> GetAll()
        {
            return Staffs.GetAll();
        }

        // GET: api/subAreas/getAllByAreaId
        [HttpGet]
        [Route("getAllByTaskId/{taskId:guid}")]
        public IEnumerable<TaskStaffDTO> GetAllByTaskId(Guid taskId)
        {
            return Staffs.GetAllByTaskId(taskId);
        }

        // GET: api/users/getAllActive
        [HttpGet]
        [Route("getAllActive")]
        public IEnumerable<StaffDTO> GetAllActive()
        {
            return Staffs.GetAll(true);
        }

        // GET api/staff/5
        [HttpGet]
        [Route("{id:guid}")]
        public StaffDTO Get(Guid id)
        {
            return Staffs.GetByID(id);
        }

        // POST api/staff
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public Guid? Post([FromBody] StaffDTO dto)
        {
            var email = _httpContextAccessor.HttpContext.User.GetCurrentUserEmail();

            try
            {
                return Staffs.Save(dto, email);
            }
            catch (EndUserException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT api/staff/toggle/5
        [HttpPut]
        [Authorize(Policy = "Admin")]
        [Route("toggle/{id:guid}")]
        public void Toggle(Guid id)
        {
            Staffs.Toggle(id);
        }

        // GET api/staff/5
        [HttpDelete]
        [Route("{id:guid}")]
        public void Delete(Guid id)
        {
            Staffs.Delete(id);
        }
    }
}
