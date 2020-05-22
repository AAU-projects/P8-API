using MongoDB.Driver;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using P8_API.Models;
using P8_API.Services;
using System.Collections.Generic;
using System.IO;

namespace P8_API_Tests.Services
{
    class ExtractionServiceTests
    {
        private Mock<IMongoDatabase> _mockDB;
        private ExtractionService _extractService;
        private JsonSerializerSettings _settings;

        [SetUp]
        public void Setup()
        {
            _mockDB = new Mock<IMongoDatabase>();
            _extractService = new ExtractionService(_mockDB.Object);

            _settings = new JsonSerializerSettings() { Culture = new System.Globalization.CultureInfo("fr-FR") };
        }

        [TestCase("routeBike", ExpectedResult = 3)]
        [TestCase("routeCar", ExpectedResult = 4)]
        [TestCase("routeBus", ExpectedResult = 2)]
        public int ExtractTrip(string filename)
        {
            // Arange
            string positions = File.ReadAllText(@$"../../../Assets/{filename}.json");
            List<Position> _testPositions = new List<Position>(JsonConvert.DeserializeObject<Position[]>(positions, _settings));

            // Act
            List<Trip> result = _extractService.ExtractTrips(_testPositions);

            // Assert
            return result.Count;
        }
    }
}
