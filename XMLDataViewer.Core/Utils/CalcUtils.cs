using XMLDataViewer.Core.Models;

namespace XMLDataViewer.Core.Utils;

public static class CalcUtils
{
    public static List<WeekendSalesSummary> CalculateWeekendSalesSummaries(IEnumerable<CarSale> sales)
    {
        ArgumentNullException.ThrowIfNull(sales);

        return sales
            .Where(s => s.Date.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday)
            .GroupBy(s => s.Name)
            .Select(g => new WeekendSalesSummary
            (
                ModelName: g.Key,
                TotalCostWithoutDph: g.Sum(s => s.Cost),
                TotalCostWithDph: g.Sum(s => s.CostWithDph)
            ))
            .OrderBy(s => s.ModelName)
            .ToList();
    }

    public static decimal CalculateDph(decimal cost, decimal dph) => (1m + (dph / 100m)) * cost;
}