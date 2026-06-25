
namespace AirportSystem.Models
{
    public class Ticket
    {
        public string? TicketId { get; set; }

        public string? PassengerUsername { get; set; }

        public Flight? Flight { get; set; }

        public Ticket() { }


        public Ticket(User passenger, Flight flight)
        {
            TicketId = Guid.NewGuid().ToString();
            PassengerUsername = passenger.Username;
            Flight = flight;
        }
    }
}