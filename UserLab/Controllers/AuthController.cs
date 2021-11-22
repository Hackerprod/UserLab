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
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> Logger;
        UserManager Users;

        public AuthController(ILogger<AuthController> logger, DBService DB)
        {
            Logger = logger;
            Users = DB.Users;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] AuthModel auth)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Wrong data" });
            }

            Logger.LogInformation(auth.UserName);

            var user = Users.Authenticate(auth.UserName, auth.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password incorrect" });

            Logger.LogInformation($"User {user.Token} logged in!!!");

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterModel register)
        {
            string remoteIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Wrong data" });
            }

            if (register.CI == 0)
            {
                return BadRequest(new { message = "Wrong CI data" });
            }

            var user = Users.Create(register, remoteIP); ;

            if (user == null)
                return BadRequest(new { message = "Internal error" });

            return Ok(user);
        }
    }
}
