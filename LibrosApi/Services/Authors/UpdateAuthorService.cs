using Dapper;
using LibrosApi.Controllers.Athors;
using LibrosApi.Dto.AuthorsDto;
using LibrosApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LibrosApi.Services.Authors
{
    public class UpdateAuthorService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<UpdateAuthorService> _logger;

        public UpdateAuthorService(IDbConnection dbConnection, ILogger<UpdateAuthorService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> UpdateAuthorAsync(int id, UpdateAuthorDto updateAuthorDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);
                parameters.Add("@Nombre", updateAuthorDto.Nombre, DbType.String);

                int affectedRows = await _dbConnection.ExecuteScalarAsync<int>(
                    "ActualizarAutor",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return affectedRows > 0;
            }
            catch (SqlException ex) when (ex.Number == 50002) // Autor no encontrado (SP)
            {
                _logger.LogWarning($"No se encontró el autor con ID {id}: {ex.Message}");
                throw new ResourceNotFoundException("No se encontró el autor."); // Lanza ResourceNotFoundException
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Error SQL al actualizar el autor con ID {id}.");
                throw; // Re-lanza la excepción SQL original
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al actualizar el autor con ID {id}.");
                throw; // Re-lanza la excepción general original
            }
        }
    }
}

