using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using P8_API.Models;
using P8_API.Services;
using IAuthenticationService = P8_API.Services.IAuthenticationService;
using P8_API.Utility;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace P8_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmissionService _emissionService;

        /// <summary>
        /// Constructor for user controller
        /// <param name="userservice">The userservice for the controller</param>
        /// <param name="authenticationService">The authenticationservice for the controller</param>
        /// </summary>
        public UserController(IUserService userservice, IAuthenticationService authenticationService, IEmissionService emissionService)
        {
            _userService = userservice;
            _authenticationService = authenticationService;
            _emissionService = emissionService;
        }

        /// <summary>
        /// GET: api/<controller> 
        /// Gets all users in the system
        /// </summary>
        /// <returns>An ActionResult with all users and statuscode</returns>
        [HttpGet]
        [Authorize]
        public ActionResult<List<UserBase>> Get()
        {
            return _userService.Get();
        }

        /// <summary>
        /// GET api/<controller>/5
        /// Gets a specific user in the system
        /// </summary>
        /// <param name="id">The unique id of the user</param>
        /// <returns>An ActionResult with the specific user and statuscode</returns>
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<UserBase> Get(string id)
        {
            UserBase userBase;
            try
            {
                User user = _userService.Get(id);
                userBase = new UserBase(user?.Id, user?.Email, user.CarEmission);
            }
            catch (NullReferenceException)
            {
                userBase = new UserBase(null, null, 0);
            }
           
            return userBase;
        }

        /// <summary>
        /// POST api/<controller>/register
        /// Registers abstract user if email is ok
        /// </summary>
        /// <param name="input">An object containing the user, kml and fueltype</param>
        /// <returns>An ActionResult that tells if the user was created or not</returns>
        [HttpPost]
        [Route("register")]
        public IActionResult PostRegister(RegisterInput input)
        {
            if (!Helper.Utility.IsEmailValid(input.User.Email))
                return BadRequest("Invalid email");

            if (_userService.Get(input.User.Email) != null)
                return Conflict("Email already exists");

            if (input.FuelType != null)
            {
                if (input.Kml != 0)
                {
                    input.User.CarEmission = _emissionService.RetrieveEmission(input.Kml, input.FuelType);
                }
                else
                {
                    input.User.CarEmission = _emissionService.RetrieveEmission(input.FuelType);
                }
            }
            else
            {
                input.User.CarEmission = _emissionService.RetrieveEmission();
            }
           
            return Ok(_userService.Create(input.User));
        }


        /// <summary>
        /// POST api/<controller>/login
        /// Login with a user using email and pincode
        /// </summary>
        /// <param name="auth">The user that contains the credentials</param>
        /// <returns>An ActionResult with the logged in user containg a valid Token and statuscode</returns>
        [HttpPost]
        [Route("login")]
        public IActionResult PostLogin([FromBody] User auth)
        {
            if (_userService.Get(auth.Email) == null)
                return BadRequest("Email does not exist");

            User result = _authenticationService.Authenticate(auth.Email, auth.Pincode);

            if (result == null)
                return Unauthorized("Invalid pin code");

            return Ok(result);
        }


        /// <summary>
        /// POST api/<controller>/pincode
        /// Generates a pincode that is sent to the specific email
        /// </summary>
        /// <param name="email">The email requesed to generate pincode for</param>
        /// <returns>An ActionResult with true if the email is succesfully sent and statuscode</returns>
        [HttpPost]
        [Route("pincode")]
        public IActionResult PostPincode(User user)
        {
            if (_userService.Get(user.Email) == null)
                return BadRequest("Email does not exist");

            return Ok(_authenticationService.GeneratePinAuthentication(user.Email));
        }

        /// <summary>
        /// PUT api/<controller>/5
        /// Updates a user in the system
        /// </summary>
        /// <param name="id">The id for the user to be updated</param>
        /// <param name="inUser">The user data</param>
        /// <returns>An ActionResult with statuscode</returns>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(string id, User inUser)
        {
            if (_userService.Get(id) == null)
                return NotFound("User not found");

            _userService.Update(id, inUser);

            return NoContent();
        }

        /// <summary>
        /// DELETE api/<controller>/5
        /// Deletes abstract user from the database
        /// </summary>
        /// <param name="id">A string with the id of the user to delete</param>
        /// <returns>An ActionResult that tells if the user was deleted or not</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(string id)
        {
            UserBase user = _userService.Get(id);

            if (user == null)
                return NotFound();

            _userService.Remove(user.Id);

            return NoContent();
        }
    }
}