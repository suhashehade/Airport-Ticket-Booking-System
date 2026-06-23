using System;
using System.Collections.Generic;
using System.Linq;
using AirportSystem.Models;

namespace AirportSystem.Services
{
    public class AuthService
    {
        private readonly List<User> _users;

        public AuthService()
        {

            _users = new List<User>
            {
                new User("admin", "admin123", User.UserRole.Manager),
                new User("suha", "123", User.UserRole.Passenger),
                new User("raghad", "123456", User.UserRole.Passenger)
            };
        }


        public User? Login(string username, string password)
        {
            User? user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);

            return user;
        }
    }
}