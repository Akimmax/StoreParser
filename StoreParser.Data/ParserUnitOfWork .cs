using StoreParser.Data.Repositories;

namespace StoreParser.Data
{
    public class ParserUnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext dbContext;

        public ParserUnitOfWork(DatabaseContext context)
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
