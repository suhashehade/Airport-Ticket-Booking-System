
namespace AirportSystem.Shared
{
    class Validator
    {
        public static string ReadValidString(string message)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(input))
            {
                Logger.PrintMessage("Invalid input ❌", Logger.MessageType.Error);
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
                    Logger.PrintMessage("Invalid number ❌", Logger.MessageType.Error);
                    continue;
                }

                if (value <= 0)
                {
                    Logger.PrintMessage("Must be > 0 ❌", Logger.MessageType.Error);
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
                    Logger.PrintMessage("Invalid number ❌", Logger.MessageType.Error);
                    continue;
                }

                if (value <= 0)
                {
                    Logger.PrintMessage("Must be > 0 ❌", Logger.MessageType.Error);
                    continue;
                }

                return value;
            }
        }

    }
}
