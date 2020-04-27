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
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Constructor for user controller
        /// <param name="userservice">The userservice for the controller</param>
        /// <param name="authenticationService">The authenticationservice for the controller</param>
        /// </summary>
        public TripController(ITripService tripService, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _tripService = tripService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<UserBase>> Get()
        {
            string token = Helper.Utility.GetToken(Request);
            User user = _authenticationService.ValidateToken(token);
            if (user == null)
                return Unauthorized("Invalid token");

            return Ok(_tripService.GetRecentTrips(user));
        }


    }
}
