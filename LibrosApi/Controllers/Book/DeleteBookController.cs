namespace LibrosApi.Controllers.Book
{
    using LibrosApi.Services.Book;
    using Microsoft.AspNetCore.Mvc;
    using LibrosApi.Exceptions;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DeleteBookController : ControllerBase
    {
        private readonly DeleteBookService _deleteBookService;
        private readonly ILogger<DeleteBookController> _logger;

        public DeleteBookController(DeleteBookService deleteBookService, ILogger<DeleteBookController> logger)
        {
            _deleteBookService = deleteBookService ?? throw new ArgumentNullException(nameof(deleteBookService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpDelete("{bookId}")] // Usa DELETE y un parámetro en la ruta
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            try
            {
                bool deleted = await _deleteBookService.DeleteBookAsync(bookId);

                if (deleted)
                {
                    return Ok(new { message = "Se elimino correctamente."});
                }
                else
                {
                    throw new ResourceNotFoundException("No se encontro el libro.");
                }
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogWarning(ex, "No se encontró el libro."); // Log como Warning, no como Error
                return NotFound(new { message = ex.Message }); // Retorna 404 Not Found con el mensaje de la excepción
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar libro con ID {Id}.", bookId);
                return StatusCode(500, new { error = "Error interno del servidor." }); // 500 Internal Server Error
            }
        }
    }
}