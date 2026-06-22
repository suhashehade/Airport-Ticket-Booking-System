using System;
namespace ManagerSpace

{
    public class Manager
    {
        public readonly User user;


        public Manager(string username, string password)
        {
            user = new User(username, password);
        }
    }
}
