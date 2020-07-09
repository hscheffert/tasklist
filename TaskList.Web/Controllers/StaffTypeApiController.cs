using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskList.Business.Helpers;
using TaskList.Core.DTOs;

namespace TaskList.Web.Controllers
{
    [Route("api/staffTypes")]
    [ApiController]
    public class StaffTypeApiController : ControllerBase
    {
        // GET: api/staffTypes
        [HttpGet]
        public IEnumerable<StaffTypeDTO> GetAll()
        {
            return StaffTypes.GetAll();
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
            return StaffTypes.Save(dto);
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
