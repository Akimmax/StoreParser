using HtmlAgilityPack;
using StoreParser.Data;
using StoreParser.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StoreParser.Business
{
    public class CitrusParserStategy : IParserStategy//2) Implemention of interface
    {
        readonly IList<ParseResultItem> itemsList = new List<ParseResultItem>();
        private bool isGetsAllItem = false;
        private string correctedUrl;
        private Regex regexDeleteDuplicatedPrefix = new Regex("\\/.*?\\/");

        public IList<ParseResultItem> GetAllItems(string url)//1) Encapsulation, prevents access to implementation details. Strategy provide one public method to using
        {
            return GetAllItems(url, Int32.MaxValue);
        }

        public IList<ParseResultItem> GetAllItems(string url, int limit)
        {
            SetCorrectedUrl(url);
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            GetAllItems(doc, limit);

            return itemsList;
        }

        public IList<ParseResultItem> GetAllItems(HtmlDocument document, int limit)
        {
            HtmlWeb web = new HtmlWeb();
            string nextPageUrl = null;
            HtmlDocument doc;

            doc = document;
            GetPageItems(doc, limit);
            nextPageUrl = FindNextPageUrl(doc);

            while (nextPageUrl != null && !isGetsAllItem)
            {
                doc = web.Load(nextPageUrl);
                GetPageItems(doc, limit);
                nextPageUrl = FindNextPageUrl(doc);
            }

            return itemsList;
        }

        private IList<ParseResultItem> GetPageItems(HtmlDocument doc, int limit)
        {
            var items = doc.DocumentNode.SelectNodes("//*[@class='catalog__items']/div");

            foreach (var item in items)
            {
                if (itemsList.Count >= limit)
                {
                    isGetsAllItem = true;
                    return itemsList;
                }

                string codeSring = item.SelectSingleNode(".//div[@class='product-card__code']").InnerHtml;
                string code = codeSring.Trim('\n').Substring(5).Trim();

                string description = item.SelectSingleNode(".//div[@class='product-card__preview']/a").Attributes["title"].Value;

                string relativeUrl = item.SelectSingleNode(".//div[@class='product-card__name']/a").Attributes["href"].Value;
                string url = correctedUrl + regexDeleteDuplicatedPrefix.Replace(relativeUrl, string.Empty);

                string imageSource = item.SelectSingleNode(".//div[@class='product-card__preview']/a/img").Attributes["data-src"].Value;

                string priceSring = item.SelectSingleNode(".//div[@class='prices__price']/span[@class='price']").InnerHtml;
                double price = Convert.ToDouble(priceSring.Replace(" ", string.Empty));

                itemsList.Add(new ParseResultItem
                {
                    Code = code,
                    ImageSource = imageSource,
                    ProductUrl = url,
                    Description = description,
                    Price = price
                });
            }

            return itemsList;
        }

        private string FindNextPageUrl(HtmlDocument doc)
        {
            var currentPagePointer = doc.DocumentNode.SelectSingleNode("//*[@class='pagination-container']/ul/li[@class='active skip']");

            if (currentPagePointer == null || currentPagePointer.NextSibling == null)
            {
                return null;
            }

            string relativeNextPageUrl = currentPagePointer.NextSibling.ChildNodes[0].Attributes["href"].Value;
            string correctedRelativeNextPageUrl = regexDeleteDuplicatedPrefix.Replace(relativeNextPageUrl, string.Empty);
            string nextPageUrl = correctedUrl + correctedRelativeNextPageUrl;

            return nextPageUrl;
        }

        private string SetCorrectedUrl(string url)//get url without parametres in end
        {
            int index = url.LastIndexOf("/");
            if (index > 0)
            {
                correctedUrl = url.Substring(0, index + 1);
            }
                
            return correctedUrl;
        }
        
    }

}
