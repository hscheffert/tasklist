using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskList.Business.Helpers;
using TaskList.Core.DTOs;

namespace TaskList.Web.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/users
        [HttpGet]
        public IEnumerable<UserDTO> GetAll()
        {
            return Users.GetAll();
        }

        // GET api/users/5
        [HttpGet]
        [Route("{id:guid}")]
        public UserDTO Get(Guid id)
        {
            return Users.GetByID(id);
        }

        // POST api/users
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public Guid? Post([FromBody] UserDTO dto)
        {
            return Users.Save(dto);            
        }

        // PUT api/users/toggle/5
        [HttpPut]
        [Authorize(Policy = "Admin")]
        [Route("toggle/{id:guid}")]
        public void Toggle(Guid id)
        {
            Users.Toggle(id);
        }

        // GET: api/users/getActiveSupervisors
        [HttpGet]
        [Route("getActiveSupervisors")]
        public IEnumerable<UserDTO> GetActiveSupervisors()
        {
            return Users.GetAll(true, true);
        }        
    }
}
