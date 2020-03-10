using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public interface IAuthenticationService
    {
        bool ValidateToken(string authToken);
        User Authenticate(string email, string pincode);
        User GeneratePinAuthentication(string email);
    }
}
