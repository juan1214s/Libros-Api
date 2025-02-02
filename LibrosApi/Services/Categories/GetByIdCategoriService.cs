using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using LibrosApi.Exceptions;
using LibrosApi.Dto.CategoriesDto;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace LibrosApi.Services.Categories
{
    public class GetByIdCategoryService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<GetByIdCategoryService> _logger;

        public GetByIdCategoryService(IDbConnection dbConnection, ILogger<GetByIdCategoryService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GetCateriesDto> GetCategoryByIdAsync(int id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

                var query = "ObtenerCategoriaPorId";

                var category = await _dbConnection.QuerySingleOrDefaultAsync<GetCateriesDto>(
                    query,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (category == null)
                {
                    _logger.LogWarning($"Categoría con ID {id} no encontrada.");
                    throw new ResourceNotFoundException("No se encontró la categoría."); // Lanza la excepción
                }

                return category;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al obtener la categoría con ID {Id} - SQL Exception.", id);
                throw; // Re-lanza la excepción original
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener la categoría con ID {Id}.", id);
                throw; // Re-lanza la excepción original
            }
        }


    }
}
