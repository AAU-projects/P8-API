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
    public class TripControllerTests : ControllerBase
    {
        private Mock<ITripService> _tripService;
        private Mock<IAuthenticationService> _authenticationService;
        private TripController _controller;

        [SetUp]
        public void Setup()
        {
            _tripService = new Mock<ITripService>();
            _authenticationService = new Mock<IAuthenticationService>();

            _controller = new TripController(_tripService.Object, _authenticationService.Object);

        }

        
    }
}
