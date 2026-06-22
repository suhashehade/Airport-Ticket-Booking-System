using System;
namespace PassengerSpace

{
    public class Passenger
    {
        public readonly User user;


        public Passenger(string username, string password)
        {
            user = new User(username, password);
        }
    }
}
