using Dapper;
using LibrosApi.Dto.AuthorsDto;
using System.Data;

namespace LibrosApi.Services.Authors
{
    public class GetAuthorsService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<GetAuthorsService> _logger;

        public GetAuthorsService(IDbConnection dbConnection, ILogger<GetAuthorsService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<GetAuthorsDto>> GetAuthorsAsync()
        {
            try
            {
                var authors = await _dbConnection.QueryAsync<GetAuthorsDto>(
                    "ObtenerAutores", 
                    commandType: CommandType.StoredProcedure
                );

                if (!authors.Any())
                {
                    _logger.LogInformation("No se encontraron autores registrados.");
                }
                else
                {
                    _logger.LogInformation("Autores obtenidos correctamente.");
                }

                return authors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los autores.");
                throw; // Re-lanza la excepción
            }
        }
    }
}
