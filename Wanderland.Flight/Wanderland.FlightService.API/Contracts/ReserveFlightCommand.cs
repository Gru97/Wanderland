namespace Wanderland.Contracts.Commands;

public class ReserveFlightCommand
{
    public Guid CustomerId { get; set; }
    public Guid TourId { get; set; }
    public Guid FlightId { get; set; }
    public int FlightSeat { get; set; }

    public ReserveFlightCommand(Guid customerId, Guid tourId, Guid flightId, int flightSeat)
    {
        CustomerId = customerId;
        TourId = tourId;
        FlightId = flightId;
        FlightSeat = flightSeat;
    }
}
public class CancelFlightReservationCommand
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