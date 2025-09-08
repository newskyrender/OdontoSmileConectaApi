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
            // Log da requisição para debug
            _logger.LogInformation("CORS Request: {Method} {Path} from Origin: {Origin}", 
                context.Request.Method, 
                context.Request.Path,
                context.Request.Headers.TryGetValue("Origin", out StringValues origin) ? origin.ToString() : "none");

            // Sempre adiciona headers CORS básicos
            var requestOrigin = context.Request.Headers.TryGetValue("Origin", out StringValues originHeader) 
                ? originHeader.ToString() 
                : "*";

            // Lista de origens permitidas
            string[] allowedOrigins = {
                "https://odontosmileconecta-production.up.railway.app",
                "http://odontosmileconecta-production.up.railway.app",
                "https://odontosmileconectaapi-production.up.railway.app",
                "http://odontosmileconectaapi-production.up.railway.app",
                "http://localhost:3000",
                "http://localhost:5173",
                "http://localhost:8080"
            };

            // Em desenvolvimento ou se a origem está na lista permitida
            bool isAllowedOrigin = _env.IsDevelopment() || allowedOrigins.Contains(requestOrigin);
            
            if (isAllowedOrigin || requestOrigin == "*")
            {
                context.Response.Headers.Append("Access-Control-Allow-Origin", 
                    _env.IsDevelopment() ? "*" : requestOrigin);
                context.Response.Headers.Append("Access-Control-Allow-Headers", 
                    "Content-Type, Authorization, X-Requested-With, Accept, Origin, X-Api-Key");
                context.Response.Headers.Append("Access-Control-Allow-Methods", 
                    "GET, POST, PUT, DELETE, PATCH, OPTIONS, HEAD");
                context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
                context.Response.Headers.Append("Access-Control-Max-Age", "86400");
            }

            // Tratamento especial para requisições OPTIONS (preflight CORS)
            if (context.Request.Method == "OPTIONS")
            {
                _logger.LogInformation("Handling OPTIONS preflight request");
                
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("");
                return;
            }

            // Continua o pipeline para outras requisições
            await _next(context);
        }
    }
}
