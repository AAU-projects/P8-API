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

namespace P8_API_Tests.Controllers
{
    public class UserControllerTests
    {
        private Mock<IUserService> _userService;
        private Mock<IMailService> _mailService;
        private Mock<IEmissionService> _emissionService;
        private IAuthenticationService _authenticationService;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            AppSettings appSettings = new AppSettings();
            appSettings.Secret = "pintusharmaqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqweqwe";

            _userService = new Mock<IUserService>();
            _mailService = new Mock<IMailService>();
            _emissionService = new Mock<IEmissionService>();
            _authenticationService = new AuthenticationService(_userService.Object, _mailService.Object, appSettings);
            _controller = new UserController(_userService.Object, _authenticationService, _emissionService.Object);
            Helper.Utility = new Utility();

            // Initializes the database with a user using the email no@gmail.com
            User mockUser = new User("13439332", "exist@gmail.com", 24.0);
            mockUser.UpdatePincode("1234", DateTime.Now.AddDays(365));
            _userService.Setup(x => x.Get("exist@gmail.com")).Returns(mockUser);
        }

        [TestCase("1", ExpectedResult = true)]
        [TestCase("2", ExpectedResult = false)]
        public bool GetUser(string id)
        {
            // Arrange
            User mockUser = new User("1", "test1@gmail.com", 24.0);
            _userService.Setup(x => x.Get("1")).Returns(mockUser);

            // Act
            var result = _controller.Get(id);

            // Assert
            return result.Value.Id == mockUser.Id;
        }

        [Test]
        public void GetAllUsers()
        {
            // Arrange
            User mockUser = new User("1", "test1@gmail.com", 24.0);
            User mockUser2 = new User("2", "test2@gmail.com", 24.0);
            _userService.Setup(x => x.Get()).Returns(new List<UserBase>{ mockUser, mockUser2});

            // Act
            List<UserBase> result = _controller.Get().Value;

            // Assert
            Assert.AreEqual(result.Count, 2);
        }

        [Test]
        public void UpdateUser()
        {
            // Arrange
            User mockUser = new User("1", "test1@gmail.com", 24.0);
            User UpdatedUser = new User("1", "testnew@gmail.com", 24.0);
            _userService.Setup(x => x.Get("1")).Returns(mockUser);
            _userService.Setup(x => x.UpdateEmail("1", UpdatedUser.Email)).Callback((string id, string email) => mockUser.Email = email);
            
            // Act
            _controller.Put(UpdatedUser);

            // Assert
            Assert.AreEqual(mockUser.Email, UpdatedUser.Email);
        }

        [TestCase("1", "test@gmail.com", "1234", 24, ExpectedResult = StatusCodes.Status400BadRequest)]
        [TestCase("2", "exist@gmail.com", "1233", 24, ExpectedResult = StatusCodes.Status401Unauthorized)]
        [TestCase("3", "exist@gmail.com", "1234", 24, ExpectedResult = StatusCodes.Status200OK)]
        public int? PostLogin(string id, string email, string pincode, double carEmission)
        {
            // Arrange
            User mockUser = new User(id, email, carEmission);
            mockUser.UpdatePincode(pincode, DateTime.Now.AddDays(365));
            _userService.Setup(x => x.ValidatePincode("exist@gmail.com", "1234")).Returns(mockUser);

            // Act
            IStatusCodeActionResult result = (IStatusCodeActionResult) _controller.PostLogin(mockUser);

            // Assert
            return result.StatusCode;
        }

        [TestCase("test@gmail.com", ExpectedResult = StatusCodes.Status400BadRequest)]
        [TestCase("exist@gmail.com", ExpectedResult = StatusCodes.Status200OK)]
        public int? PostPincode(string email)
        {
            // Arrange
            User mockUser = new User(email, 24.0);
            _mailService.Setup(x => x.SendMail(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            IStatusCodeActionResult result = (IStatusCodeActionResult) _controller.PostPincode(mockUser);

            // Assert
            return result.StatusCode;
        }

        [TestCase("1", ExpectedResult = StatusCodes.Status204NoContent)]
        [TestCase("2", ExpectedResult = StatusCodes.Status404NotFound)]
        public int? DeleteUser(string id)
        {
            // Arrange
            User mockUser = new User("1", "test1@gmail.com", 24.0);
            _userService.Setup(x => x.Get("1")).Returns(mockUser);
            _userService.Setup(x => x.Remove(id));

            // Act
            IStatusCodeActionResult result = ((IStatusCodeActionResult) _controller.Delete(id));

            // Assert
            return result.StatusCode;
        }

        [TestCase("", 10, "Petrol", ExpectedResult = StatusCodes.Status400BadRequest)]
        [TestCase("exist@gmail.com", 10, "Petrol", ExpectedResult = StatusCodes.Status409Conflict)]
        [TestCase("newuser@gmail.com", 10, "Petrol", ExpectedResult = StatusCodes.Status200OK)]
        public int? PostRegister(string email, double kml, string fuelType)
        {
            // Arrange
            User mockUser = new User(email, 24);
            RegisterInput mockInput = new RegisterInput(mockUser, kml, fuelType);
            _userService.Setup(x => x.Create(mockUser)).Returns(mockUser);

            // Act
            IStatusCodeActionResult result = (IStatusCodeActionResult) _controller.PostRegister(mockInput);

            // Assert
            return result.StatusCode;
        }
    }
}
