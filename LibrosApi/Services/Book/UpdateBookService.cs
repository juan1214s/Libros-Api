namespace LibrosApi.Services.Book
{
    using Dapper;
    using LibrosApi.Dto.BookDto;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Data;
    using System.Threading.Tasks;

    public class UpdateBookService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<UpdateBookService> _logger;

        public UpdateBookService(IDbConnection dbConnection, ILogger<UpdateBookService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> UpdateBookAsync(int id, UpdateBookDto updateBookDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);
                parameters.Add("@Titulo", updateBookDto.Titulo, DbType.String);
                parameters.Add("@IdAutor", updateBookDto.IdAutor, DbType.Int32);
                parameters.Add("@IdCategoria", updateBookDto.IdCategoria, DbType.Int32);
                parameters.Add("@FechaPublicacion", updateBookDto.FechaPublicacion, DbType.Date); // Tipo DATE

                int affectedRows = await _dbConnection.ExecuteScalarAsync<int>(
                    "ActualizarLibro", // Nombre del SP
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    _logger.LogInformation("Libro con ID {Id} actualizado correctamente.", id);
                    return true;
                }
                else
                {
                    _logger.LogWarning("No se encontró el libro con ID {Id}.", id);
                    return false;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al actualizar libro con ID {Id}.", id);
                throw new Exception($"Error en la base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar libro con ID {Id}.", id);
                throw new Exception("Error interno del servidor.");
            }
        }
    }
}