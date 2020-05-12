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
        public double TopMedian { get; set; } = double.MinValue;
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
            CaculateSpeed();
        }

        private int CalculateDuration()
        {
            Position start = GetStart();
            Position end = GetEnd();
            return (end.Timestamp - start.Timestamp).Seconds;
        }

        public void CaculateSpeed()
        {
            List<double> speeds = new List<double>();

            foreach (Position position in TripPositions)
            {
                speeds.Add(position.Speed);
                if (position.Speed > MaxSpeed) MaxSpeed = position.Speed;
                if (position.Speed < MinSpeed) MinSpeed = position.Speed;
                AverageSpeed += position.Speed;
            }

            TopMedian = FindTopMedian(speeds);

            AverageSpeed = AverageSpeed / TripPositions.Count;
        }

        private double FindTopMedian(List<double> speeds)
        {
            speeds.Sort();
            int minIndex = (int)(speeds.Count * 0.75);
            List<double> speedsTop = speeds.GetRange(minIndex, speeds.Count - minIndex);

            if (speedsTop.Count % 2 == 0)
            {
                return (speedsTop[(int)speedsTop.Count / 2] + speedsTop[(int)speedsTop.Count / 2 + 1]) / 2;
            } else
            {
                return speedsTop[(int)speedsTop.Count / 2];
            }
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
