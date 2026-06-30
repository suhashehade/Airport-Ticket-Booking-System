using static AirportSystem.Enums.AppEnums;


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

        public static void PrintUniqueFlightsHeader()
        {
            Console.WriteLine("{0,-18} {1,-15} {2,-20} {3,-20} {4,-15}", "Dep Airport", "Arr Airport", "Dep Country", "Dest Country", "Dep Date");
            Console.WriteLine(new string('-', 90));
        }

        public static void PrintFullFlightHeader()
        {
            Console.WriteLine("{0,-12} {1,-10} {2,-18} {3,-15} {4,-20} {5,-20} {6,-15}", "Class", "Price", "Dep Airport", "Arr Airport", "Dep Country", "Dest Country", "Dep Date");
            Console.WriteLine(new string('-', 115));
        }

        public static void PrintFullTicketHeader()
        {
            Console.WriteLine("{0,-10} {1,-20} {2,-10} {3,-12} {4,-18} {5,-15} {6,-20} {7,-20} {8,-15}", "#", "Passenger name", "Price", "Class", "Dep Airport", "Arr Airport", "Dep Country", "Dest Country", "Dep Date");
            Console.WriteLine(new string('-', 140));
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
            Console.WriteLine("3. Logout / Exit");
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

        public static void PrintManageFlightMenu()
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