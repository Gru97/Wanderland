using Wanderland.Hotel.Domain;

namespace Wanderland.Hotel.API.Services
{
    public class HotelReservationService
    {
        private static IEnumerable<Hotel.Domain.Hotel> _Hotels= new List<Hotel.Domain.Hotel>()
        {
            new Hotel.Domain.Hotel(new City(1), new City(2),50)
        };

        public void Reserve(ReserveHotelDto dto)
        {
            //var Hotel= _Hotels.SingleOrDefault(e=>e.Id==dto.HotelId);
            var Hotel= _Hotels.FirstOrDefault();
            if (Hotel == null)
                throw new ApplicationException("Hotel Can't be found.");

            Hotel.ReserveTicket(new Passenger(dto.PassengerId), dto.SeatNumber);
        }
    }
}
