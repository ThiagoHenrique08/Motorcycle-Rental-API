using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Infrastructure.Repository
{
    public class UserApplicationRepository : DAL<ApplicationUser>, IUserApplicationRepository
    {
        public UserApplicationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
