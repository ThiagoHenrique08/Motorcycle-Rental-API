using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motorcycle_Rental_Domain.Models;

namespace Motorcycle_Rental_Infrastructure.Configuration
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Locations");


            builder.HasKey(location => location.LocationId);

            builder.Property(location => location.LocationId).HasColumnType("UUID").ValueGeneratedOnAdd();

            builder.Property(location => location.StartDate).HasColumnType("TIMESTAMP ").IsRequired();

            builder.Property(location => location.EndDate).HasColumnType("TIMESTAMP ").IsRequired();

            builder.Property(location => location.EstimatedEndDate).HasColumnType("TIMESTAMP").IsRequired();

            builder.Property(location => location.Plan).IsRequired();



            builder.Property(location => location.EntregadorId).IsRequired();

            builder.HasOne(location => location.DeliveryMan).WithOne(dm => dm.Location).HasForeignKey<Location>(deliveryMan => deliveryMan.EntregadorId);

            builder.HasIndex(location => location.EntregadorId).IsUnique();

        }
    }
}
