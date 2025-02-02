using LibrosApi.Exceptions;
using LibrosApi.Services.Authors;
using Microsoft.AspNetCore.Mvc;

namespace LibrosApi.Controllers.Athors
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetAuthorByIdController : ControllerBase // Corregido: Authors, no Athors
    {
        private readonly GetAuthorByIdService _getAuthorByIdService;
        private readonly ILogger<GetAuthorByIdController> _logger;

        public GetAuthorByIdController(GetAuthorByIdService getAuthorByIdService, ILogger<GetAuthorByIdController> logger)
        {
            _getAuthorByIdService = getAuthorByIdService ?? throw new ArgumentNullException(nameof(getAuthorByIdService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id}")] // GET para obtener por ID
        public async Task<IActionResult> GetAuthorById(int id)
        {
            try
            {
                var author = await _getAuthorByIdService.GetAuthorByIdAsync(id);
                return Ok(author); // 200 OK con el autor
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogWarning(ex, "No se encontró el autor con ID {id}.", id);
                return NotFound(new { message = ex.Message }); // 404 Not Found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el autor con ID {id}.", id);
                return StatusCode(500, new { message = "Error interno del servidor." }); // 500 Internal Server Error
            }
        }
    }

}
