using Microsoft.EntityFrameworkCore;
using Motorcycle_Rental_Domain.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<MotorcycleNotification> Notifications { get; set; }
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet <DeliveryMan> DeliveryMans { get; set; }
    public DbSet <Location> Locations { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //garante que as configurações da classe base sejam aplicadas antes de aplicar configurações específicas
        base.OnModelCreating(modelBuilder); 

        // Esta configuração global percorre todos os relacionamentos de chave estrangeira no modelo (GetForeignKeys()) e define o comportamento de exclusão como Cascade.
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Cascade;
        }

        /*Escaneia todas as configurações do tipo IEntityTypeConfiguration<T> no assembly onde o ApplLicationDbContext está definido.
        Essas configurações são implementadas em classes separadas para cada entidade e configuradas automaticamente.*/

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);


    }
}