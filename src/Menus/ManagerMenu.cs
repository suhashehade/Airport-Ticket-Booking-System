using AirportSystem.Models;
using AirportSystem.Services;
using AirportSystem.Utils;
using AirportSystem.Validators;
using static AirportSystem.Enums.AppEnums;
using static AirportSystem.Constants.AppConstants;


namespace AirportSystem.Menus
{
    public static class ManagerMenu
    {
        public static async Task ShowManagerMenu(FlightService flightService, TicketService ticketService, User currentUser)
        {
            bool isRunning = true;

            while (isRunning)
            {

                Logger.PrintWelcomeUser(currentUser.Username, currentUser.Role);
                Logger.PrintManagerMenu();

                string choice = ConsoleValidator.ReadValidString("Select an option: ");

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        await HandleFilterTickets(ticketService);
                        Console.WriteLine("\nProcessing file, please wait...");
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        await HandleReadFromCSV(flightService);
                        Console.WriteLine("\nProcessing file, please wait...");
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

        private static async Task HandleReadFromCSV(FlightService flightService)
        {
            Console.WriteLine("=== Import Flights from CSV ===");
            string filePath = ConsoleValidator.ReadValidString("Please enter the full path of the CSV file: ");

            Console.WriteLine("\nProcessing file, please wait...");
            bool success = await flightService.ImportFlightsFromCsv(filePath);

            if (success)
            {
                Logger.PrintMessage("Flights database updated successfully!", MessageType.Info);
            }
            else
            {
                Logger.PrintMessage("Failed to import flights. Please check errors above.", MessageType.Error);
            }

        }


        private static async Task HandleFilterTickets(TicketService ticketService)
        {
            Console.WriteLine("=== Filter Tickets ===");

            List<Func<Ticket, bool>> filters = new();

            string? passengerName = ConsoleValidator.ReadOptionalString("Please enter the passenger name: ");
            double? price = ConsoleValidator.ReadOptionalDouble("Please enter the price: ");
            string? departureCountry = ConsoleValidator.ReadOptionalString("Please enter the Departure Country: ");
            string? destinationCountry = ConsoleValidator.ReadOptionalString("Please enter the Destination Country: ");
            DateTime? departureDate = ConsoleValidator.ReadOptionalDate("Please enter the Departure Date in this format [YYYY-MM-DD]: ");
            string? departureAirport = ConsoleValidator.ReadOptionalString("Please enter the Departure Airport: ");
            string? arrivalAirport = ConsoleValidator.ReadOptionalString("Please enter the Arrival Airport: ");
            FlightClass? flightClass = ConsoleValidator.ReadOptionalFlightClass(ClassMessage);

            if (passengerName != null) filters.Add(ticket => ticket.PassengerUsername == passengerName);
            if (arrivalAirport != null) filters.Add(ticket => ticket.Flight!.ArrivalAirport == arrivalAirport);
            if (departureAirport != null) filters.Add(ticket => ticket.Flight!.DepartureAirport == departureAirport);
            if (departureCountry != null) filters.Add(ticket => ticket.Flight!.DepartureCountry == departureCountry);
            if (destinationCountry != null) filters.Add(ticket => ticket.Flight!.DestinationCountry == destinationCountry);
            if (flightClass != null) filters.Add(ticket => ticket.Flight!.Class == flightClass);
            if (price != null) filters.Add(ticket => ticket.Flight!.Price == price);



            List<Ticket> filteredTickets = await ticketService.FilterTickets(filters);

            if (filteredTickets.Count == 0)
            {
                Logger.PrintMessage("No ticket is matched", MessageType.Error);
                return;
            }

            Logger.PrintAllTickets(filteredTickets);

        }
    }
}