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
        private readonly IMongoDatabase _db;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="settings">The database interface</param>
        public LoggingService(IMongoDatabase database)
        {
            _positions = database.GetCollection<PositionCollection>("Positions");
            _db = database;
        }

        /// <summary>
        /// Creates a document with positions in the database 
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="positions">The position listed to be stored</param>
        /// <returns>True if succeded</returns>
        public bool Create(string userId, List<Position> positions)
        {
            try
            {
                string now = DateTime.Now.ToString("dd-MM-yyyy");

                PositionCollection userCollection = _positions.Find(collection => collection.UserId == userId).FirstOrDefault();

                if (userCollection != null) 
                {
                    // If user does exist.
                    int userDocumentIndex = userCollection.Documents.FindIndex(document => document.DateId == now);

                    if (userDocumentIndex == -1) 
                    {
                        // If day does not exist.
                        AddDayToExistingUser(userId, positions, userCollection, now);
                    } 
                    else
                    {
                        // Updates already existing day with positions
                        UpdateExistingDay(userId, positions, userCollection, userDocumentIndex);
                    }
                }
                else
                {
                    // If the user does not exist
                    AddPositionDay(userId, positions, now);
                }

                // Extract trips from position list and saves them on the user id
                ExtractionService extractionService = new ExtractionService(_db);
                List<Trip> tripsResultList = extractionService.ExtractTrips(positions);
                extractionService.SaveTrips(tripsResultList, userId);
            }
            catch (Exception )
            {
                return false;
            }

            return true;
        }

        private void AddDayToExistingUser(string userId, List<Position> positions, PositionCollection userCollection, string now)
        {
            PositionDocument test = new PositionDocument(now, positions);
            userCollection.Documents.Add(test);

            FilterDefinition<PositionCollection> filter = Builders<PositionCollection>.Filter.Eq(x => x.UserId, userId);
            UpdateDefinition<PositionCollection> update = Builders<PositionCollection>.Update.Set(x => x.Documents, userCollection.Documents);

            _positions.UpdateOne(filter, update);
        }

        private void UpdateExistingDay(string userId, List<Position> positions, PositionCollection userCollection, int userDocumentIndex)
        {
            PositionDocument userDocument = userCollection.Documents[userDocumentIndex];
            userDocument.PositionList.AddRange(positions);

            FilterDefinition<PositionCollection> filter = Builders<PositionCollection>.Filter.Eq(x => x.UserId, userId);
            UpdateDefinition<PositionCollection> update = Builders<PositionCollection>.Update.Set(x => x.Documents[userDocumentIndex], userDocument);

            _positions.UpdateOne(filter, update);
        }

        private void AddPositionDay(string userId, List<Position> positions, string now)
        {
            PositionDocument test = new PositionDocument(now, positions);
            PositionCollection col = new PositionCollection(userId, test);
            _positions.InsertOne(col);
        }
    }
}
