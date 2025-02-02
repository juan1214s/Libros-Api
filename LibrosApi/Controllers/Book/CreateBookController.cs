using LibrosApi.Dto.BookDto;
using LibrosApi.Exceptions;
using LibrosApi.Services.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrosApi.Controllers.Book
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CreateBookController : ControllerBase
    {
        private readonly CreateBookService _createBookService;
        private readonly ILogger<CreateBookController> _logger;

        public CreateBookController(CreateBookService createBookService, ILogger<CreateBookController> logger)
        {
            _createBookService = createBookService ?? throw new ArgumentNullException(nameof(createBookService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(CreateBookDto bookDto)
        {
            try
            {
                await _createBookService.CreateBookAsync(bookDto);  // No necesitas el bookId si no lo vas a usar

                return Ok(new { message = "Libro creado correctamente." }); // Mensaje consistente
            }
            catch (ResourceAlreadyExistsException ex)
            {
                _logger.LogWarning(ex, "Intento de crear libro con título duplicado: {Titulo}", bookDto.Titulo);
                return Conflict(new { message = ex.Message }); // 409 Conflict
            }
            catch (ForeignKeyViolationException ex)
            {
                _logger.LogWarning(ex, "Error de clave externa al crear libro: {Titulo}", bookDto.Titulo);
                return BadRequest(new { message = ex.Message }); // 400 Bad Request o 422 Unprocessable Entity
                // return UnprocessableEntity(new { message = ex.Message }); // Si usas 422
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear libro: {Titulo}", bookDto.Titulo);
                return StatusCode(500, new { message = "Error interno del servidor." }); // Mensaje genérico
            }
        }
    }
}