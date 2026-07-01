using AirportSystem.Models;
using static AirportSystem.Enums.AppEnums;
using static AirportSystem.Constants.AppConstants;


namespace AirportSystem.Utils
{
    public static class Logger
    {


        public static void PrintMessage(string message, MessageType type)
        {
            Console.ForegroundColor = type switch
            {
                MessageType.Error => ConsoleColor.Red,
                MessageType.Success => ConsoleColor.Green,
                MessageType.Warning => ConsoleColor.Yellow,
                MessageType.Info => ConsoleColor.Cyan,
                _ => ConsoleColor.White
            };

            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintUniqueFlights(List<Flight> flights)
        {
            Console.WriteLine();
            Console.WriteLine("{0,-10} {1,-18} {2,-15} {3,-20} {4,-20} {5,-15}", "#", "Dep Airport", "Arr Airport", "Dep Country", "Dest Country", "Dep Date");
            Console.WriteLine(new string('-', 100));
            int index = 0;
            foreach (Flight flight in flights)
            {
                index++;
                string row = string.Format("{0,-10} {1,-18} {2,-15} {3,-20} {4,-20} {5,-15}",
                 index, flight.DepartureAirport, flight.ArrivalAirport, flight.DepartureCountry, flight.DestinationCountry, flight.DepartureDate?.ToString(DateFormat));
                PrintMessage(row, MessageType.Info);
            }

        }

        public static void PrintAllFlights(List<Flight> flights)
        {
            Console.WriteLine();
            Console.WriteLine("{0,-10} {1,-12} {2,-10} {3,-18} {4,-15} {5,-20} {6,-20} {7,-15}", "#", "Class", "Price", "Dep Airport", "Arr Airport", "Dep Country", "Dest Country", "Dep Date");
            Console.WriteLine(new string('-', 130));
            int index = 0;
            foreach (Flight flight in flights)
            {
                index++;
                string row = string.Format("{0,-10} {1,-12} {2,-10} {3,-18} {4,-15} {5,-20} {6,-20} {7,-15}",
                 index, flight.Class, flight.Price, flight.DepartureAirport, flight.ArrivalAirport, flight.DepartureCountry, flight.DestinationCountry, flight.DepartureDate?.ToString(DateFormat));
                PrintMessage(row, MessageType.Info);
            }
        }



        public static void PrintAllTickets(List<Ticket> tickets)
        {
            Console.WriteLine();
            Console.WriteLine("{0,-10} {1,-20} {2,-10} {3,-12} {4,-18} {5,-15} {6,-20} {7,-20} {8,-15}", "#", "Passenger name", "Price", "Class", "Dep Airport", "Arr Airport", "Dep Country", "Dest Country", "Dep Date");
            Console.WriteLine(new string('-', 150));
            int index = 0;
            foreach (Ticket t in tickets.ToList())
            {
                index++;
                string row = string.Format("{0,-10} {1,-20} {2,-10} {3,-12} {4,-18} {5,-15} {6,-20} {7,-20} {8,-15}",
                   index, t.PassengerUsername, t.Flight!.Price, t.Flight.Class, t.Flight.DepartureAirport, t.Flight.ArrivalAirport, t.Flight.DepartureCountry, t.Flight.DestinationCountry, t.Flight.DepartureDate?.ToString(DateFormat));
                PrintMessage(row, MessageType.Info);
            }

        }

        public static void PrintUserTickets(List<Ticket> tickets)
        {
            Console.WriteLine();
            Console.WriteLine("{0,-10} {1,-10} {2,-12} {3,-18} {4,-15} {5,-20} {6,-20} {7,-15}", "#", "Price", "Class", "Dep Airport", "Arr Airport", "Dep Country", "Dest Country", "Dep Date");
            Console.WriteLine(new string('-', 130));
            int index = 0;
            foreach (Ticket t in tickets.ToList())
            {
                index++;
                string row = string.Format("{0,-10} {1,-10} {2,-12} {3,-18} {4,-15} {5,-20} {6,-20} {7,-15}",
                   index, t.Flight!.Price, t.Flight.Class, t.Flight.DepartureAirport, t.Flight.ArrivalAirport, t.Flight.DepartureCountry, t.Flight.DestinationCountry, t.Flight.DepartureDate?.ToString(DateFormat));
                PrintMessage(row, MessageType.Info);
            }

        }

        public static void WaitForAnyKey()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }


        public static void PrintWelcome()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("    Welcome to Airport Booking System   ");
            Console.WriteLine("========================================");
        }

        public static void PrintWelcomeUser(string name, UserRole role)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Welcome {role}: {name}");
            Console.ResetColor();
        }

        public static void PrintManagerMenu()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("1. View Filtered Booked Tickets");
            Console.WriteLine("2. Import Flights from CSV");
            Console.WriteLine("3. View Flight Model Validation Details");
            Console.WriteLine("4. Logout / Exit");
            Console.WriteLine("----------------------------------------");
        }


        public static void PrintPassengerMenu()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("1. Select & Book Flight");
            Console.WriteLine("2. Search Available Flights");
            Console.WriteLine("3. Manage Bookings");
            Console.WriteLine("4. Exit");
            Console.WriteLine("----------------------------------------");
        }

        public static void PrintPassenegerFlightMenu()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("1. Cancel Booking");
            Console.WriteLine("2. Modify Booking");
            Console.WriteLine("3. View Personal Bookings");
            Console.WriteLine("4. Exit");
            Console.WriteLine("----------------------------------------");
        }
    }
}