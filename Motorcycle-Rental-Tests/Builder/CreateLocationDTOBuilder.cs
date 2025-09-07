using Motorcycle_Rental_Domain.Enum;
using Motorcycle_Rental_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Tests.Builder
{
    public class CreateLocationDTOBuilder
    {

        public string DeliveryMan_Id { get; set; }
        public string Motorcycle_Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public Plan Plan { get; set; }

        public CreateLocationDTOBuilder()
        {
            DeliveryMan_Id = "entragador1";
            Motorcycle_Id = "moto1";
            StartDate = DateTime.UtcNow;
            EndDate = StartDate.AddDays(7);
            EstimatedEndDate = EndDate;
            Plan = Plan.Plan_7Days;
        }

        public CreateLocationDTOBuilder WithEntregador_Id(string deliveryMan_Id)
        {
            DeliveryMan_Id = deliveryMan_Id;
            return this;
        }

        public CreateLocationDTOBuilder WithMotorcycle_Id(string motorcycle_Id)
        {
            Motorcycle_Id = motorcycle_Id;
            return this;
        }

        public CreateLocationDTOBuilder WithStartDate(DateTime startDate)
        {
            StartDate = startDate;
            return this;
        }

        public CreateLocationDTOBuilder WithEndtDate(DateTime endDate)
        {
            EndDate = endDate;
            return this;
        }

        public CreateLocationDTOBuilder WithEstimatedEndDate(DateTime estimatedEndDate)
        {
            EstimatedEndDate = estimatedEndDate;
            return this;
        }

        public CreateLocationDTOBuilder WithPlan(Plan plan)
        {
            Plan = plan;
            return this;
        }

        public Location Build()
        {
            return new Location
            {
                DeliveryMan_Id = this.DeliveryMan_Id,
                Motorcycle_Id = this.Motorcycle_Id,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                EstimatedEndDate = this.EstimatedEndDate,
                Plan = this.Plan
            };
        }
    }


}