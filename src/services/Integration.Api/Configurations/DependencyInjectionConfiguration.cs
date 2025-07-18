using Integration.Domain.Repositories;
using Integration.Infrastructure.Repositories;
using Integration.Infrastructure.Transactions;
using Integration.Service.Services;

namespace Integration.Api.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IUow, Uow>();

            services.AddScoped<IFakeRepository, FakeRepository>();
            services.AddScoped<FakeService>();

            return services;
        }
    }
}

