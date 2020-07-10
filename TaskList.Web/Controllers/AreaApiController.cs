using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskList.Business.Helpers;
using TaskList.Core.DTOs;

namespace TaskList.Web.Controllers
{
    [Route("api/areas")]
    [ApiController]
    public class AreaApiController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AreaApiController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/areas
        [HttpGet]
        public IEnumerable<AreaDTO> GetAll()
        {
            return Areas.GetAll();
        }

        // GET: api/users/getAllActive
        [HttpGet]
        [Route("getAllActive")]
        public IEnumerable<AreaDTO> GetAllActive()
        {
            return Areas.GetAll(true);
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
        [Authorize(Policy = "Admin")]
        public Guid? Post([FromBody] AreaDTO dto)
        {
            var email = _httpContextAccessor.HttpContext.User.GetCurrentUserEmail();

            return Areas.Save(dto, email);
        }

        // PUT api/areas/toggle/5
        [HttpPut]
        [Authorize(Policy = "Admin")]
        [Route("toggle/{id:guid}")]
        public void Toggle(Guid id)
        {
            Areas.Toggle(id);
        }
    }
}
