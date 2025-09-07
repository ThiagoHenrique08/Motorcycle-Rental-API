using Microsoft.VisualBasic;
using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Domain.Enum;

namespace Motorcycle_Rental_Application.Interfaces.Services
{
    public interface IServiceCalculateDailyValue
    {
        
        Task<int> CalculatorDailyValue (DateTime? returnDate, DateTime estimatedEndDate, Plan plan);


    }
}
