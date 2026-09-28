using System.Globalization;
using System.Xml.Linq;
using XMLDataViewer.Core.Models;

namespace XMLDataViewer.Core.Reader;

public class CarSalesReader : ICarSalesReader
{

    public async Task<List<CarSale>> ReadXmlAsync(Stream stream, CancellationToken ct)
    {
        var doc = await XDocument.LoadAsync(stream, LoadOptions.None, ct);

        if (doc.Root == null)
        {
            return [];
        }

        var elements = doc.Root.Elements("Sale");
        var result = new List<CarSale>();

        foreach (var element in elements)
        {

            ct.ThrowIfCancellationRequested();

            var name = (string?)element.Element("Name");
            if (string.IsNullOrWhiteSpace(name))
            {
                ParseFailError("Name");
            }

            if (!DateTime.TryParse((string?)element.Element("Date"), CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                ParseFailError("Date");
            }

            if (!decimal.TryParse((string?)element.Element("Cost"), CultureInfo.InvariantCulture, out var cost) || cost < 0)
            {
                ParseFailError("Cost");

            }

            if (!decimal.TryParse((string?)element.Element("Dph"), CultureInfo.InvariantCulture, out var dph) || dph < 0)
            {
                ParseFailError("Dph");
            }

            result.Add(new CarSale
            (
                Name: name!,
                Date: date,
                Cost: cost,
                Dph: dph
            ));
        }
        return result;

        static void ParseFailError(string field) => throw new FormatException(string.Format(Properties.Resources.FieldError, field));

    }
}

