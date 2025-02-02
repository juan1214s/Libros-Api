using Dapper;
using LibrosApi.Dto.AuthorsDto;
using LibrosApi.Exceptions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LibrosApi.Services.Authors
{
    public class CreateAuthorService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<CreateAuthorService> _logger;

        public CreateAuthorService(IDbConnection dbConnection, ILogger<CreateAuthorService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CreateAuthorAsync(CreateAthorsDto authorDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@nombre", authorDto.Nombre, DbType.String, ParameterDirection.Input, 50); // Ajusta el tamaño si es necesario

                await _dbConnection.ExecuteAsync("InsertarAutor", parameters, commandType: CommandType.StoredProcedure); // Nombre del SP

                _logger.LogInformation($"Autor '{authorDto.Nombre}' creado correctamente.");
                return true;
            }
            catch (SqlException ex) when (ex.Number == 50001) // Ejemplo de manejo de error específico (duplicado)
            {
                _logger.LogWarning($"No se pudo crear el autor {authorDto.Nombre}. Ya existe.", ex.Message);
                throw new ResourceAlreadyExistsException("Ya existe un autor con ese nombre."); // Lanza excepción personalizada
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Error SQL al crear el autor: {authorDto.Nombre}.");
                throw; // Re-lanza la excepción SQL
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al crear el autor: {authorDto.Nombre}.");
                throw; // Re-lanza la excepción general
            }
        }
    }

}
