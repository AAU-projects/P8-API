using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P8_API.Models;

namespace P8_API
{
    public interface ILoggingService
    {
        public bool Create(string userId, List<Position> positions);
    }
}
