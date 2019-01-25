using StoreParser.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StoreParser.Business
{
    public interface IParserStategy//4) Interface
    {
        IList<ParseResultItem> GetAllItems(string url, int limit);
        IList<ParseResultItem> GetAllItems(string url);
    }
}
