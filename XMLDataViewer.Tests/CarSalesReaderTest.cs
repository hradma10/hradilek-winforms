using System.Xml;
using XMLDataViewer.Core.Reader;

namespace XMLDataViewer.Tests
{
    public class CarSalesReaderTest
    {
        private readonly ICarSalesReader _reader = new CarSalesReader();

        [Fact]
        public async Task ReadValidXmlSucceedsTest()
        {
            const string xml = """
                               <?xml version="1.0" encoding="utf-8"?>
                               <Sales>
                                   <Sale>
                                       <Name>Škoda Oktávia</Name>
                                       <Date>2010-12-02</Date>
                                       <Cost>500000.00</Cost>
                                       <Dph>20.00</Dph>
                                   </Sale>
                                   <Sale>
                                       <Name>Škoda Felicia</Name>
                                       <Date>2000-12-03</Date>
                                       <Cost>210000.00</Cost>
                                       <Dph>20.00</Dph>
                                   </Sale>
                                   <Sale>
                                       <Name>Škoda Fabia</Name>
                                       <Date>2010-12-04</Date>
                                       <Cost>350000.00</Cost>
                                       <Dph>20.00</Dph>
                                   </Sale>
                                   <Sale>
                                       <Name>Škoda Oktávia</Name>
                                       <Date>2010-12-04</Date>
                                       <Cost>500000.00</Cost>
                                       <Dph>20.00</Dph>
                                   </Sale>
                                   <Sale>
                                       <Name>Škoda Oktávia</Name>
                                       <Date>2010-12-05</Date>
                                       <Cost>500000.00</Cost>
                                       <Dph>20.00</Dph>
                                   </Sale>
                                   <Sale>
                                       <Name>Škoda Fabia</Name>
                                       <Date>2010-12-05</Date>
                                       <Cost>350000.00</Cost>
                                       <Dph>20.00</Dph>
                                   </Sale>
                                   <Sale>
                                       <Name>Škoda Fabia</Name>
                                       <Date>2010-12-06</Date>
                                       <Cost>350000.00</Cost>
                                       <Dph>20.00</Dph>
                                   </Sale>
                                   <Sale>
                                       <Name>Škoda Forman</Name>
                                       <Date>2000-12-04</Date>
                                       <Cost>100000.00</Cost>
                                       <Dph>19.00</Dph>
                                   </Sale>
                                   <Sale>
                                       <Name>Škoda Favorit</Name>
                                       <Date>2000-12-05</Date>
                                       <Cost>80000.00</Cost>
                                       <Dph>19.00</Dph>
                                   </Sale>
                                   <Sale>
                                       <Name>Škoda Forman</Name>
                                       <Date>2000-12-06</Date>
                                       <Cost>100000.00</Cost>
                                       <Dph>19.00</Dph>
                                   </Sale>
                                   <Sale>
                                       <Name>Škoda Felicia</Name>
                                       <Date>2000-12-03</Date>
                                       <Cost>210000.00</Cost>
                                       <Dph>19.00</Dph>
                                   </Sale>
                                   <Sale>
                                       <Name>Škoda Felicia</Name>
                                       <Date>2000-12-02</Date>
                                       <Cost>210000.00</Cost>
                                       <Dph>19.00</Dph>
                                   </Sale>
                                   <Sale>
                                       <Name>Škoda Oktávia</Name>
                                       <Date>2010-12-07</Date>
                                       <Cost>500000.00</Cost>
                                       <Dph>20.00</Dph>
                                   </Sale>
                               </Sales>
                               """;

            await using Stream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            var sales = await _reader.ReadXmlAsync(stream, CancellationToken.None);

            Assert.NotEmpty(sales);
            Assert.Equal(13, sales.Count);

            var firstSale = sales[0];
            var firstDate = new DateTime(day: 2, month: 12, year: 2010);
            Assert.Equal("Škoda Oktávia", firstSale.Name);
            Assert.Equal(firstDate, firstSale.Date);
            Assert.Equal(500000m, firstSale.Cost);
            Assert.Equal(20m, firstSale.Dph);

            var secondSale = sales[1];
            var secondDate = new DateTime(day: 3, month: 12, year: 2000);
            Assert.Equal("Škoda Felicia", secondSale.Name);
            Assert.Equal(secondDate, secondSale.Date);
            Assert.Equal(210000m, secondSale.Cost);
            Assert.Equal(20m, secondSale.Dph);

            var lastSale = sales[^1];
            var lastDate = new DateTime(day: 7, month: 12, year: 2010);
            Assert.Equal("Škoda Oktávia", lastSale.Name);
            Assert.Equal(lastDate, lastSale.Date);
            Assert.Equal(500000m, lastSale.Cost);
            Assert.Equal(20m, lastSale.Dph);
        }

        [Fact]
        public async Task EmptySalesReturnsEmptyListTest()
        {
            const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Sales></Sales>";
            await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            var sales = await _reader.ReadXmlAsync(stream, CancellationToken.None);

            Assert.Empty(sales);
        }

        [Fact]
        public async Task ReadXmlAsyncCancelTaskTest()
        {
            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();
            const string xml = "<Sales><Sale><Name>Fabia</Name><Date>2010-12-04</Date><Cost>100</Cost><Dph>20</Dph></Sale></Sales>";
            await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => _reader.ReadXmlAsync(stream, cts.Token));
        }

        [Fact]
        public async Task InvalidXmlThrowsTest()
        {
            const string xml = """
                               <Sales>
                                   <Sale>
                                       <Name>Škoda Fabia</Name>
                                       <Date>2010-12-04</Date>
                                       <Cost>100.00</Cost>
                                       <Dph>20.00</Dph>
                               </Sales>
                               """;
            await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            var exception =
                await Assert.ThrowsAsync<XmlException>(() => _reader.ReadXmlAsync(stream, CancellationToken.None));

            Assert.NotNull(exception.Message);
        }

        [Theory]
        [InlineData("<Date>31.02.2020</Date><Cost>350000.00</Cost><Dph>20.00</Dph><Name>Fabia</Name>", "Date")]
        [InlineData("<Date>2020-01-01</Date><Cost>cena</Cost><Dph>20.00</Dph><Name>Fabia</Name>", "Cost")]
        [InlineData("<Date>2020-01-01</Date><Cost>-500.00</Cost><Dph>20.00</Dph><Name>Fabia</Name>", "Cost")]
        [InlineData("<Date>2020-01-01</Date><Cost>350000.00</Cost><Dph>dph</Dph><Name>Fabia</Name>", "Dph")]
        [InlineData("<Date>2020-01-01</Date><Cost>350000.00</Cost><Dph>-10.00</Dph><Name>Fabia</Name>", "Dph")]
        [InlineData("<Date>2020-01-01</Date><Cost>350000.00</Cost><Dph>20.00</Dph><Name>   </Name>", "Name")]
        [InlineData("<Date>2020-01-01</Date><Cost>350000.00</Cost><Dph>20.00</Dph><Name></Name>", "Name")]
        public async Task InvalidFieldThrowsTest(string saleXmlContent, string expectedField)
            => await AssertXmlThrowsFormatException(saleXmlContent, expectedField);

        [Theory]
        [InlineData("<Cost>350000.00</Cost><Dph>20.00</Dph><Name>Fabia</Name>", "Date")]
        [InlineData("<Date>2020-01-01</Date><Dph>20.00</Dph><Name>Fabia</Name>", "Cost")]
        [InlineData("<Date>2020-01-01</Date><Cost>350000.00</Cost><Name>Fabia</Name>", "Dph")]
        [InlineData("<Date>2020-01-01</Date><Cost>350000.00</Cost><Dph>20.00</Dph>", "Name")]
        public async Task MissingFieldThrowsTest(string saleXmlContent, string expectedField)
            => await AssertXmlThrowsFormatException(saleXmlContent, expectedField);

        private async Task AssertXmlThrowsFormatException(string saleXmlContent, string expectedField)
        {
            var xml = $"""
                       <?xml version="1.0" encoding="utf-8"?>
                       <Sales>
                           <Sale>
                               {saleXmlContent}
                           </Sale>
                       </Sales>
                       """;

            await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));
            var exception =
                await Assert.ThrowsAsync<FormatException>(() => _reader.ReadXmlAsync(stream, CancellationToken.None));
            Assert.Contains(expectedField, exception.Message);
        }
    }
}