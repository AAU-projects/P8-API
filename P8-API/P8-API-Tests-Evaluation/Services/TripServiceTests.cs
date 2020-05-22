using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using P8_API.Models;
using P8_API.Services;
using System;
using System.Collections.Generic;
using System.IO;

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

        [TestCase("tripBike",   0, ExpectedResult = Transport.Bike)]

        [TestCase("tripCar",    0, ExpectedResult = Transport.Car)]
        [TestCase("tripCar",    1, ExpectedResult = Transport.Car)]
        [TestCase("tripCar",    2, ExpectedResult = Transport.Car)]
        [TestCase("tripCar",    3, ExpectedResult = Transport.Car)]

        [TestCase("tripBus",    0, ExpectedResult = Transport.Walk)]
        [TestCase("tripBus",    1, ExpectedResult = Transport.Public)]
        [TestCase("tripBus",    2, ExpectedResult = Transport.Public)]
        public Transport PredictTransport(string filename, int tripIndex)
        {
            // Arange
            string bikeTrips = File.ReadAllText(@$"../../../Assets/{filename}.json");
            List<Trip> _testTrips = new List<Trip>(JsonConvert.DeserializeObject<Trip[]>(bikeTrips, _settings));
            Trip selectedTrip = _testTrips[tripIndex];

            // Act
            Transport predictedTransport = _tripService.PredictTransport(selectedTrip);

            // Assert
            return predictedTransport;
        }
    }
}
