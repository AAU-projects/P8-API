using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using P8_API.Models;
using P8_API.Services;
using IAuthenticationService = P8_API.Services.IAuthenticationService;
using P8_API.Utility;

namespace P8_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        // Constructor, takes a UserService and an AuthenticationService as parameter
        public UserController(IUserService _userservice, IAuthenticationService authenticationService)
        {
            _userService = _userservice;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// GET: api/<controller> 
        /// Gets all users in the system
        /// </summary>
        /// <returns>An ActionResult with all users and statuscode</returns>
        [HttpGet]
        public ActionResult<List<User>> Get()
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
        public ActionResult<User> Get(string id)
        {
            return _userService.Get(id);
        }

        /// <summary>
        /// POST api/<controller>/register
        /// Registers abstract user if email is ok
        /// </summary>
        /// <param name="User">The user to register</param>
        /// <returns>An ActionResult that tells if the user was created or not</returns>
        [HttpPost]
        [Route("register")]
        public IActionResult PostRegister(User user)
        {
            if (String.IsNullOrEmpty(user.Email))
                return BadRequest();

            if (_userService.Get(user.Email) != null)
                return Conflict();

            if (!String.IsNullOrEmpty(user.LicensePlate) && user.LicensePlate.Length > 7)
                return BadRequest();

            return Ok(_userService.Create(user));
        }


        /// <summary>
        /// POST api/<controller>/login
        /// Login with a user using email and pincode
        /// </summary>
        /// <param name="auth">The user that contains the credentials</param>
        /// <returns>An ActionResult with the logged in user containg a valid Token and statuscode</returns>
        [HttpPost]
        [Route("login")]
        public IActionResult PostLogin(User auth)
        {
            if (_userService.Get(auth.Email) == null)
                return BadRequest();

            User result = _authenticationService.Authenticate(auth.Email, auth.Pincode);

            if (result == null)
                return Unauthorized();

            return Ok(result);
        }


        /// <summary>
        /// POST api/<controller>/pincode
        /// Generates a pincode that is sent to the specifc email
        /// </summary>
        /// <param name="email">The email requesed to generate pincode for</param>
        /// <returns>An ActionResult with true if the email is succesfully sent and statuscode</returns>
        [HttpPost]
        [Route("pincode")]
        public IActionResult PostPincode(User user)
        {
            if (_userService.Get(user.Email) == null)
                return BadRequest();

            return Ok(_authenticationService.GeneratePinAuthentication(user.Email));
        }

        /// <summary>
        /// POST api/<controller>/test
        /// Shows how to valdiate a token
        /// </summary>
        /// <param name="token">A string with the token </param>
        /// <returns>An ActionResult that tells if the token is valid or not</returns>
        [HttpPost]
        [Route("test")]
        public IActionResult test()
        {
            string token = Request.Headers["Authorization"][0].Substring("Bearer ".Length).Trim();
            return Ok(_authenticationService.ValidateToken(token));
        }

        /// <summary>
        /// PUT api/<controller>/5
        /// Updates a user in the system
        /// </summary>
        /// <param name="id">The id for the user to be updated</param>
        /// <param name="inUser">The user data</param>
        /// <returns>An ActionResult with statuscode</returns>
        [HttpPut("{id}")]
        public IActionResult Put(string id, User inUser)
        {
            var user = _userService.Get(id);

            if (user == null)
                return NotFound();

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
        public IActionResult Delete(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
                return NotFound();

            _userService.Remove(user.Id);

            return NoContent();
        }
    }
}