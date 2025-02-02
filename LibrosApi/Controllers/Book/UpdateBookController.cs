namespace LibrosApi.Controllers.Book
{
    using LibrosApi.Dto.BookDto;
    using LibrosApi.Exceptions;
    using LibrosApi.Services.Book;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UpdateBookController : ControllerBase
    {
        private readonly UpdateBookService _updateBookService;
        private readonly ILogger<UpdateBookController> _logger;

        public UpdateBookController(UpdateBookService updateBookService, ILogger<UpdateBookController> logger)
        {
            _updateBookService = updateBookService ?? throw new ArgumentNullException(nameof(updateBookService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPut("{id}")] // PUT para actualizaciones, {id} en la ruta
        public async Task<IActionResult> UpdateBook(int id, UpdateBookDto updateBookDto)
        {
            try
            {
                bool updated = await _updateBookService.UpdateBookAsync(id, updateBookDto);

                if (updated)
                {
                    return Ok(new { message = "Se actualizo correctamente."}); // 204 No Content si se actualizó correctamente
                }
                else
                {
                    throw new ResourceNotFoundException("No se encontro el libro que deseas actuaalizar.");
                }
            }
            catch ( ResourceNotFoundException ex)
            {
                _logger.LogWarning(ex, "No se encontró el libro."); // Log como Warning, no como Error
                return NotFound(new { message = ex.Message }); // Retorna 404 Not Found con el mensaje de la excepción

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar libro con ID {Id}.", id);
                return StatusCode(500, new { error = "Error interno del servidor." });
            }
        }
    }
}
