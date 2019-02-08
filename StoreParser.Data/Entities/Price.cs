using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreParser.Data
{
    public class Price
    {
        [NotMapped]
        public int Id { get; set; }

        [BsonId]
        [NotMapped]        
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; set; }

        [NotMapped]        
        [BsonRepresentation(BsonType.ObjectId)]
        public string ItemMongoId { get; set; }

        [NotMapped]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public DateTimeOffset Date { get; set; }
        public double CurrentPrice { get; set; }
    }
}
