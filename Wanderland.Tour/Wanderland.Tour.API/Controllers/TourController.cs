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

        public TourController(ILogger<TourController> logger, TourReservationService tourService)
        {
            _logger = logger;
            _tourService = tourService;
        }

        [HttpPost(Name = "Reserve")]
        public async Task<IActionResult> Reserve(ReserveTourDto dto)
        {
            var tourId= await _tourService.Reserve(new ReserveTourCommand()
            {
                HotelId = dto.HotelId,
                ArrivalFlightId = dto.ArrivalFlightId,
                DepartureFlightId = dto.DepartureFlightId,
                ArrivalFlightSeat = dto.ArrivalFlightSeat,
                DepartureFlightSeat = dto.DepartureFlightSeat,
                RoomNumber = dto.RoomNumber,
                CustomerId= Guid.NewGuid()
            });
            
            return Ok(tourId);
        }

        [HttpGet("Tour/{id}/State")]
        public async Task<IActionResult> GetTour(string id)
        {
            var tour= await _tourService.GetState(id);
            return Ok(tour);
        }

    }
}