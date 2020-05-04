using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Models
{
    public enum Transport
    {
        Walk,
        Bike,
        Car,
        Public
    }

    public class Trip
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<Position> TripPositions { get; set; }
        public int TripDuration { get; set; }
        public Transport Transport { get; set; }
        public double MaxSpeed { get; set; } = double.MinValue;
        public double MinSpeed { get; set; } = double.MaxValue;
        public double AverageSpeed { get; set; } = 0;

        public Trip() {}

        public Trip(List<Position> inputPositions)
        {
            TripPositions = inputPositions;
            TripDuration = CalculateDuration();
        }

        private int CalculateDuration()
        {
            Position start = GetStart();
            Position end = GetEnd();
            return (end.Timestamp - start.Timestamp).Seconds;
        }

        public void CaculateSpeed()
        {
            foreach (Position position in TripPositions)
            {
                if (position.Speed > MaxSpeed) MaxSpeed = position.Speed;
                if (position.Speed < MinSpeed) MinSpeed = position.Speed;
                AverageSpeed += position.Speed;
            }

            AverageSpeed = AverageSpeed / TripPositions.Count;
        }

        public Position GetStart()
        {
            return TripPositions.First();
        }

        public Position GetEnd()
        {
            return TripPositions.Last();
        }
    }
}
