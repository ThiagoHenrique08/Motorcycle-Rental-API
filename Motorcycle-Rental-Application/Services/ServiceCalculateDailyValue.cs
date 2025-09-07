using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Application.Interfaces.Services;
using Motorcycle_Rental_Domain.Enum;
using Motorcycle_Rental_Infrastructure.Interfaces;
using System;
using System.Numerics;

namespace Motorcycle_Rental_Application.Services
{
    public class ServiceCalculateDailyValue : IServiceCalculateDailyValue
    {
        private readonly ILocationRepository _locationRepository;

        public const int plan7 = 30;
        public const int plan15 = 28;
        public const int plan30 = 22;
        public const int plan45 = 20;
        public const int plan50 = 18;
        public const int Additional = 50;
        public ServiceCalculateDailyValue(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }
        public async Task<int> CalculatorDailyValue(DateTime? returnDate, DateTime estimatedEndDate, Plan plan)
        {
            var planValue = plan switch
            {
                Plan.Plan_7Days => plan7 * (int)plan,
                Plan.Plan_15Days => plan15 * (int)plan,
                Plan.Plan_30Days => plan30 * (int)plan,
                Plan.Plan_45Days => plan45 * (int)plan,
                Plan.Plan_50Days => plan50 * (int)plan,
                _ => throw new ArgumentOutOfRangeException(nameof(plan), plan, "Plano inválido")
            };

            if (returnDate is null)
                return planValue;

            var daysDifference = (returnDate.Value - estimatedEndDate).Days;

            return returnDate < estimatedEndDate
                ? plan switch
                {
                    Plan.Plan_7Days => planValue + (int)(plan7 * Math.Abs(daysDifference) * 0.20),
                    Plan.Plan_15Days => planValue + (int)(plan15 * Math.Abs(daysDifference) * 0.40),
                    _ => planValue // demais planos não têm multa
                }
                : returnDate > estimatedEndDate
                ? planValue + daysDifference * Additional // diária extra fixa
                : planValue; // devolveu na data prevista
        }


    }
}
