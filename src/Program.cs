using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirportSystem.Models;
using AirportSystem.Services;
using AirportSystem.Data;
using AirportSystem.Shared;

class Program
{
    private static readonly FileContext _fileContext = new();
    private static readonly FlightService _flightService = new(_fileContext);
    private static readonly TicketService _ticketService = new(_fileContext);
    private static readonly AuthService _authService = new();
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
                case User.UserRole.Manager:
                    await ShowManagerMenu(currentUser.Username);
                    break;

                case User.UserRole.Passenger:
                    await ShowPassengerMenu(currentUser.Username);
                    break;
            }
        }
        else
        {
            Logger.PrintMessage("Invalid username or password. Access Denied!", Logger.MessageType.Error);
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    #region Manager View
    static async Task ShowManagerMenu(string name)
    {
        bool isRunning = true;
        while (isRunning)
        {

            Logger.PrintWelcomeUser(name);
            string choice = Validator.ReadValidString("Select an option: ");

            switch (choice)
            {
                case "1":
                    Logger.WaitForAnyKey();
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine("=== Import Flights from CSV ===");
                    string filePath = Validator.ReadValidString("Please enter the full path of the CSV file: ");

                    Console.WriteLine("\nProcessing file, please wait...");
                    bool success = await _flightService.ImportFlightsFromCsv(filePath);

                    if (success)
                    {
                        Logger.PrintMessage("Flights database updated successfully!", Logger.MessageType.Info);
                    }
                    else
                    {
                        Logger.PrintMessage("Failed to import flights. Please check errors above.", Logger.MessageType.Error);
                    }

                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    break;

                case "3":
                    isRunning = false;
                    Console.WriteLine("\nLogging out...");
                    break;

                default:
                    Console.WriteLine("\nInvalid option!");
                    Logger.WaitForAnyKey();
                    break;
            }
        }
    }
    #endregion

    #region Passenger View
    static async Task ShowPassengerMenu(string name)
    {
        bool isRunning = true;
        while (isRunning)
        {
            Logger.PrintWelcomeUser(name);
            Logger.PrintPassengerMenu();

            string choice = Validator.ReadValidString("Select an option: ");

            switch (choice)
            {
                case "1":
                    await HandleBookFlight();
                    break;

                case "2":
                    await HandleDisplayFlights();
                    break;

                case "3":
                    isRunning = false;
                    Console.WriteLine("\nExiting Passenger Menu...");
                    break;

                default:
                    Console.WriteLine("\nInvalid option!");
                    Logger.WaitForAnyKey();
                    break;
            }
        }
    }

    private static async Task HandleBookFlight()
    {
        Console.Clear();
        List<Flight> flights = await _flightService.GetUniqueFlights();

        Logger.PrintMessage("Please select one of these flights", Logger.MessageType.Info);
        Logger.PrintUniqueFlightsHeader();

        foreach (Flight flight in flights)
        {
            string row = string.Format("{0,-18} {1,-15} {2,-20} {3,-20} {4,-15}",
                flight.DepartureAirport, flight.ArrivalAirport, flight.DepartureCountry, flight.DestinationCountry, flight.DepartureDate);
            Logger.PrintMessage(row, Logger.MessageType.Info);
        }
        Console.WriteLine();

        string departureAirport = Validator.ReadValidString("Select the Departure Airport: ").Trim();
        string arrivalAirport = Validator.ReadValidString("Select the Arrival Airport: ").Trim();
        string departureCountry = Validator.ReadValidString("Select the Departure Country: ").Trim();
        string destinationCountry = Validator.ReadValidString("Select the Destination Country: ").Trim();
        string departureDate = Validator.ReadValidString("Select the Departure Date: ").Trim();

        string classMessage = @"
Select the Flight Class (Enter number or name): 
[0]: FirstClass
[1]: Business
[2]: Economy
Your Choice: ";

        string classInput = Validator.ReadValidString(classMessage).Trim();

        if (Enum.TryParse<Flight.FlightClass>(classInput, true, out Flight.FlightClass flightClass))
        {
            Flight? selectedFlight = await _flightService.SelectAvailableFlight(
                departureAirport, arrivalAirport, departureCountry, destinationCountry, departureDate, flightClass);

            if (selectedFlight != null)
            {
                Console.Clear();
                Logger.PrintMessage("--- Selected Flight Details ---", Logger.MessageType.Info);
                Logger.PrintFullFlightHeader();

                string flightRow = string.Format("{0,-12} {1,-10} {2,-18} {3,-15} {4,-20} {5,-20} {6,-15}",
                    selectedFlight.Class, selectedFlight.Price, selectedFlight.DepartureAirport, selectedFlight.ArrivalAirport, selectedFlight.DepartureCountry, selectedFlight.DestinationCountry, selectedFlight.DepartureDate);
                Logger.PrintMessage(flightRow, Logger.MessageType.Info);

                Ticket ticket = new Ticket(currentUser!, selectedFlight);
                bool success = await _ticketService.InsertTicket(ticket);

            }
        }
        else
        {
            Logger.PrintMessage("Invalid flight class selection!", Logger.MessageType.Error);
        }

        Logger.WaitForAnyKey();
    }

    private static async Task HandleDisplayFlights()
    {
        Console.Clear();
        List<Flight> availableFlights = await _flightService.DisplayAvailableFlights();

        Logger.PrintFullFlightHeader();

        foreach (Flight flight in availableFlights)
        {
            string row = string.Format("{0,-12} {1,-10} {2,-18} {3,-15} {4,-20} {5,-20} {6,-15}",
               flight.Class, flight.Price, flight.DepartureAirport, flight.ArrivalAirport, flight.DepartureCountry, flight.DestinationCountry, flight.DepartureDate);
            Logger.PrintMessage(row, Logger.MessageType.Info);
        }

        Logger.WaitForAnyKey();
    }
    #endregion


}