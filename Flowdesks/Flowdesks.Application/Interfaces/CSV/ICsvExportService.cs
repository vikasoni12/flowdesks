namespace Flowdesks.Application.Interfaces.CSV;

public interface ICsvExportService
{
    Task<MemoryStream> ExportToCsv<T>(IEnumerable<T> records);
}