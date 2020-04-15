using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StoreParser.Data.Repositories
{
    public class ItemsRepository : IRepository<Item>
    {
        private EntityFrameworkContext db;

        public ItemsRepository(EntityFrameworkContext context)
        {
            this.db = context;
        }

        public IEnumerable<Item> GetAll()
        {
            return db.Items.Include(t => t.Prices);
        }

        public Item Get(int id)
        {
            return db.Items.Include(t => t.Prices).First(i=>i.Id == id);
        }          

        public IEnumerable<Item> Find(Func<Item, Boolean> predicate)
        {
            return db.Items.Include(t => t.Prices).Where(predicate).ToList();
        }

        public Item FindFirst(Func<Item, Boolean> predicate)
        {
            return db.Items.Include(t => t.Prices).FirstOrDefault(predicate);
        }

        public void Create(Item item)
        {
            db.Items.Add(item);
        }

        public void CreateAll(IEnumerable<Item> items)
        {
            db.Items.AddRange(items);
        }
        //TODO to implement methods
        public Task<IEnumerable<Item>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Item> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Item>> FindAsync(Func<Item, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Item> FindFirstAsync(Func<Item, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Item I)
        {
            throw new NotImplementedException();
        }

        public Task CreateAllAsync(IEnumerable<Item> Is)
        {
            throw new NotImplementedException();
        }
    }
}
