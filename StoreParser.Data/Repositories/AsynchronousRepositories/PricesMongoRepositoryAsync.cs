using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StoreParser.Data.Repositories
{
    public class PricesMongoRepositoryAsync : IRepositoryAsync<Price>
    {
        private MongoContext db;

        public PricesMongoRepositoryAsync(MongoContext context)
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

        public async Task CreateAsync(Price price)
        {
            price.ItemMongoId = price.Item.MongoId;
            await db.Prices.InsertOneAsync(price);
        }





    }
}
