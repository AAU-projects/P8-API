using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Models
{
    public class Position
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        readonly double latitude;
        readonly double longitude;
        readonly DateTime timestamp;
        readonly double altitude;
        readonly double accuracy;
        readonly double heading;
        readonly double speed;
        readonly double speedAccuracy;

    }
}
