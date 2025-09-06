using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Application.DTOs.DeliveryManDTO
{
    public class CreateDeliveryManDTO
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateTime BirthDate { get; set; }
        public string CNHNumber { get; set; } 
        public string CNHType { get; set; }
        public string CNHImage { get; set; }
    }
}
