using MassTransit;
using Wanderland.Contracts.Commands;
using Wanderland.Contracts.Events;
using Wanderland.Flight.API.Services;
using Wanderland.Flight.Domain;

namespace Wanderland.Flight.API.Consumers;

public class ReserveFlightCommandHandler: IConsumer<ReserveFlightCommand>
{
    private readonly FlightReservationService _flightService;

    public ReserveFlightCommandHandler(FlightReservationService flightService)
    {
        _flightService = flightService;
    }

    public Task Consume(ConsumeContext<ReserveFlightCommand> context)
    {
        try
        {
            _flightService.Reserve(new ReserveFlightDto()
            {
                FlightId = context.Message.FlightId,
                PassengerId = context.Message.CustomerId,
                SeatNumber = context.Message.FlightSeat
            });
            context.Publish(new FlightReservedEvent(context.Message.TourId, DateTime.UtcNow));
        }
        catch (DomainException e)
        {
            context.Publish(new FlightReservationFailedEvent(context.Message.TourId, context.Message.FlightId));
        }
            
        return Task.CompletedTask;
    }
}