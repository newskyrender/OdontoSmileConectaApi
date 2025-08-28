using Microsoft.EntityFrameworkCore;
using Integration.Api.Configurations;
using Integration.Infrastructure.Contexts;

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
            AddDataContextConfigurations(services);

            services.AddAutoMapperConfiguration();

            services.AddWebApiConfiguration();

            services.AddSwaggerConfiguration();

            services.AddDependencyInjectionConfiguration();

            //services.AddJWTBearerConfiguration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseJWTBearerConfiguration();

            app.UseWebApiConfiguration(true);

            app.UseSwaggerConfiguration(env);
        }

        private void AddDataContextConfigurations(IServiceCollection services)
        {
            services.AddDbContext<IntegrationDataContext>(opt =>
            {
                opt.UseNpgsql(Configuration.GetConnectionString("Integration"));
                opt.EnableSensitiveDataLogging();

            }, ServiceLifetime.Scoped);
        }
    }
}

