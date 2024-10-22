﻿using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public interface IEmissionService
    {
        double RegisterRetrieveEmission(RegisterInput input);
        double RetrieveEmission(double kml, string fuelType);
        double RetrieveEmission(string fuelType);
        double RetrieveEmission();
    }
}
