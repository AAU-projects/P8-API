using Newtonsoft.Json;
using NUnit.Framework;
using P8_API.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace P8_API_Tests.Models
{
    class TripTests
    {
        Trip _testTrip;

        [SetUp]
        public void Setup()
        {
            JsonSerializerSettings _settings = new JsonSerializerSettings() { Culture = new System.Globalization.CultureInfo("fr-FR") };
            string positions = File.ReadAllText(@$"../../../Assets/testPositions.json");
            List<Position> _testPositions = new List<Position>(JsonConvert.DeserializeObject<Position[]>(positions, _settings));
            _testTrip = new Trip(_testPositions);
        }

        [Test]
        public void CalculateSpeed()
        {
            // Arrange

            // Act
            _testTrip.CalculateSpeed();

            // Assert
            Assert.AreEqual(Math.Round(_testTrip.AverageSpeed, 3), 3.014);
            Assert.AreEqual(_testTrip.MinSpeed, 0.01);
            Assert.AreEqual(_testTrip.MaxSpeed, 5.53);
            Assert.AreEqual(Math.Round(_testTrip.TopMedian, 3), 4.795);
        }   
    }
}
