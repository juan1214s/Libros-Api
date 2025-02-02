namespace LibrosApi.Services.Booking
{
    using Dapper;
    using LibrosApi.Dto.BookingDto;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public class GetBookingService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<GetBookingService> _logger;

        public GetBookingService(IDbConnection dbConnection, ILogger<GetBookingService> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        public async Task<IEnumerable<GetBookingDto>> GetBookingsAsync()
        {
            try
            {
                var bookings = await _dbConnection.QueryAsync<GetBookingDto>(
                    "ObtenerPrestamosConLibrosUsuarios", // Nombre del SP o vista
                    commandType: CommandType.StoredProcedure);

                if (bookings == null || !bookings.Any())
                {
                    _logger.LogInformation("No se encontraron préstamos.");
                    return new List<GetBookingDto>();
                }

                _logger.LogInformation("Préstamos obtenidos correctamente.");
                return bookings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los préstamos.");
                throw;
            }
        }
    }
}
