using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
