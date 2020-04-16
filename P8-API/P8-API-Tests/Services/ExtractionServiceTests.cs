using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using P8_API.Models;
using P8_API.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace P8_API_Tests.Services
{
    class ExtractionServiceTests
    {
        private List<Position> _testPositions = new List<Position>(JsonConvert.DeserializeObject<Position[]>(File.ReadAllText(@"../../../Assets/routeData.json")));
        private ExtractionService _extractService = new ExtractionService();

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TripExtraction()
        {
            List<Trip> result = _extractService.ExtractTrips(_testPositions);

            Assert.IsTrue(result.Count == 3);
        }
    }
}
