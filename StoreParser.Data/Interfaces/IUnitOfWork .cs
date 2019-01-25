namespace StoreParser.Data
{
    public interface IUnitOfWork
    {
        IRepository<Item> Items { get; }
        IRepository<Price> Prices { get; }

        void Save();
    }
}
