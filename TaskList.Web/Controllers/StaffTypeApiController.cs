using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskList.Business.Helpers;
using TaskList.Core.DTOs;

namespace TaskList.Web.Controllers
{
    [Route("api/staffTypes")]
    [ApiController]
    public class StaffTypeApiController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StaffTypeApiController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/staffTypes
        [HttpGet]
        public IEnumerable<StaffTypeDTO> GetAll()
        {
            return StaffTypes.GetAll();
        }

        // GET: api/users/getAllActive
        [HttpGet]
        [Route("getAllActive")]
        public IEnumerable<StaffTypeDTO> GetAllActive()
        {
            return StaffTypes.GetAll(true);
        }

        // GET api/staffTypes/5
        [HttpGet]
        [Route("{id:guid}")]
        public StaffTypeDTO Get(Guid id)
        {
            return StaffTypes.GetByID(id);
        }

        // POST api/staffTypes
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public Guid? Post([FromBody] StaffTypeDTO dto)
        {
            var email = _httpContextAccessor.HttpContext.User.GetCurrentUserEmail();

            return StaffTypes.Save(dto, email);
        }

        // PUT api/staffTypes/toggle/5
        [HttpPut]
        [Route("toggle/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public void Toggle(Guid id)
        {
            StaffTypes.Toggle(id);
        }
    }
}
