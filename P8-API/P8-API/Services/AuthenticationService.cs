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
        private readonly IUserService _usersService;
        private readonly IMailService _mailService;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="settings">The database interface</param>
        /// <param name="appsettings">App settings</param>
        public AuthenticationService(IUserService userService, IMailService mailService, IAppSettings appsettings)
        {
            _usersService = userService;
            _mailService = mailService;
            _appSettings = appsettings;
        }

        /// <summary>
        /// Validates the authentication token and returns email
        /// </summary>
        /// <param name="authToken">the authentication token as a string</param>
        /// <param name="appsettings">App settings</param>
        /// <returns>the email if token is valid</returns>
        public User ValidateToken(string authToken)
        {
            User user;

            // Initilize JWT token handler & parameters
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidIssuer = "RouteAPI",
                ValidAudience = "RouteAPI",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret)) // The same key as the one that generate the token
            };

            SecurityToken validatedToken;
            try
            {  
                ClaimsPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
                string email = principal.Identities.FirstOrDefault().Claims.FirstOrDefault().Value;

                // Matches the email from the token with a user in the system 
                user = _usersService.Get(email);
            }
            catch (Exception) // TODO implment all types of exception of invalid token.
            {
                return null;
            }

            // Return true if email is found ie. the token is valid 
            return user;
        }

        /// <summary>
        /// Authenticates a user and generates a unique token linked to the user's email
        /// </summary>
        /// <param name="email">the user's email as a string</param>
        /// <param name="pincode">the pincode to authenticate the user</param>
        /// <returns>A user with a valid token</returns>
        public User Authenticate(string email, string pincode)
        {
            // Checks if the pincode is valid to the email
            User user = _usersService.ValidatePincode(email, pincode);
            if (user == null)
                return null;

            // Setup credentials & securitykey
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            // Create token
            JwtSecurityToken secToken = new JwtSecurityToken(
                signingCredentials: credentials,
                issuer: "RouteAPI",
                audience: "RouteAPI",
                claims: new[] { new Claim(JwtRegisteredClaimNames.Email, email) },
                expires: DateTime.UtcNow.AddDays(365));

            // Updates user with new token
            user.UpdateToken(handler.WriteToken(secToken), DateTime.UtcNow.AddDays(365));

            return user;
        }

        /// <summary>
        /// Generates a pincode for a user
        /// </summary>
        /// <param name="email">Email linked to the pincode</param>
        /// <returns>A pincode for that user</returns>
        public bool GeneratePinAuthentication(string email)
        {
            string code = GeneratePincode();
            DateTime expirationDate = DateTime.Now.AddMinutes(15);

            User updatedUser = _usersService.Get(email);
            updatedUser.UpdatePincode(code, expirationDate);
            _usersService.Update(updatedUser.Id, updatedUser);

            return _mailService.SendMail(updatedUser.Email, updatedUser.Pincode);
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
                pincode += random.Next(1, 9);

            return pincode;
        }
    }
}
