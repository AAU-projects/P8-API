using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using P8_API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public class TripService
    {
        public void Test()
        {

            using (StreamReader r = new StreamReader("tripData.json"))
            {
                string json = r.ReadToEnd();
                List<Trip> items = JsonConvert.DeserializeObject<List<Trip>>(json);

                Predict_Transport(items[0]);
            }

        }

        public List<Trip> Get_Recent_Trips(User user)
        {
            using (StreamReader r = new StreamReader("tripData.json"))
            {
                string json = r.ReadToEnd();
                List<Trip> items = JsonConvert.DeserializeObject<List<Trip>>(json);
                return items;
            }
        }

        public void Predict_Transport(Trip trip)
        {
            trip.CaculateSpeed();
            if (IsWithin(trip.AverageSpeed, 0, 3) && trip.MaxSpeed > 12)
            {
                // Humans walk averagly 1 meter pr. second
                // Humans are not able to run faster than 12 meters pr. second
                // https://www.healthline.com/health/exercise-fitness/average-walking-speed#average-speed-by-age
                trip.Transport = Transport.Walk;
            } else if (IsWithin(trip.AverageSpeed, 0, 11) && trip.MaxSpeed > 13.8)
            {
                // Humans cycles between 0 and 11 meters pr. second
                // Humans will probably not cycle faster than 13.8 meter pr. second
                // https://www.quora.com/What-is-the-average-bike-speed
                trip.Transport = Transport.Bike;
            } else if (IsWithin(trip.AverageSpeed, 0, 36))
            {
                trip.Transport = Transport.Car;
                trip.Transport = Transport.Public;
            }
        }

        public bool IsWithin(double value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
    }
}
