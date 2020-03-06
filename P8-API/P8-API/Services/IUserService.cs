﻿using P8_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Services
{
    public interface IUserService
    {
        User Get(string id);
        List<User> Get();

        User Create(User user);

        void Update(string id, User user);

        void Remove(User user);

        void Remove(string id);
    }
}