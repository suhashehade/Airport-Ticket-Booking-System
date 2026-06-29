using AirportSystem.Models;
using AirportSystem.Data;
using AirportSystem.Utils;
using static AirportSystem.Enums.AppEnums;
using static AirportSystem.Constants.AppConstants;
using AirportSystem.Validators;
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

                    List<string> lineErrors =
                    [
                        .. CsvValidator.ValidateFlight(columns.ElementAtOrDefault(0) ?? "", "Departure Country", 0, i + 1),
                        .. CsvValidator.ValidateFlight(columns.ElementAtOrDefault(1) ?? "", "Destination Country", 1, i + 1),
                        .. CsvValidator.ValidateFlight(columns.ElementAtOrDefault(2) ?? "", "Departure Date", 2, i + 1),
                        .. CsvValidator.ValidateFlight(columns.ElementAtOrDefault(3) ?? "", "Departure Airport", 3, i + 1),
                        .. CsvValidator.ValidateFlight(columns.ElementAtOrDefault(4) ?? "", "Arrival Airport", 4, i + 1),
                        .. CsvValidator.ValidateFlight(columns.ElementAtOrDefault(5) ?? "", "Price", 5, i + 1),
                        .. CsvValidator.ValidateFlight(columns.ElementAtOrDefault(6) ?? "", "Class", 6, i + 1),
                    ];


                    if (lineErrors.Count > 0)
                    {
                        lineErrors.ForEach(e => Logger.PrintMessage(e, MessageType.Error));
                        continue;
                    }
                    string departureCountry = columns[0].Trim();
                    string destinationCountry = columns[1].Trim();
                    DateTime departureDate = DateTime.ParseExact(columns[2].Trim(), DateFormat, CultureInfo.InvariantCulture);
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


        public async Task<List<Flight>> SearchAvailableFlights(List<Func<Flight, bool>> filters)
        {
            List<Flight> flights = await _fileContext.Read<Flight>();

            var result = flights.Where(ticket => filters.All(filter => filter(ticket)));
            return result.ToList();
        }


    }
}