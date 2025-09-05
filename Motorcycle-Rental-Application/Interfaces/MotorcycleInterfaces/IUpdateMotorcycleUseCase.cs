
using FluentResults;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Application.Interfaces.Motorcycle
{
    public interface IUpdateMotorcycleUseCase : IUseCase<UpdateMotorcycleDTO>
    {
        public Task<Result> ExecuteAsync(UpdateMotorcycleDTO request, string id);
    }
}
