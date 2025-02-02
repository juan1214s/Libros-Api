namespace LibrosApi.Services.Book
{
    using Dapper;
    using Microsoft.Data.SqlClient; // Importa SqlException
    using Microsoft.Extensions.Logging;
    using System;
    using System.Data;
    using System.Threading.Tasks;

    public class DeleteBookService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<DeleteBookService> _logger;

        public DeleteBookService(IDbConnection dbConnection, ILogger<DeleteBookService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@bookId", bookId, DbType.Int32, ParameterDirection.Input);

                int affectedRows = await _dbConnection.ExecuteScalarAsync<int>("EliminarLibro", parameters, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    _logger.LogInformation("Libro con ID {Id} eliminado correctamente.", bookId);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Libro con ID {Id} no encontrado.", bookId);
                    return false;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al eliminar libro con ID {Id}.", bookId);
                throw new Exception($"Error en la base de datos: {ex.Message}"); // Lanza una excepción más genérica
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar libro con ID {Id}.", bookId);
                throw new Exception("Error interno del servidor."); // Lanza una excepción más genérica
            }
        }
    }
}
