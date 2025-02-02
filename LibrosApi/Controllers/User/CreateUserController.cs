using LibrosApi.Dto.UsersDto;
using LibrosApi.Exceptions;
using LibrosApi.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibrosApi.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CreateUserController : ControllerBase
    {
        private readonly CreateUserService _createUserService;
        private readonly ILogger<CreateUserController> _logger;

        public CreateUserController(CreateUserService createUserService, ILogger<CreateUserController> logger)
        {
            _createUserService = createUserService ?? throw new ArgumentNullException(nameof(createUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
        {
            // Validación de entrada
            if (user == null)
            {
                return BadRequest(new { message = "El cuerpo de la solicitud no puede ser nulo." });
            }

            if (string.IsNullOrWhiteSpace(user.Nombre) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest(new { message = "Todos los campos son obligatorios." });
            }

            try
            {
                // Llamada al servicio para crear el usuario
                var result = await _createUserService.CreateUserAsync(user);

                if (result)
                {
                    _logger.LogInformation("Usuario creado con éxito.");
                    return StatusCode(201, new { message = "Se creó con éxito el usuario." });
                }
                else
                {
                    _logger.LogError("Error al crear el usuario.");
                    return BadRequest(new { error = "Error al crear el usuario." });
                }
            }
            catch (AppExceptions ex)
            {
                _logger.LogError(ex.Message); // Registra el mensaje de la excepción personalizada.
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                _logger.LogError(ex, "Error al crear el usuario.");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }
    }
}
