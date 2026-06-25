using AirportSystem.Models;
using AirportSystem.Services;
using AirportSystem.Shared;
using static AirportSystem.Enums.AppEnums;


namespace AirportSystem.Menus
{
    public static class ManagerMenus
    {
        public static async Task ShowManagerMenu(FlightService flightService, User currentUser)
        {
            bool isRunning = true;

            while (isRunning)
            {

                Logger.PrintWelcomeUser(currentUser.Username);
                Logger.PrintManagerMenu();

                string choice = Validator.ReadValidString("Select an option: ");

                switch (choice)
                {
                    case "1":
                        Logger.WaitForAnyKey();
                        Console.WriteLine("\nView Booked Tickets");
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("=== Import Flights from CSV ===");
                        string filePath = Validator.ReadValidString("Please enter the full path of the CSV file: ");

                        Console.WriteLine("\nProcessing file, please wait...");
                        bool success = await flightService.ImportFlightsFromCsv(filePath);

                        if (success)
                        {
                            Logger.PrintMessage("Flights database updated successfully!", MessageType.Info);
                        }
                        else
                        {
                            Logger.PrintMessage("Failed to import flights. Please check errors above.", MessageType.Error);
                        }

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        break;

                    case "3":
                        isRunning = false;
                        Console.WriteLine("\nLogging out...");
                        break;

                    default:
                        Console.WriteLine("\nInvalid option!");
                        Logger.WaitForAnyKey();
                        break;
                }
            }
        }
    }
}