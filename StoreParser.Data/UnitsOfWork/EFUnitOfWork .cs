using StoreParser.Data.Repositories;

namespace StoreParser.Data
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly EFContext dbContext;

        public EFUnitOfWork(EFContext context)
        {
            dbContext = context;
            Items = new ItemsRepository(context);
            Prices = new PricesRepository(context);
            ItemsAsync = new ItemsRepositoryAsync(context);
            PricesAsync = new PricesRepositoryAsync(context);
        }

        public IRepository<Item> Items { get; }
        public IRepository<Price> Prices { get; }
        public  IRepositoryAsync<Item> ItemsAsync { get; }
        public  IRepositoryAsync<Price> PricesAsync { get; }

        public void Save()
        {
            dbContext.SaveChanges();
        }

    }
}
