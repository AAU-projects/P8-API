using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using P8_API.Services;
using P8_API.Controllers;
using P8_API.Models;

namespace P8_API_Tests
{
    public class UserControllerTests
    {
        private Mock<IUserService> _mockDB;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _mockDB = new Mock<IUserService>();
            _controller = new UserController(_mockDB.Object);
        }

        [TestCase("1", ExpectedResult = true)]
        [TestCase("2", ExpectedResult = false)]
        public bool GetUser_UserExists_Success(string id)
        {
            // Prepare objects
            User mockUser = new User("1", "test1@gmail.com");
            _mockDB.Setup(x => x.Get("1")).Returns(mockUser);

            // Run Get Request
            var result = _controller.Get(id);

            // Do Assertions
            return result.Value == mockUser;
        }

        [Test]
        public void GetAllUsers_Success()
        {
            // Prepare objects
            User mockUser = new User("1", "test1@gmail.com");
            User mockUser2 = new User("2", "test2@gmail.com");
            _mockDB.Setup(x => x.Get()).Returns(new List<User>{ mockUser, mockUser2});

            // Run Get Request
            List<User> result = _controller.Get().Value;

            // Do Assertions
            Assert.AreEqual(result.Count, 2);
        }
    }
}
