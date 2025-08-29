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

            // Controllers com configurações personalizadas
            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<ValidationFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            services.AddAutoMapperConfiguration();
            services.AddWebApiConfiguration();
            services.AddSwaggerConfiguration();
            services.AddDependencyInjectionConfiguration();

            // Apenas CORS para funcionar
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            // Configurações de seções
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.Configure<DatabaseSettings>(Configuration.GetSection("Database"));
            services.Configure<CacheSettings>(Configuration.GetSection("Cache"));

            // HTTP Client Factory
            services.AddHttpClient();

            // Model State Validation
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Configurações de Performance
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 52428800; // 50MB
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 52428800; // 50MB
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Log de todas as requisições para debug
            app.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();
                logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} from {context.Request.Headers["User-Agent"]}");
                await next();
                logger.LogInformation($"Response: {context.Response.StatusCode}");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // CORS
            app.UseCors("AllowAll");

            // Health Checks
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(x => new
                        {
                            name = x.Key,
                            status = x.Value.Status.ToString(),
                            exception = x.Value.Exception?.Message,
                            duration = x.Value.Duration.ToString()
                        }),
                        duration = report.TotalDuration.ToString()
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });

            app.UseWebApiConfiguration(true);
            app.UseSwaggerConfiguration(env);

            // Routing e Controllers
            app.UseRouting();
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
