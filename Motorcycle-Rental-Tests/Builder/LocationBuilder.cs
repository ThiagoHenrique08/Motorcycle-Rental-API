using Bogus;
using Motorcycle_Rental_Domain.Enum;
using Motorcycle_Rental_Domain.Models;
using System;

namespace Motorcycle_Rental_Tests.Builder
{
    internal class LocationBuilder
    {
        public string LocationId { get; set; } = Guid.NewGuid().ToString();
        public string DeliveryMan_Id { get; set; }
        public string Motorcycle_Id { get; set; }

        public int DailyValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public Plan Plan { get; set; }
        public DateTime ReturnDate { get; set; }

        public LocationBuilder()
        {
            DeliveryMan_Id = Guid.NewGuid().ToString();
            Motorcycle_Id = Guid.NewGuid().ToString();
            StartDate = DateTime.UtcNow;
            EndDate = StartDate.AddDays(7);
            EstimatedEndDate = StartDate.AddDays(7);
            ReturnDate = StartDate.AddDays(7);
            Plan = Plan.Plan_7Days;
            LocationId = Guid.NewGuid().ToString();
        }
        public LocationBuilder WithLocationId(string locationId)
        {
            LocationId = locationId;
            return this;
        }
        public LocationBuilder WithEntregador_Id(string deliveryMan_Id)
        {
            DeliveryMan_Id = deliveryMan_Id;
            return this;
        }

        public LocationBuilder WithMotorcycle_Id(string motorcycle_Id)
        {
            Motorcycle_Id = motorcycle_Id;
            return this;
        }

        public LocationBuilder WithDailyValue(int dailyValue)
        {
            DailyValue = dailyValue;
            return this;
        }

        public LocationBuilder WithStartDate(DateTime startDate)
        {
            StartDate = startDate;
            return this;
        }

        public LocationBuilder WithEndtDate(DateTime endDate)
        {
            EndDate = endDate;
            return this;
        }

        public LocationBuilder WithEstimatedEndDate(DateTime estimatedEndDate)
        {
            EstimatedEndDate = estimatedEndDate;
            return this;
        }

        public LocationBuilder WithReturnDate(DateTime returnDate)
        {
            ReturnDate = returnDate;
            return this;
        }

        public LocationBuilder WithPlan(Plan plan)
        {
            Plan = plan;
            return this;
        }

        public Location Build()
        {

            // return new  CreateLocationDTO(DeliveryMan_Id, Motorcycle_Id, StartDate, EndDate, EstimatedEndDate, Plan);
            return new Location
            {
                LocationId = LocationId,
                DeliveryMan_Id = this.DeliveryMan_Id,
                Motorcycle_Id = this.Motorcycle_Id,
                StartDate = this.StartDate,
                DailyValue = this.DailyValue,
                EndDate = this.EndDate,
                EstimatedEndDate = this.EstimatedEndDate,
                ReturnDate = this.ReturnDate,
                Plan = this.Plan
            };
        }
    }
}

