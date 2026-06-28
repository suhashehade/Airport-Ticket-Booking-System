
using static AirportSystem.Enums.AppEnums;

namespace AirportSystem.Models
{
    public class Flight
    {
        public FlightClass Class { get; set; }
        public double Price { get; set; }
        public string? DepartureCountry { get; set; }
        public string? DestinationCountry { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public bool IsAvailable { get; set; }

        public Flight() { }

        public Flight(string departureCountry, string destinationCountry, DateTime departureDate,
                      string departureAirport, string arrivalAirport, double price, FlightClass flightClass)
        {
            DepartureCountry = departureCountry;
            DestinationCountry = destinationCountry;
            DepartureDate = departureDate;
            DepartureAirport = departureAirport;
            ArrivalAirport = arrivalAirport;
            Class = flightClass;
            IsAvailable = true;
            switch (flightClass)
            {
                case FlightClass.Economy: Price = price; break;
                case FlightClass.Business: Price = price * 2; break;
                case FlightClass.FirstClass: Price = price * 3; break;
            }
        }
    }
}