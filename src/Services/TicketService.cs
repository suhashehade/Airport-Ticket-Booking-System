using System;
using System.Collections.Generic;
using System.Linq;
using AirportSystem.Models;
using AirportSystem.Data;

namespace AirportSystem.Services
{
    public class TicketService
    {

        private FileContext _fileContext;

        public List<Ticket>? Tickets;

        public TicketService(FileContext fileContext)
        {
            _fileContext = fileContext;

        }

        public async Task<bool> InsertTicket(Ticket ticket)
        {
            List<Ticket> existingTickets = await _fileContext.LoadTickets();

            bool isDuplicate = existingTickets.Any(et =>
                                    et.TicketId == ticket.TicketId &&
                                    et.PassengerUsername == ticket.PassengerUsername &&
                                     et.Flight!.DepartureCountry == ticket.Flight!.DepartureCountry &&
                                        et.Flight!.DestinationCountry == ticket.Flight.DestinationCountry &&
                                        et.Flight!.DepartureAirport == ticket.Flight.DepartureAirport &&
                                        et.Flight!.ArrivalAirport == ticket.Flight.ArrivalAirport &&
                                        et.Flight!.DepartureDate == ticket.Flight.DepartureDate &&
                                        et.Flight!.Price == ticket.Flight.Price &&
                                        et.Flight!.Class == ticket.Flight.Class

                );

            if (!isDuplicate)
            {
                existingTickets.Add(ticket);
            }

            _fileContext.SaveTickets(existingTickets);

            Logger.PrintMessage($"Successfully processed {existingTickets.Count} tickets.", Logger.MessageType.Success);
            return true;
        }



        public async Task<bool> ImportFlightsFromCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Logger.PrintMessage($"Error: File not found at path: {filePath}", Logger.MessageType.Error);
                return false;
            }

            try
            {

                var lines = await File.ReadAllLinesAsync(filePath);

                if (lines.Length <= 1)
                {
                    Logger.PrintMessage("Warning: The CSV file is empty or only contains headers.", Logger.MessageType.Warning);
                    return false;
                }

                List<Flight> existingFlights = await _fileContext.LoadFlights();

                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];


                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] columns = line.Split(',');

                    if (columns.Length < 6)
                    {
                        Logger.PrintMessage($"Skipping invalid line {i + 1}: {line}", Logger.MessageType.Warning);
                        continue;
                    }


                    string departureCountry = columns[0].Trim();
                    string destinationCountry = columns[1].Trim();
                    string departureDate = columns[2].Trim();
                    string departureAirport = columns[3].Trim();
                    string arrivalAirport = columns[4].Trim();
                    string classString = columns[5].Trim();


                    if (!Enum.TryParse(classString, true, out Flight.FlightClass flightClass))
                    {

                        flightClass = Flight.FlightClass.Economy;
                    }


                    Flight flight = new(
                        departureCountry,
                        destinationCountry,
                        departureDate,
                        departureAirport,
                        arrivalAirport,
                        flightClass
                    );

                    bool isDuplicate = existingFlights.Any(ef =>
                                        ef.DepartureCountry == flight.DepartureCountry &&
                                        ef.DestinationCountry == flight.DestinationCountry &&
                                        ef.DepartureAirport == flight.DepartureAirport &&
                                        ef.ArrivalAirport == flight.ArrivalAirport &&
                                        ef.DepartureDate == flight.DepartureDate &&
                                        ef.Price == flight.Price &&
                                        ef.Class == flight.Class
                    );

                    if (!isDuplicate)
                    {
                        existingFlights.Add(flight);
                    }

                }

                _fileContext.SaveFlights(existingFlights);

                Logger.PrintMessage($"Successfully processed {existingFlights.Count} flights.", Logger.MessageType.Success);
                return true;
            }
            catch (Exception ex)
            {
                Logger.PrintMessage($"An error occurred while reading the CSV: {ex.Message}", Logger.MessageType.Error);
                return false;
            }
        }


        public async Task<List<Flight>> DisplayAvailableFlights()
        {
            List<Flight> existingFlights = await _fileContext.LoadFlights();

            var flights = existingFlights
                .Where(f => f.IsAvailable)
                .GroupBy(f => new { f.DepartureCountry, f.DestinationCountry, f.DepartureDate })
                .Select(g => g.First())
                .ToList();

            return flights;
        }

        public async Task<List<Flight>> GetUniqueFlights()
        {
            List<Flight> existingFlights = await _fileContext.LoadFlights();

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
                string departureDate,
                Flight.FlightClass selectedClass)
        {
            List<Flight> existingFlights = await _fileContext.LoadFlights();


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
                Logger.PrintMessage("No flight matched the selected criteria or class!", Logger.MessageType.Error);
            }

            return flight;
        }

    }
}