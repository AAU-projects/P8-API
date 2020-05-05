using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using P8_API.Models;
using P8_API.Services;
using SharpCompress.Archives.SevenZip;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P8_API.Controllers
{
    [Route("api/v1/[controller]")]
    public class InfoController : Controller
    {
        private readonly ITripService _tripService;

        public InfoController(ITripService tripService)
        {
            _tripService = tripService;
        }
        // GET: api/<controller>
        [HttpGet]
        public string Get()
        {
            _tripService.ExtractUserTrips(null);
            return "ok";
        }
    }
}
