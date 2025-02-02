using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using LibrosApi.Dto.CategoriesDto;
using Microsoft.Extensions.Logging;

namespace LibrosApi.Services.Categories
{
    public class GetCategoriesService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<GetCategoriesService> _logger;

        public GetCategoriesService(IDbConnection dbConnection, ILogger<GetCategoriesService> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        public async Task<IEnumerable<GetCateriesDto>> ObtenerCategoriasAsync()
        {
            try
            {
                var categories = await _dbConnection.QueryAsync<GetCateriesDto>(
                    "ObtenerCategorias",
                    commandType: CommandType.StoredProcedure
                );

                if (!categories.Any())
                {
                    _logger.LogInformation("No se encontraron categorías registradas.");
                }
                else
                {
                    _logger.LogInformation("Categorías obtenidas correctamente.");
                }

                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las categorías.");
                throw;
            }
        }
    }
}
