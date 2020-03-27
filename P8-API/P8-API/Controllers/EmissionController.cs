using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P8_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Controllers
{
    [Route("api/v1/[controller]")]
    public class EmissionController : ControllerBase
    {
        private readonly IEmissionService _emissionService;

        public EmissionController(IEmissionService emissionService)
        {
            _emissionService = emissionService;
        }

        [HttpGet]
        public ActionResult<double> Get()
        {
            return _emissionService.RetrieveEmission();
        }

        [HttpGet("{fuelType}")]
        public ActionResult<double> Get(string fuelType)
        {
            return _emissionService.RetrieveEmission(fuelType);
        }

        [HttpGet("{fuelType}/{kml}")]
        public ActionResult<double> Get(double kml, string fuelType)
        {
            return _emissionService.RetrieveEmission(kml, fuelType); 
        }
    }
}
