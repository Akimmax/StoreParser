using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace StoreParser.Data
{
    public class MongoContext 
    {
        public MongoContext(string connectionString)
        {
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
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
