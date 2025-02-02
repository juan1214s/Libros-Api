using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace LibrosApi.Services.Categories
{
    public class DeleteCategoriesService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<DeleteCategoriesService> _logger;

        public DeleteCategoriesService(IDbConnection dbConnection, ILogger<DeleteCategoriesService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@categoryId", categoryId, DbType.Int32, ParameterDirection.Input);

                int affectedRows = await _dbConnection.ExecuteScalarAsync<int>("EliminarCategoria", parameters, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    _logger.LogInformation("Categoría con ID {Id} eliminada correctamente.", categoryId);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Categoría con ID {Id} no encontrada.", categoryId);
                    return false;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al eliminar categoría con ID {Id}.", categoryId);
                throw new Exception($"Error en la base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar categoría con ID {Id}.", categoryId);
                throw new Exception("Error interno del servidor.");
            }
        }
    }
}
