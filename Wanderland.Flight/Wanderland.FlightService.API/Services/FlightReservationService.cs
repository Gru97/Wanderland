using Wanderland.Flight.Domain;

namespace Wanderland.Flight.API.Services
{
    public class FlightReservationService
    {
        private static IEnumerable<Domain.Flight> _flights= new List<Domain.Flight>()
        {
            new Domain.Flight(new City(1), new City(2),50)
        };

        public void Reserve(ReserveFlightDto dto)
        {
            //var flight= _flights.SingleOrDefault(e=>e.Id==dto.FlightId);
            var flight= _flights.FirstOrDefault();
            if (flight == null)
                throw new ApplicationException("Flight Can't be found.");

            flight.ReserveTicket(new Passenger(dto.PassengerId), dto.SeatNumber);
        }
    }
}
