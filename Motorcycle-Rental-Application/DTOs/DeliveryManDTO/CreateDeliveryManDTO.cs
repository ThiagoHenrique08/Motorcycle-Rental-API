using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Application.DTOs.DeliveryManDTO
{
    public class CreateDeliveryManDTO(string identifier, string name, string cNPJ, DateTime birthDate, string cNHNumber, string cNHType, string cNHImage)
    {


        public string Identifier { get; set; } = identifier;
        public string Name { get; set; } = name;
        public string CNPJ { get; set; } = cNPJ;
        public DateTime BirthDate { get; set; } = birthDate;
        public string CNHNumber { get; set; } = cNHNumber;
        public string CNHType { get; set; } = cNHType;
        public string CNHImage { get; set; } = cNHImage;
    }
}
