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

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseLazyLoadingProxies(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FakeMap());
        }
    }
}

