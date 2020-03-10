using MongoDB.Driver;
using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="settings">The database interface</param>
        public UserService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>("Users");
        }

        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <returns>a user list</returns>
        public List<User> Get()
        {
            return _users.Find(u => true).ToList();
        }

        /// <summary>
        /// Gets a user from the database
        /// </summary>
        /// <param name="id">The unique id of the user</param>
        /// <returns>a user</returns>
        public User Get(string input)
        {
            try
            {
                User user = _users.Find(u => u.Email == input).FirstOrDefault();
                if (user != null)
                    return user;

                return _users.Find(u => u.Id == input).FirstOrDefault();
            }
            catch (FormatException)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a user in the DB
        /// </summary>
        /// <param name="user">The new user object</param>
        /// <returns>a user</returns>
        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        /// <summary>
        /// Updates a user in the DB
        /// </summary>
        /// <param name="id">The unique id of the user</param>
        /// <param name="user">The new user object</param>
        public void Update(string id, User user)
        {
            _users.ReplaceOne(u => u.Id == id, user);
        }

        /// <summary>
        /// Removes a user from the DB.
        /// </summary>
        /// <param name="user">The user object</param>
        public void Remove(User user)
        {
            _users.DeleteOne(u => u.Id == user.Id);
        }
            
        /// <summary>
        /// Removes a user from the DB.
        /// </summary>
        /// <param name="id">The unique id of the user</param>
        public void Remove(string id)
        {
            _users.DeleteOne(u => u.Id == id);
        }
    }
}
