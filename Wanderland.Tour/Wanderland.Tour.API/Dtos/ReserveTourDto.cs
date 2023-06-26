namespace Wanderland.Tour.API.Dtos;

public class ReserveTourDto
{
    public Guid HotelId { get; set; }
    public int RoomNumber { get; set; }
    public Guid ArrivalFlightId { get; set; }
    public int ArrivalFlightSeat { get; set; }
    public Guid DepartureFlightId { get; set; }
    public int DepartureFlightSeat { get; set; }
}