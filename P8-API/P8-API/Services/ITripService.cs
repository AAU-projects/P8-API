﻿using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public interface ITripService
    {
        void PredictTransport(Trip trip);
        List<Trip> GetRecentTrips(User user);
        void UpdateTrip(string tripId, Transport transport, string userId, string dateId);
    }
}
