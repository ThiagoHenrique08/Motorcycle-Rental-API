using Motorcycle_Rental_Domain.Models;
using System.Linq.Expressions;

namespace Motorcycle_Rental_Infrastructure.Interfaces
{
    public interface IApplicationUserRoleRepository : IDAL<ApplicationUserRole>
    {
        public Task RemoveRange(IEnumerable<ApplicationUserRole> listObjects);
        public  Task<IEnumerable<ApplicationUserRole>> SearchForAsync(Expression<Func<ApplicationUserRole, bool>> condicao);
    
    }
}
