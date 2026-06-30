using System.Globalization;
using AirportSystem.Models;
using AirportSystem.Services;
using AirportSystem.Utils;
using static AirportSystem.Enums.AppEnums;
using static AirportSystem.Constants.AppConstants;
using AirportSystem.Validators;

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
                        await HandleManageBookFlight(ticketService, currentUser);
                        Logger.WaitForAnyKey();
                        break;
                    case "4":
                        Logger.WaitForAnyKey();
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
                    flight.DepartureAirport, flight.ArrivalAirport, flight.DepartureCountry, flight.DestinationCountry, flight.DepartureDate?.ToString(DateFormat));
                Logger.PrintMessage(row, MessageType.Info);
            }
            Console.WriteLine();

            string departureAirport = ConsoleValidator.ReadValidString("Select the Departure Airport: ").Trim();
            string arrivalAirport = ConsoleValidator.ReadValidString("Select the Arrival Airport: ").Trim();
            string departureCountry = ConsoleValidator.ReadValidString("Select the Departure Country: ").Trim();
            string destinationCountry = ConsoleValidator.ReadValidString("Select the Destination Country: ").Trim();
            DateTime departureDate = ConsoleValidator.ReadValidDate("Select the Departure Date: ");
            FlightClass flightClass = ConsoleValidator.ReadValidFlightClass(ClassMessage);

            Flight? selectedFlight = await flightService.SelectAvailableFlight(
                departureAirport, arrivalAirport, departureCountry, destinationCountry, departureDate, flightClass);

            if (selectedFlight != null)
            {
                Console.Clear();
                Logger.PrintMessage("--- Selected Flight Details ---", MessageType.Info);
                Logger.PrintFullFlightHeader();

                string flightRow = string.Format("{0,-12} {1,-10} {2,-18} {3,-15} {4,-20} {5,-20} {6,-15}",
                    selectedFlight.Class, selectedFlight.Price, selectedFlight.DepartureAirport, selectedFlight.ArrivalAirport, selectedFlight.DepartureCountry, selectedFlight.DestinationCountry, selectedFlight.DepartureDate?.ToString(DateFormat));
                Logger.PrintMessage(flightRow, MessageType.Info);

                Ticket ticket = new Ticket(currentUser!, selectedFlight);
                bool success = await ticketService.BookFlight(ticket);
                if (success)
                {
                    Logger.PrintMessage($"Successfully processed tickets.", MessageType.Success);
                    return;
                }
                Logger.PrintMessage("This ticket already exists!", MessageType.Warning);

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
            if (price != null) keys.Add(flight => Math.Abs(flight.Price - price.Value) < 0.01);

            Logger.PrintFullFlightHeader();

            List<Flight> flights = await filghtService.SearchAvailableFlights(keys);

            if (flights.Count == 0)
            {
                Logger.PrintMessage("No ticket is matched", MessageType.Error);
                return;
            }

            foreach (Flight flight in flights)
            {
                string row = string.Format("{0,-12} {1,-10} {2,-18} {3,-15} {4,-20} {5,-20} {6,-15}",
                  flight.Class, flight.Price, flight.DepartureAirport, flight.ArrivalAirport, flight.DepartureCountry, flight.DestinationCountry, flight.DepartureDate?.ToString(DateFormat));
                Logger.PrintMessage(row, MessageType.Info);
            }

        }

        private static async Task HandleManageBookFlight(TicketService ticketService, User currentUser)
        {
            Console.Clear();
            Console.WriteLine("=== Manage Bookings ===");
            Logger.PrintManageFlightMenu();

            string choice = ConsoleValidator.ReadValidString("Select an option: ");

            switch (choice)
            {
                case "1":
                    await HandleCancelBooking(ticketService, currentUser);
                    Logger.WaitForAnyKey();
                    break;

                case "2":
                    // TODO: Modify Booking here
                    Logger.WaitForAnyKey();
                    break;

                case "3":
                    // TODO: View personal bookings here
                    await HandleViewUserBooking(ticketService, currentUser);
                    Logger.WaitForAnyKey();
                    break;

                default:
                    Console.WriteLine("\nInvalid option!");
                    Logger.WaitForAnyKey();
                    break;
            }

        }

        private static async Task HandleCancelBooking(TicketService ticketService, User currentUser)
        {
            Console.WriteLine("===  Cancel Booking ===");

            List<Ticket> tickets = await ticketService.ViewBookingsByUser(currentUser.Username);

            Logger.PrintUserTickets(tickets);


            int index = ConsoleValidator.ReadValidIntRange("Enter the row's number: ", tickets.Count);

            bool success = await ticketService.CancelBooking(index, currentUser.Username);
            if (success)
            {
                Logger.PrintMessage("The booking is canceled successfully!", MessageType.Success);
                return;
            }

            Logger.PrintMessage("Something wrong!", MessageType.Error);
            return;

        }

        private static async Task HandleViewUserBooking(TicketService ticketService, User currentUser)
        {
            Console.WriteLine($"===  View all bookings for the user [{currentUser.Username}] ===");


            List<Ticket> tickets = await ticketService.ViewBookingsByUser(currentUser.Username);


            if (tickets.Count == 0)
            {
                Logger.PrintMessage("No bookings to view", MessageType.Error);
                return;
            }
            Logger.PrintUserTickets(tickets);

        }



        private static Flight ReadRequiredValues()
        {
            double price = ConsoleValidator.ReadValidDouble("Please enter the price: ");
            string departureCountry = ConsoleValidator.ReadValidString("Please enter the Departure Country: ");
            string destinationCountry = ConsoleValidator.ReadValidString("Please enter the Destination Country: ");
            DateTime departureDate = ConsoleValidator.ReadValidDate("Please enter the Departure Date in this format [YYYY-MM-DD]: ");
            string departureAirport = ConsoleValidator.ReadValidString("Please enter the Departure Airport: ");
            string arrivalAirport = ConsoleValidator.ReadValidString("Please enter the Arrival Airport: ");
            FlightClass flightClass = ConsoleValidator.ReadValidFlightClass(ClassMessage);

            Flight flight = new(departureCountry, destinationCountry, departureDate,
                       departureAirport, arrivalAirport, price, flightClass);

            return flight;

        }

        private static Flight ReadOptionalValues()
        {
            double price = ConsoleValidator.ReadValidDouble("Please enter the price: ");
            string departureCountry = ConsoleValidator.ReadValidString("Please enter the Departure Country: ");
            string destinationCountry = ConsoleValidator.ReadValidString("Please enter the Destination Country: ");
            DateTime departureDate = ConsoleValidator.ReadValidDate("Please enter the Departure Date in this format [YYYY-MM-DD]: ");
            string departureAirport = ConsoleValidator.ReadValidString("Please enter the Departure Airport: ");
            string arrivalAirport = ConsoleValidator.ReadValidString("Please enter the Arrival Airport: ");
            FlightClass flightClass = ConsoleValidator.ReadValidFlightClass(ClassMessage);

            Flight flight = new(departureCountry, destinationCountry, departureDate,
                       departureAirport, arrivalAirport, price, flightClass);

            return flight;

        }


    }
}