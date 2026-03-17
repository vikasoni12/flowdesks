using Flowdesks.Application.Interfaces.CSV;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Flowdesks.Infrastructure.Services.CSV;

public class CsvExportService : ICsvExportService
{
    public async Task<MemoryStream> ExportToCsv<T>(IEnumerable<T> records)
    {
        var stream = new MemoryStream();
        using (var writeFile = new StreamWriter(stream, leaveOpen: true))
        {
            var csv = new CsvWriter(writeFile, new CsvConfiguration(new CultureInfo("en-US")));
            await csv.WriteRecordsAsync(records);
        }
        stream.Position = 0;
        return stream;
    }
}
