﻿using MongoDB.Driver;
using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace P8_API.Services
{
    /// <summary>
    /// Service to extract trips from logs
    /// </summary>
    public class ExtractionClusterService : IExtractionService
    {
        private readonly int TripTimeInterval = 5;
        private readonly int MinPositions = 3;
        private readonly IMongoCollection<TripsCollection> _trips;

        /// <summary>
        /// ExtractionService Constructor
        /// </summary>
        /// <param name="database">The database</param>
        public ExtractionClusterService(IMongoDatabase database)
        {
            _trips = database.GetCollection<TripsCollection>("Trips");
        }

        /// <summary>
        /// Extracts trips from a list of positions
        /// </summary>
        /// <param name="positions">The list to extract trips from</param>
        /// <returns>A list of Trips</returns>
        public List<Trip> ExtractTrips(List<Position> positions)
        {
            positions = RemoveClusters(positions);
            List<Trip> tripsResultList = new List<Trip>();

            List<Position> currentPositions = new List<Position>();

            foreach (var pos in positions)
            {
                if(currentPositions.Count == 0 ||
                    pos.Timestamp - currentPositions.Last().Timestamp < TimeSpan.FromMinutes(TripTimeInterval))
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
                    currentPositions = new List<Position>() {pos};
                }
            }

            if (currentPositions.Count > MinPositions)
            {
                tripsResultList.Add(new Trip(currentPositions));
            }

            return tripsResultList;       
        }

        private List<Position> RemoveClusters(List<Position> positions)
        {
            List<Position> newPositions = new List<Position>();

            int lookahead = 10;
            int threshold = 130;

            List<double> avgDistances = new List<double>();
            double similiartyDefault = 0;

            for (int i = 0; i < positions.Count; i++)
            {
                int lookaheadMax = positions.Count >= i + lookahead ? i + lookahead : positions.Count;
                Position a = positions[i];

                double avgDistance = 0;

                for (int j = i + 1; j < lookaheadMax; j++)
                {
                    Position b = positions[j];

                    avgDistance += EuclideanDistance(a, b);
                }

                avgDistance = avgDistance / lookaheadMax;
                avgDistances.Add(avgDistance);
                similiartyDefault += avgDistance;
            }

            similiartyDefault = similiartyDefault / avgDistances.Count;

            List<double> similiarty = new List<double>();
            int thredsholdCounter = 0;
            for (int i = 0; i < avgDistances.Count; i++)
            {
                double value = 100 * Math.Abs(similiartyDefault - avgDistances[i]) / ((similiartyDefault + avgDistances[i]) / 2);
                similiarty.Add(value);
                Debug.WriteLine(value);

                if (value > threshold)
                {
                    thredsholdCounter++;

                } else
                {
                    thredsholdCounter = 0;
                }

                if (thredsholdCounter < lookahead)
                {
                    newPositions.Add(positions[i]);
                }
            }

            return newPositions;
        }

        private double EuclideanDistance(Position a, Position b)
        {
            return Math.Sqrt(Math.Pow(a.Latitude - b.Latitude, 2) + Math.Pow(a.Longitude - b.Longitude, 2));
        }

        public List<TripDocument> GetTrips(string userId)
        {
            TripsCollection tripsCollection = _trips.Find(collection => collection.UserId == userId).FirstOrDefault();

            if (tripsCollection == null)
                return null;

            // This step is done to avoid sending the list of positions from a trip
            foreach (TripDocument document in tripsCollection.TripDocuments)
            {
                foreach (Trip trip in document.TripList)
                {
                    trip.TripPositions = null;
                }
            }
            return tripsCollection.TripDocuments;
        }

        public bool UpdateTrip(string date, string tripId, string userId, Transport transport)
        {
            TripsCollection tripsCollection = _trips.Find(collection => collection.UserId == userId).FirstOrDefault();
            TripDocument tripsOnDate;

            if (tripsCollection != null)
            {
                tripsOnDate = tripsCollection.TripDocuments.FirstOrDefault(x => x.DateId == date);

                if (tripsOnDate != null)
                {
                    Trip trip = tripsOnDate.TripList.FirstOrDefault(x => x.Id == tripId);
                    trip.Transport = transport;

                    FilterDefinition<TripsCollection> filter = Builders<TripsCollection>.Filter.Eq(x => x.UserId, userId);
                    UpdateDefinition<TripsCollection> update = Builders<TripsCollection>.Update.Set(x => x.TripDocuments, tripsCollection.TripDocuments);

                    _trips.UpdateOne(filter, update);
                    return true;

                }
            }
            return false;
        }

        /// <summary>
        /// Saves a list of Trips to the given userId in the database
        /// </summary>
        /// <param name="trips">The trips to save</param>
        /// <param name="userId">The userId to save the trips to</param>
        /// <returns>A bool indicating success</returns>
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

        /// <summary>
        /// Gets the TripDocument from a userId and date
        /// </summary>
        /// <param name="userId">The userId to look for</param>
        /// <param name="date">The date to look for</param>
        /// <returns>A TripDocument containing the trips for the specific user and date</returns>
        public TripDocument GetTripsOfUserAndDate(string userId, string date)
        {
            TripsCollection tripCol = _trips.Find(collection => collection.UserId == userId).FirstOrDefault();
            TripDocument tripDoc = null;

            if (tripCol != null)
            {
                tripDoc = tripCol.TripDocuments.FirstOrDefault(x => x.DateId == date);
            }

            return tripDoc;
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
            UpdateDefinition<TripsCollection> update = Builders<TripsCollection>.Update.Set(x => x.TripDocuments, newDocsList);

            _trips.UpdateOne(filter, update);
        }

        private void AddTripsToDocument(List<Trip> trips, ref List<TripDocument> newDocsList)
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
    }
}