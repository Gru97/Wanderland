using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wanderland.Contract.Events
{
    public record FlightReservationFailedEvent
    {
        public FlightReservationFailedEvent(Guid tourId, Guid flightId)
        {
            TourId = tourId;
            FlightId = flightId;
        }

        public Guid TourId { get; set; }
        public Guid FlightId { get; set; }
    }
}
