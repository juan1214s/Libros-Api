using LibrosApi.Middleware;
using LibrosApi.Services.User;
using LibrosApi.Context; // Si usas DbContext, mantenlo
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.IdentityModel.Tokens; // Importa los namespaces necesarios
using System.Text;
using LibrosApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de la conexi�n a la base de datos
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios personalizados (usando extensi�n)
builder.Services.AddCustomService(); // Si tienes una clase de extensi�n, mantenla. Aseg�rate de que registre los servicios que necesitas.

// Servicios espec�ficos para los controladores (ejemplo)
builder.Services.AddScoped<CreateUserService>();
builder.Services.AddScoped<DeleteUserService>();
// ... otros servicios

// Agregar servicios al contenedor (controladores y otros)
builder.Services.AddControllers();

// Configuraci�n de autenticaci�n JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // Esquema JWT
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Ajusta a tus necesidades (true en producci�n)
            ValidateAudience = false, // Ajusta a tus necesidades (true en producci�n)
            ValidateLifetime = true,   // Valida la expiraci�n del token
            ValidateIssuerSigningKey = true, // Valida la firma del token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])) // Clave secreta
        };
    });

// Configuraci�n de autorizaci�n (despu�s de la autenticaci�n)
builder.Services.AddAuthorization();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware de excepciones
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<TokenMiddleware>();

// Configuraci�n del pipeline de la solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Middleware de autenticaci�n (�IMPORTANTE!)
app.UseAuthorization();  // Middleware de autorizaci�n (�DESPU�S de la autenticaci�n!)

app.MapControllers();

app.Run();