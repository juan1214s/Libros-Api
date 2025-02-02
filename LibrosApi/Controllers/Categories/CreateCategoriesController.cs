using LibrosApi.Dto.CategoriesDto;
using LibrosApi.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LibrosApi.Controllers.Categories
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CreateCategoriesController : ControllerBase
    {
        private readonly CreateCategoriesService _createCategoriesService;
        private readonly ILogger<CreateCategoriesController> _logger;

        public CreateCategoriesController(CreateCategoriesService createCategoriesService, ILogger<CreateCategoriesController> logger)
        {
            _createCategoriesService = createCategoriesService ?? throw new ArgumentNullException(nameof(createCategoriesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoriesDto categoryDto)
        {
            if (categoryDto == null || string.IsNullOrWhiteSpace(categoryDto.Nombre))
            {
                return BadRequest("El nombre de la categoría es obligatorio.");
            }

            try
            {
                var result = await _createCategoriesService.CreateCategoryAsync(categoryDto);

                if (result)
                {
                    _logger.LogInformation("Categoria creada con éxito.");
                    return StatusCode(201, new { message = "Categoria creada con éxito." });
                }
                else
                {
                    _logger.LogError("Error al crear la categoria.");
                    return BadRequest(new { error = "Error al crear la categoria." });
                }
            }
            catch (ResourceAlreadyExistsException ex)
            {
                _logger.LogWarning("Error al crear la categoria.");
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno al intentar crear una categoria.");
                return StatusCode(500, new { error = "Error interno al intentar crear una categoria." });

            }
        }
    }
}
