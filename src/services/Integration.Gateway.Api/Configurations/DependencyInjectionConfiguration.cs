using Integration.Gateway.Api.Service;
using Polly;
// using Seven.Core.Lib.Extensions; - Temporarily disabled
using System.Net.Http.Headers;

namespace Integration.Gateway.Api.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services, IConfiguration configuration = default)
        {

            var baseUrl = (configuration.GetSection("UseLocal").Value.ToLower().Trim() == "true" ? "BaseUrl-Local" : "BaseUrl");
            var tryAlowedBeforeBreak = Convert.ToInt16(configuration.GetSection("ResiliencyConfigurations")["TryAlowedBeforeBreak"]);
            var durationOfBreak = Convert.ToDouble(configuration.GetSection("ResiliencyConfigurations")["DurationOfBreak"]);

            services.AddHttpClient<FakeService>(config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection(baseUrl)["IntegrationApi"]);
                config.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            // .AddPolicyHandler(PollyExtensions.WaitAndRetry()) - Temporarily disabled
            // .AddTransientHttpErrorPolicy(x => x.CircuitBreakerAsync(tryAlowedBeforeBreak, TimeSpan.FromSeconds(durationOfBreak))); - Temporarily disabled

            return services;
        }
    }
}

