
using AirportSystem.Utils;
using static AirportSystem.Enums.AppEnums;
using static AirportSystem.Constants.AppConstants;
using System.Globalization;

namespace AirportSystem.Validators
{
    static class Validator
    {
        public static bool IsEmpty(string? prop)
        {
            if (string.IsNullOrWhiteSpace(prop))
            {
                return true;
            }

            return false;
        }

        public static bool IsValidDouble(string? prop)
        {
            if (!double.TryParse(prop, out double value) || value <= 0)
            {
                return false;
            }

            return true;
        }

        public static bool IsValidDate(string prop)

        {
            if (!DateTime.TryParseExact(prop, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) || date < DateTime.Today)
            {
                return false;
            }

            return true;
        }

        public static bool IsValidFlightClass(string? prop)
        {
            if (!Enum.TryParse(prop, true, out FlightClass flightClass))
            {
                return false;
            }

            return true;
        }
    }
}