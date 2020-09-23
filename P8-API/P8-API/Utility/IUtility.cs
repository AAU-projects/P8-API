using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Utility
{
    public interface IUtility
    {
        string GetToken(HttpRequest request);
        bool IsEmailValid(string email);
    }
}
