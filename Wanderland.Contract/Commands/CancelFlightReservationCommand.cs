namespace Wanderland.Contract.Commands;

public record CancelFlightReservationCommand
{
    public Guid CustomerId { get; set; }
    public Guid TourId { get; set; }
    public Guid FlightId { get; set; }
    public int FlightSeat { get; set; }

    public CancelFlightReservationCommand(Guid customerId, Guid tourId, Guid flightId, int flightSeat)
    {
        CustomerId = customerId;
        TourId = tourId;
        FlightId = flightId;
        FlightSeat = flightSeat;
    }
}