using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    interface IExtractionService
    {
        public List<Trip> ExtractTrips(List<Position> positions);
    }
}
