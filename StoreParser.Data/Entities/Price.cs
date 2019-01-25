using System;

namespace StoreParser.Data
{
    public class Price
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public DateTimeOffset Date { get; set; }
        public double CurrentPrice { get; set; }
    }
}
