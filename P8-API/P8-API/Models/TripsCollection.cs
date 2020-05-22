using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace P8_API.Models
{
    [ExcludeFromCodeCoverage]
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
