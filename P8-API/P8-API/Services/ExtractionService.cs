using MongoDB.Driver;
using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public class ExtractionService : IExtractionService
    {
        private readonly int TripTimeInterval = 5;
        private readonly int MinPositions = 3;
        private readonly IMongoCollection<TripsCollection> _trips;

        public ExtractionService(IMongoDatabase database)
        {
            _trips = database.GetCollection<TripsCollection>("Trips");
        }

        public List<Trip> ExtractTrips(List<Position> positions)
        {
            List<Trip> tripsResultList = new List<Trip>();

            List<Position> currentPositions = new List<Position>();

            foreach (var pos in positions)
            {
                if(currentPositions.Count == 0)
                {
                    currentPositions.Add(pos);
                    continue;
                }

                if(pos.Timestamp - currentPositions.Last().Timestamp < TimeSpan.FromMinutes(TripTimeInterval))
                {
                    currentPositions.Add(pos);
                    continue;
                }
                else
                {
                    if(currentPositions.Count > MinPositions)
                    {
                        tripsResultList.Add(new Trip(currentPositions));
                    }
                    currentPositions.Clear();
                    currentPositions.Add(pos);
                }
            }

            if(currentPositions.Count > 0)
            {
                if (currentPositions.Count > MinPositions)
                {
                    tripsResultList.Add(new Trip(currentPositions));
                }
            }

            return tripsResultList;
        }

        

        public bool SaveTrips(List<Trip> trips, string userId)
        {
            try
            {
                TripsCollection tripsCollection = _trips.Find(collection => collection.UserId == userId).FirstOrDefault();

                if (tripsCollection != null)
                {
                    AddTripsToExistingUser(tripsCollection, trips);
                }
                else
                {
                    AddNewUserAndTrips(userId, trips);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void AddNewUserAndTrips(string userId, List<Trip> trips)
        {
            List<TripDocument> docList = new List<TripDocument>();

            AddTripsToDocument(trips, ref docList);

            TripsCollection collection = new TripsCollection(userId, docList);

            _trips.InsertOne(collection);
        }

        private void AddTripsToExistingUser(TripsCollection collection, List<Trip> trips)
        {
            List<TripDocument> newDocsList = collection.TripDocuments;
            AddTripsToDocument(trips, ref newDocsList);

            FilterDefinition<TripsCollection> filter = Builders<TripsCollection>.Filter.Eq(x => x.UserId, collection.UserId);
            UpdateDefinition<TripsCollection> update = Builders<TripsCollection>.Update.Set(x => collection.TripDocuments, newDocsList);

            _trips.UpdateOne(filter, update);
        }

        private static void AddTripsToDocument(List<Trip> trips, ref List<TripDocument> newDocsList)
        {
            foreach (var trip in trips)
            {
                string tripDate = trip.GetStart().Timestamp.Date.ToString("dd-MM-yyyy");

                var existingDoc = newDocsList.FirstOrDefault(x => x.DateId == tripDate);

                if (existingDoc != null)
                {
                    existingDoc.TripList.Add(trip);
                }
                else
                {
                    newDocsList.Add(new TripDocument(tripDate, new List<Trip> { trip }));
                }
            }
        }

        public TripDocument GetTripsOfUserAndDate(string userId, string date)
        {
            TripsCollection tripCol = _trips.Find(collection => collection.UserId == userId).FirstOrDefault();
            TripDocument tripDoc = null;

            if(tripCol != null)
            {
                tripDoc = tripCol.TripDocuments.FirstOrDefault(x => x.DateId == date);
            }

            return tripDoc;
        }
    }
}
