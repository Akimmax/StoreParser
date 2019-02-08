using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreParser.Data.Repositories
{
    public class PricesRepository : IRepository<Price>
    {
        private EFContext db;

        public PricesRepository(EFContext context)
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
    }
}
