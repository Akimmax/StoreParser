namespace StoreParser.Data
{
    public interface IUnitOfWork
    {
        IRepository<Item> Items { get; }
        IRepository<Price> Prices { get; }
        IRepositoryAsync<Item> ItemsAsync { get; }
        IRepositoryAsync<Price> PricesAsync { get; }

        void Save();
    }
}
