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

                // Em desenvolvimento, permite qualquer origem
                if (_env.IsDevelopment())
                {
                    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                    context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization, X-Requested-With, Accept, Origin");
                    context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, PATCH, OPTIONS, HEAD");
                }
                else
                {
                    // Em produção, usa origins específicos
                    string allowedOrigins = "https://odontosmileconecta-production.up.railway.app";
                    
                    context.Response.Headers.Append("Access-Control-Allow-Origin", 
                        context.Request.Headers.TryGetValue("Origin", out StringValues requestOrigin) 
                            ? requestOrigin.ToString() 
                            : allowedOrigins);
                    context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization, X-Requested-With, Accept, Origin");
                    context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, PATCH, OPTIONS, HEAD");
                    context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
                }
                
                context.Response.Headers.Append("Access-Control-Max-Age", "86400"); // 24 horas

                // Responde 200 OK para requisições preflight
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("");
                return;
            }

            // Adiciona headers CORS para requisições normais também
            if (_env.IsDevelopment())
            {
                context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
            }
            else if (context.Request.Headers.TryGetValue("Origin", out StringValues requestOrigin))
            {
                string[] allowedOrigins = {
                    "https://odontosmileconecta-production.up.railway.app",
                    "http://odontosmileconecta-production.up.railway.app"
                };
                
                if (allowedOrigins.Contains(requestOrigin.ToString()))
                {
                    context.Response.Headers.Append("Access-Control-Allow-Origin", requestOrigin.ToString());
                    context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
                }
            }

            // Continua o pipeline para outras requisições
            await _next(context);
        }
    }
}
