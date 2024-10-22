﻿using MongoDB.Driver;
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
        private List<Position> _testPositions = new List<Position>(JsonConvert.DeserializeObject<Position[]>(File.ReadAllText(@"../../../Assets/routeData.json")));
        private Mock<IMongoDatabase> _mockDB;
        private ExtractionService _extractService;

        [SetUp]
        public void Setup()
        {
            _mockDB = new Mock<IMongoDatabase>();
            _extractService = new ExtractionService(_mockDB.Object);
        }

        [Test]
        public void TripExtraction()
        {
            List<Trip> result = _extractService.ExtractTrips(_testPositions);

            Assert.AreEqual(3, result.Count);
        }
    }
}
