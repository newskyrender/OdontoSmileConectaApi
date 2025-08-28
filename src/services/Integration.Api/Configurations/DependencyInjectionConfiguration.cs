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
            // Unit of Work
            services.AddScoped<IUow, Uow>();

            // Repositories
            services.AddScoped<IFakeRepository, FakeRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
            services.AddScoped<IProfissionalEspecialidadeRepository, ProfissionalEspecialidadeRepository>();
            services.AddScoped<IProfissionalEquipamentoRepository, ProfissionalEquipamentoRepository>();
            services.AddScoped<IProfissionalFacilidadeRepository, ProfissionalFacilidadeRepository>();
            services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
            services.AddScoped<IPlanejamentoDigitalRepository, PlanejamentoDigitalRepository>();
            services.AddScoped<ISolicitacaoOrcamentoRepository, SolicitacaoOrcamentoRepository>();
            services.AddScoped<IDocumentoRepository, DocumentoRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IRelatorioFinanceiroRepository, RelatorioFinanceiroRepository>();

            // Services
            services.AddScoped<FakeService>();

            return services;
        }
    }
}

