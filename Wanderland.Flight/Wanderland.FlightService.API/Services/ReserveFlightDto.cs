namespace Wanderland.Flight.API.Services;

public class ReserveFlightDto
{
    public Guid FlightId { get; set; }
    public Guid PassengerId { get; set; }
    public int SeatNumber { get; set; }
}