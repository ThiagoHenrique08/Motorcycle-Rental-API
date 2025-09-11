using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace Motorcycle_Rental_Infrastructure.Repository
{
    public class ApplicationUserRoleRepository : DAL<ApplicationUserRole>, IApplicationUserRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserRoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task RemoveRange(IEnumerable<ApplicationUserRole> listObjects)
        {
            _context.Set<ApplicationUserRole>().RemoveRange(listObjects);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationUserRole>> SearchForAsync(Expression<Func<ApplicationUserRole, bool>> condicao)
        {

            return _context.Set<ApplicationUserRole>().AsQueryable().Where(condicao);
        }
    }
}
