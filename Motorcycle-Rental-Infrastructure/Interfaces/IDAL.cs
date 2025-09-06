using System.Linq.Expressions;

namespace Motorcycle_Rental_Infrastructure.Interfaces
{
    public interface IDAL<T> where T : class
    {
        public Task<ICollection<T>> ToListAsync();
        public Task<T> RegisterAsync(T objeto);

        public Task<T> UpdateAsync(T objeto);

        public Task DeleteAsync(T objeto);

        public Task<T?> RecoverByAsync(Expression<Func<T, bool>> condicao);

    }
}
