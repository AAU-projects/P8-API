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
    public class UserController : Controller
    {
        private readonly IDatabaseService _databaseService;

        // Constructor
        public UserController(IDatabaseService dbservice)
        {
            _databaseService = dbservice;
        }

        // GET: api/<controller>
        [HttpGet]
        public List<User> Get()
        {
            return _databaseService.GetAllUsers();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public User Get(string id)
        {
            return _databaseService.GetUser(id);
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
