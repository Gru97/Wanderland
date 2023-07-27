namespace Wanderland.Contracts.Events;

public class ReservationSubmittedEvent
{
    public Guid TourId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid HotelId { get; set; }
    public int RoomNumber { get; set; }
    public Guid FlightId { get; set; }
    public int FlightSeat { get; set; }
    public Guid ReturnFlightId { get; set; }
    public int ReturnFlightSeat { get; set; }
}
public class FlightReservedEvent
{
    public Guid TourId { get; set; }
    public DateTime ReservedAt { get; set; }
}
public class FlightReservationFailedEvent
{
    public Guid TourId { get; set; }
    public Guid FlightId { get; set; }
}
