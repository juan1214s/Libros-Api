namespace LibrosApi.Services.Book
{
    using Dapper;
    using LibrosApi.Dto.BookDto;
    using LibrosApi.Exceptions; // Importa tus excepciones personalizadas
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Data;
    using System.Threading.Tasks;

    public class GetBookByIdService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<GetBookByIdService> _logger;

        public GetBookByIdService(IDbConnection dbConnection, ILogger<GetBookByIdService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GetBookDto> GetBookByIdAsync(int id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

                var book = await _dbConnection.QuerySingleOrDefaultAsync<GetBookDto>(
                    "ObtenerLibroPorId", // Nombre del SP
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (book == null)
                {
                    _logger.LogWarning($"Libro con ID {id} no encontrado.");
                    throw new ResourceNotFoundException("No se encontró el libro."); // Lanza excepción personalizada
                }

                return book;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al obtener libro con ID {Id}.", id);
                throw; // Re-lanza la excepción
            }
            catch (ResourceNotFoundException ex) // Captura la excepción personalizada
            {
                _logger.LogWarning(ex, "Libro no encontrado.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener libro con ID {Id}.", id);
                throw; // Re-lanza la excepción
            }
        }
    }
}