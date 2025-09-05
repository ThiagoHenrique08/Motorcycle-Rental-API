using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Infrastructure.Repository
{
    public class MotorcylceRepository : DAL<Motorcycle>, IMotorcycleRepository
    {
        public MotorcylceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
