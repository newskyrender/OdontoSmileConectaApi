using Microsoft.EntityFrameworkCore;
using Integration.Api.Configurations;
using Integration.Infrastructure.Contexts;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Integration.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuração do contexto do banco de dados
            AddDataContextConfigurations(services);

            // Controllers básicos para Railway
            services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            // Configurações essenciais para Railway
            services.AddSwaggerConfiguration();
            services.AddDependencyInjectionConfiguration();

            // CORS para Railway e domínio público
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .WithOrigins(
                            "https://odontosmileconectaapi-production.up.railway.app",
                            "http://odontosmileconectaapi-production.up.railway.app",
                            "https://localhost:7221",
                            "http://localhost:5221"
                        )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
                
                options.AddPolicy("Production", builder =>
                {
                    builder
                        .WithOrigins("https://odontosmileconectaapi-production.up.railway.app")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            // HTTP Client Factory
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure forwarded headers for Railway proxy
            app.UseForwardedHeaders();

            // Log de todas as requisições para debug Railway
            app.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();
                logger.LogInformation($"Railway Request: {context.Request.Method} {context.Request.Path} from {context.Connection.RemoteIpAddress}");
                await next();
                logger.LogInformation($"Railway Response: {context.Response.StatusCode}");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // CORS first
            app.UseCors(env.IsProduction() ? "Production" : "AllowAll");

            // Health Checks before routing
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        status = "Healthy",
                        timestamp = DateTime.UtcNow,
                        environment = env.EnvironmentName,
                        port = Environment.GetEnvironmentVariable("PORT") ?? "8080",
                        domain = env.IsProduction() ? "odontosmileconectaapi-production.up.railway.app" : "localhost",
                        checks = report.Entries.Select(x => new
                        {
                            name = x.Key,
                            status = x.Value.Status.ToString()
                        })
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });

            // Simple routing for Railway
            app.UseRouting();
            
            app.UseSwaggerConfiguration(env);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }

        private void AddDataContextConfigurations(IServiceCollection services)
        {
            services.AddDbContext<OdontoSmileDataContext>(opt =>
            {
                opt.UseMySql(
                    Configuration.GetConnectionString("IntegrationMySql"),
                    new MySqlServerVersion(new Version(8, 0, 36))
                );
                opt.EnableSensitiveDataLogging();
            }, ServiceLifetime.Scoped);
        }
    }

    // Filtros globais
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);

            var response = new
            {
                error = "Erro interno do servidor",
                message = exception.Message,
                details = context.HttpContext.RequestServices
                    .GetService<IWebHostEnvironment>()?
                    .IsDevelopment() == true ? exception.StackTrace : null
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }

    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage);

                var response = new
                {
                    error = "Dados inválidos",
                    message = "Verifique os dados enviados",
                    details = errors
                };

                context.Result = new BadRequestObjectResult(response);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}

// Extensão para security headers
public static class SecurityHeadersExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
            context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
            context.Response.Headers.Add("Content-Security-Policy",
                "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:;");

            await next();
        });
    }
}
