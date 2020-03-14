using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace StoreParser.Data
{
    public class MongoContext
    {
        public MongoContext(string connectionString)
        {
            var connectionString1 = "mongodb://localhost/test";

            var connection = new MongoUrlBuilder(connectionString1);
            MongoClient client = new MongoClient(connectionString1);
            database = client.GetDatabase(connection.DatabaseName);
        }

        IMongoDatabase database;

        public IMongoCollection<Item> Items
        {
            get { return database.GetCollection<Item>("Items"); }
        }

        public IMongoCollection<Price> Prices
        {
            get { return database.GetCollection<Price>("Prices"); }
        }
    }
}
