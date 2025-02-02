using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace LibrosApi.Services.Authors
{
    public class DeleteAuthorService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<DeleteAuthorService> _logger;

        public DeleteAuthorService(IDbConnection dbConnection, ILogger<DeleteAuthorService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> DeleteAuthorAsync(int id) 
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@authorId", id, DbType.Int32, ParameterDirection.Input);

                int affectedRows = await _dbConnection.ExecuteScalarAsync<int>("EliminarAutor", parameters,
                                                                            commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    _logger.LogInformation($"Autor con ID {id} eliminado correctamente.");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Autor con ID {id} no encontrado.");
                    return false;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Error SQL al eliminar autor con ID {id}.");
                throw; // Re-lanza la excepción SQL
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al eliminar autor con ID {id}.");
                throw; // Re-lanza la excepción general
            }
        }
    }
}
