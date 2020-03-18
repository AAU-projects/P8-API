using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P8_API.Models;
using P8_API.Services;
using P8_API.Utility;

namespace P8_API.Controllers
{
    [Route("api/v1/[controller]")]
    public class LoggingController : ControllerBase
    {
        private readonly ILoggingService _loggingService;
        private readonly IAuthenticationService _authenticationService;

        public LoggingController(ILoggingService loggingService, IAuthenticationService authenticationService)
        {
            _loggingService = loggingService;
            _authenticationService = authenticationService;
        }
            
        // Post: api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody] List<Position> positionList)
        {
            string token = Utility.Utility.GetToken(Request);
            if (token == null)
                return BadRequest("Bearer token required");

            User user = _authenticationService.ValidateToken(token);
            if (user == null)
                return Unauthorized("Invalid token");

            return Ok(_loggingService.Create(user.Id, positionList));
        }
    }
}
