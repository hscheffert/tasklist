using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskList.Business.Helpers;
using TaskList.Core.DTOs;
using TaskList.Data.Model;

namespace TaskList.Web.Controllers
{
    [Authorize(Policy = "AdminRole")]
    //[Authorize(Roles = "Admin")]
    [Route("api/areas")]
    [ApiController]
    public class AreaApiController : ControllerBase
    {
        // GET: api/areas
        [HttpGet]
        public IEnumerable<AreaDTO> GetAll()
        {
            return Areas.GetAll();
        }

        // GET api/areas/5
        [HttpGet]
        [Route("{id:guid}")]
        public AreaDTO Get(Guid id)
        {
            return Areas.GetByID(id);
        }

        // POST api/areas
        [HttpPost]
        public Guid? Post([FromBody] AreaDTO dto)
        {
            return Areas.Save(dto);
        }

        // PUT api/areas/toggle/5
        [HttpPut]
        [Route("toggle/{id:guid}")]
        public void Toggle(Guid id)
        {
            Areas.Toggle(id);
        }
    }
}
