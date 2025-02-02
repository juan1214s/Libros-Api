using System.Net;
using System.Text.Json;

namespace LibrosApi.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)  // Constructor correcto: Recibe RequestDelegate
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) // Método InvokeAsync
        {
            await _next(context); // Llama al siguiente middleware

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                context.Response.ContentType = "application/json";
                var response = new { Message = "No estás autorizado para realizar esta acción." };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}