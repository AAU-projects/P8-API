using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Models
{
    public class Trip
    {
        public List<Position> TripPositions { get; private set; }
        public int TripDuration { get; private set; }

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
