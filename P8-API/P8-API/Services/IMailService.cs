using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public interface IMailService
    {
        bool SendMail(string toEmail, string pincode);
    }
}
