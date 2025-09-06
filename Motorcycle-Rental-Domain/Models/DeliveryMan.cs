using System.ComponentModel.DataAnnotations;

namespace Motorcycle_Rental_Domain.Models
{
    public class DeliveryMan
    {
        [Key]
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateTime BirthDate { get; set; }
        public string CNHNumber { get; set; }
        public string CNHType { get; set; }
        public string? CNHImage { get; set; }

        public Location Location { get; set; }
    }
}
