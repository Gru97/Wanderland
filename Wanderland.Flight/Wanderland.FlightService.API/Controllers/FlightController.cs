using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Wanderland.Contracts.Commands;
using Wanderland.Contracts.Events;
using Wanderland.Flight.API.Services;

namespace Wanderland.Flight.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly ILogger<FlightController> _logger;
        private readonly FlightReservationService _flightService;

        public FlightController(ILogger<FlightController> logger)
        {
            _flightService = new FlightReservationService();
            _logger = logger;
        }

        [HttpPost(Name = "Reserve")]
        public IActionResult Reserve(ReserveFlightDto dto)
        {
            _flightService.Reserve(dto);
            return Ok();
        }
    }

    public class ReserveFlightCommandHandler: IConsumer<ReserveFlightCommand>
    {
        private readonly FlightReservationService _flightService;

        public ReserveFlightCommandHandler(FlightReservationService flightService)
        {
            _flightService = flightService;
        }

        public Task Consume(ConsumeContext<ReserveFlightCommand> context)
        {
            _flightService.Reserve(new ReserveFlightDto()
            {
                FlightId = context.Message.FlightId,
                PassengerId = context.Message.CustomerId,
                SeatNumber = context.Message.FlightSeat
            });
            context.Publish(new FlightReservedEvent(context.Message.TourId, DateTime.UtcNow));
            return Task.CompletedTask;
        }
    }
}