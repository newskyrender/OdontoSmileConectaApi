using Integration.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Integration.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Railway-specific settings
            ConfigureRailwaySettings(builder);

            // Configure services using Startup
            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            // Configure pipeline using Startup
            startup.Configure(app, app.Environment);

            // Run migrations in production
            if (app.Environment.IsProduction())
            {
                RunMigrationsAsync(app).GetAwaiter().GetResult();
            }

            app.Run();
        }

        private static void ConfigureRailwaySettings(WebApplicationBuilder builder)
        {
            // Railway port configuration
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(int.Parse(port));
            });

            // Railway-specific logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Information);

            // Add health checks for Railway
            builder.Services.AddHealthChecks();
        }

        private static async Task RunMigrationsAsync(WebApplication app)
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<OdontoSmileDataContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("Verificando migrações pendentes...");

                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                {
                    logger.LogInformation($"Aplicando {pendingMigrations.Count()} migrações...");
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Migrações aplicadas com sucesso!");
                }
                else
                {
                    logger.LogInformation("Banco de dados está atualizado.");
                }

                await SeedInitialDataAsync(context, logger);
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Erro ao aplicar migrações do banco de dados");
                // Don't throw in production - continue without database
                if (app.Environment.IsDevelopment())
                {
                    throw;
                }
            }
        }

        private static Task SeedInitialDataAsync(OdontoSmileDataContext context, ILogger logger)
        {
            try
            {
                // Implemente seu seed de dados conforme necessário
                // Exemplo:
                /*
                if (!context.Usuarios.Any())
                {
                    logger.LogInformation("Criando dados iniciais...");
                    // Adicione entidades iniciais
                    context.SaveChanges();
                }
                */
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao criar dados iniciais");
                return Task.CompletedTask; // Don't fail the app for seed issues
            }
        }
    }
}
