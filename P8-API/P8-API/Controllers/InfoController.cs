using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P8_API.Controllers
{
    [Route("api/v1/[controller]")]
    public class InfoController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public string Get()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
    }
}
