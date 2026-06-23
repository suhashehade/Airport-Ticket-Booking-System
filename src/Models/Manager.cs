using System;

namespace AirportSystem.Models
{
    public class Manager
    {
        private User? _user;

        public User? User
        {
            get { return _user; }
            set { _user = value; }
        }

        public Manager() { }

        public Manager(string username, string password)
        {
            _user = new User(username, password, User.UserRole.Manager);
        }
    }
}