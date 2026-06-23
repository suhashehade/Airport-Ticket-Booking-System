using System;
using AirportSystem.Models;
using AirportSystem.Services;
using AirportSystem.Data;

class Program
{
    private static AuthService _authService = new AuthService();
    private static FileContext _fileContext = new FileContext();
    private static FlightService _flightService = new FlightService(_fileContext);
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
        bool isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Welcome Manager: [{name}]");
            Console.ResetColor();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("1. View Booked Tickets");
            Console.WriteLine("2. Import Flights from CSV");
            Console.WriteLine("3. Logout / Exit");
            Console.WriteLine("----------------------------------------");
            Console.Write("Select an option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n[Feature coming soon: View Booked Tickets]");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine("=== Import Flights from CSV ===");
                    Console.Write("Please enter the full path of the CSV file: ");
                    string? filePath = Console.ReadLine();

                    Console.WriteLine("\nProcessing file, please wait...");
                    bool success = _flightService.ImportFlightsFromCsv(filePath!);

                    if (success)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Flights database updated successfully!");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Failed to import flights. Please check errors above.");
                    }

                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    break;

                case "3":
                    isRunning = false;
                    Console.WriteLine("\nLogging out...");
                    break;

                default:
                    Console.WriteLine("\nInvalid option! Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
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