

using Microsoft.AspNetCore.Identity;

namespace Motorcycle_Rental_Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpirytime { get; set; }

        public virtual ICollection<ApplicationUserRole>? ApplicationUserRole { get; set; }
    }
}
