using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Models
{
    public class RegisterInput
    {
        public User User { get; set; }
        public double Kml { get; set; }
        public string FuelType { get; set; }

        [JsonConstructor]
        public RegisterInput(User user, double kml, string fuelType)
        {
            User = user;
            Kml = kml;
            FuelType = fuelType;
        }
    }
}
