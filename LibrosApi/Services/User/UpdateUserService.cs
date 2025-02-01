using Dapper;
using LibrosApi.Dto;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using LibrosApi.Exceptions;
using System.Threading.Tasks;
using LibrosApi.Services.Bcrypt;

namespace LibrosApi.Services.User
{
    public class UpdateUserService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<UpdateUserService> _logger;
        private readonly GetUserByIdService _getUserByIdService;
        private readonly BcryptService _bcryptService;

        public UpdateUserService(BcryptService bcryptService, IDbConnection dbConnection, ILogger<UpdateUserService> logger, GetUserByIdService getUserByIdService)
        {
            _bcryptService = bcryptService ?? throw new ArgumentNullException(nameof(bcryptService));
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getUserByIdService = getUserByIdService ?? throw new ArgumentNullException(nameof(getUserByIdService));
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserDto userDto)
        {
            try
            {
                // Verificar si el usuario existe
                var existUser = await _getUserByIdService.GetUserByIdAsync(id);

                if (existUser == null)  // Si no existe el usuario
                {
                    _logger.LogWarning("No se encontró el usuario con ID {Id}.", id);
                    throw new ResourceNotFoundException("No se encontró el usuario.");
                }

                string hashedPassword = _bcryptService.HashPassword(userDto.Password);

                // Parámetros para el procedimiento almacenado
                var parameters = new
                {
                    userId = id,
                    name = string.IsNullOrEmpty(userDto.Nombre) ? (object)DBNull.Value : userDto.Nombre,
                    password = string.IsNullOrEmpty(userDto.Password) ? (object)DBNull.Value : hashedPassword,
                    email = string.IsNullOrEmpty(userDto.Email) ? (object)DBNull.Value : userDto.Email
                };

                string storedProcedure = "UpdateUser";  // Nombre del procedimiento almacenado

                // Ejecutar el procedimiento almacenado
                int rowsAffected = await _dbConnection.ExecuteAsync(
                    storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (rowsAffected > 0)
                {
                    _logger.LogInformation($"Usuario con ID {id} actualizado correctamente.");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"No se encontró el usuario con ID {id} para actualizar.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el usuario con ID {id}.");
                throw new Exception("Error interno al actualizar el usuario.");
            }
        }
    }
}
