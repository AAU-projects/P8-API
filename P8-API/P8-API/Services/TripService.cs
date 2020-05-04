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
        private readonly IMongoCollection<TripsCollection> _trips;

        public TripService(IGoogleService googleService, IMongoDatabase database)
        {
            _googleService = googleService;
            _trips = database.GetCollection<TripsCollection>("Trips");
        }

        public void Test()
        {

            using (StreamReader r = new StreamReader("tripData.json"))
            {
                string json = r.ReadToEnd();
                List<Trip> items = JsonConvert.DeserializeObject<List<Trip>>(json);

                PredictTransport(items[0]);
            }

        }

        public List<Trip> GetRecentTrips(User user)
        {
            using (StreamReader r = new StreamReader("tripData.json"))
            {
                string json = r.ReadToEnd();
                List<Trip> items = JsonConvert.DeserializeObject<List<Trip>>(json);
                return items;
            }
        }

        public void PredictTransport(Trip trip)
        {
            trip.CaculateSpeed();
            if (IsWithin(trip.AverageSpeed, 0, 3) && trip.MaxSpeed < 12)
            {
                // Humans walk averagly 1 meter pr. second
                // Humans are not able to run faster than 12 meters pr. second
                // https://www.healthline.com/health/exercise-fitness/average-walking-speed#average-speed-by-age
                trip.Transport = Transport.Walk;
            } else if (IsWithin(trip.AverageSpeed, 0, 11) && trip.MaxSpeed < 13.8)
            {
                // Humans cycles between 0 and 11 meters pr. second
                // Humans will probably not cycle faster than 13.8 meter pr. second
                // https://www.quora.com/What-is-the-average-bike-speed
                trip.Transport = Transport.Bike;
            } else if (IsWithin(trip.AverageSpeed, 0, 36))
            {
                double percentageTransitStops = DetectTransitStops(trip);
                if (percentageTransitStops >= 0.1)
                    trip.Transport = Transport.Public;
                else
                    trip.Transport = Transport.Car;
            }
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

        public void UpdateTrip(string tripId, Transport transport, string userId, string dateId)
        {
            TripsCollection tripsCollection = _trips.Find(collection => collection.UserId == userId).FirstOrDefault();
            TripDocument tripsOnDate;

            if (tripsCollection != null)
            {
                tripsOnDate = tripsCollection.TripDocuments.FirstOrDefault(x => x.DateId == dateId);
                
                if(tripsOnDate != null)
                {
                    Trip trip = tripsOnDate.TripList.FirstOrDefault(x => x.Id == tripId);
                    trip.Transport = transport;

                    FilterDefinition<TripsCollection> filter = Builders<TripsCollection>.Filter.Eq(x => x.UserId, userId);
                    UpdateDefinition<TripsCollection> update = Builders<TripsCollection>.Update.Set(x => x.TripDocuments, tripsCollection.TripDocuments);

                    _trips.UpdateOne(filter, update);
                }
            }
        }

        private bool IsWithin(double value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
    }
}
