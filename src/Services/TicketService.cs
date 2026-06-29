using AirportSystem.Models;
using AirportSystem.Data;
using AirportSystem.Utils;
using static AirportSystem.Enums.AppEnums;

namespace AirportSystem.Services
{
    public class TicketService
    {

        private readonly FileContext _fileContext;

        public TicketService(FileContext fileContext)
        {
            _fileContext = fileContext;

        }

        public async Task<bool> BookFlight(Ticket ticket)
        {
            List<Ticket> existingTickets = await _fileContext.Read<Ticket>();

            bool isDuplicate = existingTickets.Any(et =>
                                    et.PassengerUsername == ticket.PassengerUsername &&
                                     et.Flight!.DepartureCountry == ticket.Flight!.DepartureCountry &&
                                        et.Flight!.DestinationCountry == ticket.Flight.DestinationCountry &&
                                        et.Flight!.DepartureAirport == ticket.Flight.DepartureAirport &&
                                        et.Flight!.ArrivalAirport == ticket.Flight.ArrivalAirport &&
                                        et.Flight!.DepartureDate == ticket.Flight.DepartureDate &&
                                        et.Flight!.Price == ticket.Flight.Price &&
                                        et.Flight!.Class == ticket.Flight.Class

                );

            if (!isDuplicate)
            {
                existingTickets.Add(ticket);
                await _fileContext.Write(existingTickets);

                Logger.PrintMessage($"Successfully processed {existingTickets.Count} tickets.", MessageType.Success);
                return true;
            }

            Logger.PrintMessage("This ticket already exists!", MessageType.Warning);
            return false;
        }


        public async Task<List<Ticket>> FilterTickets(List<Func<Ticket, bool>> filters)
        {
            List<Ticket> tickets = await _fileContext.Read<Ticket>();

            var result = tickets.Where(ticket => filters.All(filter => filter(ticket)));
            return result.ToList();
        }



    }
}