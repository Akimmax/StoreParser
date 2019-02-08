using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StoreParser.Data.Repositories
{
    public class ItemsRepository : IRepository<Item>
    {
        private EFContext db;

        public ItemsRepository(EFContext context)
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
       
    }
}
