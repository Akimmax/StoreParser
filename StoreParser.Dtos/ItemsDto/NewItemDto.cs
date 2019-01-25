using System;
using System.Collections.Generic;

namespace StoreParser.Dtos
{
    public class NewItemDto
    {
        public string ImageSource { get; set; }
        public string ProductUrl { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
