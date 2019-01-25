using StoreParser.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StoreParser.Business
{
    public class Parser
    {
        private IParserStategy _strategy;

        public Parser()
        {

        }

        public Parser(IParserStategy strategy)
        {
            this._strategy = strategy;
        }

        public void SetStrategy(IParserStategy strategy)
        {
            this._strategy = strategy;
        }

        public virtual IEnumerable<ParseResultItem> GetAllItems(string url)
        {
            return this._strategy.GetAllItems(url); ;
        }

        public virtual IEnumerable<ParseResultItem> GetAllItems(string url, int limit)
        {    
            return this._strategy.GetAllItems(url, limit); ;
        }
    }
}
