using Microsoft.EntityFrameworkCore;

namespace Motorcycle_Rental_Tests.Integration
{
    public class TestDbContextFactory : IDisposable
    {
        public readonly string ConnectionString;

        public TestDbContextFactory()
        {
            // Cria um banco único para cada execução de teste
            var databaseName = $"MotorcycleRental_Test_{Guid.NewGuid():N}";
            ConnectionString = $"Host=localhost;Port=5432;Database={databaseName};Username=postgres;Password=Criativos123@";

            CreateDbContext();
        }

        public ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(ConnectionString) // PostgreSQL aqui!
                .Options;

            var context = new ApplicationDbContext(options);

            //// Garante que o schema do banco seja aplicado
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            //// Se preferir usar migrations em vez de EnsureCreated:
            //// context.Database.Migrate();
            // return context;


            // Ensure the database is created and apply migrations
            if (context.Database is not null)
            {
                context.Database.EnsureDeleted();
            }
            context.Database.EnsureCreated();

            return context;
        
        }

        public void Dispose()
        {
            // Remove o banco de testes depois da execução
            using (var context = CreateDbContext())
            {
                context.Database.EnsureDeleted();
            }
        }
    }


}
