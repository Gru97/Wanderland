namespace Wanderland.Contracts.Events
{
    public class FlightReservedEvent  //TODO:
    {
        public Guid TourId { get; set; }
        public DateTime ReservedAt { get; set; }

        public FlightReservedEvent(Guid tourId, DateTime reservedAt)
        {
            TourId = tourId;
            ReservedAt = reservedAt;
        }
    }
    public class FlightReservationFailedEvent
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
