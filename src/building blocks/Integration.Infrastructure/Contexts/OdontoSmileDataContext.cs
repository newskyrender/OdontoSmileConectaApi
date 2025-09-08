using Microsoft.EntityFrameworkCore;
using Integration.Domain.Entities;
using Integration.Infrastructure.Mappings;
using Integration.Domain.Common;

namespace Integration.Infrastructure.Contexts
{
    public class OdontoSmileDataContext : DbContext
    {
        public OdontoSmileDataContext() { }

        public OdontoSmileDataContext(DbContextOptions<OdontoSmileDataContext> options) : base(options) { }

        // DbSets das entidades principais
        public DbSet<Fake> Fakes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<SolicitacaoOrcamento> SolicitacoesOrcamento { get; set; }
        public DbSet<PlanejamentoDigital> PlanejamentosDigitais { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

        // DbSets das entidades relacionais
        public DbSet<ProfissionalEspecialidade> ProfissionalEspecialidades { get; set; }
        public DbSet<ProfissionalEquipamento> ProfissionalEquipamentos { get; set; }
        public DbSet<ProfissionalFacilidade> ProfissionalFacilidades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                // Usando as credenciais do Railway
                options.UseMySql("Server=turntable.proxy.rlwy.net;Port=36043;Database=dbSmileConecta;User=root;Password=JFtefKitylMhYWDieeJDyVqyrstLVJWF;SslMode=None;AllowPublicKeyRetrieval=true;", 
                    new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ignorar a entidade Notification do FluentValidator
            modelBuilder.Ignore<FluentValidator.Notification>();
            
            // Aplicar configurações de mapeamento
            modelBuilder.ApplyConfiguration(new FakeMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new PacienteMap());
            modelBuilder.ApplyConfiguration(new ProfissionalMap());
            modelBuilder.ApplyConfiguration(new ProfissionalEspecialidadeMap());
            modelBuilder.ApplyConfiguration(new ProfissionalEquipamentoMap());
            modelBuilder.ApplyConfiguration(new ProfissionalFacilidadeMap());
            modelBuilder.ApplyConfiguration(new SolicitacaoOrcamentoMap());
            modelBuilder.ApplyConfiguration(new PlanejamentoDigitalMap());
            modelBuilder.ApplyConfiguration(new AgendamentoMap());
            modelBuilder.ApplyConfiguration(new DocumentoMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new CompanyMap());
            modelBuilder.ApplyConfiguration(new BankMap());
            modelBuilder.ApplyConfiguration(new BankAccountMap());

            // Configurações globais
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Configurar propriedades string com tamanho padrão
                var stringProperties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(string));

                foreach (var property in stringProperties)
                {
                    if (property.GetMaxLength() == null)
                    {
                        property.SetMaxLength(255);
                    }
                }

                // Configurar propriedades DateTime para UTC
                var dateTimeProperties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

                foreach (var property in dateTimeProperties)
                {
                    property.SetColumnType("DATETIME");
                }

                // Configurar propriedades decimal
                var decimalProperties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));

                foreach (var property in decimalProperties)
                {
                    if (property.GetPrecision() == null)
                    {
                        property.SetPrecision(10);
                        property.SetScale(2);
                    }
                }
            }

            // Seed data inicial (usuário admin)
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario(Guid.NewGuid(), "admin@odontosmileddigital.com",
                    "$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi",
                    "Administrador Sistema", true)
            );

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // As colunas created_at e updated_at são gerenciadas pelo banco de dados
            // com DEFAULT CURRENT_TIMESTAMP e ON UPDATE CURRENT_TIMESTAMP
            // Não precisamos definir os valores manualmente
            
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}