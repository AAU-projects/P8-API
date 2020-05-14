using MongoDB.Driver;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using P8_API.Models;
using P8_API.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace P8_API_Tests.Services
{
    class TripServiceTests
    {
        private Mock<IGoogleService> _googleService;
        private Mock<IExtractionService> _extractionService;
        private TripService _tripService;
        List<TransitStop> _transitStops;

        private JsonSerializerSettings _settings;

        [SetUp]
        public void Setup()
        {
            _extractionService = new Mock<IExtractionService>();
            _googleService = new Mock<IGoogleService>();
            _tripService = new TripService(_googleService.Object, _extractionService.Object);
            _settings = new JsonSerializerSettings() { Culture = new System.Globalization.CultureInfo("fr-FR") };
            // mock specific locations to be a busstation
            _googleService.Setup(x => x.NearbyTransit(Convert.ToInt32(20), 59.330064, 18.0685984)).Returns(true);


            // Mock transit stops
            string transitStops = File.ReadAllText(@"../../../Assets/transitStops.json");
            _transitStops = new List<TransitStop>(JsonConvert.DeserializeObject<TransitStop[]>(transitStops));
            _googleService.Setup(x => x.NearbyTransit(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double>())).Returns<int, double, double>((arg0, arg1, arg2) => IsTransitStop(arg0, arg1, arg2));
        }

        private bool IsTransitStop(int range, double lattiude, double longitude)
        {
            foreach (TransitStop stop in _transitStops)
            {
                if (stop.Lattitude == lattiude && stop.Longitude == longitude)
                {
                    return stop.Result;
                }
            }

            return false;
        }

        [Test]
        public void PredictBikeTransport()
        {
            string busTrips = File.ReadAllText(@"../../../Assets/tripBike.json");
            List<Trip> _testTrips = new List<Trip>(JsonConvert.DeserializeObject<Trip[]>(busTrips, _settings));

            foreach (var trip in _testTrips)
            {
                Transport expectedTrasnport = trip.Transport;
                Transport predictedTransport = _tripService.PredictTransport(trip);
                Assert.AreEqual(expectedTrasnport, predictedTransport);
            }
        }

        [Test]
        public void PredictCarTransport()
        {
            string busTrips = File.ReadAllText(@"../../../Assets/tripCar.json");
            List<Trip> _testTrips = new List<Trip>(JsonConvert.DeserializeObject<Trip[]>(busTrips, _settings));

            foreach (var trip in _testTrips)
            {
                Transport expectedTrasnport = trip.Transport;
                Transport predictedTransport = _tripService.PredictTransport(trip);
                Assert.AreEqual(expectedTrasnport, predictedTransport);
            }
        }

        [Test]
        public void PredictBusTransport()
        {
            string busTrips = File.ReadAllText(@"../../../Assets/tripBus.json");
            List<Trip> _testTrips = new List<Trip>(JsonConvert.DeserializeObject<Trip[]>(busTrips, _settings));

            foreach (var trip in _testTrips)
            {
                Transport expectedTrasnport = trip.Transport;
                Transport predictedTransport =  _tripService.PredictTransport(trip);
                Assert.AreEqual(expectedTrasnport, predictedTransport);
            }
        }
    }
}
