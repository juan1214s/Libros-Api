using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using LibrosApi.Services.Authors; // Namespace de tus servicios
using LibrosApi.Dto.AuthorsDto; // Namespace de tus DTOs
using LibrosApi.Exceptions;
using Microsoft.AspNetCore.Authorization; // Namespace de tus excepciones

namespace LibrosApi.Controllers.Authors // Corregido: Authors, no Athors
{
    [ApiController]
    [Route("api/[controller]")] // Ruta estándar para controladores API
    [Authorize]
    public class CreateAuthorsController : ControllerBase // Hereda de ControllerBase
    {
        private readonly CreateAuthorService _createAuthorService;
        private readonly ILogger<CreateAuthorsController> _logger;

        public CreateAuthorsController(CreateAuthorService createAuthorService, ILogger<CreateAuthorsController> logger)
        {
            _createAuthorService = createAuthorService ?? throw new ArgumentNullException(nameof(createAuthorService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor(CreateAthorsDto authorDto)
        {
            try
            {
                bool result = await _createAuthorService.CreateAuthorAsync(authorDto);

                if (result)
                {
                    return Ok(new { message = "Autor creado con éxito." });
                }
                else
                {
                    return BadRequest(new { message = "No se pudo crear el autor." }); // 400 Bad Request
                }
            }
            catch (ResourceAlreadyExistsException ex)
            {
                _logger.LogWarning(ex, "Intento de crear un autor duplicado: {Nombre}", authorDto.Nombre);
                return Conflict(new { message = ex.Message }); // 409 Conflict
            }
            catch (Exception ex) // Excepciones generales
            {
                _logger.LogError(ex, "Error al crear el autor: {Nombre}", authorDto.Nombre);
                return StatusCode(500, new { message = "Error interno del servidor." }); // 500 Internal Server Error
            }
        }
    }
}