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
}
