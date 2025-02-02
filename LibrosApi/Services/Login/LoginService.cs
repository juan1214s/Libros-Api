namespace LibrosApi.Services.Login
{
    using Dapper;
    using LibrosApi.Dto.LoginDto;
    using LibrosApi.Exceptions;
    using LibrosApi.Dto.LoginDto;
    using LibrosApi.Services.Bcrypt;
    using LibrosApi.Services.Token;
    using Microsoft.Extensions.Logging;
    using System.Data;
    using System.Threading.Tasks;

    public class LoginService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<LoginService> _logger;
        private readonly BcryptService _bcryptService;
        private readonly TokenService _tokenService;

        public LoginService(
            IDbConnection dbConnection,
            ILogger<LoginService> logger,
            BcryptService bcryptService,
            TokenService tokenService)
        {
            _bcryptService = bcryptService;
            _dbConnection = dbConnection;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto> GenerateToken(string email, string password)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Email", email, DbType.String);

                var user = await _dbConnection.QuerySingleOrDefaultAsync<dynamic>("ObtenerUsuarioPorEmail", parameters, commandType: CommandType.StoredProcedure);

                if (user == null)
                {
                    _logger.LogWarning($"Usuario no encontrado: {email}");
                    throw new UnauthorizedAccessException("Credenciales inválidas."); // 401
                }

                // user.password asume que tienes una columna password en la base de datos
                if (!_bcryptService.VerifyPassword(password, user.password)) // user.password debe ser el hash almacenado
                {
                    _logger.LogWarning($"Contraseña incorrecta para el usuario: {email}");
                    throw new UnauthorizedAccessException("Credenciales inválidas.");  // 401
                }

                string token = _tokenService.GenerateToken(user.id); // user.id asume que tienes una columna id en la base de datos

                return new LoginResponseDto { Token = token, UserId = user.id }; // Devuelve el objeto LoginResponseDto
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Intento de inicio de sesión no autorizado.");
                throw; // Re-lanza la excepción para que el controlador la maneje
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al generar token para el usuario: {email}");
                throw new Exception("Error interno del servidor."); // Lanza una excepción más genérica
            }
        }
    }
}