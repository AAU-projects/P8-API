using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// POST api/v1/<controller>/
        /// Post a list of positons to the user in the database
        /// </summary>
        /// <param name="positionList">The position listed to be stored</param>
        /// <returns>An ActionResult with a bool and statuscode</returns>
        [HttpPost]
        [Authorize]
        public IActionResult PostPositions([FromBody] List<Position> positionList)
        {
            string token = Helper.Utility.GetToken(Request);
            User user = _authenticationService.ValidateToken(token);
            if (user == null)
                return Unauthorized("Invalid token");

            return Ok(_loggingService.Create(user.Id, positionList));
        }
    }
}
