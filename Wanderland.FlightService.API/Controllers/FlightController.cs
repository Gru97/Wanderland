using Microsoft.AspNetCore.Mvc;
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
}