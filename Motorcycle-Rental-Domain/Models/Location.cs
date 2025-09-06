using Motorcycle_Rental_Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Motorcycle_Rental_Domain.Models
{
    public class Location
    {
        [Key]
        public Guid LocationId { get; set; } = Guid.NewGuid();
       
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public Plan Plan { get; set; }

        public DeliveryMan? DeliveryMan { get; set; }
        public string EntregadorId  { get; set; }
        
    }
}
