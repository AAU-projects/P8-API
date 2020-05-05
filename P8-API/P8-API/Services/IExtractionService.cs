using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public interface IExtractionService
    {
        /// <summary>
        /// Extracts trips from a list of positions
        /// </summary>
        /// <param name="positions">The list of positions</param>
        /// <returns>A list of Trips</returns>
        public List<Trip> ExtractTrips(List<Position> positions);

        /// <summary>
        /// Saves a list of Trips to the database
        /// </summary>
        /// <param name="trips">The list of Trips to save</param>
        /// <returns>A bool indicating success</returns>
        public bool SaveTrips(List<Trip> trips, string userId);

        /// <summary>
        /// Gets the TripDocument for the given userId and date
        /// </summary>
        /// <param name="userId">The userId to retrieve from</param>
        /// <param name="date">The date to retrieve from</param>
        /// <returns>A TripDocument containing date and trips</returns>
        public TripDocument GetTripsOfUserAndDate(string userId, string date);

        bool UpdateTrip(string date, string tripId, string userId, Transport transport);

        List<TripDocument> GetTrips(string userId);

    }
}
