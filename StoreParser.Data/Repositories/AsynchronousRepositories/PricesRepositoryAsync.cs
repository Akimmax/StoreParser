using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StoreParser.Data.Repositories
{
    public class PricesRepositoryAsync : IRepositoryAsync<Price>
    {
        private EFContext db;

        public PricesRepositoryAsync(EFContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<Price>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Price> FindFirstAsync(Expression<Func<Item, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Price price)
        {
            db.Prices.Add(price);
            return Task.CompletedTask;
        }
    }
}
