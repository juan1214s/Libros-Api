using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using LibrosApi.Dto.CategoriesDto;
using Microsoft.Extensions.Logging;

namespace LibrosApi.Services.Categories
{
    public class UpdateCategoriesService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<UpdateCategoriesService> _logger;

        public UpdateCategoriesService(IDbConnection dbConnection, ILogger<UpdateCategoriesService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoriDto updateCategoriDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);
                parameters.Add("@Nombre", updateCategoriDto.Nombre, DbType.String);

                int affectedRows = await _dbConnection.ExecuteScalarAsync<int>(
                    "ActualizarCategoria",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (affectedRows > 0)
                {
                    _logger.LogInformation("Categoría con ID {Id} actualizada correctamente.", id);
                    return true;
                }
                else
                {
                    _logger.LogWarning("No se encontró la categoría con ID {Id}.", id);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la categoría con ID {Id}.", id);
                throw new Exception("Error interno del servidor.");
            }
        }
    }
}
