using LibrosApi.Dto;
using LibrosApi.Exceptions;
using LibrosApi.Services.User;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibrosApi.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateUserController : ControllerBase
    {
        private readonly UpdateUserService _updateUserService;
        private readonly ILogger<UpdateUserController> _logger;

        public UpdateUserController(UpdateUserService updateUserService, ILogger<UpdateUserController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _updateUserService = updateUserService ?? throw new ArgumentNullException(nameof(updateUserService));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            try
            {
                bool success = await _updateUserService.UpdateUserAsync(id, userDto);
                if (success)
                {
                    return Ok(new { message = "Usuario actualizado con éxito." });
                }
                return NotFound(new { error = "Usuario no encontrado." });
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogError(ex, $"Error al actualizar el usuario con ID {id}.");
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el usuario con ID {id}.");
                return StatusCode(500, new { error = "Ocurrió un error interno al actualizar el usuario.", details = ex.Message });
            }
        }
    }
}
