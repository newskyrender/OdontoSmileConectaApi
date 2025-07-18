using Integration.Domain.Profiles;

namespace Integration.Api.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(FakeProfile)
            );

            return services;
        }
    }
}

