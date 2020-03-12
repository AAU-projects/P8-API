using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P8_API.Models;
using P8_API.Services;

namespace P8_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        // Constructor
        public UserController(IUserService _userservice, IAuthenticationService authenticationService)
        {
            _userService = _userservice;
            _authenticationService = authenticationService;
        }

        // GET: api/<controller>
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return _userService.Get();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(string id)
        {
            return _userService.Get(id);
        }

        // POST api/<controller>/register
        [HttpPost]
        [Route("register")]
        public IActionResult PostRegister(User user)
        {
            if (String.IsNullOrEmpty(user.LicensePlate) || String.IsNullOrEmpty(user.Email))
                return BadRequest();

            if (_userService.Get(user.Email) != null)
                return Conflict();

            if (user.LicensePlate.Length > 7)
                return BadRequest();

            return Ok(_userService.Create(user));
        }

        // POST api/<controller>/login
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

        // POST api/<controller>/pincode
        [HttpPost]
        [Route("pincode")]
        public IActionResult PostPincode([FromBody] string email)
        {
            if (_userService.Get(email) == null)
                return BadRequest();


            return Ok(_authenticationService.GeneratePinAuthentication(email));
        }

        // POST api/<controller>/test
        [HttpPost]
        [Route("test")]
        public IActionResult test([FromBody] string token)
        {
            return Ok(_authenticationService.ValidateToken(token));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, User inUser)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.Update(id, inUser);

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.Remove(user.Id);

            return NoContent();
        }
    }
}