using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motorcycle_Rental_Domain.Models;

namespace Motorcycle_Rental_Infrastructure.Configuration
{
    public class DeliveryManConfiguration : IEntityTypeConfiguration<DeliveryMan>
    {
        public void Configure(EntityTypeBuilder<DeliveryMan> builder)
        {
            builder.ToTable("DeliveryMans");


            builder.HasKey(deliveryman => deliveryman.Identifier);

            builder.Property(deliveryman => deliveryman.Identifier)
                   .HasColumnType("VARCHAR(36)")
                   .IsRequired();

            builder.Property(deliveryman => deliveryman.Name)
                   .HasColumnType("VARCHAR(100)")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(deliveryman => deliveryman.CNPJ)
                   .HasColumnType("VARCHAR(14)").HasMaxLength(14)
                   .IsRequired();

            builder.Property(deliveryman => deliveryman.BirthDate)
                   .HasColumnType("timestamp with time zone")
                   .IsRequired();
            builder.Property(deliveryman => deliveryman.CNHNumber)
                    .HasColumnType("VARCHAR(15)").HasMaxLength(15)
                    .IsRequired();
            builder.Property(deliveryman => deliveryman.CNHType)
                    .HasColumnType("VARCHAR(10)")
                    .IsRequired();
            builder.Property(d => d.CNHImage)
                    .HasColumnName("ImageCnh")
                    .HasColumnType("TEXT") // precisa ser TEXT porque o base64 pode ser gigante
                .IsRequired();

            builder.HasIndex(deliveryman => deliveryman.CNPJ).IsUnique();
            builder.HasIndex(deliveryman => deliveryman.CNHNumber).IsUnique();




        }
    }
}
