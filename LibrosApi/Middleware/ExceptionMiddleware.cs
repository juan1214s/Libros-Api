using LibrosApi.Exceptions;
using System.Net;
using System.Text.Json;

namespace LibrosApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppExceptions appException)
            {
                // Si es una excepción personalizada (AppExceptions), maneja y responde
                await HandleExceptionAsync(context, appException);
            }
            catch (Exception ex)
            {
                // Si es una excepción general, maneja y responde
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, AppExceptions exception)
        {
            HttpStatusCode statusCode = (HttpStatusCode)exception.StatusCode;
            string message = exception.Message;

            _logger.LogError(exception, "Excepción capturada en el middleware de manejo de errores.");

            var response = new
            {
                status = (int)statusCode,
                error = message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Para cualquier otra excepción no controlada
            _logger.LogError(exception, "Error interno en la aplicación.");

            var response = new
            {
                status = 500,
                error = "Ocurrió un error interno."
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
