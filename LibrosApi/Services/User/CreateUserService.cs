using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using LibrosApi.Dto.UsersDto;
using LibrosApi.Exceptions;
using LibrosApi.Services.Bcrypt;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace LibrosApi.Services.User
{
    public class CreateUserService
    {
        private readonly IDbConnection _dbConnection;  // Usamos IDbConnection en lugar de DbConnection
        private readonly ILogger<CreateUserService> _logger;
        private readonly BcryptService _bcryptService;

        // Inyección de dependencias
        public CreateUserService(IDbConnection dbConnection, ILogger<CreateUserService> logger, BcryptService bcryptService)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bcryptService = bcryptService ?? throw new ArgumentNullException(nameof(bcryptService));
        }

        // Método para crear un usuario utilizando un procedimiento almacenado
        public async Task<bool> CreateUserAsync(CreateUserDto user)
        {
            try
            {
                string hashedPassword = _bcryptService.HashPassword(user.Password);

                // Usamos la conexión inyectada en lugar de abrir una nueva
                var parameters = new DynamicParameters();
                parameters.Add("@nombre", user.Nombre, DbType.String);
                parameters.Add("@password", hashedPassword, DbType.String);
                parameters.Add("@email", user.Email, DbType.String);

                int affectedRows = await _dbConnection.ExecuteScalarAsync<int>("InsertarUsuario", parameters, commandType: CommandType.StoredProcedure);


                if (affectedRows > 0)
                {
                    _logger.LogInformation("Usuario {Email} creado correctamente.", user.Email);
                    return true;
                }
                else
                {
                    _logger.LogWarning("El procedimiento almacenado no insertó filas.");
                    return false;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al ejecutar InsertarUsuario con el email {Email}.", user.Email);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear el usuario {Email}.", user.Email);
                return false;
            }
        }
    }
}
