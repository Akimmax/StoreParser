using System;
using System.Collections.Generic;
using System.Text;

namespace StoreParser.Dtos
{
    public class PriceDto
    {
        public DateTimeOffset Date { get; set; }
        public double Price { get; set; }
    }
}
