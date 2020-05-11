using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using P8_API.Models;
using P8_API.Services;
using P8_API.Utility;

namespace P8_API.Controllers
{
    [Route("api/v1/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly IExtractionService _extractionService;
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Constructor for user controller
        /// <param name="userservice">The userservice for the controller</param>
        /// <param name="authenticationService">The authenticationservice for the controller</param>
        /// </summary>
        public TripController(ITripService tripService, IExtractionService extractionService, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _extractionService = extractionService;
            _tripService = tripService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            string token = Helper.Utility.GetToken(Request);
            User user = _authenticationService.ValidateToken(token);
            if (user == null)
                return Unauthorized("Invalid token");

            return Ok(_extractionService.GetTrips(user.Id));
        }

        [HttpPatch]
        [Authorize]
        public IActionResult Patch([FromBody] JObject data)
        {
            string date = data["date"].ToString();
            string tripId = data["tripId"].ToString();
            Transport transport = data["transport"].ToObject<Transport>();

            string token = Helper.Utility.GetToken(Request);
            User user = _authenticationService.ValidateToken(token);
            if (user == null)
                return Unauthorized("Invalid token");

            if (!_extractionService.UpdateTrip(date, tripId, user.Id, transport))
                return BadRequest();

            return NoContent();
        }
    }
}
