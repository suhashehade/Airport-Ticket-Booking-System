using System.Globalization;
using AirportSystem.Models;
using AirportSystem.Services;
using AirportSystem.Utils;
using static AirportSystem.Enums.AppEnums;
using static AirportSystem.Constants.AppConstants;
using AirportSystem.Validators;
using System.Text.Json;

namespace AirportSystem.Menus
{
    public static class PassengerMenu
    {
        public static async Task ShowPassengerMenu(FlightService flightService, TicketService ticketService, User currentUser)
        {
            bool isRunning = true;
            while (isRunning)
            {
                Logger.PrintWelcomeUser(currentUser.Username, currentUser.Role);
                Logger.PrintPassengerMenu();

                string choice = ConsoleValidator.ReadValidString("Select an option: ");

                switch (choice)
                {
                    case "1":
                        await HandleBookFlight(flightService, currentUser, ticketService);
                        Logger.WaitForAnyKey();
                        break;

                    case "2":
                        await HandleSearchAvailableFlights(flightService);
                        Logger.WaitForAnyKey();
                        break;

                    case "3":
                        await HandleManageBookFlight(ticketService, flightService, currentUser);
                        break;
                    case "4":
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

        private static async Task HandleBookFlight(FlightService flightService, User currentUser, TicketService ticketService)
        {
            Console.Clear();
            List<Flight> flights = await flightService.GetUniqueFlights();

            Logger.PrintMessage("Please select one of these flights", MessageType.Info);
            Logger.PrintUniqueFlights(flights);
            Console.WriteLine();

            int flightIndex = ConsoleValidator.ReadValidIntRange("Enter the row's number: ", flights.Count);
            FlightClass flightClass = ConsoleValidator.ReadValidFlightClass(ClassMessage);


            Flight? selectedFlight = await flightService.SelectAvailableFlight(flightIndex, flightClass);

            if (selectedFlight != null)
            {
                // var options = new JsonSerializerOptions { WriteIndented = true };
                // Console.Clear();
                // Console.WriteLine($"This is the selected flight: {JsonSerializer.Serialize(selectedFlight, options)} at index: {flightIndex}");

                // Logger.PrintMessage("--- Selected Flight Details ---", MessageType.Info);

                // Logger.PrintAllFlights([selectedFlight]);

                // Ticket ticket = new(currentUser!, selectedFlight);
                // bool success = await ticketService.BookFlight(ticket);
                // if (success)
                // {
                //     Logger.PrintMessage($"Successfully processed tickets.", MessageType.Success);
                //     return;
                // }
                // Logger.PrintMessage("This ticket already exists!", MessageType.Warning);

            }
            else
            {
                Logger.PrintMessage("No flight matched the selected criteria or class!", MessageType.Error);
            }


        }

        private static async Task HandleSearchAvailableFlights(FlightService filghtService)
        {
            Console.WriteLine("=== Search Available Flights ===");

            List<Func<Flight, bool>> keys = new();

            double? price = ConsoleValidator.ReadOptionalDouble("Please enter the price: ");
            string? departureCountry = ConsoleValidator.ReadOptionalString("Please enter the Departure Country: ");
            string? destinationCountry = ConsoleValidator.ReadOptionalString("Please enter the Destination Country: ");
            DateTime? departureDate = ConsoleValidator.ReadOptionalDate("Please enter the Departure Date in this format [YYYY-MM-DD]: ");
            string? departureAirport = ConsoleValidator.ReadOptionalString("Please enter the Departure Airport: ");
            string? arrivalAirport = ConsoleValidator.ReadOptionalString("Please enter the Arrival Airport: ");
            FlightClass? flightClass = ConsoleValidator.ReadOptionalFlightClass(ClassMessage);

            if (arrivalAirport != null) keys.Add(flight => flight.ArrivalAirport == arrivalAirport);
            if (departureAirport != null) keys.Add(flight => flight.DepartureAirport == departureAirport);
            if (departureCountry != null) keys.Add(flight => flight.DepartureCountry == departureCountry);
            if (destinationCountry != null) keys.Add(flight => flight.DestinationCountry == destinationCountry);
            if (flightClass != null) keys.Add(flight => flight.Class == flightClass);
            if (price != null) keys.Add(flight => flight.Price == price);



            List<Flight> flights = await filghtService.SearchAvailableFlights(keys);

            if (flights.Count == 0)
            {
                Logger.PrintMessage("No ticket is matched", MessageType.Error);
                return;
            }
            Logger.PrintAllFlights(flights);


        }

        private static async Task HandleManageBookFlight(TicketService ticketService, FlightService flightService, User currentUser)
        {
            Console.Clear();
            Console.WriteLine("=== Manage Bookings ===");
            Logger.PrintPassenegerFlightMenu();

            string choice = ConsoleValidator.ReadValidString("Select an option: ");
            bool isRunning = true;
            while (isRunning)
            {
                switch (choice)
                {
                    case "1":
                        await HandleCancelTicket(ticketService, currentUser);
                        Logger.WaitForAnyKey();
                        break;

                    case "2":
                        await HandleModifyTicket(ticketService, flightService, currentUser);
                        Logger.WaitForAnyKey();
                        break;

                    case "3":
                        await HandleViewUserTicket(ticketService, currentUser);
                        Logger.WaitForAnyKey();
                        break;
                    case "4":
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("\nInvalid option!");
                        Logger.WaitForAnyKey();
                        break;
                }
            }

        }

        private static async Task HandleCancelTicket(TicketService ticketService, User currentUser)
        {
            Console.WriteLine("===  Cancel Booking ===");

            List<Ticket> tickets = await ticketService.ViewTicketsByUser(currentUser.Username);

            Logger.PrintUserTickets(tickets);


            int index = ConsoleValidator.ReadValidIntRange("Enter the row's number: ", tickets.Count);

            bool success = await ticketService.CancelTicket(index, currentUser.Username);
            if (!success)
            {
                Logger.PrintMessage("Something wrong!", MessageType.Error);
                return;

            }

            Logger.PrintMessage("The booking is canceled successfully!", MessageType.Success);


        }

        private static async Task HandleModifyTicket(TicketService ticketService, FlightService flightService, User currentUser)
        {
            Console.WriteLine("=== Modify Booking ===");

            List<Ticket> tickets = await ticketService.ViewTicketsByUser(currentUser.Username);
            Logger.PrintUserTickets(tickets);

            int ticketIndex = ConsoleValidator.ReadValidIntRange("Enter the row's number: ", tickets.Count);

            List<Flight> flights = await flightService.GetUniqueFlights();

            Logger.PrintMessage("Please select one of these flights to change your booking", MessageType.Default);
            Logger.PrintUniqueFlights(flights);

            int flightIndex = ConsoleValidator.ReadValidIntRange("Enter the row's number: ", flights.Count);
            FlightClass? flightClass = ConsoleValidator.ReadValidFlightClass(ClassMessage);

            Flight? selectedFlight = await flightService.SelectAvailableFlight(flightIndex, flightClass);

            if (selectedFlight != null)
            {
                Console.Clear();
                Logger.PrintMessage("--- Selected Flight Details ---", MessageType.Info);

                Logger.PrintAllFlights([selectedFlight]);

                bool success = await ticketService.ModifyTicket(ticketIndex,
                             selectedFlight,
                             currentUser.Username);
                if (!success)
                {
                    Logger.PrintMessage("Ticket not found", MessageType.Error);
                    return;
                }

                Logger.PrintMessage("The booking is modified successfully!", MessageType.Success);

            }
            else
            {
                Logger.PrintMessage("No flight matched the selected criteria or class!", MessageType.Error);
            }




        }


        private static async Task HandleViewUserTicket(TicketService ticketService, User currentUser)
        {
            Console.WriteLine($"===  View all bookings for the user [{currentUser.Username}] ===");


            List<Ticket> tickets = await ticketService.ViewTicketsByUser(currentUser.Username);

            if (tickets.Count == 0)
            {
                Logger.PrintMessage("No bookings to view", MessageType.Error);
                return;
            }
            Logger.PrintUserTickets(tickets);

        }


    }
}