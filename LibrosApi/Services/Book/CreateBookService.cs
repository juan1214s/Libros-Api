using Dapper;
using LibrosApi.Dto.BookDto;
using LibrosApi.Exceptions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LibrosApi.Services.Books
{
    public class CreateBookService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<CreateBookService> _logger;

        public CreateBookService(IDbConnection dbConnection, ILogger<CreateBookService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CreateBookAsync(CreateBookDto bookDto) // Devuelve el ID del libro creado
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Titulo", bookDto.Titulo, DbType.String);
                parameters.Add("@IdAutor", bookDto.IdAutor, DbType.Int32);
                parameters.Add("@IdCategoria", bookDto.IdCategoria, DbType.Int32);
                parameters.Add("@fechaPublicacion", bookDto.FechaPublicacion, DbType.DateTime);

                // Ejecuta el SP y obtiene el ID del libro insertado
                await _dbConnection.ExecuteAsync("InsertarLibro", parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation($"Libro creado exitosamente.");
                return true;
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627) // Clave única duplicada (título)
            {
                _logger.LogWarning($"Intento de crear un libro con título duplicado: {bookDto.Titulo}");
                throw new ResourceAlreadyExistsException("Ya existe un libro con ese título.");
            }
            catch (SqlException ex) when (ex.Number == 547) // Violación de restricción de clave externa
            {
                _logger.LogWarning($"Error de clave externa al crear libro: {ex.Message}");
                throw new ForeignKeyViolationException("Error de clave externa. Asegúrese de que el autor y la categoría existen.");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Error SQL al crear libro: {bookDto.Titulo}.");
                throw; // Re-lanza la excepción SQL original
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al crear libro: {bookDto.Titulo}.");
                throw; // Re-lanza la excepción general original
            }
        }
    }
}
