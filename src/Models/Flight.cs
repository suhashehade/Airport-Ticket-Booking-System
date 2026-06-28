
using static AirportSystem.Enums.AppEnums;

namespace AirportSystem.Models
{
    public class Flight
    {


        private double _price;
        private FlightClass _class;
        public FlightClass Class
        {
            get { return _class; }
            set
            {
                _class = value;

                switch (_class)
                {
                    case FlightClass.Economy: _price = 100; break;
                    case FlightClass.Business: _price = 200; break;
                    case FlightClass.FirstClass: _price = 300; break;
                }
            }
        }
        public double Price
        {
            get { return _price; }
        }
        public string? DepartureCountry { get; set; }
        public string? DestinationCountry { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public bool IsAvailable { get; set; }

        public Flight() { }

        public Flight(string departureCountry, string destinationCountry, DateTime departureDate,
                      string departureAirport, string arrivalAirport, FlightClass flightClass)
        {
            DepartureCountry = departureCountry;
            DestinationCountry = destinationCountry;
            DepartureDate = departureDate;
            DepartureAirport = departureAirport;
            ArrivalAirport = arrivalAirport;
            Class = flightClass;
            IsAvailable = true;
        }
    }
}