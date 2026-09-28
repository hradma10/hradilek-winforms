using XMLDataViewer.Core.Utils;

namespace XMLDataViewer.Core.Models;

public record CarSale(string Name, DateTime Date, decimal Cost, decimal Dph)
{
    public decimal CostWithDph => CalcUtils.CalculateDph(Cost, Dph);
}