namespace Wanderland.Contract.Events;

public record FlightReservedEvent
{
    public Guid TourId { get; set; }
    public DateTime ReservedAt { get; set; }

    public FlightReservedEvent(Guid tourId, DateTime reservedAt)
    {
        TourId = tourId;
        ReservedAt = reservedAt;
    }
}