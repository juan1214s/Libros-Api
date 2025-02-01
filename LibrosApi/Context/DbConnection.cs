using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LibrosApi.Context
{
    public class DbConnection
    {
        private readonly string _cadenaSql;
        private readonly ILogger<DbConnection> _logger;

        // Constructor con inyección de dependencias
        public DbConnection(IConfiguration configuration, ILogger<DbConnection> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cadenaSql = configuration.GetConnectionString("CadenaSql")
                ?? throw new InvalidOperationException("La cadena de conexión no se pudo obtener.");
        }

        // Método para abrir la conexión a la BD
        public async Task<SqlConnection> OpenConnectionAsync()
        {
            try
            {
                var connection = new SqlConnection(_cadenaSql);
                await connection.OpenAsync();
                _logger.LogInformation("Conexión a la base de datos abierta correctamente.");
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al abrir la conexión a la base de datos.");
                throw new Exception("No se pudo abrir la conexión a la base de datos.", ex);
            }
        }

        // Método para cerrar la conexión
        public void CloseConnection(SqlConnection connection)
        {
            if (connection == null) return;

            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    _logger.LogInformation("Conexión a la base de datos cerrada correctamente.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cerrar la conexión a la base de datos.");
            }
            finally
            {
                connection.Dispose(); // Asegura que la conexión se libere completamente
            }
        }
    }
}
