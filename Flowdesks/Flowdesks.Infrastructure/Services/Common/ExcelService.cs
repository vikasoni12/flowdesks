using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Shared.Wrapper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Drawing;

namespace Flowdesks.Infrastructure.Services.Common
{
    public class ExcelService : IExcelService
    {
        private readonly IStringLocalizer<ExcelService> _localizer;
        private readonly ILogger<ExcelService> _logger;

        public ExcelService(IStringLocalizer<ExcelService> localizer, ILogger<ExcelService> logger)
        {
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<string> ExportAsync<TData>(IEnumerable<TData> data, Dictionary<string, Func<TData, object>> mappers, string sheetName = "Sheet1")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var p = new ExcelPackage();
            p.Workbook.Properties.Author = "";
            p.Workbook.Worksheets.Add(_localizer[sheetName]);
            var ws = p.Workbook.Worksheets[0];
            ws.Name = sheetName;
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            var colIndex = 1;
            var rowIndex = 1;

            var headers = mappers.Keys.Select(x => x).ToList();

            foreach (var header in headers)
            {
                var cell = ws.Cells[rowIndex, colIndex];

                var fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightBlue);

                var border = cell.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                        border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                cell.Value = header;

                colIndex++;
            }

            var dataList = data.ToList();
            foreach (var item in dataList)
            {
                colIndex = 1;
                rowIndex++;

                var result = headers.Select(header => mappers[header](item));

                foreach (var value in result)
                {
                    // Check if the value is a DateTime and format it accordingly
                    if (value is DateTime dateTimeValue)
                    {
                        ws.Cells[rowIndex, colIndex++].Value = dateTimeValue.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        ws.Cells[rowIndex, colIndex++].Value = value;
                    }
                }
            }

            using (ExcelRange autoFilterCells = ws.Cells[1, 1, dataList.Count + 1, headers.Count])
            {
                autoFilterCells.AutoFilter = true;
                autoFilterCells.AutoFitColumns();
            }

            var byteArray = await p.GetAsByteArrayAsync();

            return Convert.ToBase64String(byteArray);
        }

        public async Task<byte[]> ExportAsByteArrayAsync<TData>(IEnumerable<TData> data, Dictionary<string, Func<TData, object>> mappers, string sheetName = "Sheet1")
        {
            try
            {
                _logger.LogInformation("Starting ExportAsByteArrayAsync method.");

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var p = new ExcelPackage();
                p.Workbook.Properties.Author = "";

                if (!string.IsNullOrEmpty(sheetName))
                {
                    _logger.LogInformation("Sheet name is valid. Creating worksheet.");

                    p.Workbook.Worksheets.Add(sheetName);
                    var ws = p.Workbook.Worksheets[0];
                    ws.Name = sheetName;
                    ws.Cells.Style.Font.Size = 11;
                    ws.Cells.Style.Font.Name = "Calibri";

                    _logger.LogInformation("Worksheet created. Setting up headers.");

                    var colIndex = 1;
                    var rowIndex = 1;

                    var headers = mappers?.Keys?.Select(x => x).ToList();
                    _logger.LogInformation($"Headers count: {headers?.Count ?? 0}");

                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];

                            var fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.LightBlue);

                            var border = cell.Style.Border;
                            border.Bottom.Style =
                                border.Top.Style =
                                    border.Left.Style =
                                        border.Right.Style = ExcelBorderStyle.Thin;

                            cell.Value = header;

                            colIndex++;
                        }
                        _logger.LogInformation("Headers setup completed.");
                    }

                    var dataList = data?.ToList();
                    _logger.LogInformation($"Data count: {dataList?.Count ?? 0}");

                    if (dataList != null)
                    {
                        foreach (var item in dataList)
                        {
                            colIndex = 1;
                            rowIndex++;

                            var result = headers?.Select(header => mappers.ContainsKey(header) ? mappers[header](item) : null);

                            if (result != null)
                            {
                                foreach (var value in result)
                                {
                                    // Check if the value is null
                                    if (value == null)
                                    {
                                        ws.Cells[rowIndex, colIndex++].Value = "N/A";
                                    }
                                    else
                                    {
                                        // Check if the value is a DateTime and format it accordingly
                                        if (value is DateTime dateTimeValue)
                                        {
                                            ws.Cells[rowIndex, colIndex++].Value = dateTimeValue.ToString("yyyy-MM-dd");
                                        }
                                        else
                                        {
                                            ws.Cells[rowIndex, colIndex++].Value = value;
                                        }
                                    }
                                }
                            }
                        }
                        _logger.LogInformation("Data rows written to worksheet.");
                    }

                    using (ExcelRange autoFilterCells = ws.Cells[1, 1, dataList?.Count + 1 ?? 1, headers?.Count ?? 1])
                    {
                        autoFilterCells.AutoFilter = true;
                        autoFilterCells.AutoFitColumns();
                        _logger.LogInformation("Auto-filter and auto-fit applied.");
                    }

                    var resultArray = await p.GetAsByteArrayAsync();
                    _logger.LogInformation("Excel byte array generated successfully.");

                    return resultArray;
                }
                else
                {
                    throw new InvalidOperationException("Worksheet name cannot be null or empty.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in ExportAsByteArrayAsync: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex, $"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task<IResult<IEnumerable<TEntity>>> ImportAsync<TEntity>(Stream stream, Dictionary<string, Func<DataRow, TEntity, object>> mappers, string sheetName = "Sheet1")
        {
            var result = new List<TEntity>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var p = new ExcelPackage();
            stream.Position = 0;
            await p.LoadAsync(stream);
            var ws = p.Workbook.Worksheets[sheetName];

            if (ws == null)
            {
                return await Result<IEnumerable<TEntity>>.FailAsync(string.Format(_localizer["Sheet with name {0} does not exist!"], sheetName));
            }

            var dt = new DataTable();
            var titlesInFirstRow = true;
            foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
            {
                dt.Columns.Add(titlesInFirstRow ? firstRowCell.Text : $"Column {firstRowCell.Start.Column}");
            }
            var startRow = titlesInFirstRow ? 2 : 1;
            var headers = mappers.Keys.Select(x => x).ToList();
            var errors = new List<string>();
            foreach (var header in headers)
            {
                if (!dt.Columns.Contains(header))
                {
                    errors.Add(string.Format(_localizer["Header '{0}' does not exist in table!"], header));
                }
            }

            if (errors.Any())
            {
                return await Result<IEnumerable<TEntity>>.FailAsync(errors);
            }

            for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                try
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = dt.Rows.Add();
                    var item = (TEntity)Activator.CreateInstance(typeof(TEntity));
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                    headers.ForEach(x => mappers[x](row, item));
                    result.Add(item);
                }
                catch (Exception e)
                {
                    return await Result<IEnumerable<TEntity>>.FailAsync(_localizer[e.Message]);
                }
            }

            return await Result<IEnumerable<TEntity>>.SuccessAsync(result, _localizer["Import Success"]);
        }
    }
}