using Microsoft.Extensions.Primitives;

namespace Integration.Api.Middleware
{
    public class CorsPreflightMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorsPreflightMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public CorsPreflightMiddleware(RequestDelegate next, ILogger<CorsPreflightMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Tratamento especial para requisições OPTIONS (preflight CORS)
            if (context.Request.Method == "OPTIONS")
            {
                _logger.LogInformation("Handling OPTIONS preflight request from: {Origin}", 
                    context.Request.Headers.TryGetValue("Origin", out StringValues origin) ? origin.ToString() : "unknown");

                // Determinar qual política usar com base no ambiente
                string allowedOrigins = _env.IsProduction() 
                    ? "https://odontosmileconecta-production.up.railway.app"
                    : "https://localhost:7221,http://localhost:5221,http://localhost:3000,http://localhost:8080";

                // Adiciona headers CORS manualmente
                context.Response.Headers.Append("Access-Control-Allow-Origin", 
                    context.Request.Headers.TryGetValue("Origin", out StringValues requestOrigin) 
                        ? requestOrigin.ToString() 
                        : allowedOrigins.Split(',')[0]);
                context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization, X-Requested-With");
                context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, PATCH, OPTIONS");
                context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
                context.Response.Headers.Append("Access-Control-Max-Age", "86400"); // 24 horas

                // Responde 200 OK para requisições preflight
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("");
                return;
            }

            // Continua o pipeline para outras requisições
            await _next(context);
        }
    }
}
