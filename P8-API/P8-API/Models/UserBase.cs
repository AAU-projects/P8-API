﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Models
{
    public class UserBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Email { get; set; }
        public double CarEmission { get; set; }

        [JsonConstructor]
        public UserBase(string id, string email, double carEmission)
        {
            Id = id;
            Email = email;
            CarEmission = carEmission;
        }

        public UserBase(string email, double carEmission)
        {
            Email = email;
            CarEmission = carEmission;
        }
    }
}
