using Integration.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Integration.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task RunMigrationsAsync(WebApplication app)
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<IntegrationDataContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("Verificando migra��es pendentes...");

                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                {
                    logger.LogInformation($"Aplicando {pendingMigrations.Count()} migra��es...");
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Migra��es aplicadas com sucesso!");
                }
                else
                {
                    logger.LogInformation("Banco de dados est� atualizado.");
                }

                await SeedInitialDataAsync(context, logger);
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Erro ao aplicar migra��es do banco de dados");
                throw;
            }
        }

        // Removido 'async' pois n�o h� await
        private static Task SeedInitialDataAsync(IntegrationDataContext context, ILogger logger)
        {
            try
            {
                // Implemente seu seed de dados conforme necess�rio
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
                throw;
            }
        }
    }
}
