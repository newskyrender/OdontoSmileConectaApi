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
            services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IDocumentoRepository, DocumentoRepository>();
            services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
            services.AddScoped<IProfissionalEspecialidadeRepository, ProfissionalEspecialidadeRepository>();
            services.AddScoped<IProfissionalEquipamentoRepository, ProfissionalEquipamentoRepository>();
            services.AddScoped<IProfissionalFacilidadeRepository, ProfissionalFacilidadeRepository>();
            services.AddScoped<ISolicitacaoOrcamentoRepository, SolicitacaoOrcamentoRepository>();

            // Services
            services.AddScoped<PacienteService>();
            services.AddScoped<FakeService>();
            services.AddScoped<AgendamentoService>();
            services.AddScoped<UsuarioService>();
            services.AddScoped<DocumentoService>();
            services.AddScoped<ProfissionalService>();
            services.AddScoped<SolicitacaoOrcamentoService>();

            return services;
        }
    }
}

