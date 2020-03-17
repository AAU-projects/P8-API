using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace P8_API.Utility
{
    public static class Utility
    {
        public static string GetToken(HttpRequest request)
        {
            return request.Headers["Authorization"][0].Substring("Bearer ".Length).Trim();
        }
    }
}
