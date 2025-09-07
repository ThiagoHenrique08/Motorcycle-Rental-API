using Motorcycle_Rental_Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Motorcycle_Rental_Domain.Models
{
    public class Location
    {
        [Key]
        public string LocationId { get; set; } = Guid.NewGuid().ToString();
        public Motorcycle? Motorcycle { get; set; }
        public string Motorcycle_Id { get; set; }
        public int DailyValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Plan Plan { get; set; }
        public DeliveryMan? DeliveryMan { get; set; }
        public string DeliveryMan_Id  { get; set; }

    }
}
