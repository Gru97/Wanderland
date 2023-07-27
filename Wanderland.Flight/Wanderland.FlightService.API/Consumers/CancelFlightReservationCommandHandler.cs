using MassTransit;
using Wanderland.Contract.Commands;
using Wanderland.Flight.API.Services;

namespace Wanderland.Flight.API.Consumers
{
    public class CancelFlightReservationCommandHandler : IConsumer<CancelFlightReservationCommand>
    {
        private readonly FlightReservationService _flightService;

        public CancelFlightReservationCommandHandler(FlightReservationService flightService)
        {
            _flightService = flightService;
        }

        public Task Consume(ConsumeContext<CancelFlightReservationCommand> context)
        {

            _flightService.Cancel(new ReserveFlightDto()
            {
                FlightId = context.Message.FlightId,
                PassengerId = context.Message.CustomerId,
                SeatNumber = context.Message.FlightSeat
            });

            return Task.CompletedTask;
        }
    }
}
