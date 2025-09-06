using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Application.Interfaces.DeliveryManInterfaces
{
    public interface IUploadCNHImageUseCase
    {
        Task<Result> ExecuteAsync(string deliveryManId, string base64Image, string fileName, string contentType);
    }
}
