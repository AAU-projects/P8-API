using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public interface IEmissionService
    {
        Task<double> retrieveEmission(string licenseplate);
    }
}
