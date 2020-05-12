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
        private Mock<IGoogleService> _googleService;
        private Mock<IExtractionService> _extractionService;
        private TripService _tripService;

        [SetUp]
        public void Setup()
        {
            _extractionService = new Mock<IExtractionService>();
            _googleService = new Mock<IGoogleService>();
            _tripService = new TripService(_googleService.Object, _extractionService.Object);

            // mock specific locations to be a busstation
            _googleService.Setup(x => x.NearbyTransit(Convert.ToInt32(20), 59.330064, 18.0685984)).Returns(true);
        }


        // TODO ADD REAL DATA TO TEST ON 
        [Test]
        public void PredictTransport()
        {
            List<Transport> predictions = new List<Transport>();
            foreach (var trip in _testTrips)
            {
                trip.CalculateSpeed();
                predictions.Add(_tripService.PredictTransport(trip));
            }

            Assert.AreEqual(predictions[0], Transport.Car);
            Assert.AreEqual(predictions[1], Transport.Public);
            Assert.AreEqual(predictions[2], Transport.Walk);
            Assert.AreEqual(predictions[3], Transport.Walk); // SHOULD PROB BE BIKE
        }
    }
}
