﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskList.Business.Helpers;
using TaskList.Core.DTOs;

namespace TaskList.Web.Controllers
{
    [Route("api/subAreas")]
    [ApiController]
    public class SubAreaApiController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubAreaApiController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/subAreas
        [HttpGet]
        public IEnumerable<SubAreaDTO> GetAll()
        {
            return SubAreas.GetAll();
        }

        // GET: api/subAreas/getAllByAreaId
        [HttpGet]
        [Route("getAllByAreaId/{areaId:guid}")]
        public IEnumerable<SubAreaDTO> GetAllByAreaId(Guid areaId)
        {
            return SubAreas.GetAllByAreaId(areaId);
        }

        // GET api/subAreas/5
        [HttpGet]
        [Route("{id:guid}")]
        public SubAreaDTO Get(Guid id)
        {
            return SubAreas.GetByID(id);
        }

        // POST api/subAreas
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public Guid? Post([FromBody] SubAreaDTO dto)
        {
            var email = _httpContextAccessor.HttpContext.User.GetCurrentUserEmail();

            return SubAreas.Save(dto, email);
        }

        // PUT api/subAreas/toggle/5
        [HttpPut]
        [Route("toggle/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public void Toggle(Guid id)
        {
            SubAreas.Toggle(id);
        }
    }
}