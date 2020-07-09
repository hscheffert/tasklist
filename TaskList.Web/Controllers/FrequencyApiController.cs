using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskList.Business.Helpers;
using TaskList.Core.DTOs;

namespace TaskList.Web.Controllers
{
    [Route("api/frequencies")]
    [ApiController]
    public class FrequencyApiController : ControllerBase
    {
        // GET: api/frequencies
        [HttpGet]
        public IEnumerable<FrequencyDTO> GetAll()
        {
            return Frequencies.GetAll();
        }

        // GET api/frequencies/5
        [HttpGet]
        [Route("{id:guid}")]
        public FrequencyDTO Get(Guid id)
        {
            return Frequencies.GetByID(id);
        }

        // POST api/frequencies
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public Guid? Post([FromBody] FrequencyDTO dto)
        {
            return Frequencies.Save(dto);
        }

        // PUT api/frequencies/toggle/5
        [HttpPut]
        [Authorize(Policy = "Admin")]
        [Route("toggle/{id:guid}")]
        public void Toggle(Guid id)
        {
            Frequencies.Toggle(id);
        }
    }
}
