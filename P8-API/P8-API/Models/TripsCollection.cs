using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Models
{
    public class TripsCollection
    {
        [BsonId]
        public string UserId { get; set; }
        public List<TripDocument> TripDocuments { get; set; }

        public TripsCollection(string userId, List<TripDocument> tripDocuments)
        {
            UserId = userId;
            TripDocuments = tripDocuments;
        }
    }
}
