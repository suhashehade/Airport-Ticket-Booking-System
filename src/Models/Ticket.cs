
namespace AirportSystem.Models
{
    public class Ticket
    {
        private string? _ticketId;
        private Passenger? _passenger;
        private Flight? _flight;

        public string? TicketId
        {
            get { return _ticketId; }
            set { _ticketId = value; }
        }

        public Passenger? Passenger
        {
            get { return _passenger; }
            set { _passenger = value; }
        }

        public Flight? Flight
        {
            get { return _flight; }
            set { _flight = value; }
        }

        public Ticket() { }


        public Ticket(Passenger passenger, Flight flight)
        {
            TicketId = Guid.NewGuid().ToString();
            Passenger = passenger;
            Flight = flight;
        }
    }
}