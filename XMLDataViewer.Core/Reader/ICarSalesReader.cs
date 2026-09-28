using XMLDataViewer.Core.Models;

namespace XMLDataViewer.Core.Reader;

public interface ICarSalesReader
{
    Task<List<CarSale>> ReadXmlAsync(Stream stream, CancellationToken ct);
}

