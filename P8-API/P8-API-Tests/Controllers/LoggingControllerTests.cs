using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using NUnit.Framework;
using P8_API;
using P8_API.Controllers;
using P8_API.Models;
using P8_API.Services;
using P8_API.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace P8_API_Tests.Controllers
{
    class LoggingControllerTests : ControllerBase
    {
        private Mock<IUserService> _userService;
        private Mock<IMailService> _mailService;
        private Mock<ILoggingService> _loggingService;
        private Mock<IUtility> _utility;
        private IAuthenticationService _authenticationService;
        private LoggingController _controller;

        [SetUp]
        public void Setup()
        {
            AppSettings appSettings = new AppSettings();
            appSettings.Secret = "pintusharmaqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqweqwe";

            _userService = new Mock<IUserService>();
            _mailService = new Mock<IMailService>();
            _loggingService = new Mock<ILoggingService>();
            _utility = new Mock<IUtility>();
            Helper.Utility = _utility.Object;

            _authenticationService = new AuthenticationService(_userService.Object, _mailService.Object, appSettings);
            _controller = new LoggingController(_loggingService.Object, _authenticationService);

            // Initializes the database with a valid user with pincode
            User mockUser = new User("13439332", "exist@gmail.com", 24.0);
            mockUser.UpdatePincode("1234", DateTime.Now.AddDays(365));
            _userService.Setup(x => x.ValidatePincode(mockUser.Email, "1234")).Returns(mockUser);
            _userService.Setup(x => x.Get(mockUser.Email)).Returns(mockUser);

        }

        [TestCase("bad@token.com", "2342", ExpectedResult = StatusCodes.Status401Unauthorized)]
        [TestCase("exist@gmail.com", "1234", ExpectedResult = StatusCodes.Status200OK)]
        public int? PostPositions(string email, string pincode)
        {
            // Arrange
            Position pos1 = new Position(2.5, 2.5);
            Position pos2 = new Position(3.0, 3.0);
            List<Position> listPos = new List<Position>{ pos1, pos2 };

            User user = _authenticationService.Authenticate(email, pincode);
            if (user != null) 
                _utility.Setup(x => x.GetToken(It.IsAny<HttpRequest>())).Returns(user.Token);
            _loggingService.Setup(x => x.Create(It.IsAny<string>(), listPos)).Returns(true);

            // Act
            IStatusCodeActionResult result = (IStatusCodeActionResult)_controller.PostPositions(listPos);

            // Assert
            return result.StatusCode;
        }

    }
}
