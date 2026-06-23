using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AirportSystem.Models;

namespace AirportSystem.Data
{
    public class FileContext
    {

        private readonly string _storageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");

        private readonly string _flightsFilePath;

        public FileContext()
        {

            if (!Directory.Exists(_storageDirectory))
            {
                Directory.CreateDirectory(_storageDirectory);
            }

            _flightsFilePath = Path.Combine(_storageDirectory, "flights.json");
        }


        public void SaveFlights(List<Flight> flights)
        {

            var options = new JsonSerializerOptions { WriteIndented = true };

            string jsonString = JsonSerializer.Serialize(flights, options);

            File.WriteAllText(_flightsFilePath, jsonString);
        }


        public List<Flight> LoadFlights()
        {

            if (!File.Exists(_flightsFilePath))
            {
                return new List<Flight>();
            }

            string jsonString = File.ReadAllText(_flightsFilePath);

            List<Flight>? flights = JsonSerializer.Deserialize<List<Flight>>(jsonString);

            return flights ?? new List<Flight>();
        }
    }
}