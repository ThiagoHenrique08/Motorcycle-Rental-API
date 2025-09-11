namespace Motorcycle_Rental_Application.DTOs.LoginDTO
{
    public class GetUsersDTO
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
    }
}
