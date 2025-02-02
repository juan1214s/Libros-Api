using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using LibrosApi.Services.Categories;
using LibrosApi.Dto.CategoriesDto;
using LibrosApi.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace LibrosApi.Controllers.Categories
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UpdateCategoriesController : ControllerBase
    {
        private readonly UpdateCategoriesService _updateCategoriesService;
        private readonly ILogger<UpdateCategoriesController> _logger;

        public UpdateCategoriesController(UpdateCategoriesService updateCategoriesService, ILogger<UpdateCategoriesController> logger)
        {
            _updateCategoriesService = updateCategoriesService ?? throw new ArgumentNullException(nameof(updateCategoriesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoriDto updateCategoriDto)
        {
            try
            {
                bool result = await _updateCategoriesService.UpdateCategoryAsync(id, updateCategoriDto);

                if (result)
                {
                    return Ok(new { message = "Categoría actualizada con éxito." });
                }
                else
                {
                    throw new ResourceNotFoundException("No se encontró la categoría que intentas actualizar.");
                }
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogWarning(ex, "No se encontró la categoría que intentas actualizar."); // Log como Warning, no como Error
                return NotFound(new { message = ex.Message }); // Retorna 404 Not Found con el mensaje de la excepción
            }
            catch (Exception ex)
            {
                // El error ya fue registrado en el servicio
                return StatusCode(500, new { error = "Error interno al intentar actualizar la categoría." });
            }
        }
    }
}
