using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace P8_API.Utility
{
    public static class Helper
    {
        public static IUtility Utility = new Utility();
    }
    public class Utility : IUtility
    {
        /// <summary>
        /// Retrives a token from HTTPRequest
        /// </summary>
        /// <param name="request">The HTTPRequest</param>
        /// <returns>The bearer token from the request</returns>
        public string GetToken(HttpRequest request)
        {
            try
            {
                return request.Headers["Authorization"][0].Substring("Bearer ".Length).Trim();

            }
            catch (Exception )
            {
                return null;
            }
        }

        /// <summary>
        /// Validates a email
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns>True if valid</returns>
        public bool IsEmailValid(string email)
        {
            EmailAddressAttribute e = new EmailAddressAttribute();

            return e.IsValid(email);
        }
    }
}
