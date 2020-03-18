using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace P8_API.Utility
{
    public static class Utility
    {
        public static string GetToken(HttpRequest request)
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

        public static bool IsEmailValid(string email)
        {
            EmailAddressAttribute e = new EmailAddressAttribute();

            return e.IsValid(email);
        }
    }
}
