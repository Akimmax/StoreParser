using StoreParser.Data.Repositories;

namespace StoreParser.Data
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private readonly MongoContext dbContext;

        public MongoUnitOfWork(MongoContext context)
        {
            dbContext = context;
            Items = new MongoItemsRepository(dbContext); 
            Prices = null;
        }

        public IRepository<Item> Items { get; }
        public IRepository<Price> Prices { get; }

        public void Save()
        {

        }
    }
}
