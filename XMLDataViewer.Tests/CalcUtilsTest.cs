using XMLDataViewer.Core.Models;
using XMLDataViewer.Core.Utils;

namespace XMLDataViewer.Tests;

public class CalcUtilsTest
{
    [Fact]
    public void ValidWeekendSalesSummariesTest()
    {
        var sales = new List<CarSale>
        {
            new("Škoda Oktávia", new DateTime(2010, 12, 2), 500000m, 20m),
            new("Škoda Felicia", new DateTime(2000, 12, 3), 210000m, 20m),
            new("Škoda Fabia", new DateTime(2010, 12, 4), 350000m, 20m),
            new("Škoda Oktávia", new DateTime(2010, 12, 4), 500000m, 20m),
            new("Škoda Oktávia", new DateTime(2010, 12, 5), 500000m, 20m),
            new("Škoda Fabia", new DateTime(2010, 12, 5), 350000m, 20m),
            new("Škoda Fabia", new DateTime(2010, 12, 6), 350000m, 20m),
            new("Škoda Forman", new DateTime(2000, 12, 4), 100000m, 19m),
            new("Škoda Favorit", new DateTime(2000, 12, 5), 80000m, 19m),
            new("Škoda Forman", new DateTime(2000, 12, 6), 100000m, 19m),
            new("Škoda Felicia", new DateTime(2000, 12, 3), 210000m, 19m),
            new("Škoda Felicia", new DateTime(2000, 12, 2), 210000m, 19m),
            new("Škoda Oktávia", new DateTime(2010, 12, 7), 500000m, 20m)
        };

        var summaries = CalcUtils.CalculateWeekendSalesSummaries(sales);

        Assert.Equal(3, summaries.Count);

        Assert.Equal("Škoda Fabia", summaries[0].ModelName);
        Assert.Equal(700000m, summaries[0].TotalCostWithoutDph);
        Assert.Equal(840000m, summaries[0].TotalCostWithDph);

        Assert.Equal("Škoda Felicia", summaries[1].ModelName);
        Assert.Equal(630000m, summaries[1].TotalCostWithoutDph);
        Assert.Equal(751800m, summaries[1].TotalCostWithDph);

        Assert.Equal("Škoda Oktávia", summaries[2].ModelName);
        Assert.Equal(1000000m, summaries[2].TotalCostWithoutDph);
        Assert.Equal(1200000m, summaries[2].TotalCostWithDph);
    }

    [Fact]
    public void NullInputSummaryThrowsArgumentNullExceptionTest()
    {
        Assert.Throws<ArgumentNullException>(() => CalcUtils.CalculateWeekendSalesSummaries(null!));
    }

    [Fact]
    public void TestOnlyWeekdaySalesReturnsEmpty()
    {
        var weekdaySales = new List<CarSale>
        {
            new("Škoda Oktávia", new DateTime(2010, 12, 2), 500000m, 20m),
            new("Škoda Forman", new DateTime(2000, 12, 4), 100000m, 19m),
            new("Škoda Favorit", new DateTime(2000, 12, 5), 80000m, 19m),
            new("Škoda Forman", new DateTime(2000, 12, 6), 100000m, 19m),
            new("Škoda Oktávia", new DateTime(2010, 12, 7), 500000m, 20m)
        };

        var result = CalcUtils.CalculateWeekendSalesSummaries(weekdaySales);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(100000, 20, 120000)]
    [InlineData(500000, 21, 605000)]
    [InlineData(100000, 0, 100000)]
    [InlineData(0, 20, 0)]
    public void CalculateDphCalculatesCorrectly(decimal cost, decimal dph, decimal expected)
    {
        var result = CalcUtils.CalculateDph(cost, dph);

        Assert.Equal(expected, result);
    }
}