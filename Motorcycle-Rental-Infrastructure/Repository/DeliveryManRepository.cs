using MassTransit;
using Microsoft.EntityFrameworkCore;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Infrastructure.Repository
{
    public class DeliveryManRepository : DAL<DeliveryMan>, IDeliveryManRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DeliveryManRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;

        }
        // Métodos específicos da interface
        public async Task<bool> ExistsByCNPJAsync(string cnpj)
        {
            return await _dbContext.DeliveryMans.AnyAsync(d => d.CNPJ == cnpj);
        }

        public async Task<bool> ExistsByCNHAsync(string cnh)
        {
            return await _dbContext.DeliveryMans.AnyAsync(d => d.CNHNumber == cnh);
        }

    }

}

