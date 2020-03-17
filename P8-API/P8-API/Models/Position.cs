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
        [BsonElement]
        private double Latitude { get; set; }
        [BsonElement]
        private double Longitude { get; set; }
        [BsonElement]
        private DateTime Timestamp { get; set; }
        [BsonElement]
        private double Altitude { get; set; }
        [BsonElement]
        private double Accuracy { get; set; }
        [BsonElement]
        private double Heading { get; set; }
        [BsonElement]
        private double Speed { get; set; }
        [BsonElement]
        private double SpeedAccuracy { get; set; }

        public Position(double latitude, double longitude, DateTime timestamp, double altitude, double accuracy, double heading, double speed, double speedAccuracy)
        {
            Latitude = latitude;
            Longitude = longitude;
            Timestamp = timestamp;
            Altitude = altitude;
            Accuracy = accuracy;
            Heading = heading;
            Speed = speed;
            SpeedAccuracy = speedAccuracy;
        }

        public BsonDocument ConvertToBsonDoc()
        {
            return new BsonDocument
            {
                {"latitude", Latitude},
                {"longitude", Longitude},
                {"timestamp", Timestamp},
                {"altitude", Altitude},
                {"accuracy", Accuracy},
                {"heading", Heading},
                {"speed", Speed},
                {"speedAccuracy", SpeedAccuracy},
            };
        }
    }
}
