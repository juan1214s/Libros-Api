using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using LibrosApi.Services.Authors; // Namespace de tus servicios
using LibrosApi.Exceptions; // Namespace de tus excepciones

namespace LibrosApi.Controllers.Authors // Corregido: Authors, no Athors
{
    [ApiController]
    [Route("api/[controller]")] // Ruta estándar para controladores API
    public class GetAuthorsController : ControllerBase // Hereda de ControllerBase
    {
        private readonly GetAuthorsService _getAuthorsService;
        private readonly ILogger<GetAuthorsController> _logger;

        public GetAuthorsController(GetAuthorsService getAuthorsService, ILogger<GetAuthorsController> logger)
        {
            _getAuthorsService = getAuthorsService ?? throw new ArgumentNullException(nameof(getAuthorsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet] // GET para obtener todos los autores
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                var authors = await _getAuthorsService.GetAuthorsAsync();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los autores.");
                return StatusCode(500, new { message = "Error interno del servidor." }); // 500 Internal Server Error
            }
        }
    }
}