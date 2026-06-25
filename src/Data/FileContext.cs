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

        private readonly string _ticketsFilePath;

        public FileContext()
        {

            if (!Directory.Exists(_storageDirectory))
            {
                Directory.CreateDirectory(_storageDirectory);
            }

            _flightsFilePath = Path.Combine(_storageDirectory, "flights.json");
            _ticketsFilePath = Path.Combine(_storageDirectory, "tickets.json");
        }


        public async void SaveFlights(List<Flight> flights)
        {

            var options = new JsonSerializerOptions { WriteIndented = true };

            string jsonString = JsonSerializer.Serialize(flights, options);

            await File.WriteAllTextAsync(_flightsFilePath, jsonString);
        }


        public async Task<List<Flight>> LoadFlights()
        {

            if (!File.Exists(_flightsFilePath))
            {
                return new List<Flight>();
            }

            string jsonString = await File.ReadAllTextAsync(_flightsFilePath);

            List<Flight>? flights = JsonSerializer.Deserialize<List<Flight>>(jsonString);

            return flights ?? new List<Flight>();
        }


        public async void SaveTickets(List<Ticket> tickets)
        {

            var options = new JsonSerializerOptions { WriteIndented = true };

            string jsonString = JsonSerializer.Serialize(tickets, options);

            await File.WriteAllTextAsync(_ticketsFilePath, jsonString);
        }


        public async Task<List<Ticket>> LoadTickets()
        {

            if (!File.Exists(_ticketsFilePath))
            {
                return new List<Ticket>();
            }

            string jsonString = await File.ReadAllTextAsync(_ticketsFilePath);

            List<Ticket>? tickets = JsonSerializer.Deserialize<List<Ticket>>(jsonString);

            return tickets ?? new List<Ticket>();
        }
    }
}