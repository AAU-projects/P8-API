using MongoDB.Driver;
using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

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

        public bool Create(string userId, List<Position> positions)
        {
            try
            {
                PositionCollection posCollection = _positions.Find(doc => doc.UserId == userId).FirstOrDefault();

                if (posCollection != null)
                {
                    posCollection.PositionList.AddRange(positions);
                    //_positions.UpdateOne(posCollection);
                }
                else
                    _positions.InsertOne(new PositionCollection(userId, positions));
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
