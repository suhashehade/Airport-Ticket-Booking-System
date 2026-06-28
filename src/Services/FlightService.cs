using AirportSystem.Models;
using AirportSystem.Data;
using AirportSystem.Shared;
using static AirportSystem.Enums.AppEnums;
using System.Globalization;

namespace AirportSystem.Services
{
    public class FlightService
    {

        private readonly FileContext _fileContext;

        public FlightService(FileContext fileContext)
        {
            _fileContext = fileContext;
        }


        public async Task<bool> ImportFlightsFromCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Logger.PrintMessage($"Error: File not found at path: {filePath}", MessageType.Error);
                return false;
            }

            try
            {

                var lines = await File.ReadAllLinesAsync(filePath);

                if (lines.Length <= 1)
                {
                    Logger.PrintMessage("Warning: The CSV file is empty or only contains headers.", MessageType.Warning);
                    return false;
                }

                List<Flight> existingFlights = await _fileContext.Read<Flight>();

                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];


                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] columns = line.Split(',');

                    if (columns.Length < 6)
                    {
                        Logger.PrintMessage($"Skipping invalid line {i + 1}: {line}", MessageType.Warning);
                        continue;
                    }


                    string departureCountry = columns[0].Trim();
                    string destinationCountry = columns[1].Trim();
                    DateTime departureDate = DateTime.ParseExact(columns[2].Trim(), Constants.DateFormat, CultureInfo.InvariantCulture);
                    string departureAirport = columns[3].Trim();
                    string arrivalAirport = columns[4].Trim();
                    double price = double.Parse(columns[5].Trim());
                    string classString = columns[6].Trim();

                    if (!Enum.TryParse(classString, true, out FlightClass flightClass))
                    {
                        flightClass = FlightClass.Economy;
                    }

                    Flight flight = new(
                        departureCountry,
                        destinationCountry,
                        departureDate,
                        departureAirport,
                        arrivalAirport,
                        price,
                        flightClass
                    );

                    bool isDuplicate = existingFlights.Any(ef =>
                                        ef.DepartureCountry == flight.DepartureCountry &&
                                        ef.DestinationCountry == flight.DestinationCountry &&
                                        ef.DepartureAirport == flight.DepartureAirport &&
                                        ef.ArrivalAirport == flight.ArrivalAirport &&
                                        ef.DepartureDate?.Date == flight.DepartureDate?.Date &&
                                        ef.Price == flight.Price &&
                                        ef.Class == flight.Class
                    );

                    if (!isDuplicate)
                    {
                        existingFlights.Add(flight);
                    }

                }

                await _fileContext.Write(existingFlights);

                Logger.PrintMessage($"Successfully processed {existingFlights.Count} flights.", MessageType.Success);
                return true;
            }
            catch (Exception ex)
            {
                Logger.PrintMessage($"An error occurred while reading the CSV: {ex.Message}", MessageType.Error);
                return false;
            }
        }


        public async Task<List<Flight>> DisplayAvailableFlights()
        {
            List<Flight> existingFlights = await _fileContext.Read<Flight>();

            var flights = existingFlights
                .Where(f => f.IsAvailable)
                .GroupBy(f => new { f.DepartureCountry, f.DestinationCountry, f.DepartureDate })
                .Select(g => g.First())
                .ToList();

            return flights;
        }

        public async Task<List<Flight>> GetUniqueFlights()
        {
            List<Flight> existingFlights = await _fileContext.Read<Flight>();

            var uniqueFlights = existingFlights
                .Where(f => f.IsAvailable)
                .GroupBy(f => new { f.DepartureCountry, f.DestinationCountry, f.DepartureDate, f.ArrivalAirport, f.DepartureAirport })
                .Select(g => g.First())
                .ToList();

            return uniqueFlights;
        }

        public async Task<Flight?> SelectAvailableFlight(
                string departureAirport,
                string arrivalAirport,
                string departureCountry,
                string destinationCountry,
                DateTime departureDate,
                FlightClass selectedClass)
        {
            List<Flight> existingFlights = await _fileContext.Read<Flight>();


            Flight? flight = existingFlights.FirstOrDefault(ef =>
                ef.DepartureCountry == departureCountry &&
                ef.DestinationCountry == destinationCountry &&
                ef.DepartureDate == departureDate &&
                ef.ArrivalAirport == arrivalAirport &&
                ef.DepartureAirport == departureAirport &&
                ef.Class == selectedClass &&
                ef.IsAvailable
            );

            if (flight == null)
            {
                Logger.PrintMessage("No flight matched the selected criteria or class!", MessageType.Error);
            }

            return flight;
        }

    }
}