using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Models
{
    public enum Transport
    {
        Unknown = -1,
        Walk,
        Bike,
        Car,
        Public,
    }

    public class Trip
    {
        public string Id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Position> TripPositions { get; set; }
        public int TripDuration { get; set; }
        public DateTime TripDate { get; set; }
        public Transport Transport { get; set; }
        public double MaxSpeed { get; set; } = double.MinValue;
        public double MinSpeed { get; set; } = double.MaxValue;
        public double AverageSpeed { get; set; } = 0;

        public Trip() {}

        public Trip(List<Position> inputPositions)
        {
            Guid guid = Guid.NewGuid();
            Id = guid.ToString();

            TripDate = inputPositions.First().Timestamp;

            TripPositions = inputPositions;
            TripDuration = CalculateDuration();
            CalculateSpeed();
        }

        private int CalculateDuration()
        {
            Position start = GetStart();
            Position end = GetEnd();
            return (int)(end.Timestamp - start.Timestamp).TotalSeconds;
        }

        public void CalculateSpeed()
        {
            foreach (Position position in TripPositions)
            {
                if (position.Speed > MaxSpeed) MaxSpeed = position.Speed;
                if (position.Speed < MinSpeed) MinSpeed = position.Speed;
                AverageSpeed += position.Speed;
            }

            AverageSpeed /= TripPositions.Count;
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
