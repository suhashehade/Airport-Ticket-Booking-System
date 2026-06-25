using System;

namespace AirportSystem.Models
{
    public class Passenger
    {
        private User? _user;

        public User? User
        {
            get { return _user; }
            set { _user = value; }
        }

        public Passenger() { }

        public Passenger(string username, string password)
        {
            _user = new User(username, password, User.UserRole.Passenger);
        }
    }
}