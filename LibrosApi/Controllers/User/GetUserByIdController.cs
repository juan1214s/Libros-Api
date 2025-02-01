using LibrosApi.Dto;
using LibrosApi.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace LibrosApi.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetUserByIdController : ControllerBase
    {
        private readonly GetUserByIdService _getUserByIdService;
        private readonly ILogger<GetUserByIdController> _logger;

        public GetUserByIdController(GetUserByIdService getUserByIdService, ILogger<GetUserByIdController> logger)
        {
            _getUserByIdService = getUserByIdService ?? throw new ArgumentNullException(nameof(getUserByIdService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/GetUser/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "El ID del usuario no es válido." });
            }

            try
            {
                var user = await _getUserByIdService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new { error = "Usuario no encontrado." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID {Id}.", id);
                return StatusCode(500, new { error = "Error interno del servidor." });
            }
        }
    }
}
