using StoreParser.Data.Repositories;

namespace StoreParser.Data
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private readonly EntityFrameworkContext dbContext;

        public EntityFrameworkUnitOfWork(EntityFrameworkContext context)
        {
            dbContext = context;
            Items = new ItemsRepository(context);           
            Prices = new PricesRepository(context);
        }

        public IRepository<Item> Items { get; }
        public IRepository<Price> Prices { get; }

        public void Save()
        {
            dbContext.SaveChanges();
        }

    }
}
