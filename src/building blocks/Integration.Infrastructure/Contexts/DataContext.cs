using Microsoft.EntityFrameworkCore;
using Integration.Domain.Entities;
using Integration.Infrastructure.Mappings;

namespace Integration.Infrastructure.Contexts
{
    public class IntegrationDataContext : DbContext
    {
        public IntegrationDataContext() { }

        public IntegrationDataContext(DbContextOptions<IntegrationDataContext> options) : base(options) { }

        public DbSet<Fake> FakeEntities { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<ProfissionalEspecialidade> ProfissionaisEspecialidades { get; set; }
        public DbSet<ProfissionalEquipamento> ProfissionaisEquipamentos { get; set; }
        public DbSet<ProfissionalFacilidade> ProfissionaisFacilidades { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<PlanejamentoDigital> PlanejamentosDigitais { get; set; }
        public DbSet<SolicitacaoOrcamento> SolicitacoesOrcamento { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                // Usando as novas credenciais fornecidas
                options.UseMySql("Server=turntable.proxy.rlwy.net;Port=36043;Database=dbSmileConecta;User=root;Password=JFtefKitylMhYWDieeJDyVqyrstLVJWF;SslMode=None;AllowPublicKeyRetrieval=true;", 
                    new MySqlServerVersion(new Version(8, 0, 21)));
            }
            //options.UseLazyLoadingProxies(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ignorar a entidade Notification do FluentValidator
            modelBuilder.Ignore<FluentValidator.Notification>();
            
            // Aplicar todas as configurações de mapeamento
            modelBuilder.ApplyConfiguration(new FakeMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new PacienteMap());
            modelBuilder.ApplyConfiguration(new ProfissionalMap());
            modelBuilder.ApplyConfiguration(new ProfissionalEspecialidadeMap());
            modelBuilder.ApplyConfiguration(new ProfissionalEquipamentoMap());
            modelBuilder.ApplyConfiguration(new ProfissionalFacilidadeMap());
            modelBuilder.ApplyConfiguration(new AgendamentoMap());
            modelBuilder.ApplyConfiguration(new PlanejamentoDigitalMap());
            modelBuilder.ApplyConfiguration(new SolicitacaoOrcamentoMap());
            modelBuilder.ApplyConfiguration(new DocumentoMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new CompanyMap());
            modelBuilder.ApplyConfiguration(new BankMap());
            modelBuilder.ApplyConfiguration(new BankAccountMap());
        }
    }
}

