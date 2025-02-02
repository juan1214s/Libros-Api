namespace LibrosApi.Controllers.Booking
{
    using LibrosApi.Dto.BookingDto;
    using LibrosApi.Exceptions;
    using LibrosApi.Services.Booking;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CreateBookingController : ControllerBase
    {
        private readonly CreateBookingService _createBookingService;
        private readonly ILogger<CreateBookingController> _logger;

        public CreateBookingController(CreateBookingService createBookingService, ILogger<CreateBookingController> logger)
        {
            _createBookingService = createBookingService ?? throw new ArgumentNullException(nameof(createBookingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost] // POST para crear un nuevo recurso
        public async Task<IActionResult> CreateBooking(CreateBookingDto bookingDto)
        {
            try
            {
                bool created = await _createBookingService.CreateBookingAsync(bookingDto);
                if (created)
                {
                    return Ok(new { mesage = "Se creo correctamente la reserva."});
                }
                else
                {
                    return BadRequest(); // 400 Bad Request (debería ser manejado por excepciones)
                }
            }
            catch (ForeignKeyViolationException ex)
            {
                _logger.LogWarning(ex, "Error al crear el prestamo");
                return BadRequest(new { error = ex.Message }); // 400 Bad Request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el prestamo");
                return StatusCode(500, new { error = "Error interno del servidor." }); // 500 Internal Server Error
            }
        }
    }
}