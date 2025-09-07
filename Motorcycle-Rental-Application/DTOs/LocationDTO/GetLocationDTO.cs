using Motorcycle_Rental_Domain.Enum;
using Motorcycle_Rental_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Application.DTOs.LocationDTO
{
    public class GetLocationDTO()
    {
        public string LocationId { get; set; }
        public int DailyValue { get; set; }
        public string DeliveryMan_Id { get; set; }
        public string Motorcycle_Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public DateTime ReturnDate { get; set; }

        
    }


}
