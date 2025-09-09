using Motorcycle_Rental_Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Application.DTOs.LocationDTO
{
    public class CreateLocationDTO(string deliveryMan_Id, string motorcycle_Id, DateTime startDate, DateTime endDate, DateTime estimatedEndDate, Plan plan)
    {

        public string DeliveryMan_Id { get; set; } = deliveryMan_Id;
        public string Motorcycle_Id { get; set; } = motorcycle_Id;
        public DateTime StartDate { get; set; } = startDate;
        public DateTime EndDate { get; set; } = endDate;
        public DateTime EstimatedEndDate { get; set; } = estimatedEndDate;
        public Plan Plan { get; set; } = plan;


    }   
}
