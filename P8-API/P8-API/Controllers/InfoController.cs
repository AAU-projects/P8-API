using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P8_API.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/v1/[controller]")]
    public class InfoController : Controller
    {
        public InfoController()
        {
        }
        
        // GET: api/<controller>
        [HttpGet]
        public string Get()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
    }
}
