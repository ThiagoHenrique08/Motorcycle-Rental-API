using Motorcycle_Rental_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Infrastructure.Interfaces
{
    public interface IMotorcycleNotificationRepository : IEFRepository<MotorcycleNotification>
    {
    }
}
