using System;
using System.Collections.Generic;

namespace StoreParser.Data
{
    public class Item
    {
        public Item()
        {
            Prices = new List<Price>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string ImageSource { get; set; }
        public string ProductUrl { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Price> Prices { get; set; }

    }   
}
