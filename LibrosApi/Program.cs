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

// Configuración de la conexión a la base de datos
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios personalizados (usando extensión)
builder.Services.AddCustomService(); // Si tienes una clase de extensión, mantenla. Asegúrate de que registre los servicios que necesitas.

// Servicios específicos para los controladores (ejemplo)
builder.Services.AddScoped<CreateUserService>();
builder.Services.AddScoped<DeleteUserService>();
// ... otros servicios

// Agregar servicios al contenedor (controladores y otros)
builder.Services.AddControllers();

// Configuración de autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // Esquema JWT
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Ajusta a tus necesidades (true en producción)
            ValidateAudience = false, // Ajusta a tus necesidades (true en producción)
            ValidateLifetime = true,   // Valida la expiración del token
            ValidateIssuerSigningKey = true, // Valida la firma del token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])) // Clave secreta
        };
    });

// Configuración de autorización (después de la autenticación)
builder.Services.AddAuthorization();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware de excepciones
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<TokenMiddleware>();

// Configuración del pipeline de la solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Middleware de autenticación (¡IMPORTANTE!)
app.UseAuthorization();  // Middleware de autorización (¡DESPUÉS de la autenticación!)

app.MapControllers();

app.Run();