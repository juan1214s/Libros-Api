namespace LibrosApi.Controllers.Book
{
    using LibrosApi.Services.Book;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using LibrosApi.Exceptions;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class GetBookByIdController : ControllerBase
    {
        private readonly GetBookByIdService _getBookByIdService;
        private readonly ILogger<GetBookByIdController> _logger;

        public GetBookByIdController(GetBookByIdService getBookByIdService, ILogger<GetBookByIdController> logger)
        {
            _getBookByIdService = getBookByIdService ?? throw new ArgumentNullException(nameof(getBookByIdService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id}")] // GET con parámetro en la ruta
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var book = await _getBookByIdService.GetBookByIdAsync(id);
                return Ok(book); // 200 OK con el libro
            }
            catch (ResourceNotFoundException) // Captura la excepción personalizada
            {
                return NotFound(); // 404 Not Found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener libro por ID {Id}.", id);
                return StatusCode(500, new { error = "Error interno del servidor." }); // 500 Internal Server Error
            }
        }
    }
}
