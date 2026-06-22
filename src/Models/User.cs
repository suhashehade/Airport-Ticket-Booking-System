using System;

class User
{

    private string _username;
    private string _password;

    public string Username
    {
        get { return _username; }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _username = value;
            }
        }
    }

    public string Password
    {
        get { return _password; }
        set {
            if (!string.IsNullOrEmpty(value))
            {
                _password = value;
            }
        }
    }

   
    public User(string username, string password)
    {
        Username = username;       
        Password = password;
    }
}