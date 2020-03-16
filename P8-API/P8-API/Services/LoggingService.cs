using MongoDB.Driver;
using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly IMongoCollection<PositionCollection> _positions;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="settings">The database interface</param>
        public LoggingService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _positions = database.GetCollection<PositionCollection>("Positions");
        }

        public List<Position> Create(string userId, List<Position> positions)
        {
            PositionCollection postionData = _positions.Find(x => x.Id == userId).FirstOrDefault();
            return positions;
        }
    }
}
