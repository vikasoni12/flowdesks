using AutoMapper;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.SystemPreferences;
using Flowdesks.Application.Responses.BulkUpload;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Globalization;

namespace Flowdesks.Infrastructure.Services.Common;

public class BulkUploadService : IBulkUploadService
{
    string _errMessage = string.Empty;
    bool _isInvalidFile;
    bool _isColumnMissing;


    public BulkUploadService()
    {
    }

    public async Task<List<BulkUploadGenericDatatableResponse<object>>> GetDataFromExcel(IFormFile file,CancellationToken Token)
    {
        List<BulkUploadGenericDatatableResponse<object>> bulkUploadDatatableResponse = new List<BulkUploadGenericDatatableResponse<object>>();

        using MemoryStream newMemoryStream = new();
        file.CopyTo(newMemoryStream);


        using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(newMemoryStream, false))
        {

            WorkbookPart? workbookPart = spreadSheetDocument.WorkbookPart;
            IEnumerable<Sheet> sheets = workbookPart?.Workbook?.GetFirstChild<Sheets>()?.Elements<Sheet>() ?? Enumerable.Empty<Sheet>();
            List<string> expectedSheetNames = new() { "sites", "building", "locations", "suppliers", "stock", "technicians", "assets" };
            List<string> actualSheetNames = sheets.Select(sheet => sheet.Name?.Value.ToLower()).ToList();



            if (sheets != Enumerable.Empty<Sheet>() && sheets.Count() == 7)
            {
                if (actualSheetNames.Count == 7 && expectedSheetNames.All(expectedName => actualSheetNames.Contains(expectedName)))
                {
                    sheets = sheets.OrderBy(sheet => expectedSheetNames.IndexOf(sheet.Name.Value.ToLower()));

                    List<BulkUploadGenericDatatableResponse<object>> validationErrors = new List<BulkUploadGenericDatatableResponse<object>>();

                    //validate  excel template
                    foreach (var expectedSheetName in expectedSheetNames)
                    {
                        BulkUploadGenericDatatableResponse<object> response = new BulkUploadGenericDatatableResponse<object>
                        {
                            SheetName = expectedSheetName
                        };

                        var sheet = sheets.FirstOrDefault(s => s.Name.Value.ToLower() == expectedSheetName);
                        if (sheet == null)
                        {
                            response.Error = $"Sheet {expectedSheetName} not found";
                            validationErrors.Add(response);
                            continue;
                        }

                        string relationshipId = sheet.Id.Value;
                        WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(relationshipId);
                        Worksheet workSheet = worksheetPart.Worksheet;
                        SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                        // Retrieve header row
                        Row headerRow = sheetData.Descendants<Row>().FirstOrDefault();

                        if (headerRow == null)
                        {
                            response.Error = $"Header row not found in the sheet {expectedSheetName}";
                            validationErrors.Add(response);
                            continue;
                        }

                        List<string> expectedColumns = GetExpectedColumns(expectedSheetName); // Define this method

                        List<string> actualColumns = headerRow?.Elements<Cell>().Select(cell => GetCellValue(spreadSheetDocument, cell)).ToList();

                        foreach (var expectedColumn in expectedColumns)
                        {
                            if (!actualColumns.Contains(expectedColumn))
                            {
                                response.Error = $"Column '{expectedColumn}' not found in the sheet {expectedSheetName}";
                                validationErrors.Add(response);
                                continue;

                            }
                        }

                    }

                        if (validationErrors.Count>0)
                        {
                            return validationErrors; // Return errors or  can show message to correct the temaplate
                        }                       //bulkUploadDatatableResponse.Add(response);

                    //validate data , bind and save
                    foreach (var expectedSheetName in expectedSheetNames)
                    {
                        BulkUploadGenericDatatableResponse<object> response = new BulkUploadGenericDatatableResponse<object>
                        {
                            SheetName = expectedSheetName
                        };
                        var sheet = sheets.FirstOrDefault(s => s.Name.Value.ToLower() == expectedSheetName);
                        string relationshipId = sheet.Id.Value;
                        WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(relationshipId);
                        Worksheet workSheet = worksheetPart.Worksheet;
                        SheetData sheetData = workSheet.GetFirstChild<SheetData>();


                        //IEnumerable<Row> rows = sheetData.Descendants<Row>();
                        IEnumerable<Row> rows = sheetData.Descendants<Row>()
                                 .Where(row => row.Elements<Cell>().Any());
                        // Skip the header row
                        foreach (Row row in rows.Skip(1))
                        {
                            Dictionary<int, string> cellValues = new Dictionary<int, string>();
                            foreach (Cell cell in row.Elements<Cell>())
                            {
                                int columnIndex = GetColumnIndex(cell.CellReference);
                                if (!string.IsNullOrWhiteSpace(expectedSheetName) && expectedSheetName.ToLower().Equals("assets") && (columnIndex == 11 || columnIndex == 13))
                                {
                                    if (!IsValidDateFormat(cell.InnerText, "dd-MM-yyyy"))
                                    {
                                        _ = double.TryParse(cell.InnerText, out double numericValue);
                                        cellValues[columnIndex] = DateTime.FromOADate(numericValue).ToString("dd-MM-yyyy");
                                    }
                                }
                                else
                                {
                                    string cellValue = GetCellValue(spreadSheetDocument, cell);
                                    cellValues[columnIndex] = cellValue;
                                }
                            }
                            if (expectedSheetName.ToLower().Equals("suppliers"))
                            {
                                if (cellValues.Count == 1 && cellValues.ContainsKey(4) && string.IsNullOrEmpty(cellValues[4]))
                                {
                                    continue; // Skip this row
                                }
                            }
                            response.Data.Add(cellValues);
                            response.SheetName = expectedSheetName;
                            
                        }
                        bulkUploadDatatableResponse.Add(response);
                    }
                }
                else
                {

                    throw new InvalidOperationException($"Sheets not found");
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid File, Use the correct template ");
            }

        }
            return bulkUploadDatatableResponse;
    }
    private List<string> GetExpectedColumns(string sheetName)
    {
        // Define expected columns for each sheet
        switch (sheetName)
        {
            case "sites":
                return new List<string> { "Code", "Name", "Address", "TelephoneNumber" }; // Add other expected columns
            case "building":
                return new List<string> { "Code", "Name", "Address", "Site", "BuildingType", "CostCentre" };
            case "locations":
                return new List<string> { "BuildingCode", "BuildingName", "Name", "Floor", "Description" };
            case "suppliers":
                return new List<string> { "Code", "Name", "Phone", "Email", "Category", "WebAddress" }; 
            case "stock":
                return new List<string> { "PartCode", "PartName", "Category", "Building", "Location", "Bin", "Manufacturer", "Supplier", "Quantity", "UnitCost", "MinQuantity", "Description" }; 
            case "technicians":
                return new List<string> { "IdNumber", "Name", "MobileNumber", "Email","Skill", "Supplier", "StandardHourlyRate", "Building", "Site" }; 
            case "assets":
                return new List<string> { "Type", "Name", "Site", "Building","Location", "Model", "Manufacturer", "SerialNumber", "Barcode", "Condition", "PurchaseCost", "PurchaseDate", "LifeSpan", "WarrantyExpiresDate", "ReplacementCost" }; 
            default:
                return new List<string>();
        }
    }

    private string GetCellValue(SpreadsheetDocument doc, Cell cell)
    {
        if (cell == null)
            return string.Empty;

        string cellValue = cell.InnerText;

        if (cell.DataType != null)
        {
            string value = cell.DataType.ToString();
            switch (value)
            {
                case "s":
                    if (int.TryParse(cellValue, out int sharedStringIndex))
                    {
                        return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(sharedStringIndex).InnerText;
                    }
                    break;

                case "b":
                    return cellValue == "0" ? "FALSE" : "TRUE";

                case "n":
                case "d":
                    if (!IsValidDateFormat(cellValue, "dd-MM-yyyy"))
                    {
                        _ = double.TryParse(cellValue, out double numericValue);
                        return DateTime.FromOADate(numericValue).ToString("dd-MM-yyyy");
                    }
                    break;
                case "str":
                    return cellValue;

                default:
                    return cellValue;
            }
        }
        return cellValue;
    }
     private int GetColumnIndex(string cellReference)
     {
        int columnIndex = 0;
        foreach (char ch in cellReference)
        {
            if (char.IsLetter(ch))
            {
                columnIndex = (columnIndex * 26) + (ch - 'A' + 1);
            }
            else
            {
                break;
            }
        }
        return columnIndex - 1; // Zero-based index
     }

    private static bool IsValidDateFormat(string value, string format)
    {
        return DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }

}

