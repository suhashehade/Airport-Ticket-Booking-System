using AirportSystem.Models;
using static AirportSystem.Enums.AppEnums;

namespace AirportSystem.Services
{
    public class AuthService
    {
        private readonly List<User> _users;

        public AuthService()
        {

            _users = new List<User>
            {
                new User("admin", "admin123", UserRole.Manager),
                new User("suha", "123", UserRole.Passenger),
                new User("raghad", "123456", UserRole.Passenger)
            };
        }


        public User? Login(string username, string password)
        {
            User? user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);

            return user;
        }
    }
}