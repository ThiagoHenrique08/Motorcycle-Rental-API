namespace Motorcycle_Rental_Domain.Models
{
    public class ApplicationUserRole
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public string? RoleId { get; set; }
        public virtual ApplicationRole? Role { get; set; }
    }
}
