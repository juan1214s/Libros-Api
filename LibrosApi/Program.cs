using LibrosApi.Middleware;
using LibrosApi.Services.User;
using LibrosApi.Services;
using LibrosApi.Context;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddCustomService();

// Agregar servicios al contenedor
builder.Services.AddControllers();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuraci�n de la conexi�n a la base de datos
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//builder.Services.AddScoped<DbConnection>(); // Aseg�rate de agregar esto

// Agregar servicios espec�ficos para los controladores
builder.Services.AddScoped<CreateUserService>();
builder.Services.AddScoped<DeleteUserService>();

var app = builder.Build();

// Middleware de excepciones
app.UseMiddleware<ExceptionMiddleware>();

// Configuraci�n del pipeline de la solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
