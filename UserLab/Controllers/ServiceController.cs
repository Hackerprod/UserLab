using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SKYNET.Managers;
using SKYNET.Models;
using SKYNET.Services;

namespace SKYNET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase 
    {
        private readonly ILogger<ServiceController> Logger;
        ServiceManager Services;

        public ServiceController(ILogger<ServiceController> logger, DBService DB)
        {
            Logger = logger;
            Services = DB.Services;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add([FromBody] ServiceModel service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Wrong data" });
            }

            var s = Services.Get(service.Name);

            if (s != null)
            {
                return BadRequest(new { message = "The service exists" });
            }

            if (Services.Add(service))
            {
                return Ok();
            }
            else
                return BadRequest(new { message = "Error adding Service" });
        }
        [Authorize]
        [HttpPut]
        public IActionResult Put(string Name)
        {

            if (!Services.Remove(Name))
            {
                return BadRequest(new { message = "The service not exists" });
            }

            return Ok();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Wrong data" });
            }

            var s = Services.AllServices();

            if (!s.Any())
            {
                return NoContent();
            }

            return Ok(s);
        }

        [Authorize]
        [HttpGet("{Name}")]
        public IActionResult Get(string Name)
        {
            var s = Services.Get(Name);

            if (s == null)
            {
                return NoContent();
            }

            return Ok(s);
        }

        [Authorize]
        [HttpPatch]
        public IActionResult Update(string Name, uint ID)
        {
            var s = Services.Get(ID);

            if (s == null)
            {
                return NoContent();
            }

            s.Name = Name;

            Services.Update(s);

            return Ok(s);
        }

    }
}
