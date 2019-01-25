using Xunit;
using StoreParser.Dtos;
using StoreParser.Business;
using HtmlAgilityPack;

namespace StoreParser.Test
{
    public class ParserTest
    {
        [Fact]
        public void Should_Get_All_Items_FromPage()//Inner implementations methods shold be inaccessible, because test work of one public method "GetAllItems"
        {
            CitrusParserStategy ps = new Business.CitrusParserStategy();
            HtmlDocument doc = new HtmlDocument();
            doc.Load("AppleWatchTestFile.html");//use prepared local html file            

        
            var items = ps.GetAllItems(doc, 10);

            Assert.NotEmpty(items);
            Assert.Equal(3, items.Count);
            Assert.Equal(9999, items[0].Price);
            Assert.Equal(14999, items[1].Price);
            Assert.Equal(14999, items[2].Price);
        }

    }
}
