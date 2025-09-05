using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motorcycle_Rental_Domain.Models;

namespace Motorcycle_Rental_Infrastructure.Configuration
{
    internal class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.ToTable("Motorcycles");

  
            builder.HasKey(i => i.Identifier);

            builder.Property(i => i.Identifier)
                   .HasColumnType("VARCHAR(36)")
                   .IsRequired();

            builder.Property(i => i.Year)
                   .IsRequired();

            builder.Property(i => i.Model)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(i => i.Plate)
                   .HasColumnType("VARCHAR(20)")
                   .IsRequired();

            // Garante que a placa seja única
            builder.HasIndex(i => i.Plate).IsUnique();
        }
    }
}
