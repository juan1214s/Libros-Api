using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using LibrosApi.Dto;
using LibrosApi.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace LibrosApi.Services.User
{
    public class DeleteUserService
    {
        private readonly IDbConnection _dbConnection;  // Cambiado de DbConnection a IDbConnection
        private readonly ILogger<DeleteUserService> _logger;

        // Inyección de dependencias
        public DeleteUserService(IDbConnection dbConnection, ILogger<DeleteUserService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                // Usamos la conexión inyectada directamente
                var parametros = new DynamicParameters();
                parametros.Add("@userId", userId, DbType.Int32, ParameterDirection.Input);

                int filasAfectadas = await _dbConnection.ExecuteAsync("EliminarUsuario", parametros, commandType: CommandType.StoredProcedure);

                if (filasAfectadas > 0)
                {
                    _logger.LogInformation("Usuario con ID {Id} eliminado correctamente.", userId);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Usuario con ID {Id} no encontrado.", userId);
                    return false;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al eliminar usuario con ID {Id}.", userId);
                throw new Exception($"Error en la base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar usuario con ID {Id}.", userId);
                throw new Exception("Error interno del servidor.");
            }
        }
    }
}
