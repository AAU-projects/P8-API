using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using NUnit.Framework;
using P8_API.Controllers;
using P8_API.Models;
using P8_API.Services;
using System;
using System.Collections.Generic;

namespace P8_API_Tests
{
    public class UserControllerTests
    {
        private Mock<IUserService> _userService;
        private Mock<IAuthenticationService> _authenticationService;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _userService = new Mock<IUserService>();
            _authenticationService = new Mock<IAuthenticationService>();
            _controller = new UserController(_userService.Object, _authenticationService.Object);

            // Initilizes the database with a user using the email no@gmail.com
            User mockUser = new User("13439332", "exist@gmail.com", "FG35382");
            mockUser.UpdatePincode("1234", DateTime.Now.AddDays(365));
            _userService.Setup(x => x.Get("exist@gmail.com")).Returns(mockUser);
        }

        [TestCase("1", ExpectedResult = true)]
        [TestCase("2", ExpectedResult = false)]
        public bool GetUser_UserExists_Success(string id)
        {
            // Prepare objects
            User mockUser = new User("1", "test1@gmail.com", "FG35383");
            _userService.Setup(x => x.Get("1")).Returns(mockUser);

            // Run Get Request
            var result = _controller.Get(id);

            // Do Assertions
            return result.Value == mockUser;
        }

        [Test]
        public void GetAllUsers_Success()
        {
            // Prepare objects
            User mockUser = new User("1", "test1@gmail.com", "asdsssss");
            User mockUser2 = new User("2", "test2@gmail.com", "asd");
            _userService.Setup(x => x.Get()).Returns(new List<User>{ mockUser, mockUser2});

            // Run Get Request
            List<User> result = _controller.Get().Value;

            // Do Assertions
            Assert.AreEqual(result.Count, 2);
        }

        [Test]
        public void UpdateUser_Success()
        {
            // Prepare objects
            User mockUser = new User("1", "test1@gmail.com", "FG35383");
            User UpdatedUser = new User("1", "testnew@gmail.com", "FG35383");
            _userService.Setup(x => x.Get("1")).Returns(mockUser);
            _userService.Setup(x => x.Update("1", UpdatedUser)).Callback((string id, User user) => mockUser = user);
            
            // Run Post Request
            _controller.Put("1", UpdatedUser);

            // Do Assertions
            Assert.AreEqual(mockUser.Email, UpdatedUser.Email);
        }

        [TestCase("1", "test@gmail.com", "1234", "AB34567", ExpectedResult = StatusCodes.Status400BadRequest)]
        [TestCase("2", "exist@gmail.com", "1233", "AB34567", ExpectedResult = StatusCodes.Status401Unauthorized)]
        [TestCase("3", "exist@gmail.com", "1234", "AB34567", ExpectedResult = StatusCodes.Status200OK)]
        public int? PostLogin_Success(string id, string email, string pincode, string licenseplate)
        {
            // Prepare objects
            User mockUser = new User(id, email, licenseplate);
            mockUser.UpdatePincode(pincode, DateTime.Now.AddDays(365));

            // Run Post Request
            var res = _controller.PostLogin(mockUser);

            // Do Assertions
            return ((IStatusCodeActionResult)_controller.PostLogin(mockUser)).StatusCode;
        }

        [TestCase("test@gmail.com", ExpectedResult = StatusCodes.Status400BadRequest)]
        [TestCase("exist@gmail.com", ExpectedResult = StatusCodes.Status200OK)]
        public int? PostPincode_Success(string email)
        {
            // Do Assertions
            return ((IStatusCodeActionResult)_controller.PostPincode(email)).StatusCode;
        }

        [TestCase("1", ExpectedResult = StatusCodes.Status204NoContent)]
        [TestCase("2", ExpectedResult = StatusCodes.Status404NotFound)]
        public int? DeleteUser_Success(string id)
        {
            // Prepare objects
            User mockUser = new User("1", "test1@gmail.com", "FG35383");
            _userService.Setup(x => x.Get("1")).Returns(mockUser);
            _userService.Setup(x => x.Remove(id));

            // Run Post Request
            return ((IStatusCodeActionResult)_controller.Delete(id)).StatusCode;
        }

        [TestCase("hello@you.com", "", ExpectedResult = StatusCodes.Status400BadRequest)]
        [TestCase("", "SG43534", ExpectedResult = StatusCodes.Status400BadRequest)]
        [TestCase("exist@gmail.com", "SG43534", ExpectedResult = StatusCodes.Status409Conflict)]
        [TestCase("licenseplate@gmail.com", "SG435346", ExpectedResult = StatusCodes.Status400BadRequest)]
        [TestCase("newuser@gmail.com", "SG43534", ExpectedResult = StatusCodes.Status200OK)]
        public int? PostRegister_Sucess(string email, string licenseplate)
        {
            // Prepare objects
            User mockUser = new User(email, licenseplate);

            _userService.Setup(x => x.Create(mockUser)).Returns(mockUser);

            // Run Post Request
            return ((IStatusCodeActionResult)_controller.PostRegister(mockUser)).StatusCode;
        }
    }
}
