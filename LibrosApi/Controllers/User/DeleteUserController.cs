using LibrosApi.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace LibrosApi.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class DeleteUserController : ControllerBase
    {
        private readonly DeleteUserService _deleteUserService;
        private readonly ILogger<DeleteUserController> _logger;

        public DeleteUserController(DeleteUserService deleteUserService, ILogger<DeleteUserController> logger)
        {
            _deleteUserService = deleteUserService ?? throw new ArgumentNullException(nameof(deleteUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                bool deleted = await _deleteUserService.DeleteUserAsync(id);

                if (deleted)
                {
                    _logger.LogInformation("Usuario con ID {Id} eliminado correctamente.", id);
                    return Ok(new { message = "Usuario eliminado correctamente." });
                }
                else
                {
                    _logger.LogWarning("Usuario con ID {Id} no encontrado.", id);
                    return NotFound(new { error = "Usuario no encontrado." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID {Id}.", id);
                return StatusCode(500, new { error = "Error interno del servidor." });
            }
        }
    }
}
