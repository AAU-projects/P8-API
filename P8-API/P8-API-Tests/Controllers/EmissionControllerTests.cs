using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using P8_API.Controllers;
using P8_API.Models;
using P8_API.Services;
using P8_API.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace P8_API_Tests.Controllers
{
    public class EmissionControllerTests
    {
        private EmissionService _emissionService;
        private EmissionController _controller;

        [SetUp]
        public void Setup()
        {
            _emissionService = new EmissionService();
            _controller = new EmissionController(_emissionService);
        }

        [TestCase(ExpectedResult = 127.0)]
        public double GetNoParameters()
        {
            // Arrange
            double result;

            // Act
            result = _controller.Get().Value;

            // Assert
            return result;
        }

        [TestCase("Petrol", ExpectedResult = 127.0)]
        [TestCase("Diesel", ExpectedResult = 127.0)]
        [TestCase("Electric", ExpectedResult = 38.0)]
        public double GetWithFueltype(string fuelType)
        {
            // Arrange
            double result;

            // Act
            result = _controller.Get(fuelType).Value;

            // Assert
            return result;
        }

        [TestCase(0.0, "Petrol", ExpectedResult = 127.0)]
        [TestCase(0.0, "Diesel", ExpectedResult = 127.0)]
        [TestCase(0.0, "Electric", ExpectedResult = 38.0)]
        [TestCase(24.0, "Petrol", ExpectedResult = 99.66666666666669)]
        [TestCase(24.0, "Diesel", ExpectedResult = 110.0)]
        [TestCase(24.0, "Electric", ExpectedResult = 38.0)]
        public double GetWithKmlFueltype(double kml, string fuelType)
        {
            // Arrange
            double result;

            // Act
            result = _controller.Get(kml, fuelType).Value;

            // Assert
            return result;
        }
    }
}
