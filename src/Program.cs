using System;
using AirportSystem.Models;
using AirportSystem.Services;

class Program
{
    static void Main(string[] args)
    {
        AuthService authService = new();

        Console.WriteLine("========================================");
        Console.WriteLine("    Welcome to Airport Booking System   ");
        Console.WriteLine("========================================");

        Console.Write("Enter Username: ");
        string? username = Console.ReadLine();

        Console.Write("Enter Password: ");
        string? password = Console.ReadLine();


        User? loggedInUser = authService.Login(username!, password!);

        Console.Clear();


        if (loggedInUser != null)
        {

            switch (loggedInUser.Role)
            {
                case User.UserRole.Manager:
                    ShowManagerMenu(loggedInUser.Username);
                    break;

                case User.UserRole.Passenger:
                    ShowPassengerMenu(loggedInUser.Username);
                    break;
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid username or password. Access Denied!");
            Console.ResetColor();
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static void ShowManagerMenu(string name)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Welcome Manager: [{name}]");
        Console.ResetColor();
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1. View Booked Tickets");
        Console.WriteLine("2. Import Flights from CSV");
        Console.WriteLine("3. Exit");
        Console.Write("\nSelect an option: ");
    }

    static void ShowPassengerMenu(string name)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Welcome Passenger: [{name}]");
        Console.ResetColor();
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1. Search & Book Flights");
        Console.WriteLine("2. Cancel Booking");
        Console.WriteLine("3. Exit");
        Console.Write("\nSelect an option: ");
    }
}