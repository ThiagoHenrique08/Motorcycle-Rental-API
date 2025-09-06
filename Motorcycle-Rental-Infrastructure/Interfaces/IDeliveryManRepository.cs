using Motorcycle_Rental_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Infrastructure.Interfaces
{
    public interface IDeliveryManRepository : IDAL<DeliveryMan>
    {
        Task<bool> ExistsByCNPJAsync(string cnpj);
        Task<bool> ExistsByCNHAsync(string cnh);
    }
}
