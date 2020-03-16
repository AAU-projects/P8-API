using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Models
{
    public interface IAppSettings
    {
        string Secret { get; set; }
        string EmailAddress { get; set; }
        string EmailPassword { get; set; }
    }
}
