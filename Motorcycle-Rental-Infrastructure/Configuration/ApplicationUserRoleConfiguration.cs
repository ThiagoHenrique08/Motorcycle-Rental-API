using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Motorcycle_Rental_Domain.Models;

namespace Motorcycle_Rental_Infrastructure.Configuration
{
    internal class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.ToTable("ApplicationUserRoles");
            builder.HasKey(e => e.Id);
            builder.Property(p => p.Id).HasColumnType("VARCHAR(100)").ValueGeneratedOnAdd();
            builder.Property(p => p.RoleId).HasColumnName("RoleId").IsRequired(false);
            builder.HasOne(p => p.Role).WithMany(c => c.ApplicationUserRole).HasPrincipalKey(c => c.Id);
            builder.Property(p => p.UserId).HasColumnName("UserId").IsRequired(false);
            builder.HasOne(p => p.User).WithMany(c => c.ApplicationUserRole).HasPrincipalKey(c => c.Id);

        }
    }
}
