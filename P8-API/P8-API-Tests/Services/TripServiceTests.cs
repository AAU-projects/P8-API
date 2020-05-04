using MongoDB.Driver;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using P8_API.Models;
using P8_API.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace P8_API_Tests.Services
{
    class TripServiceTests
    {
        public List<Trip> _testTrips = new List<Trip>(JsonConvert.DeserializeObject<Trip[]>(File.ReadAllText(@"../../../Assets/tripData.json")));
        private Mock<IMongoDatabase> _mockDB;
        private Mock<IGoogleService> _mockGoogle;
        private TripService _tripService;

        [SetUp]
        public void Setup()
        {
            _mockDB = new Mock<IMongoDatabase>();
            _mockGoogle = new Mock<IGoogleService>();
            _tripService = new TripService(_mockGoogle.Object, _mockDB.Object);

            // mock specific locations to be a busstation
            _mockGoogle.Setup(x => x.NearbyTransit(Convert.ToInt32(20), 59.330064, 18.0685984)).Returns(true);
        }

        [Test]
        public void PredictTransport()
        {
            foreach (var trip in _testTrips)
            {
                _tripService.PredictTransport(trip);
            }

            Assert.AreEqual(_testTrips[0].Transport, Transport.Car);
            Assert.AreEqual(_testTrips[1].Transport, Transport.Public);
            Assert.AreEqual(_testTrips[2].Transport, Transport.Walk);
            Assert.AreEqual(_testTrips[3].Transport, Transport.Bike);
        }
    }
}
