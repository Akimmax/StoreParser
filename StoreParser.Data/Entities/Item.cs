using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreParser.Data
{
    public class Item
    {
        public Item()
        {
            Prices = new List<Price>();
        }

        [NotMapped]
        public int Id { get; set; }

        [BsonId]
        [NotMapped]        
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; set; }

        public string Code { get; set; }
        public string ImageSource { get; set; }
        public string ProductUrl { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public virtual ICollection<Price> Prices { get; set; }

    }   
}
