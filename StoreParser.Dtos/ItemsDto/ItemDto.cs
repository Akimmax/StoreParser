using System;
using System.Collections.Generic;

namespace StoreParser.Dtos
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ImageSource { get; set; }
        public string ProductUrl { get; set; }
        public string Description { get; set; }

        public IEnumerable<PriceDto> Prices { get; set; }
    }
}
