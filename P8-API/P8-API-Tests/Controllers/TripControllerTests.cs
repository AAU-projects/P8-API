using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
    public class TripControllerTests : ControllerBase
    {
        private Mock<ITripService> _tripService;
        private Mock<IExtractionService> _extractionService;
        private Mock<IAuthenticationService> _authenticationService;
        private Mock<IUtility> _utility;

        private TripController _controller;

        [SetUp]
        public void Setup()
        {
            _tripService = new Mock<ITripService>();
            _extractionService = new Mock<IExtractionService>();
            _authenticationService = new Mock<IAuthenticationService>();

            _utility = new Mock<IUtility>();
            Helper.Utility = _utility.Object;

            _controller = new TripController(_tripService.Object, _extractionService.Object, _authenticationService.Object);
        }

        [TestCase("bad@token.com", ExpectedResult = StatusCodes.Status401Unauthorized)]
        [TestCase("exist@token.com", ExpectedResult = StatusCodes.Status200OK)]
        public int? GetTrips(string email)
        {
            // Arrange
            if (email == "exist@token.com")
            {
                _utility.Setup(x => x.GetToken(It.IsAny<HttpRequest>())).Returns("test");
                _authenticationService.Setup(x => x.ValidateToken("test")).Returns(new User("1", 1));
            }

            // Act
            IStatusCodeActionResult result = (IStatusCodeActionResult)_controller.Get();

            // Assert
            return result.StatusCode;
        }

        [TestCase("bad@token.com", "1", "1", Transport.Walk, ExpectedResult = StatusCodes.Status401Unauthorized)]
        [TestCase("exist@token.com", "1", "1", Transport.Walk, ExpectedResult = StatusCodes.Status204NoContent)]
        public int? Patch(string email, string date, string tripId, Transport transport)
        {
            // Arrange
            if (email == "exist@token.com")
            {
                _utility.Setup(x => x.GetToken(It.IsAny<HttpRequest>())).Returns("test");
                _authenticationService.Setup(x => x.ValidateToken("test")).Returns(new User("1", 1));
            }

            // Act
            IStatusCodeActionResult result = (IStatusCodeActionResult)_controller.Patch(date, tripId, transport);

            // Assert
            return result.StatusCode;
        }


    }
}
