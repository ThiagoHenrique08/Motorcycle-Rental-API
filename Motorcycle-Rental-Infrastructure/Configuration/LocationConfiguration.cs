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

            builder.Property(location => location.LocationId).HasColumnType("VARCHAR(100)").ValueGeneratedOnAdd();
            builder.Property(location => location.DailyValue).HasColumnType("integer");
            builder.Property(location => location.StartDate).HasColumnType("timestamp with time zone").IsRequired();

            builder.Property(location => location.EndDate).HasColumnType("timestamp with time zone").IsRequired();

            builder.Property(location => location.EstimatedEndDate).HasColumnType("timestamp with time zone").IsRequired();
            
            builder.Property(location => location.ReturnDate).HasColumnType("timestamp with time zone").IsRequired();
            
            builder.Property(location => location.Plan).IsRequired();


            builder.Property(location => location.DeliveryMan_Id).IsRequired();

            builder.HasOne(location => location.DeliveryMan).WithOne(dm => dm.Location).HasForeignKey<Location>(location => location.DeliveryMan_Id);

            builder.HasIndex(location => location.DeliveryMan_Id).IsUnique();

           
            builder.Property(location => location.Motorcycle_Id).IsRequired();

            builder.HasOne(location => location.Motorcycle).WithOne(dm => dm.Location).HasForeignKey<Location>(location => location.Motorcycle_Id);

            builder.HasIndex(location => location.Motorcycle_Id).IsUnique();

        }
    }
}
