using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public User(string id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}
