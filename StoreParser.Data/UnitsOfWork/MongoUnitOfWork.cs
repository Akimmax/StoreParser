using StoreParser.Data.Repositories;
using System;

namespace StoreParser.Data
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private readonly MongoContext dbContext;

        public MongoUnitOfWork(MongoContext context)
        {
            dbContext = context;           
            ItemsAsync = new ItemsMongoRepositoryAsync(context);
            PricesAsync = new PricesMongoRepositoryAsync(context);
        }

        public IRepository<Item> Items
        {
            get { throw new NotImplementedException(); }
        }
        public IRepository<Price> Prices
        {
            get { throw new NotImplementedException(); }
        }
        public IRepositoryAsync<Item> ItemsAsync { get; }
        public IRepositoryAsync<Price> PricesAsync { get; }

        public void Save()
        {
          
        }
    }
}
