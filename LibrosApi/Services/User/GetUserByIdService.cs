using Dapper;
using LibrosApi.Context;
using LibrosApi.Dto;
using LibrosApi.Exceptions;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Threading.Tasks;

namespace LibrosApi.Services.User
{
    public class GetUserByIdService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<GetUserByIdService> _logger;

        public GetUserByIdService(IDbConnection dbConnection, ILogger<GetUserByIdService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GetUserDto> GetUserByIdAsync(int id)
        {
            try
            {
                var parameters = new { userId = id };  // Cambié 'Id' por 'userId'
                string storedProcedure = "GetUserById";  // Nombre del procedimiento almacenado en la base de datos

                // Llamada al procedimiento almacenado
                var user = await _dbConnection.QueryFirstOrDefaultAsync<GetUserDto>(
                    storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (user == null)
                {
                    _logger.LogWarning("Usuario con ID {Id} no encontrado.", id);
                    throw new ResourceNotFoundException($"El usuario con el ID: {id} no se encontró.");
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID {Id}.", id);
                throw new Exception("Error interno al obtener el usuario.");
            }
        }

    }
}
