using Microsoft.EntityFrameworkCore;
using Motorcycle_Rental_Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace Motorcycle_Rental_Infrastructure.Repository
{
    public class DAL<T> : IDAL<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public DAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> RegisterAsync(T objeto)
        {
            await _context.Set<T>().AddAsync(objeto);
            await _context.SaveChangesAsync();
            return objeto;
        }

        public async Task<T> UpdateAsync(T objeto)
        {
            _context.Set<T>().Update(objeto);
            await _context.SaveChangesAsync();
            return objeto;
        }

        public async Task DeleteAsync(T objeto)
        {
            _context.Set<T>().Remove(objeto);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<T>> ToListAsync()
        {
            var Lista = await _context.Set<T>().ToListAsync();
            return Lista;
        }

        public async Task<T?> RecoverByAsync(Expression<Func<T, bool>> condicao)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(condicao);

        }


    }

}

