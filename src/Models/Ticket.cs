
namespace AirportSystem.Models
{
    public class Ticket
    {
        private string? _ticketId;
        private string? _passenger;
        private Flight? _flight;

        public string? TicketId
        {
            get { return _ticketId; }
            set { _ticketId = value; }
        }

        public string? PassengerUsername
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


        public Ticket(User passenger, Flight flight)
        {
            TicketId = Guid.NewGuid().ToString();
            PassengerUsername = passenger.Username;
            Flight = flight;
        }
    }
}