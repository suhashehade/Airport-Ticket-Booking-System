using AirportSystem.Models;
using static AirportSystem.Utils.StringHelper;

namespace AirportSystem.Validators
{
    public static class FlightModelInfo
    {
        public static void PrintModelInfo()
        {
            var properties = typeof(Flight).GetProperties();

            foreach (var prop in properties)
            {
                Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (prop.Name == "IsAvailable") continue;
                Console.WriteLine($"{SplitPascalCase(prop.Name)}:* ");
                string typeName = propType.Name switch
                {
                    "String" => $"  - Type: Free Text",
                    "DateTime" => $"  - Type: Date Time",
                    "Double" => $"  - Type: Number",
                    _ when propType.IsEnum => "  - Type: Enum",
                    _ => propType.Name
                };
                Console.WriteLine(typeName);
                var attributes = prop.GetCustomAttributes(true);
                string constraints = "";
                foreach (var attr in attributes)
                {
                    string constraint = attr.GetType().Name switch
                    {
                        "RequiredAttribute" => $"Required",
                        "PositiveAttribute" => $"Must be positive",
                        "FutureDateAttribute" => $"Allowed Range (today → future)",
                        "ValidEnumAttribute" => "Must be a FlightClass",
                        _ => ""
                    };
                    constraints += constraints.Length != 0 ? $", {constraint}" : constraint;


                }
                Console.WriteLine($"  - Constraint: {constraints}");
            }
        }
    }
}