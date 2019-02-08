using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StoreParser.Data
{
    public interface IRepositoryAsync<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindFirstAsync(Expression<Func<Item, bool>> predicate);
        Task CreateAsync(T item);
    }
}
