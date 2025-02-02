namespace LibrosApi.Controllers
{
    using LibrosApi.Dto.LoginDto;
    using LibrosApi.Exceptions;
    using LibrosApi.Services.Login;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase // Cambiado a ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(LoginService loginService, ILogger<LoginController> logger)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginRequestDto)
        {
            try
            {
                LoginResponseDto response = await _loginService.GenerateToken(loginRequestDto.Email, loginRequestDto.Password);

                return Ok(response); // Devuelve el token en un objeto
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(); // 401 Unauthorized
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el inicio de sesión.");
                return StatusCode(500, new { error = "Error interno del servidor." }); // 500 Internal Server Error
            }
        }
    }
}
