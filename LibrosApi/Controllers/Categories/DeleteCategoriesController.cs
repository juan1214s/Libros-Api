using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using LibrosApi.Services.Categories;
using LibrosApi.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace LibrosApi.Controllers.Categories
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DeleteCategoriesController : ControllerBase
    {
        private readonly DeleteCategoriesService _deleteCategoriesService;
        private readonly ILogger<DeleteCategoriesController> _logger;

        public DeleteCategoriesController(DeleteCategoriesService deleteCategoriesService, ILogger<DeleteCategoriesController> logger)
        {
            _deleteCategoriesService = deleteCategoriesService ?? throw new ArgumentNullException(nameof(deleteCategoriesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                bool result = await _deleteCategoriesService.DeleteCategoryAsync(categoryId);

                if (result)
                {
                    _logger.LogInformation("Categoría eliminada con éxito.");
                    return Ok(new { message = "Categoría eliminada con éxito." });
                }
                else
                {
                    throw new ResourceNotFoundException("No se encontró la categoría."); // Lanza la excepción directamente
                }
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogWarning(ex, "No se encontró la categoría al intentar eliminar."); // Log como Warning, no como Error
                return NotFound(new { message = ex.Message }); // Retorna 404 Not Found con el mensaje de la excepción
            }
            catch (Exception ex) // Captura excepciones generales después de las específicas
            {
                _logger.LogError(ex, "Error al eliminar la categoría.");
                return StatusCode(500, new { error = "Error interno al intentar eliminar la categoría." });
            }
        }
    }
}
