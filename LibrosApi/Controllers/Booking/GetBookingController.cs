namespace LibrosApi.Controllers.Booking
{
    using LibrosApi.Services.Booking;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class GetBookingController : ControllerBase
    {
        private readonly GetBookingService _getBookingService;
        private readonly ILogger<GetBookingController> _logger;

        public GetBookingController(GetBookingService getBookingService, ILogger<GetBookingController> logger)
        {
            _getBookingService = getBookingService ?? throw new ArgumentNullException(nameof(getBookingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            try
            {
                var bookings = await _getBookingService.GetBookingsAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los préstamos.");
                return StatusCode(500, new { error = "Error interno del servidor." });
            }
        }
    }
}
