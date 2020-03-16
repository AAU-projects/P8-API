using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Models
{
    public class AppSettings : IAppSettings
    {
        public string Secret { get; set; }
        public string EmailAddress { get; set; }
        public string EmailPassword { get; set; }
    }
}
