using System;

public class Manager
{
    public User? User;


    public Manager(string username, string password)
    {
        User = new User(username, password);
    }
}

