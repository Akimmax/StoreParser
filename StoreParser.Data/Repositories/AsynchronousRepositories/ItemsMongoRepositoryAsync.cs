using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace StoreParser.Data.Repositories
{
    public class ItemsMongoRepositoryAsync : IRepositoryAsync<Item>
    {
        private MongoContext db;

        public ItemsMongoRepositoryAsync(MongoContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {            
            var items = await db.Items.Find(new BsonDocument() { }).ToListAsync();

            foreach (var item in items.AsEnumerable())
            {
                var prices = await db.Prices.Find(c => c.ItemMongoId == item.MongoId).ToListAsync();
                item.Prices = prices;
            }
            return items;
        }            

        public async Task<Item> FindFirstAsync(Expression<Func<Item, bool>> predicate)
        {            
            var item = db.Items.AsQueryable<Item>().FirstOrDefault(predicate.Compile());

            if (item != null)
            {
                var prices = await db.Prices.Find(c => c.ItemMongoId == item.MongoId).ToListAsync();
                item.Prices = prices;
            }

            return item;
        }
        
        public async Task CreateAsync(Item item)
        {            
            await db.Items.InsertOneAsync(item);
        }
       
    }
}
