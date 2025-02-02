using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using LibrosApi.Services.Categories;
using LibrosApi.Exceptions;

namespace LibrosApi.Controllers.Categories
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetByIdCategoryController : ControllerBase
    {
        private readonly GetByIdCategoryService _getByIdCategoryService;
        private readonly ILogger<GetByIdCategoryController> _logger;

        public GetByIdCategoryController(GetByIdCategoryService getByIdCategoryService, ILogger<GetByIdCategoryController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getByIdCategoryService = getByIdCategoryService ?? throw new ArgumentNullException(nameof(getByIdCategoryService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _getByIdCategoryService.GetCategoryByIdAsync(id);
                return Ok(category); // Retorna la categoría si se encuentra
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogWarning(ex, "No se encontró la categoría con ID {id}.", id); // Log como Warning, no Error
                return NotFound(new { message = ex.Message }); // Retorna 404 Not Found con el mensaje
            }
            catch (Exception ex) // Captura excepciones generales después de las específicas
            {
                _logger.LogError(ex, "Error al obtener la categoría con ID {id}.", id);
                return StatusCode(500, new { message = "Error interno del servidor." }); // Mensaje genérico para el cliente
            }
        }
    }
}
