using System.Text.RegularExpressions;

namespace AirportSystem.Utils
{

    public static class StringHelper
    {
        public static string SplitPascalCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
        }
    }

}