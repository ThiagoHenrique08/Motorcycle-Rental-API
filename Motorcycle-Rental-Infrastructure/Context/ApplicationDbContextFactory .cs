using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MotorcycleRental.Infrastructure
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Connection string igual ao docker-compose
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=dbMotorcycleRental;Username=postgres;Password=Criativos123@");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
