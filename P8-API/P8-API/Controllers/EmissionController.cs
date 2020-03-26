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

        [HttpGet("{licenseplate}")]
        public ActionResult<double> Get(string licenseplate)
        {
            return _emissionService.retrieveEmission(licenseplate).Result; 
        }
    }
}
