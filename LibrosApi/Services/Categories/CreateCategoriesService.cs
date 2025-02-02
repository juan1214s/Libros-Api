using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using LibrosApi.Dto.CategoriesDto;
using LibrosApi.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

public class CreateCategoriesService
{
    private readonly IDbConnection _dbConnection;
    private readonly ILogger<CreateCategoriesService> _logger;

    public CreateCategoriesService(IDbConnection dbConnection, ILogger<CreateCategoriesService> logger)
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> CreateCategoryAsync(CreateCategoriesDto categoriDto)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@nombre", categoriDto.Nombre, DbType.String, ParameterDirection.Input, 50);

            await _dbConnection.ExecuteAsync("InsertarCategoria", parameters, commandType: CommandType.StoredProcedure);

            _logger.LogInformation($"Categoría '{categoriDto.Nombre}' creada correctamente.");
            return true;
        }

        catch (SqlException ex) when (ex.Number == 50001)
        {
            _logger.LogWarning($"No se pudo crear la categoría {categoriDto.Nombre}", ex.Message);
            throw new ResourceAlreadyExistsException("Ya la categoria esta registrada.");
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, $"Error SQL al crear la categoría: {categoriDto.Nombre}.");
            throw new Exception($"Error en la base de datos: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inesperado al crear la categoría: {categoriDto.Nombre}");
            throw new Exception("Error interno del servidor.");
        }
    }
}
