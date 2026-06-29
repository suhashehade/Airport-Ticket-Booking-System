using static AirportSystem.Validators.Validator;

namespace AirportSystem.Validators
{
    static class CsvValidator
    {
        public static List<string> ValidateFlight(string column, string columnName, int index, int lineNumber)
        {
            List<string> errors = new();
            if (IsEmpty(column))
                errors.Add($"Line {lineNumber}: {columnName} is required");
            else
            {
                switch (index)
                {
                    case 2:
                        {
                            if (!IsValidDate(column))
                                errors.Add($"Line {lineNumber}: Invalid date, shouldn't be a date earlier than now ❌");
                            break;
                        }

                    case 5:
                        {
                            if (!IsValidDouble(column))
                                errors.Add($"Line {lineNumber}: Invalid number, must be a number > 0 ❌");
                            break;
                        }
                    case 6:
                        {
                            if (!IsValidFlightClass(column))
                                errors.Add($"Line {lineNumber}: Invalid class ❌");
                            break;
                        }
                }
            }

            return errors;
        }

    }
}