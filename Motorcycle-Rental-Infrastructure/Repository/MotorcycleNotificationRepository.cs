using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Infrastructure.Repository
{
    public class MotorcycleNotificationRepository : DAL<MotorcycleNotification>, IMotorcycleNotificationRepository
    {
        public MotorcycleNotificationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
