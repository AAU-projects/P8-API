using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace P8_API.Services
{
    public class EmissionService : IEmissionService
    {
        /// <summary>
        /// Returns the emission in CO2 g/km.
        /// </summary>
        /// <param name="kml">kilometers pr. liter petrol</param>
        /// <param name="fuelType">Fuel Type of the vehicle</param>
        /// <returns>Returns the emission in CO2 g/km</returns>
        public double RetrieveEmission(double kml, string fuelType)
        {
            if (kml == 0.0 || fuelType == "Electric")
            {
                return RetrieveEmission(fuelType);
            }

            double l_100km = 100 / kml;
            double result = 0;

            if (fuelType == "Petrol")
                result = l_100km * 2392 / 100;
            else if (fuelType == "Diesel")
                result = l_100km * 2640 / 100;

            return result;
        }

        /// <summary>
        /// Returns the avg emission in CO2 g/km, in case no kml is given.
        /// </summary>
        /// <param name="fuelType"></param>
        /// <returns>Returns the avg emission in CO2 g/km</returns>
        public double RetrieveEmission(string fuelType)
        {
            double result = 0;

            if (fuelType == "Petrol" || fuelType == "Diesel")
                result = 127.0;
            else if (fuelType == "Electric")
                result = 38.0;

            return result;
        }

        /// <summary>
        /// Returns the avg emission in CO2 g/km, in case no kml and fuel type is given.
        /// </summary>
        /// <returns>Returns the avg emission in CO2 g/km</returns>
        public double RetrieveEmission()
        {
            return 127.0;
        }
    }
}
