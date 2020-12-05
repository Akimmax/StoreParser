using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreParser.Data.Repositories
{
    public class PricesRepository : IRepository<Price>
    {
        private EntityFrameworkContext db;

        public PricesRepository(EntityFrameworkContext context)
        {
            this.db = context;
        }

        public IEnumerable<Price> GetAll()
        {
            return db.Prices;
        }

        public Price Get(int id)
        {
            return db.Prices.Find(id);
        }   

        public IEnumerable<Price> Find(Func<Price, Boolean> predicate)
        {
            return db.Prices.Where(predicate).ToList();
        }

        public Price FindFirst(Func<Price, Boolean> predicate)
        {
            return db.Prices.FirstOrDefault(predicate);
        }

        public void Create(Price price)
        {
            db.Prices.Add(price);
        }

        public void CreateAll(IEnumerable<Price> prices)
        {
            db.Prices.AddRange(prices);
        }
        //TODO to implement methods
        public Task<IEnumerable<Price>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Price> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Price>> FindAsync(Func<Price, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Price> FindFirstAsync(Func<Price, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Price I)
        {
            throw new NotImplementedException();
        }

        public Task CreateAllAsync(IEnumerable<Price> Is)
        {
            throw new NotImplementedException();
        }
    }
}
