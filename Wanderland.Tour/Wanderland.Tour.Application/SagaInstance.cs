using MassTransit;

namespace Wanderland.Tour.Application;

public class SagaInstance : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public int CurrentState { get; set; }
    public ReservationDetail ReservationDetail { get; set; }
}
public class ReservationDetail
{
    public Guid CustomerId { get; set; }
    public Guid HotelId { get; set; }
    public int RoomNumber { get; set; }
    public Guid FlightId { get; set; }
    public int FlightSeat { get; set; }
    public Guid ReturnFlightId { get; set; }
    public int ReturnFlightSeat { get; set; }
    
    public ReservationDetail(Guid customerId, Guid hotelId, int roomNumber, Guid flightId, int flightSeat, Guid returnFlightId, int returnFlightSeat)
    {
        CustomerId = customerId;
        HotelId = hotelId;
        RoomNumber = roomNumber;
        FlightId = flightId;
        FlightSeat = flightSeat;
        ReturnFlightId = returnFlightId;
        ReturnFlightSeat = returnFlightSeat;
    }
}