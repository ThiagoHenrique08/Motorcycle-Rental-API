using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motorcycle_Rental_Domain.Models;

namespace Motorcycle_Rental_Infrastructure.Configuration
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasDiscriminator<string>("Discriminator").HasValue<ApplicationRole>("ApplicationRole");
        }
    }
}

