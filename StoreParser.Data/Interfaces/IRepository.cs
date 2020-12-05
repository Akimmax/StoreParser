using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoreParser.Data
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        T FindFirst(Func<T, Boolean> predicate);
        void Create(T item);
        void CreateAll(IEnumerable<T> items);

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> FindAsync(Func<T, Boolean> predicate);
        Task<T> FindFirstAsync(Func<T, Boolean> predicate);
        Task CreateAsync(T I);
        Task CreateAllAsync(IEnumerable<T> Is);
    }
}
