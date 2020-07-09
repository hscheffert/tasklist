using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskList.Business.Helpers;
using TaskList.Core.DTOs;

namespace TaskList.Web.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffApiController : ControllerBase
    {
        // GET: api/staff
        [HttpGet]
        public IEnumerable<StaffDTO> GetAll()
        {
            return Staffs.GetAll();
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
            return Staffs.Save(dto);
        }

        // PUT api/staff/toggle/5
        [HttpPut]
        [Authorize(Policy = "Admin")]
        [Route("toggle/{id:guid}")]
        public void Toggle(Guid id)
        {
            Staffs.Toggle(id);
        }
    }
}
