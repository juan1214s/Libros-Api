using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using LibrosApi.Services.Categories;
using Microsoft.Extensions.Logging;

namespace LibrosApi.Controllers.Categories
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetCategoriesController : ControllerBase
    {
        private readonly GetCategoriesService _getCategoriesService;
        private readonly ILogger<GetCategoriesController> _logger;

        public GetCategoriesController(GetCategoriesService getCategoriesService, ILogger<GetCategoriesController> logger)
        {
            _getCategoriesService = getCategoriesService ?? throw new ArgumentNullException(nameof(getCategoriesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _getCategoriesService.ObtenerCategoriasAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las categorías.");
                return StatusCode(500, new { error = "Error interno al intentar obtener las categorías." });
            }
        }
    }
}
