using System.Globalization;
using AirportSystem.Models;
using AirportSystem.Services;
using AirportSystem.Shared;
using static AirportSystem.Enums.AppEnums;

namespace AirportSystem.Menus
{
    public static class PassengerMenus
    {
        public static async Task ShowPassengerMenu(FlightService flightService, TicketService ticketService, User currentUser)
        {
            bool isRunning = true;
            while (isRunning)
            {
                Logger.PrintWelcomeUser(currentUser.Username);
                Logger.PrintPassengerMenu();

                string choice = Validator.ReadValidString("Select an option: ");

                switch (choice)
                {
                    case "1":
                        await HandleBookFlight(flightService, currentUser, ticketService);
                        break;

                    case "2":
                        await HandleDisplayFlights(flightService);
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

        private static async Task HandleBookFlight(FlightService flightService, User currentUser, TicketService ticketService)
        {
            Console.Clear();
            List<Flight> flights = await flightService.GetUniqueFlights();

            Logger.PrintMessage("Please select one of these flights", MessageType.Info);
            Logger.PrintUniqueFlightsHeader();

            foreach (Flight flight in flights)
            {
                string row = string.Format("{0,-18} {1,-15} {2,-20} {3,-20} {4,-15}",
                    flight.DepartureAirport, flight.ArrivalAirport, flight.DepartureCountry, flight.DestinationCountry, flight.DepartureDate?.ToString(Constants.DateFormat));
                Logger.PrintMessage(row, MessageType.Info);
            }
            Console.WriteLine();

            string departureAirport = Validator.ReadValidString("Select the Departure Airport: ").Trim();
            string arrivalAirport = Validator.ReadValidString("Select the Arrival Airport: ").Trim();
            string departureCountry = Validator.ReadValidString("Select the Departure Country: ").Trim();
            string destinationCountry = Validator.ReadValidString("Select the Destination Country: ").Trim();
            string departureDate = Validator.ReadValidString("Select the Departure Date: ").Trim();



            FlightClass flightClass = Validator.ReadValidFlightClass(Constants.ClassMessage);


            Flight? selectedFlight = await flightService.SelectAvailableFlight(
                departureAirport, arrivalAirport, departureCountry, destinationCountry, DateTime.ParseExact(departureDate, Constants.DateFormat, CultureInfo.InvariantCulture), flightClass);

            if (selectedFlight != null)
            {
                Console.Clear();
                Logger.PrintMessage("--- Selected Flight Details ---", MessageType.Info);
                Logger.PrintFullFlightHeader();

                string flightRow = string.Format("{0,-12} {1,-10} {2,-18} {3,-15} {4,-20} {5,-20} {6,-15}",
                    selectedFlight.Class, selectedFlight.Price, selectedFlight.DepartureAirport, selectedFlight.ArrivalAirport, selectedFlight.DepartureCountry, selectedFlight.DestinationCountry, selectedFlight.DepartureDate?.ToString(Constants.DateFormat));
                Logger.PrintMessage(flightRow, MessageType.Info);

                Ticket ticket = new Ticket(currentUser!, selectedFlight);
                bool success = await ticketService.InsertTicket(ticket);

            }

            else
            {
                Logger.PrintMessage("Invalid flight class selection!", MessageType.Error);
            }

            Logger.WaitForAnyKey();
        }

        private static async Task HandleDisplayFlights(FlightService flightService)
        {
            Console.Clear();
            List<Flight> availableFlights = await flightService.DisplayAvailableFlights();

            Logger.PrintFullFlightHeader();

            foreach (Flight flight in availableFlights)
            {
                string row = string.Format("{0,-12} {1,-10} {2,-18} {3,-15} {4,-20} {5,-20} {6,-15}",
                   flight.Class, flight.Price, flight.DepartureAirport, flight.ArrivalAirport, flight.DepartureCountry, flight.DestinationCountry, flight.DepartureDate?.ToString(Constants.DateFormat));
                Logger.PrintMessage(row, MessageType.Info);
            }

            Logger.WaitForAnyKey();
        }
    }
}