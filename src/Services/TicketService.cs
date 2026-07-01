using AirportSystem.Models;
using AirportSystem.Data;
using static AirportSystem.Enums.AppEnums;
using System.Text.Json;


namespace AirportSystem.Services
{
    public class TicketService
    {

        private readonly FileContext _fileContext;

        public TicketService(FileContext fileContext)
        {
            _fileContext = fileContext;

        }

        public static bool IsExists(List<Ticket> tickets, Ticket ticket)
        {
            return tickets.Any(et => et.TicketId == ticket.TicketId);
        }

        public async Task<bool> BookFlight(Ticket ticket)
        {
            List<Ticket> existingTickets = await _fileContext.Read<Ticket>();

            bool isExists = IsExists(existingTickets, ticket);

            if (!isExists)
            {
                existingTickets.Add(ticket);
                await _fileContext.Write(existingTickets);
                return true;
            }
            return false;
        }


        public async Task<List<Ticket>> FilterTickets(List<Func<Ticket, bool>> filters)
        {
            List<Ticket> existingTickets = await _fileContext.Read<Ticket>();

            var filteredTickets = existingTickets.Where(ticket => filters.All(filter => filter(ticket)));
            return filteredTickets.ToList();
        }


        public async Task<List<Ticket>> ViewTicketsByUser(string username)
        {
            List<Ticket> existingTickets = await _fileContext.Read<Ticket>();
            var result = existingTickets.Where(ex => ex.PassengerUsername == username).ToList();
            return result.ToList();

        }

        public async Task<Ticket?> GetByIndexByUser(int index, string username)
        {
            List<Ticket> existingTickets = await ViewTicketsByUser(username);
            if (index < 0)
            {
                return null;
            }

            Ticket ticket = existingTickets.ElementAt(index);
            return ticket;
        }

        public async Task<bool> CancelTicket(int index, string username)
        {
            List<Ticket> existingTickets = await _fileContext.Read<Ticket>();

            Ticket? ticket = await GetByIndexByUser(index - 1, username);

            if (ticket != null)
            {
                existingTickets.RemoveAll(et => et.TicketId == ticket.TicketId);
                await _fileContext.Write(existingTickets);
                return true;
            }
            return false;

        }

        public async Task<bool> ModifyTicket(int index, string? departureCountry, string? destinationCountry,
                DateTime? departureDate,
                string? departureAirport,
                string? arrivalAirport,
                FlightClass? flightClass,
                string username
            )
        {
            List<Ticket> existingTickets = await _fileContext.Read<Ticket>();

            Ticket? oldTicket = await GetByIndexByUser(index - 1, username);
            if (oldTicket != null)
            {

                Ticket? ticketToModify = existingTickets.FirstOrDefault(et => et.TicketId == oldTicket?.TicketId);
                if (ticketToModify?.Flight != null)
                {
                    if (departureCountry != null) ticketToModify?.Flight?.DepartureCountry = departureCountry;
                    if (destinationCountry != null) ticketToModify?.Flight?.DestinationCountry = destinationCountry;
                    if (departureDate != null) ticketToModify?.Flight?.DepartureDate = departureDate;
                    if (departureAirport != null) ticketToModify?.Flight?.DepartureAirport = departureAirport;
                    if (arrivalAirport != null) ticketToModify?.Flight?.ArrivalAirport = arrivalAirport;
                    if (flightClass != null) ticketToModify?.Flight?.Class = flightClass;

                    await _fileContext.Write(existingTickets);
                    return true;
                }


            }
            return false;

        }


    }
}