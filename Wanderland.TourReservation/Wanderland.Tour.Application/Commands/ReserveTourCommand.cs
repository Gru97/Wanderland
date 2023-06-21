using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wanderland.Tour.Application.Commands
{
    public class ReserveTourCommand
    {
        public Guid CustomerId { get; set; }
        public Guid HotelId { get; set; }
        public int RoomNumber { get; set; }
        public Guid ArrivalFlightId { get; set; }
        public int ArrivalFlightSeat { get; set; }
        public Guid DepartureFlightId { get; set; }
        public int DepartureFlightSeat { get; set; }
    }
}
