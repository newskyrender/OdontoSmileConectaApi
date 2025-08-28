using Microsoft.EntityFrameworkCore;
using Integration.Domain.Repositories;
using Integration.Infrastructure.Contexts;
using Integration.Infrastructure.Repositories;
using Integration.Infrastructure.Transactions;
using Integration.Service.Services;
using Integration.Service.AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.RateLimiting;
using Asp.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Integration.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<OdontoSmileDataContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("IntegrationMySql");
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)),
                    mysqlOptions =>
                    {
                        mysqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        mysqlOptions.CommandTimeout(60);
                    });

                // Configurações adicionais para desenvolvimento
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            // Unit of Work
            services.AddScoped<IUow, Uow>();
            services.AddScoped<ITransactionManager, TransactionManager>();

            // Repositories
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
            services.AddScoped<ISolicitacaoOrcamentoRepository, SolicitacaoOrcamentoRepository>();
            services.AddScoped<IPlanejamentoDigitalRepository, PlanejamentoDigitalRepository>();
            services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
            services.AddScoped<IDocumentoRepository, DocumentoRepository>();
            services.AddScoped<IProfissionalEspecialidadeRepository, ProfissionalEspecialidadeRepository>();
            services.AddScoped<IProfissionalEquipamentoRepository, ProfissionalEquipamentoRepository>();
            services.AddScoped<IProfissionalFacilidadeRepository, ProfissionalFacilidadeRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IRelatorioFinanceiroRepository, RelatorioFinanceiroRepository>();

            // Services
            services.AddScoped<UsuarioService>();
            services.AddScoped<PacienteService>();
            services.AddScoped<ProfissionalService>();
            services.AddScoped<SolicitacaoOrcamentoService>();
            services.AddScoped<AgendamentoService>();
            services.AddScoped<PlanejamentoDigitalService>();
            services.AddScoped<DocumentoService>();
            services.AddScoped<DashboardService>();
            services.AddScoped<RelatorioFinanceiroService>();

            // AutoMapper
            services.AddAutoMapper(typeof(OdontoSmileAutoMapperProfile), typeof(EnumConverterProfile));

            return services;
        }

        public static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("role", "admin"));
                options.AddPolicy("Profissional", policy => policy.RequireClaim("role", "profissional"));
                options.AddPolicy("Paciente", policy => policy.RequireClaim("role", "paciente"));
            });

            return services;
        }

        public static IServiceCollection AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });

                options.AddPolicy("Production", builder =>
                {
                    builder
                        .WithOrigins("https://odontosmileddigital.com", "https://www.odontosmileddigital.com")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            return services;
        }

        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "OdontoSmile Conecta API",
                    Version = "v1",
                    Description = "API para gestão do sistema OdontoSmile Conecta",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Suporte Técnico",
                        Email = "suporte@odontosmileddigital.com"
                    }
                });

                // Configuração para JWT
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Incluir comentários XML se existirem
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }

        public static IServiceCollection AddHealthChecksConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy("API is running"))
                .AddDbContextCheck<OdontoSmileDataContext>(name: "database", failureStatus: HealthStatus.Degraded);

            return services;
        }

        public static IServiceCollection AddApiVersioningConfig(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("version"),
                    new HeaderApiVersionReader("X-Version"),
                    new UrlSegmentApiVersionReader()
                );
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static IServiceCollection AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
                builder.AddConsole();
                builder.AddDebug();

                // Adicionar Serilog se configurado
                if (configuration.GetSection("Serilog").Exists())
                {
                    // builder.AddSerilog(); // Configurar Serilog conforme necessário
                }
            });

            return services;
        }

        public static IServiceCollection AddCachingConfig(this IServiceCollection services, IConfiguration configuration)
        {
            // Memory Cache
            services.AddMemoryCache(options =>
            {
                options.SizeLimit = 1024; // Limite de 1024 entradas
            });

            // Distributed Cache (Redis se configurado)
            var redisConnectionString = configuration.GetConnectionString("Redis");
            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnectionString;
                    options.InstanceName = "OdontoSmileConecta";
                });
            }
            else
            {
                // Fallback para in-memory distributed cache
                services.AddDistributedMemoryCache();
            }

            return services;
        }

        public static IServiceCollection AddCompressionConfig(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
                options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
            });

            return services;
        }

        public static IServiceCollection AddRateLimitingConfig(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("GlobalLimiter", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 100;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.AutoReplenishment = true;
                });

                options.AddFixedWindowLimiter("AuthPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 10;
                    limiterOptions.Window = TimeSpan.FromMinutes(5);
                    limiterOptions.AutoReplenishment = true;
                });
            });

            return services;
        }
    }
}
