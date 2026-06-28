
using System.Globalization;
using static AirportSystem.Enums.AppEnums;


namespace AirportSystem.Shared
{
    public static class Validator
    {
        public static string ReadValidString(string message)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(input))
            {
                Logger.PrintMessage("Invalid input ❌", MessageType.Error);
                input = Console.ReadLine();
            }

            return input;
        }

        public static double ReadValidDouble(string message)
        {
            Console.Write(message);

            while (true)
            {
                string? input = Console.ReadLine();

                if (!double.TryParse(input, out double value))
                {
                    Logger.PrintMessage("Invalid number ❌", MessageType.Error);
                    continue;
                }

                if (value <= 0)
                {
                    Logger.PrintMessage("Must be > 0 ❌", MessageType.Error);
                    continue;
                }

                return value;
            }
        }

        public static int ReadValidInt(string message)
        {
            Console.Write(message);

            while (true)
            {
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int value))
                {
                    Logger.PrintMessage("Invalid number ❌", MessageType.Error);
                    continue;
                }

                if (value <= 0)
                {
                    Logger.PrintMessage("Must be > 0 ❌", MessageType.Error);
                    continue;
                }

                return value;
            }
        }

        public static FlightClass ReadValidFlightClass(string message)
        {
            Console.Write(message);
            while (true)
            {
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Logger.PrintMessage("Invalid input ❌", MessageType.Error);
                    continue;
                }

                if (!Enum.TryParse(input, true, out FlightClass flightClass))
                {
                    Logger.PrintMessage("Invalid class ❌", MessageType.Error);
                    continue;
                }

                return flightClass;
            }
        }

        public static DateTime ReadValidDate(string message)
        {
            Console.Write(message);

            while (true)
            {
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Logger.PrintMessage("Invalid date ❌", MessageType.Error);
                    continue;
                }
                DateTime date = DateTime.ParseExact(input, Constants.DateFormat, CultureInfo.InvariantCulture);

                if (date.Equals(null))
                {
                    Logger.PrintMessage("Invalid number ❌", MessageType.Error);
                    continue;
                }

                if (date < new DateTime())
                {
                    Logger.PrintMessage("Not accepted date ❌", MessageType.Error);
                    continue;
                }

                return date;
            }
        }

        public static string? ReadOptionalString(string message)
        {
            Console.Write(message);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            return input;
        }

        public static double? ReadOptionalDouble(string message)
        {
            Console.Write(message);

            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (!double.TryParse(input, out double value))
            {
                Logger.PrintMessage("Invalid number ❌", MessageType.Error);
                return null;

            }

            if (value <= 0)
            {
                Logger.PrintMessage("Must be > 0 ❌", MessageType.Error);
                return null;

            }

            return value;

        }

        public static int? ReadOptionalInt(string message)
        {
            Console.Write(message);

            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (!int.TryParse(input, out int value))
            {
                Logger.PrintMessage("Invalid number ❌", MessageType.Error);
                return null;
            }

            if (value <= 0)
            {
                Logger.PrintMessage("Must be > 0 ❌", MessageType.Error);
                return null;

            }

            return value;

        }

        public static FlightClass? ReadOptionalFlightClass(string message)
        {
            Console.Write(message);

            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (!Enum.TryParse(input, true, out FlightClass flightClass))
            {
                Logger.PrintMessage("Invalid class ❌", MessageType.Error);
                return null;
            }

            return flightClass;

        }

        public static DateTime? ReadOptionalDate(string message)
        {
            Console.Write(message);

            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
            DateTime date = DateTime.ParseExact(input, Constants.DateFormat, CultureInfo.InvariantCulture);

            if (date.Equals(null))
            {
                Logger.PrintMessage("Invalid number ❌", MessageType.Error);
                return null;
            }

            if (date < new DateTime())
            {
                Logger.PrintMessage("Not accepted date ❌", MessageType.Error);
                return null;
            }

            return date;

        }


    }
}
