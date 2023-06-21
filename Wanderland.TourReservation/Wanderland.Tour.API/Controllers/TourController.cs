using Microsoft.AspNetCore.Mvc;
using Wanderland.Tour.API.Dtos;
using Wanderland.Tour.Application;
using Wanderland.Tour.Application.Commands;

namespace Wanderland.Tour.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TourController : ControllerBase
    {
        private readonly ILogger<TourController> _logger;
        private readonly TourReservationService _tourService;

        public TourController(ILogger<TourController> logger)
        {
            _tourService = new TourReservationService();
            _logger = logger;
        }

        [HttpPost(Name = "Reserve")]
        public IActionResult Reserve(ReserveTourDto dto)
        {
            _tourService.Reserve(new ReserveTourCommand()
            {
                HotelId = dto.HotelId,
                ArrivalFlightId = dto.ArrivalFlightId,
                DepartureFlightId = dto.DepartureFlightId,
                ArrivalFlightSeat = dto.ArrivalFlightSeat,
                DepartureFlightSeat = dto.DepartureFlightSeat,
                RoomNumber = dto.RoomNumber,
                CustomerId= Guid.NewGuid()
            });
            return Ok();
        }
    }
}