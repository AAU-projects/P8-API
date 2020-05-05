using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public interface IGoogleService
    {
        bool NearbyTransit(int range, double lattitude, double longitude);
    }
}
