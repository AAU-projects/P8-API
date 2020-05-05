using MongoDB.Driver;
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
    public class TripService : ITripService
    {
        private readonly IGoogleService _googleService;
        private readonly IExtractionService _extractionService;

        public TripService(IGoogleService googleService, IExtractionService extractionService)
        {
            _googleService = googleService;
            _extractionService = extractionService;
        }

        public void ExtractUserTrips(User user)
        {
            // TODO extract user positions and remove them when they are included in a trip
            List<Position> _testPositions = new List<Position>(JsonConvert.DeserializeObject<Position[]>(System.IO.File.ReadAllText(@"../../../Assets/routeData.json")));
            List<Trip> _testTrips = _extractionService.ExtractTrips(_testPositions);

            foreach (Trip trip in _testTrips)
            {
                trip.Transport = PredictTransport(trip);
            }

            _extractionService.SaveTrips(_testTrips, user.Id);
        }

        public Transport PredictTransport(Trip trip)
        {
            Transport prediction = Transport.Unknown;

            if (IsWithin(trip.AverageSpeed, 0, 3) && trip.MaxSpeed < 12)
            {
                // Humans walk averagly 1 meter pr. second
                // Humans are not able to run faster than 12 meters pr. second
                // https://www.healthline.com/health/exercise-fitness/average-walking-speed#average-speed-by-age
                prediction = Transport.Walk;
            } else if (IsWithin(trip.AverageSpeed, 0, 11) && trip.MaxSpeed < 13.8)
            {
                // Humans cycles between 0 and 11 meters pr. second
                // Humans will probably not cycle faster than 13.8 meter pr. second
                // https://www.quora.com/What-is-the-average-bike-speed
                prediction = Transport.Bike;
            } else if (IsWithin(trip.AverageSpeed, 0, 36))
            {
                double percentageTransitStops = DetectTransitStops(trip);
                if (percentageTransitStops >= 0.1)
                    prediction = Transport.Public;
                else
                    prediction = Transport.Car;
            }
            return prediction;
        }

        private double DetectTransitStops(Trip trip)
        {
            double stops = 0;
            double transit_stops = 0;

            foreach (Position pos in trip.TripPositions)
            {
                if (pos.Speed == 0 && pos.Accuracy <= 30)
                {
                    stops++;

                    if (_googleService.NearbyTransit(Convert.ToInt32(pos.Accuracy), pos.Latitude, pos.Longitude))
                        transit_stops++;
                }
            }

            if (stops == 0)
                return 0;

            return transit_stops / stops;
        }

        private bool IsWithin(double value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
    }
}
