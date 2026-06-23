using System;
using System.Collections.Generic;
using System.Linq;
using AirportSystem.Models;
using AirportSystem.Data;

namespace AirportSystem.Services
{
    public class FlightService
    {

        private FileContext _fileContext;
        public FlightService(FileContext fileContext)
        {
            _fileContext = fileContext;

        }


        public bool ImportFlightsFromCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: File not found at path: {filePath}");
                Console.ResetColor();
                return false;
            }

            try
            {

                var lines = File.ReadAllLines(filePath);

                if (lines.Length <= 1)
                {
                    Console.WriteLine("Warning: The CSV file is empty or only contains headers.");
                    return false;
                }

                List<Flight> importedFlights = new List<Flight>();


                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];


                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] columns = line.Split(',');

                    if (columns.Length < 6)
                    {
                        Console.WriteLine($"Skipping invalid line {i + 1}: {line}");
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

                    importedFlights.Add(flight);
                }

                List<Flight> existingFlights = _fileContext.LoadFlights();


                existingFlights.AddRange(importedFlights);


                _fileContext.SaveFlights(existingFlights);

                Console.WriteLine($"Successfully processed {importedFlights.Count} flights.");
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while reading the CSV: {ex.Message}");
                Console.ResetColor();
                return false;
            }
        }

    }
}