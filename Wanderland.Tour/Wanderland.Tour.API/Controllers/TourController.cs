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
                FlightId = dto.ArrivalFlightId,
                ReturnFlightId = dto.DepartureFlightId,
                FlightSeat = dto.ArrivalFlightSeat,
                ReturnFlightSeat = dto.DepartureFlightSeat,
                RoomNumber = dto.RoomNumber,
                CustomerId= Guid.NewGuid()  //TODO:
            });
            
            return Ok(tourId);
        }

        [HttpGet("Tour/{id}/State")]
        public async Task<IActionResult> GetTour(string id, CancellationToken token)
        {
            var tour= await _tourService.GetState(id, token);
            return Ok(tour);
        }

    }
}