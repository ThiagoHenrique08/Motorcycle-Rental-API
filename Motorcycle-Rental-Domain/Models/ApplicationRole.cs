using Microsoft.AspNetCore.Identity;

namespace Motorcycle_Rental_Domain.Models
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole>? ApplicationUserRole { get; set; }
    }
}
