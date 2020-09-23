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

        /// <summary>
        /// Constructor for Emission Controller
        /// </summary>
        /// <param name="emissionService"></param>
        public EmissionController(IEmissionService emissionService)
        {
            _emissionService = emissionService;
        }

        /// <summary>
        /// Gets the average emission if no fueltype or fuel consumption is given
        /// </summary>
        /// <returns>Average emission as a double</returns>
        [HttpGet]
        public ActionResult<double> Get()
        {
            return _emissionService.RetrieveEmission();
        }

        /// <summary>
        /// Gets the fuel consumption from a given fuel type
        /// </summary>
        /// <param name="fuelType"></param>
        /// <returns>Average emission for a given fuel type as a double</returns>
        [HttpGet("{fuelType}")]
        public ActionResult<double> Get(string fuelType)
        {
            return _emissionService.RetrieveEmission(fuelType);
        }

        /// <summary>
        /// Gets the fuel consumption from a given fuel type and km/l
        /// </summary>
        /// <param name="kml"></param>
        /// <param name="fuelType"></param>
        /// <returns>Calculated emission from fuel type and km/l as a double</returns>
        [HttpGet("{fuelType}/{kml}")]
        public ActionResult<double> Get(double kml, string fuelType)
        {
            return _emissionService.RetrieveEmission(kml, fuelType); 
        }
    }
}
