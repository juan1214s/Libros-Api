using Dapper;
using LibrosApi.Dto.AuthorsDto;
using LibrosApi.Exceptions;
using LibrosApi.Services.Authors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LibrosApi.Controllers.Athors
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UpdateAuthorController : ControllerBase
    {
        private readonly UpdateAuthorService _updateAuthorService;
        private readonly ILogger<UpdateAuthorController> _logger;

        public UpdateAuthorController(UpdateAuthorService updateAuthorService, ILogger<UpdateAuthorController> logger)
        {
            _updateAuthorService = updateAuthorService ?? throw new ArgumentNullException(nameof(updateAuthorService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, UpdateAuthorDto updateAuthorDto)
        {
            try
            {
                bool result = await _updateAuthorService.UpdateAuthorAsync(id, updateAuthorDto);

                if (result)
                {
                    return Ok(new { message = "Autor actualizado correctamente." }); // Mensaje consistente
                }
                else
                {
                    return NotFound(new { message = "Autor no encontrado." }); // Mensaje consistente
                }
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogWarning(ex, "No se encontró el autor con ID {Id} que intentas actualizar.", id);
                return NotFound(new { message = ex.Message }); // Mensaje de la excepción (si es seguro exponerlo)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el autor con ID {Id}.", id);
                return StatusCode(500, new { message = "Error interno del servidor." }); // Mensaje genérico
            }
        }

    }
}
