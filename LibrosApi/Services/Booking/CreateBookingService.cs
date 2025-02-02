namespace LibrosApi.Services.Booking
{
    using Dapper;
    using LibrosApi.Dto.BookingDto;
    using LibrosApi.Exceptions; // Importa tus excepciones personalizadas
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Data;
    using System.Threading.Tasks;

    public class CreateBookingService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<CreateBookingService> _logger;

        public CreateBookingService(IDbConnection dbConnection, ILogger<CreateBookingService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CreateBookingAsync(CreateBookingDto bookingDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdLibro", bookingDto.IdLibro, DbType.Int32);
                parameters.Add("@IdUsuario", bookingDto.IdUsuario, DbType.Int32);
                parameters.Add("@FechaPrestamo", bookingDto.FechaPrestamo, DbType.Date);
                parameters.Add("@FechaDevolucion", bookingDto.FechaDevolucion, DbType.Date); // Permite NULL

                await _dbConnection.ExecuteAsync("InsertarPrestamo", parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation($"Préstamo creado exitosamente.");
                return true;
            }
            catch (SqlException ex) when (ex.Number == 547) // Violación de restricción de clave externa
            {
                _logger.LogWarning($"Error de clave externa al crear préstamo: {ex.Message}");
                throw new ForeignKeyViolationException("Error de clave externa. Asegúrese de que el libro y el usuario existen.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al crear préstamo.");
                throw;
            }
        }
    }
}