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
            var items = db.Items.Find(new BsonDocument() { }).ToList();
            return items;
        }

        public Item FindFirst(Expression<Func<Item, bool>> predicate)
        {
            var item = db.Items.Find(predicate).FirstOrDefault();
            return item;
        }

        public void Create(Item item)
        {     
            db.Items.ReplaceOne(
                i => i.Id == item.Id, 
                item,
                new UpdateOptions { IsUpsert = true }
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
            throw new NotImplementedException();
        }

        public Item FindFirst(Func<Item, Boolean> predicate)
        {
           var item = db.Items.AsQueryable<Item>().Where(predicate).FirstOrDefault();
            return item;
        }
    }
}