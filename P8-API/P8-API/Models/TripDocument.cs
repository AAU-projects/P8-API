﻿using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace P8_API.Models
{
    [ExcludeFromCodeCoverage]
    public class TripDocument
    {
        [BsonId]
        public string DateId { get; set; }
        public List<Trip> TripList { get; set; }
        public TripDocument(string dateId, List<Trip> tripList)
        {
            DateId = dateId;
            TripList = tripList;
        }
    }
}