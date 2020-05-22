using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace P8_API.Models
{
    [ExcludeFromCodeCoverage]
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
