namespace LibrosApi.Controllers.Book // Namespace consistente
{
    using LibrosApi.Services.Book; // Importa el servicio
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class GetBookController : ControllerBase
    {
        private readonly GetBooksService _getBooksService;
        private readonly ILogger<GetBookController> _logger;

        public GetBookController(GetBooksService getBooksService, ILogger<GetBookController> logger)
        {
            _getBooksService = getBooksService ?? throw new ArgumentNullException(nameof(getBooksService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetLibros()
        {
            try
            {
                var libros = await _getBooksService.GetLibrosAsync();
                return Ok(libros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los libros.");
                return StatusCode(500, new { error = "Error interno del servidor." });
            }
        }
    }
}