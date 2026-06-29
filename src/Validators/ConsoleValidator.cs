
using System.Globalization;
using static AirportSystem.Enums.AppEnums;
using AirportSystem.Utils;
using static AirportSystem.Validators.Validator;
using static AirportSystem.Constants.AppConstants;


namespace AirportSystem.Validators
{
    public static class ConsoleValidator
    {
        public static string ReadValidString(string message)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            while (IsEmpty(input))
            {
                Logger.PrintMessage("Invalid input ❌", MessageType.Error);
                input = Console.ReadLine();
            }

            return input!;
        }

        public static double ReadValidDouble(string message)
        {
            Console.Write(message);

            while (true)
            {
                string? input = Console.ReadLine();

                if (IsEmpty(input) || !IsValidDouble(input))
                {
                    Logger.PrintMessage("Invalid number, must be a number > 0 ❌", MessageType.Error);
                    continue;
                }
                double value = double.Parse(input!);
                return value;
            }
        }

        public static FlightClass ReadValidFlightClass(string message)
        {
            Console.Write(message);
            while (true)
            {
                string? input = Console.ReadLine();
                if (IsEmpty(input))
                {
                    Logger.PrintMessage("Invalid input ❌", MessageType.Error);
                    continue;
                }

                if (!IsValidFlightClass(input))
                {
                    Logger.PrintMessage("Invalid class ❌", MessageType.Error);
                    continue;
                }
                Enum.TryParse(input, true, out FlightClass flightClass);
                return flightClass;
            }
        }

        public static DateTime ReadValidDate(string message)
        {
            Console.Write(message);

            while (true)
            {
                string? input = Console.ReadLine();

                if (IsEmpty(input) || !IsValidDate(input!))
                {
                    Logger.PrintMessage("Invalid date, shouldn't be a date earlier than now ❌", MessageType.Error);
                    continue;
                }


                DateTime date = DateTime.ParseExact(input!, DateFormat, CultureInfo.InvariantCulture);
                return date;
            }
        }

        public static string? ReadOptionalString(string message)
        {
            Console.Write(message);
            string? input = Console.ReadLine();
            if (IsEmpty(input))
            {
                return null;
            }

            return input;
        }

        public static double? ReadOptionalDouble(string message)
        {
            Console.Write(message);

            string? input = Console.ReadLine();

            if (IsEmpty(input) || !IsValidDouble(input))
            {
                return null;
            }

            double value = double.Parse(input!);
            return value;

        }

        public static FlightClass? ReadOptionalFlightClass(string message)
        {
            Console.Write(message);

            string? input = Console.ReadLine();
            if (IsEmpty(input) || !IsValidFlightClass(input!))
            {
                return null;
            }

            Enum.TryParse(input, true, out FlightClass flightClass);
            return flightClass;

        }

        public static DateTime? ReadOptionalDate(string message)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (IsEmpty(input) || !IsValidDate(input!))
            {
                return null;
            }

            DateTime date = DateTime.ParseExact(input!, DateFormat, CultureInfo.InvariantCulture);
            return date;

        }


    }
}
