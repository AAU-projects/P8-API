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
    public class LoggingController : ControllerBase
    {
        private readonly ILoggingService _loggingService;

        public LoggingController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            Position p1 = new Position(1.2, 1.2, DateTime.Now, 1.2, 1.2, 1.2, 1.2, 1.2);
            Position p2 = new Position(1.3, 1.2, DateTime.Now, 1.2, 1.2, 1.2, 1.2, 1.3);
            List<Position> possList = new List<Position>();
            possList.Add(p1);
            possList.Add(p2);

            return Ok(_loggingService.Create("123", possList));
        }
    }
}
