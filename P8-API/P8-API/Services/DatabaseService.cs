using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public class DatabaseService : IDatabaseService
    {
        public List<User> GetAllUsers()
        {
            return new List<User> { new Models.User("123", "Test@email.com"), new Models.User("asdsad", "Test2@email.com") };
        }

        public User GetUser(string id)
        {
            throw new NotImplementedException();
        }
    }
}
