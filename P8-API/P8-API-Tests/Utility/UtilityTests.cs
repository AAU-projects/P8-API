using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using P8_API.Utility;

namespace P8_API_Tests.UtilityTest
{
    class UtilityTests
    {
        [TestCase("Bearer test_token", ExpectedResult = "test_token")]
        [TestCase("test_token", ExpectedResult = null)]
        public string GetToken(string token)
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = new StringValues(token);

            // Act
            string result = Helper.Utility.GetToken(httpContext.Request);

            // Assert
            return result;
        }
    }
}
