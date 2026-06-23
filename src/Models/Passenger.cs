using System;

public class Passenger
{
    // 1. جعل الحقل private لحمايته داخل الكلاس
    private User? _user;

    // 2. عمل Property عامة للوصول إليه بأمان (تذكري عمل كلاس User نفسه public)
    public User? User
    {
        get { return _user; }
        set { _user = value; }
    }

    public Passenger() { }

    public Passenger(string username, string password)
    {
        _user = new User(username, password);
    }
}
