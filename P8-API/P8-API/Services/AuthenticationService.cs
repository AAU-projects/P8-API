using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using P8_API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAppSettings _appSettings;
        private readonly IMongoCollection<User> _users;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="settings">The database interface</param>
        /// <param name="appsettings">App settings</param>
        public AuthenticationService(IDatabaseSettings settings, IAppSettings appsettings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _appSettings = appsettings;

            _users = database.GetCollection<User>("Users");
        }

        public bool ValidateToken(string authToken)
        {
            User user;
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
                string email = principal.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;
                user = _users.Find(u => u.Email == email).FirstOrDefault();
            }
            catch (Exception) // TODO implment all types of exception of invalid token.
            {
                return false;
            }

            return user != null;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "RouteAPI",
                ValidAudience = "RouteAPI",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret)) // The same key as the one that generate the token
            };
        }

        public User Authenticate(string email, string pincode)
        {
            User user = GetPincode(email, pincode);
            if (user == null)
                return null;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var secToken = new JwtSecurityToken(
                signingCredentials: credentials,
                issuer: "RouteAPI",
                audience: "RouteAPI",
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, email)
                },
                expires: DateTime.UtcNow.AddDays(365));

            var handler = new JwtSecurityTokenHandler();
            user.UpdateToken(handler.WriteToken(secToken), DateTime.UtcNow.AddDays(365));

            return user;
        }


        /// <summary>
        /// Creates a pincode in the DB
        /// </summary>
        /// <param name="email">Email linked to the pincode</param>
        public User GeneratePinAuthentication(string email)
        {
            string code = GeneratePincode();
            DateTime expirationDate = DateTime.Now.AddMinutes(15);
            User updatedUser = _users.Find(u => u.Email == email).FirstOrDefault();
            updatedUser.UpdatePincode(code, expirationDate);

            _users.ReplaceOne(u => u.Id == updatedUser.Id, updatedUser);

            return updatedUser;
        }



        /// <summary>
        /// Validates that a pincode exisits for that email
        /// </summary>
        /// <param name="email">Email linked to the pincode</param>
        /// <param name="pincode">Pincode that is valid</param>
        /// <returns>A pincode for that user</returns>
        private User GetPincode(string email, string pincode)
        {
            User user = _users.Find(p =>
                              p.Email == email &&
                              p.Pincode == pincode &&
                              p.PinExpirationDate >= DateTime.Now).FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Generate a random pincode number of length 4
        /// </summary>
        /// <returns>a pincode</returns>
        private string GeneratePincode()
        {
            Random random = new Random();
            string pincode = "";

            for (int i = 0; i < 4; i++)
            {
                pincode += random.Next(1, 9);
            }
            return pincode;
        }
    }
}
