using Dapper;
using LibrosApi.Dto.AuthorsDto;
using LibrosApi.Exceptions;
using System.Data;

namespace LibrosApi.Services.Authors
{
    public class GetAuthorByIdService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<GetAuthorByIdService> _logger;

        public GetAuthorByIdService(IDbConnection dbConnection, ILogger<GetAuthorByIdService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GetAuthorsDto> GetAuthorByIdAsync(int id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

                var author = await _dbConnection.QuerySingleOrDefaultAsync<GetAuthorsDto>(
                    "ObtenerAutorPorId", // Nombre del SP
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (author == null)
                {
                    _logger.LogWarning($"Autor con ID {id} no encontrado.");
                    throw new ResourceNotFoundException("No se encontró el autor.");
                }

                return author;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el autor con ID {id}.");
                throw; // Re-lanza la excepción
            }
        }
    }
}
