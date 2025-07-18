using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Integration.Domain.Entities;
// using Seven.Core.Lib.Constants; - Temporarily disabled

namespace Integration.Infrastructure.Mappings
{
    public class FakeMap : IEntityTypeConfiguration<Fake>
    {
        public void Configure(EntityTypeBuilder<Fake> entity)
        {
            //Entity
            entity.ToTable("Fake");
            entity.HasKey(x => x.Id);

            //Properties
            entity.Property(x => x.Nome).IsRequired().HasMaxLength(255).HasColumnType("VARCHAR");
            entity.Property(x => x.Email).IsRequired().HasMaxLength(1000).HasColumnType("VARCHAR");

            //Ignore equivalent NotMapping
            // entity.Ignore(x => x.Notifications); - Temporarily disabled

            //Relationchip cardinality
        }
    }
}

