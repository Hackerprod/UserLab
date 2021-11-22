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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> Logger;
        UserManager Users;

        public UserController(ILogger<UserController> logger, DBService DB)
        {
            Logger = logger;
            Users = DB.Users;
        }

        [Authorize]
        [HttpPatch("{userId}")]
        public IActionResult Update([FromBody] User userUpdate, ulong userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Wrong data" });
            }

            Users.Update(userUpdate, userId);

            return Ok();
        }

        [Authorize]
        [HttpPost("AddServices")]
        public IActionResult AddServices([FromBody] List<Service> Services, ulong userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Wrong data" });
            }

            if (Services.Any())
            {
                foreach (var Service in Services)
                {
                    Users.AddService(userId, Service);
                }
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("AddDevices")]
        public IActionResult AddDevices([FromBody] List<Device> Devices, ulong userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Wrong data" });
            }

            if (Devices.Any())
            {
                foreach (var Device in Devices)
                {
                    Users.AddDevice(userId, Device);
                }
            }
            return Ok();
        }

        [Authorize]
        [HttpPut("RemoveService{serviceID}")]
        public IActionResult RemoveService(uint serviceID, ulong userId)
        {
            Users.RemoveService(userId, serviceID);
            return Ok();
        }

        [Authorize]
        [HttpPut("RemoveDevice{deviceID}")]
        public IActionResult RemoveDevice(uint deviceID, ulong userId)
        {
            Users.RemoveDevice(userId, deviceID);
            return Ok();
        }

        [Authorize]
        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            return Ok(Users.AllUsers());
        }
    }
}
