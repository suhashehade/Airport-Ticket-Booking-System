using System;

namespace AirportSystem.Models
{
    public class User
    {

        public enum UserRole
        {
            Manager,
            Passenger
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        public User(string username, string password, UserRole role)
        {
            Username = username;
            Password = password;
            Role = role;
        }
    }
}