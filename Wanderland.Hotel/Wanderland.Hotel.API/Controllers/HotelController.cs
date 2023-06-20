using Microsoft.AspNetCore.Mvc;
using Wanderland.Hotel.API.Services;

namespace Wanderland.Hotel.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly HotelReservationService _hotelService;

        public HotelController(ILogger<HotelController> logger)
        {
            _hotelService = new HotelReservationService();
            _logger = logger;
        }

        [HttpPost(Name = "Reserve")]
        public IActionResult Reserve(ReserveHotelDto dto)
        {
            _hotelService.Reserve(dto);
            return Ok();
        }
    }
}