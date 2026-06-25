using AirportSystem.Models;
using AirportSystem.Services;
using AirportSystem.Data;
using AirportSystem.Shared;
using static AirportSystem.Enums.AppEnums;
using static AirportSystem.Menus.ManagerMenus;
using static AirportSystem.Menus.PassengerMenus;


class Program
{
    private static readonly FileContext _flightsFileContext = new("flights.json");
    private static readonly FileContext _ticketsFileContext = new("tickets.json");
    private static readonly AuthService _authService = new();

    private static readonly FlightService _flightService = new(_flightsFileContext);
    private static readonly TicketService _ticketService = new(_ticketsFileContext);
    private static User? currentUser;
    static async Task Main(string[] args)
    {
        Logger.PrintWelcome();

        string username = Validator.ReadValidString("Enter Username: ").Trim();
        string password = Validator.ReadValidString("Enter Password: ").Trim();

        currentUser = _authService.Login(username, password);
        Console.Clear();

        if (currentUser != null)
        {
            switch (currentUser.Role)
            {
                case UserRole.Manager:
                    await ShowManagerMenu(_flightService, currentUser);
                    break;

                case UserRole.Passenger:
                    await ShowPassengerMenu(_flightService, _ticketService, currentUser);
                    break;
            }
        }
        else
        {
            Logger.PrintMessage("Invalid username or password. Access Denied!", MessageType.Error);
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }


}