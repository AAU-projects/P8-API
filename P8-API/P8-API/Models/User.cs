﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace P8_API.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Email { get; set; }
        public string LicensePlate { get; set; }
        public string Pincode { get; set; }
        public string Token { get; set; }

        public DateTime PinExpirationDate { get; set; }
        public DateTime TokenExpirationDate { get; set; }



        public User(string id, string email, string licenseplate)
        {
            Id = id;
            Email = email;
            LicensePlate = licenseplate;
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
