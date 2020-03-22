using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
    }
}