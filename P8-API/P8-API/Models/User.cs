using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace P8_API.Models
{
    public class User : UserBase
    {
        public string Pincode { get; set; }
        public string Token { get; set; }
        public DateTime PinExpirationDate { get; set; }
        public DateTime TokenExpirationDate { get; set; }

        [JsonConstructor]
        public User(string id, string email, double carEmission) : base(id, email, carEmission)
        {
        }

        public User(string email, double carEmission) : base(email, carEmission)
        {
        }

        public void UpdatePincode(string code, DateTime expirationDate)
        {
            Pincode = code;
            PinExpirationDate = expirationDate;
        }

        public void UpdateToken(string token, DateTime expirationDate)
        {
            Token = token;
            TokenExpirationDate = expirationDate;
        }
    }
}
