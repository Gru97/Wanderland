namespace Wanderland.Hotel.API.Services;

public class ReserveHotelDto
{
    public Guid HotelId { get; set; }
    public Guid PassengerId { get; set; }
    public int RoomNumber { get; set; }
}