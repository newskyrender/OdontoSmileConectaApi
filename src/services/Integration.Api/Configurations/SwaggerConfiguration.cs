using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Integration.Api.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Integration Api",
                    Description = "Api Integration Project",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Stramandinoli",
                        Email = "desenvolvimento@stramandinoli.com.br",
                        Url = new Uri("https://stramandinoli.com.br")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Licença de uso: Integration - API",
                        Url = new Uri("https://stramandinoli.com.br/api-licenca")
                    }
                });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                #region Settings Authentication Use

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }});
                
                #endregion

            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    // Configure servers for Railway domain
                    if (env.IsProduction())
                    {
                        swaggerDoc.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer { Url = "https://odontosmileconectaapi-production.up.railway.app", Description = "Production Server" },
                            new OpenApiServer { Url = "http://odontosmileconectaapi-production.up.railway.app", Description = "Production Server (HTTP)" }
                        };
                    }
                    else
                    {
                        swaggerDoc.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer { Url = "https://localhost:7221", Description = "Development Server (HTTPS)" },
                            new OpenApiServer { Url = "http://localhost:5221", Description = "Development Server (HTTP)" }
                        };
                    }
                });
            });

            // Configure Swagger UI for both development and production
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Integration API v1");
                
                if (env.IsProduction())
                {
                    // In production (Railway), make swagger available at root
                    options.RoutePrefix = "swagger";
                    options.DocumentTitle = "Integration API - Production";
                }
                else
                {
                    // In development, standard swagger route
                    options.RoutePrefix = "swagger";
                    options.DocumentTitle = "Integration API - Development";
                }
                
                // Additional configurations for Railway
                options.DisplayRequestDuration();
                options.EnableDeepLinking();
                options.EnableFilter();
                options.ShowExtensions();
                options.EnableValidator();
            });

            return app;
        }

    }
}

