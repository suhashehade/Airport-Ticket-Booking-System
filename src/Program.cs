using System;
using System.Collections.Generic;
using AirportSystem.Data;

Console.WriteLine("=== Testing FileContext ===");

FileContext context = new();

List<Flight> dummyFlights =
        [
            new Flight("Palestine", "Jordan", "2026-07-01", "QAIA", "AMM", Flight.FlightClass.Business),
            new Flight("Palestine", "Egypt", "2026-08-15", "CAI", "HBE", Flight.FlightClass.Economy)
        ];


Console.WriteLine("Saving flights to JSON file...");
context.SaveFlights(dummyFlights);
Console.WriteLine("Save completed successfully!\n");


Console.WriteLine("Loading flights from JSON file...");
List<Flight> loadedFlights = context.LoadFlights();
Console.WriteLine($"Loaded {loadedFlights.Count} flights from storage.\n");


Console.WriteLine("--- Flight Details ---");
foreach (var flight in loadedFlights)
{
    Console.WriteLine($"From: {flight.DepartureCountry} To: {flight.DestinationCountry} | Class: {flight.Class} | Price: ${flight.Price}");
}

Console.WriteLine("\nTesting finished! Press any key to exit.");
Console.ReadKey();
