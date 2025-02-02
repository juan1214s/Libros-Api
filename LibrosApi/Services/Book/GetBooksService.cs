namespace LibrosApi.Services.Book // Namespace consistente
{
    using Dapper;
    using LibrosApi.Dto.BookDto; // Importa el DTO
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public class GetBooksService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<GetBooksService> _logger;

        public GetBooksService(IDbConnection dbConnection, ILogger<GetBooksService> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        public async Task<IEnumerable<GetBookDto>> GetLibrosAsync()
        {
            try
            {
                var libros = await _dbConnection.QueryAsync<GetBookDto>(
                    "ObtenerLibrosConAutoresCategorias",
                    commandType: CommandType.StoredProcedure);

                if (libros == null || !libros.Any())
                {
                    _logger.LogInformation("No se encontraron libros.");
                    return new List<GetBookDto>(); // Devuelve una lista vacía
                }

                _logger.LogInformation("Libros obtenidos correctamente.");
                return libros;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los libros.");
                throw; // Re-lanza la excepción
            }
        }
    }
}