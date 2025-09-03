using Integration.Domain.Repositories;
using Integration.Infrastructure.Repositories;
using Integration.Infrastructure.Transactions;
using Integration.Service.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Integration.Infrastructure.Contexts;

namespace Integration.Api.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            // Health checks para Railway - super simples
            services.AddHealthChecks()
                .AddCheck("api", () => HealthCheckResult.Healthy("API está funcionando no Railway"));

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // UOW - Unit of Work
            services.AddScoped<IUow, Uow>();

            // Repositories
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IFakeRepository, FakeRepository>();

            // Services
            services.AddScoped<PacienteService>();
            services.AddScoped<FakeService>();

            return services;
        }
    }
}

