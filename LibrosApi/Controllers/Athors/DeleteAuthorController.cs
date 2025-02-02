using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using LibrosApi.Services.Authors; // Namespace de tus servicios
using LibrosApi.Exceptions;
using Microsoft.AspNetCore.Authorization; // Namespace de tus excepciones

namespace LibrosApi.Controllers.Authors // Corregido: Authors, no Athors
{
    [ApiController]
    [Route("api/[controller]")] // Ruta estándar para controladores API
    [Authorize]
    public class DeleteAuthorController : ControllerBase // Hereda de ControllerBase
    {
        private readonly DeleteAuthorService _deleteAuthorService;
        private readonly ILogger<DeleteAuthorController> _logger;

        public DeleteAuthorController(DeleteAuthorService deleteAuthorService, ILogger<DeleteAuthorController> logger)
        {
            _deleteAuthorService = deleteAuthorService ?? throw new ArgumentNullException(nameof(deleteAuthorService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpDelete("{id}")] // DELETE para eliminar, recibe el ID en la URL
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                bool result = await _deleteAuthorService.DeleteAuthorAsync(id);

                if (result)
                {
                    return Ok(new { message = "Se elimino con exito el autor." });
                }
                else
                {
                    return NotFound(new { message = "Autor no encontrado." }); // 404 Not Found
                }
            }
            catch (Exception ex) // Captura excepciones (puedes agregar excepciones más específicas)
            {
                _logger.LogError(ex, "Error al eliminar autor con ID {Id}.", id);
                return StatusCode(500, new { message = "Error interno del servidor." }); // 500 Internal Server Error
            }
        }
    }
}