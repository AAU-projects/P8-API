using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public interface IUserService
    {
        User Get(string id);
        List<UserBase> Get();

        User Create(User user);

        void Update(string id, User user);

        void UpdateEmail(string id, string email);

        void Remove(User user);

        void Remove(string id);

        User ValidatePincode(string email, string pincode);
    }
}
