using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StoreParser.Data.Repositories
{
    public class MongoItemsRepository: IRepository<Item>
    {
        private MongoContext db;
        public MongoItemsRepository(MongoContext context)
        {
            this.db = context;
        }

        public IEnumerable<Item> GetAll()
        {
            var items = db.Items.Find(_ => true).ToList();
            return items;
        }

        public void Create(Item item)
        {
            db.Items.ReplaceOne(
                i => i.Id == item.Id, 
                item,
                new UpdateOptions {
                    IsUpsert = true
                    } 
                );
        }

        public IEnumerable<Item> Find(Func<Item, Boolean> predicate)
        {
            return db.Items.AsQueryable<Item>().Where(predicate).ToList();
        }

        public Item Get(int id)
        {
            var item = db.Items.Find(i => i.Id == id).FirstOrDefault();
            return item;
        }

        public void CreateAll(IEnumerable<Item> items)
        {
            var bulkOperations = new List<WriteModel<Item>>();
            foreach (var item in items)
            {
                var upsertOne = new ReplaceOneModel<Item>(
                    Builders<Item>.Filter.Where(x => x.Id == item.Id),
                    item);
                upsertOne.IsUpsert = true;
                bulkOperations.Add(upsertOne);
            }
            db.Items.BulkWrite(bulkOperations);
        }

        public Item FindFirst(Func<Item, Boolean> predicate)
        {
           var item = db.Items.AsQueryable<Item>().Where(predicate).FirstOrDefault();
            return item;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            var items = await db.Items.Find(_ => true).ToListAsync();
            return items;
        }

        public async Task CreateAsync(Item item)
        {
            db.Items.ReplaceOneAsync(
                i => i.Id == item.Id,
                item,
                new UpdateOptions
                {
                    IsUpsert = true
                }
                );
        }

        public async Task<IEnumerable<Item>> FindAsync(Func<Item, Boolean> predicate)
        {
            return await db.Items.AsQueryable<Item>().Where(predicate).ToAsyncEnumerable().ToList();
        }

        public async Task<Item> GetAsync(int id)
        {
            var item = await db.Items.Find(i => i.Id == id).FirstOrDefaultAsync();
            return item;
        }

        public async Task CreateAllAsync(IEnumerable<Item> items)
        {
            var bulkOperations = new List<WriteModel<Item>>();
            foreach (var item in items)
            {
                var upsertOne = new ReplaceOneModel<Item>(
                    Builders<Item>.Filter.Where(x => x.Id == item.Id),
                    item);
                upsertOne.IsUpsert = true;
                bulkOperations.Add(upsertOne);
            }
            await db.Items.BulkWriteAsync(bulkOperations);
        }

        public async Task<Item> FindFirstAsync(Func<Item, Boolean> predicate)
        {
            // var item = await db.Items.Find(i => predicate()).FirstOrDefaultAsync();
            var item = await db.Items.AsQueryable<Item>().Where(predicate).ToAsyncEnumerable().FirstOrDefault();
            return item;
        }
    }
}