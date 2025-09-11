using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Infrastructure.Repository
{
    public class ApplicationRoleRepository : DAL<ApplicationRole>, IApplicationRoleRepository
    {
        public ApplicationRoleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
