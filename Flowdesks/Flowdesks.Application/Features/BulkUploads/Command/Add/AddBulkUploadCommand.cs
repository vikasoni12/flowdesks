using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.SystemPreferences;
using Flowdesks.Application.Requests.Asset;
using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Requests.BulkUpload;
using Flowdesks.Application.Requests.Permit;
using Flowdesks.Application.Requests.Site;
using Flowdesks.Application.Requests.Stocks;
using Flowdesks.Application.Requests.Supplier;
using Flowdesks.Application.Requests.Technicians;
using Flowdesks.Application.Responses.BulkUpload;
using Flowdesks.Application.Validators.BulkUpload;
using Flowdesks.Domain.Entities.BulkUpload;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Flowdesks.Application.Features.BulkUploads.Command.Create;
public class AddBulkUploadCommand : IRequestHandler<BulkUploadRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBulkUploadService _bulkUploadService;
    private readonly IRequiredFieldService _requiredFieldsService;
    private readonly IDuplicateRecordsForBulkService _duplicateRecordsForBulkService;

    public AddBulkUploadCommand(IUnitOfWork unitOfWork, IUploadService uploadService, IMapper mapper, IBulkUploadService bulkUploadService,  IRequiredFieldService requiredFieldsService,IDuplicateRecordsForBulkService duplicateRecordsForBulkService)
    {
        _unitOfWork = unitOfWork;
        _bulkUploadService = bulkUploadService;
        _mapper = mapper;
        _requiredFieldsService = requiredFieldsService;
        _bulkUploadService=bulkUploadService;
        _duplicateRecordsForBulkService = duplicateRecordsForBulkService;
    }

    public async Task<Result<int>> Handle(BulkUploadRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var bulkUploadGenericResponse = await _bulkUploadService.GetDataFromExcel(request.file, cancellationToken);

            if (bulkUploadGenericResponse == null)
            {
                return Result<int>.Fail("Something went Wrong");
            }
            bool hasError = bulkUploadGenericResponse.Any(response => !string.IsNullOrEmpty(response.Error));

            if (hasError)
            {
                var error = string.Join("<br>", bulkUploadGenericResponse.Where(response => !string.IsNullOrEmpty(response.Error)).Select(response => response.Error));

                return Result<int>.Fail(error);
            }

            Guid fileCode = Guid.NewGuid();

            var bulkUploadFile = new BulkUploadImportFileResponse
            {
                FileCode = fileCode,
                StartDate = DateTime.UtcNow,
                Name = request.file.FileName,
                HasError = false
                
            };
            
            var importFileDetail = _mapper.Map<ImportFileDetail>(bulkUploadFile);
            _unitOfWork.Repository<ImportFileDetail>().Add(importFileDetail);
            await _unitOfWork.SaveAsync(cancellationToken);
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None // Serialize without whitespace
            };
            bool invalidRecord = false;
            foreach (var item in bulkUploadGenericResponse)
            {
                switch (item.SheetName)
                {
                    case "sites":

                        List<BulkSiteRequest> convertedSiteRequest = new();
                        foreach (var siteItem in item.Data)
                        {
                            if (siteItem == null)
                                break;

                            Dictionary<int, string> cellValues = siteItem as Dictionary<int, string>;
                            var siteObject = new BulkSiteRequest
                            {
                                Code = cellValues.ContainsKey(0) ? cellValues[0] : string.Empty,
                                Name = cellValues.ContainsKey(1) ? cellValues[1] : string.Empty,
                                Address = cellValues.ContainsKey(2) ? cellValues[2] : string.Empty,
                                TelephoneNumber = cellValues.ContainsKey(3) ? cellValues[3] : string.Empty,
                                // Add other properties
                            };
                            convertedSiteRequest.Add(siteObject);

                        }

                        List<BulkUploadErrorLogResponse> inValidSiteRequestRecord = new();
                        List<BulkSiteRequest> ValidSitequestRecord = new();
                        var siteValidator = new ExcelImportSiteValidator(_duplicateRecordsForBulkService);

                        foreach (var siteValidate in convertedSiteRequest)
                        {

                            var validationResult = await siteValidator.ValidateAsync(siteValidate);
                            if (validationResult.IsValid)
                                ValidSitequestRecord.Add(siteValidate);
                            else
                            {
                                invalidRecord = true;
                                 var errorLog = new BulkUploadErrorLogResponse
                                 {
                                    FileCode = fileCode,
                                    SheetName = item.SheetName,
                                    Record = JsonConvert.SerializeObject(siteValidate, jsonSerializerSettings), // Serialize the invalid record
                                    Error = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)) 
                                 };

                                inValidSiteRequestRecord.Add(errorLog);
                            }

                        }

                        var site = _mapper.Map<List<BulkSiteRequest>, List<Flowdesks.Domain.Entities.Sites.Site>>(ValidSitequestRecord);
                        if (site?.Count > 0)
                        {
                            await _unitOfWork.Repository<Flowdesks.Domain.Entities.Sites.Site>().AddRangeAsync(site, cancellationToken);
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }

                        var errorLogRecords = _mapper.Map<List<BulkUploadErrorLogResponse>, List<ImportFileErrorLog>>(inValidSiteRequestRecord);
                        if (errorLogRecords.Count > 0)
                        {
                            await _unitOfWork.Repository<ImportFileErrorLog>().AddRangeAsync(errorLogRecords, cancellationToken);
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }

                        break;

                    case "building":


                        List<BulkBuildingRequest> convertedBuildingRequest = new();
                        foreach (var buildingItem in item.Data)
                        {
                            if (buildingItem == null)
                                break;

                            Dictionary<int, string> cellValues = buildingItem as Dictionary<int, string>;
                            var building = new BulkBuildingRequest
                            {
                                Code = cellValues.ContainsKey(0) ? cellValues[0] : string.Empty,
                                Name = cellValues.ContainsKey(1) ? cellValues[1] : string.Empty,
                                Address = cellValues.ContainsKey(2) ? cellValues[2] : string.Empty,
                                SiteId = GetSiteId(cellValues.ContainsKey(3) ? cellValues[3] : string.Empty),
                                BuildingTypeId = GetBuildingtypeId(cellValues.ContainsKey(4) ? cellValues[4] : string.Empty),
                                CostCentreId = GetCostCenter(cellValues.ContainsKey(5) ? cellValues[5] : string.Empty),
                            };
                            convertedBuildingRequest.Add(building);

                        }

                        List<BulkUploadErrorLogResponse> inValidBuildingRequestRecord = new();
                     
                        List<BulkBuildingRequest> ValidBuildingRequestRecord = new();
                        var validator = new ExcelImportBuildingRequestValidator(_requiredFieldsService, _duplicateRecordsForBulkService);

                        foreach (var buildingValidate in convertedBuildingRequest)
                        {

                            var validationResult = await validator.ValidateAsync(buildingValidate);
                            if (validationResult.IsValid)
                                ValidBuildingRequestRecord.Add(buildingValidate);
                            else
                            {
                                invalidRecord = true;
                                var errorLog = new BulkUploadErrorLogResponse
                                {
                                    FileCode = fileCode,
                                    SheetName = item.SheetName,
                                    Record = JsonConvert.SerializeObject(buildingValidate, jsonSerializerSettings), // Serialize the invalid record
                                    Error = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)) // Join error messages
                                };

                                inValidBuildingRequestRecord.Add(errorLog);
                            }

                        }
                        var buildingEntity = _mapper.Map<List<BulkBuildingRequest>, List<Building>>(ValidBuildingRequestRecord);
                        if (buildingEntity?.Count > 0)
                        {
                            await _unitOfWork.Repository<Building>().AddRangeAsync(buildingEntity, cancellationToken);
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }

                        var buildingErrorLogRecords = _mapper.Map<List<BulkUploadErrorLogResponse>, List<ImportFileErrorLog>>(inValidBuildingRequestRecord);
                        if (buildingErrorLogRecords.Count > 0)
                        {
                            await _unitOfWork.Repository<ImportFileErrorLog>().AddRangeAsync(buildingErrorLogRecords, cancellationToken);
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }



                        break;
                    case "locations":
                        List<BulkLocationRequest> convertedLocationRequest = new();
                        foreach (var locationsItem in item.Data)
                        {
                            if (locationsItem == null)
                                break;

                            Dictionary<int, string> cellValues = locationsItem as Dictionary<int, string>;

                            var locations = new BulkLocationRequest
                            {
                                BuildingId = GetBuildingId(cellValues.ContainsKey(0) ? cellValues[0] : string.Empty, cellValues.ContainsKey(1) ? cellValues[1] : string.Empty).Value,
                                Name = cellValues.ContainsKey(2) ? cellValues[2] : string.Empty,
                                Floor = cellValues.ContainsKey(3) ? cellValues[3] : string.Empty,
                                Description = cellValues.ContainsKey(4) ? cellValues[4] : string.Empty,

                            };
                            convertedLocationRequest.Add(locations);

                        }


                        List<BulkUploadErrorLogResponse> inValidLocationRequestRecord = new();
                        List<BulkLocationRequest> ValidLocationRequestRecord = new();
                        var locationValidator = new ExcelImportLocationValidator(_duplicateRecordsForBulkService);

                        foreach (var locationitem in convertedLocationRequest)
                        {

                            var validationResult = await locationValidator.ValidateAsync(locationitem);
                            if (validationResult.IsValid)
                                ValidLocationRequestRecord.Add(locationitem);
                            else
                            {
                                invalidRecord = true;
                                var errorLog = new BulkUploadErrorLogResponse
                                {
                                    FileCode = fileCode,
                                    SheetName = item.SheetName,
                                    Record = JsonConvert.SerializeObject(locationitem, jsonSerializerSettings), // Serialize the invalid record
                                    Error = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)) 
                                };

                                inValidLocationRequestRecord.Add(errorLog);

                            }
                        }
                        var location = _mapper.Map<List<BulkLocationRequest>, List<Flowdesks.Domain.Entities.Buildings.BuildingLocation>>(ValidLocationRequestRecord);

                        if (location?.Count > 0)
                        {
                            await _unitOfWork.Repository<Flowdesks.Domain.Entities.Buildings.BuildingLocation>().AddRangeAsync(location, cancellationToken);
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }

                        var locationErrorLogRecords = _mapper.Map<List<BulkUploadErrorLogResponse>, List<ImportFileErrorLog>>(inValidLocationRequestRecord);
                        if (locationErrorLogRecords.Count > 0)
                        {
                            await _unitOfWork.Repository<ImportFileErrorLog>().AddRangeAsync(locationErrorLogRecords, cancellationToken);
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }


                        break;
                    case "suppliers":

                        List<BulKSupplierRequest> convertedSupplierRequest = new();
                        foreach (var locationsItem in item.Data)
                        {
                            if (locationsItem == null)
                                break;

                            Dictionary<int, string> cellValues = locationsItem as Dictionary<int, string>;


                            var supplierObject = new BulKSupplierRequest
                            {
                                Code = cellValues.ContainsKey(0) ? cellValues[0] : string.Empty,
                                Name = cellValues.ContainsKey(1) ? cellValues[1] : string.Empty,
                                Phone = cellValues.ContainsKey(2) ? cellValues[2] : string.Empty,
                                Email = cellValues.ContainsKey(3) ? cellValues[3] : string.Empty,
                                CategoryId = GetSupplierCategoryId(cellValues.ContainsKey(4) ? cellValues[4] : string.Empty),
                                WebAddress = cellValues.ContainsKey(5) ? cellValues[5] : string.Empty,
                            };
                            convertedSupplierRequest.Add(supplierObject);

                        }

                        List<BulkUploadErrorLogResponse> inValidSupplierRequestRecord = new();
                        List<BulKSupplierRequest> ValidSupplierRequestRecord = new();
                        var supplierValidator = new ExcelImportSupplierValidator(_requiredFieldsService, _duplicateRecordsForBulkService);

                        foreach (var suppliersitem in convertedSupplierRequest)
                        {

                            var validationResult = await supplierValidator.ValidateAsync(suppliersitem);
                            if (validationResult.IsValid)
                                ValidSupplierRequestRecord.Add(suppliersitem);
                            else
                            {
                                invalidRecord = true;
                                var errorLog = new BulkUploadErrorLogResponse
                                {
                                    FileCode = fileCode,
                                    SheetName = item.SheetName,
                                    Record = JsonConvert.SerializeObject(suppliersitem, jsonSerializerSettings), // Serialize the invalid record
                                    Error = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))
                                };

                                inValidSupplierRequestRecord.Add(errorLog);

                            }

                        }
                        var supplier = _mapper.Map<List<BulKSupplierRequest>, List<Flowdesks.Domain.Entities.Suppliers.Supplier>>(ValidSupplierRequestRecord);

                        if (supplier?.Count > 0)
                        {
                            await _unitOfWork.Repository<Flowdesks.Domain.Entities.Suppliers.Supplier>().AddRangeAsync(supplier, cancellationToken);
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }
                        var supplierErrorLogRecords = _mapper.Map<List<BulkUploadErrorLogResponse>, List<ImportFileErrorLog>>(inValidSupplierRequestRecord);

                        if (supplierErrorLogRecords.Count > 0)
                        {
                            await _unitOfWork.Repository<ImportFileErrorLog>().AddRangeAsync(supplierErrorLogRecords, cancellationToken);
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }

                        break;
                    case "stock":

                        List<BulkStockRequest> convertedStockRequest = new();
                        foreach (var locationsItem in item.Data)
                        {
                            if (locationsItem == null)
                                break;

                            Dictionary<int, string> cellValues = locationsItem as Dictionary<int, string>;

                            var stockObject = new BulkStockRequest
                            {
                                PartCode = cellValues.ContainsKey(0) ? cellValues[0] : string.Empty,
                                PartName = cellValues.ContainsKey(1) ? cellValues[1] : string.Empty,
                                CategoryId = GetStockCategoryId(cellValues.ContainsKey(2) ? cellValues[2] : string.Empty),
                                BuildingId = GetBuildingIdByName(cellValues.ContainsKey(3) ? cellValues[3] : string.Empty),
                                LocationId = GetLocationId(cellValues.ContainsKey(4) ? cellValues[4] : string.Empty),
                                Bin = cellValues.ContainsKey(5) ? cellValues[5] : string.Empty,
                                Manufacturer = cellValues.ContainsKey(6) ? cellValues[6] : string.Empty,
                                SupplierId = GetSupplierId(cellValues.ContainsKey(7) ? cellValues[7] : string.Empty),
                                Quantity = cellValues.ContainsKey(8) && TryParseDecimal(cellValues[8], out decimal quantity) ? (decimal?)quantity : null,
                                UnitCost = cellValues.ContainsKey(9) && TryParseDecimal(cellValues[9], out decimal unitCost) ? (decimal?)unitCost : null,
                                MinQuantity = cellValues.ContainsKey(10) && TryParseDecimal(cellValues[10], out decimal minQuantity) ? (decimal?)minQuantity : null,
                                Description = cellValues.ContainsKey(11) ? cellValues[11] : string.Empty,
                            };

                            convertedStockRequest.Add(stockObject);

                        }

                        List<BulkUploadErrorLogResponse> inValidStockRequestRecord = new();
                        List<BulkStockRequest> ValidStockRequestRecord = new();
                        var stockValidator = new ExcelImportStockValidator(_requiredFieldsService, _duplicateRecordsForBulkService);

                        foreach (var stockitem in convertedStockRequest)
                        {

                            var validationResult = await stockValidator.ValidateAsync(stockitem);
                            if (validationResult.IsValid)
                                ValidStockRequestRecord.Add(stockitem);
                            else
                            {
                                invalidRecord = true;
                                var errorLog = new BulkUploadErrorLogResponse
                                {
                                    FileCode = fileCode,
                                    SheetName = item.SheetName,
                                    Record = JsonConvert.SerializeObject(stockitem, jsonSerializerSettings), // Serialize the invalid record
                                    Error = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)) 
                                };
                                inValidStockRequestRecord.Add(errorLog);

                            }
                        }
                        var stock = _mapper.Map<List<BulkStockRequest>, List<Flowdesks.Domain.Entities.Stocks.Stock>>(ValidStockRequestRecord);

                        if (stock?.Count > 0)
                        {
                            await _unitOfWork.Repository<Flowdesks.Domain.Entities.Stocks.Stock>().AddRangeAsync(stock, cancellationToken);
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }

                        var stockErrorLogRecords = _mapper.Map<List<BulkUploadErrorLogResponse>, List<ImportFileErrorLog>>(inValidStockRequestRecord);
                        if (stockErrorLogRecords.Count > 0)
                        {
                            await _unitOfWork.Repository<ImportFileErrorLog>().AddRangeAsync(stockErrorLogRecords, cancellationToken);
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }

                        break;

                    case "technicians":

                        List<BulkTechnicianRequest> convertedTechnicianRequest = new();
                        foreach (var techniciansItem in item.Data)
                        {
                            if (techniciansItem == null)
                                break;

                            Dictionary<int, string> cellValues = techniciansItem as Dictionary<int, string>;


                            List<Guid> getBuildingsGuid = GetBuildingIds(cellValues.ContainsKey(6) ? cellValues[6] : string.Empty);

                            List<Guid> getSitesGuid = GetSiteIds(cellValues.ContainsKey(7) ? cellValues[7] : string.Empty);

                           
                                var technicians = new BulkTechnicianRequest
                                {
                                    IdNumber = cellValues.ContainsKey(0) ? cellValues[0] : string.Empty,
                                    Name = cellValues.ContainsKey(1) ? cellValues[1] : string.Empty,
                                    MobileNumber = cellValues.ContainsKey(2) ? cellValues[2] : string.Empty,
                                    Email = cellValues.ContainsKey(3) ? cellValues[3] : string.Empty,
                                    SkillIds = GetTechnicianSkillIds(cellValues.ContainsKey(4) ? cellValues[4] : string.Empty),
                                    SupplierId = GetSupplierId(cellValues.ContainsKey(5) ? cellValues[5] : string.Empty),
                                    StandardHourlyRate = cellValues.ContainsKey(6) && TryParseDouble(cellValues[6], out double standardHourlyRate) ? (double?)standardHourlyRate : null,

                                    BuildingIds = getBuildingsGuid,
                                    SiteIds = getSitesGuid,
                                };
                                convertedTechnicianRequest.Add(technicians);
                           
                        }

                        List<BulkUploadErrorLogResponse> inValidTechnicianRequestRecord = new();
                        List<BulkTechnicianRequest> ValidTechnicianRequestRecord = new();
                        var technicianValidator = new ExcelImportTechnicianValidator(_requiredFieldsService, _duplicateRecordsForBulkService);

                        foreach (var TechnicianItem in convertedTechnicianRequest)
                        {

                            var validationResult = await technicianValidator.ValidateAsync(TechnicianItem);
                            if (validationResult.IsValid)
                                ValidTechnicianRequestRecord.Add(TechnicianItem);
                            else
                            {
                                invalidRecord = true;
                                var errorLog = new BulkUploadErrorLogResponse
                                {
                                    FileCode = fileCode,
                                    SheetName = item.SheetName,
                                    Record = JsonConvert.SerializeObject(TechnicianItem, jsonSerializerSettings), // Serialize the invalid record
                                    Error = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)) 
                                };
                                inValidTechnicianRequestRecord.Add(errorLog);

                            }

                        }
                        var tech = _mapper.Map<List<BulkTechnicianRequest>, List<Flowdesks.Domain.Entities.Technicians.Technician>>(ValidTechnicianRequestRecord);

                        if (tech?.Count > 0)
                        {
                            await _unitOfWork.Repository<Flowdesks.Domain.Entities.Technicians.Technician>().AddRangeAsync(tech, cancellationToken);

                            foreach (var validTech in ValidTechnicianRequestRecord)
                            {
                                // Find the corresponding technician entity
                                var techEntity = tech.FirstOrDefault(t => t.IdNumber == validTech.IdNumber && t.Name == validTech.Name);

                                if (techEntity != null && validTech.SiteIds?.Count > 0)
                                {
                                    var technicianSites = validTech.SiteIds
                                        .Select(id => new TechnicianSite { SiteId = id, TechnicianId = techEntity.Id })
                                        .ToList();

                                    await _unitOfWork.Repository<TechnicianSite>().AddRangeAsync(technicianSites, cancellationToken);
                                }
                                if (techEntity != null && validTech.BuildingIds?.Count > 0)
                                {
                                    var technicianBuildings = validTech.BuildingIds
                                    .Select(id => new TechnicianBuilding { BuildingId = id, TechnicianId = techEntity.Id })
                                    .ToList();
                                    _unitOfWork.Repository<TechnicianBuilding>().AddRange(technicianBuildings);
                                }

                                if (techEntity != null && validTech.SkillIds?.Count > 0)
                                {
                                    var technicianSkills = validTech.SkillIds
                                    .Select(id => new TechnicianSkill { SkillId = id, TechnicianId = techEntity.Id })
                                    .ToList();
                                    _unitOfWork.Repository<TechnicianSkill>().AddRange(technicianSkills);
                                }
                            }

                            await _unitOfWork.SaveAsync(cancellationToken);
                        }

                        var techErrorLogRecords = _mapper.Map<List<BulkUploadErrorLogResponse>, List<ImportFileErrorLog>>(inValidTechnicianRequestRecord);
                        if (techErrorLogRecords.Count > 0)
                        {
                            await _unitOfWork.Repository<ImportFileErrorLog>().AddRangeAsync(techErrorLogRecords, cancellationToken);
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }
                        break;

                    case "assets":
                        List<BulkAssetRequest> convertedAssetRequest = new();
                        foreach (var locationsItem in item.Data)
                        {
                            if (locationsItem == null)
                                break;

                            Dictionary<int, string> cellValues = locationsItem as Dictionary<int, string>;

                            var assets = new BulkAssetRequest
                            {
                                TypeId = GetAssetType(cellValues.ContainsKey(0) ? cellValues[0] : string.Empty),
                                Name = cellValues.ContainsKey(1) ? cellValues[1] : string.Empty,
                                SiteId = GetSiteId(cellValues.ContainsKey(2) ? cellValues[2] : string.Empty),
                                BuildingId = GetBuildingIdByName(cellValues.ContainsKey(3) ? cellValues[3] : string.Empty),
                                LocationId = GetLocationId(cellValues.ContainsKey(4) ? cellValues[4] : string.Empty),
                                Model = cellValues.ContainsKey(5) ? cellValues[5] : string.Empty,
                                Manufacturer = cellValues.ContainsKey(6) ? cellValues[6] : string.Empty,
                                SerialNumber = cellValues.ContainsKey(7) ? cellValues[7] : string.Empty,
                                BarCode = cellValues.ContainsKey(8) ? cellValues[8] : string.Empty,
                                ConditionId = GetAssetCondition(cellValues.ContainsKey(9) ? cellValues[9] : string.Empty),

                                PurchaseCost = cellValues.ContainsKey(10) && TryParseDecimal(cellValues[10], out decimal purchaseCost) ? (decimal?)purchaseCost : null,
                                PurchaseDate = cellValues.ContainsKey(11) ? DateTimeOffset.Parse(cellValues[11]).UtcDateTime : null,
                                LifeSpan = cellValues.ContainsKey(12) && TryParseDecimal(cellValues[12], out decimal lifeSpan) ? (decimal?)lifeSpan : null,
                                WarrantyExpiresDate = cellValues.ContainsKey(13) ? DateTimeOffset.Parse(cellValues[13]).UtcDateTime : null,
                                ReplacementCost = cellValues.ContainsKey(14) && TryParseDecimal(cellValues[14], out decimal replacementCost) ? (decimal?)replacementCost : null,

                            };
                            convertedAssetRequest.Add(assets);

                        }

                        List<BulkUploadErrorLogResponse> inValidAssetRequestRecord = new();
                        List<BulkAssetRequest> ValidAssetRequestRecord = new();
                        var assetValidator = new ExcelImportAssetValidator(_requiredFieldsService, _duplicateRecordsForBulkService);

                        foreach (var assetsitem in convertedAssetRequest)
                        {

                            var validationResult = await assetValidator.ValidateAsync(assetsitem);
                            if (validationResult.IsValid)
                                ValidAssetRequestRecord.Add(assetsitem);
                            else
                            {
                                invalidRecord = true;
                                var errorLog = new BulkUploadErrorLogResponse
                                {
                                    FileCode = fileCode,
                                    SheetName = item.SheetName,
                                    Record = JsonConvert.SerializeObject(assetsitem, jsonSerializerSettings), // Serialize the invalid record
                                    Error = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)) // Join error messages
                                };
                                inValidAssetRequestRecord.Add(errorLog);

                            }
                        }

                        var assetEntities = _mapper.Map<List<BulkAssetRequest>, List<Flowdesks.Domain.Entities.Assets.Asset>>(ValidAssetRequestRecord);

                        foreach (var validAssetRequest in ValidAssetRequestRecord)
                        {
                            var assetEntity = assetEntities.FirstOrDefault(a => a.Name == validAssetRequest.Name);
                            if (assetEntity != null)
                            {
                                assetEntity.Code = GenerateAssetCode(validAssetRequest);
                                validAssetRequest.TempCode = assetEntity.Code;

                            }
                        }

                        if (assetEntities?.Count > 0)
                        {
                            await _unitOfWork.Repository<Flowdesks.Domain.Entities.Assets.Asset>().AddRangeAsync(assetEntities, cancellationToken);

                            foreach (var validTech in ValidAssetRequestRecord)
                            {
                                // Find the corresponding technician entity
                                var assetEntity = assetEntities.FirstOrDefault(t => t.Code == validTech?.TempCode && t.Name == validTech?.Name);

                                if (assetEntity != null && (validTech.PurchaseCost.HasValue || validTech.PurchaseDate.HasValue || validTech.WarrantyExpiresDate.HasValue || validTech.LifeSpan.HasValue || validTech.ReplacementCost.HasValue || validTech.ConditionId.HasValue))
                                {
                                    var assetHealth = new AssetHealthAndFinanceDetail { PurchaseCost = validTech.PurchaseCost, WarrantyExpiresDate = validTech.WarrantyExpiresDate, PurchaseDate = validTech.PurchaseDate, LifeSpan = validTech.LifeSpan, AssetId = assetEntity.Id, ReplacementCost = validTech.ReplacementCost, ConditionId = validTech.ConditionId }
                                        ;

                                    _unitOfWork.Repository<AssetHealthAndFinanceDetail>().Add(assetHealth);
                                }
                            }
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }
                        var assetErrorLogRecords = _mapper.Map<List<BulkUploadErrorLogResponse>, List<ImportFileErrorLog>>(inValidAssetRequestRecord);
                        if (assetErrorLogRecords.Count > 0)
                        {
                            await _unitOfWork.Repository<ImportFileErrorLog>().AddRangeAsync(assetErrorLogRecords, cancellationToken);
                            await _unitOfWork.SaveAsync(cancellationToken);
                        }

                        break;

                    default:
                        break;
                }
            }

            var importFile = _unitOfWork.Repository<ImportFileDetail>().Entities().Where(x => x.FileCode.Equals(fileCode)).FirstOrDefault();

            if (importFile != null)
            {
                importFile.EndDate = DateTime.UtcNow;
                importFile.HasError = invalidRecord;
                _unitOfWork.Repository<ImportFileDetail>().Update(importFile);
                await _unitOfWork.SaveAsync(cancellationToken);
            }

            if (invalidRecord == true)
            {
                return Result<int>.Success("Data Uploaded Successfully, Download the Error log file below");
            }
        }
        catch (Exception ex)
        {
            throw;
        }
  
        return Result<int>.Success("Data Uploaded Successfully");
    }

    private Guid? GetSiteId(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var siteId = _unitOfWork.Repository<Flowdesks.Domain.Entities.Sites.Site>().Entities()
        .FirstOrDefault(x => x.Name.Equals(name))?.Id;

        return siteId;
    }

    private Guid? GetBuildingtypeId(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var typeId = _unitOfWork.Repository<Flowdesks.Domain.Entities.SystemPreferences.Buildings.BuildingType>().Entities()
        .FirstOrDefault(x => x.Name.Trim().ToLower().Equals(name.Trim().ToLower()))?.Id;

        return typeId;
    }

    private Guid? GetCostCenter(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var costCenterId = _unitOfWork.Repository<Domain.Entities.SystemPreferences.Finance.CostCentre>().Entities()
        .FirstOrDefault(x => x.Name.Equals(name))?.Id;

        return costCenterId;
    }

    private Guid? GetBuildingId(string code, string name)
    {
        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(code))
            return null;
        var buildingId = _unitOfWork.Repository<Flowdesks.Domain.Entities.Buildings.Building>().Entities()
        .FirstOrDefault(x => x.Code.Equals(code))?.Id;
        if (buildingId == null)
        {
            return Guid.Empty;
        }
        return buildingId;
    }

    private Guid? GetSupplierCategoryId(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var categoryId = _unitOfWork.Repository<SupplierCategory>().Entities()
        .FirstOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));

        return categoryId?.Id;
    }

    private Guid? GetBuildingIdByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var buildingId = _unitOfWork.Repository<Flowdesks.Domain.Entities.Buildings.Building>().Entities()
        .FirstOrDefault(x => x.Name.Equals(name));

        return buildingId?.Id;
    }

    private Guid? GetStockCategoryId(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var categoryId = _unitOfWork.Repository<StockCategory>().Entities()
        .FirstOrDefault(x => x.Name.Equals(name));

        return categoryId?.Id;
    }

    private Guid? GetLocationId(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var locationId = _unitOfWork.Repository<BuildingLocation>().Entities()
        .FirstOrDefault(x => x.Name.Equals(name));

        return locationId?.Id;
    }


    private Guid? GetSupplierId(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var supplierId = _unitOfWork.Repository<Flowdesks.Domain.Entities.Suppliers.Supplier>().Entities()
        .FirstOrDefault(x => x.Name.Equals(name));

        return supplierId?.Id;
    }

    private bool TryParseDecimal(string value, out decimal result)
    {
        return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
    }

    private Guid? GetAssetType(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var assetTypeId = _unitOfWork.Repository<AssetType>().Entities()
        .FirstOrDefault(x => x.Name.Equals(name));

        return assetTypeId?.Id;
    }

    private Guid? GetAssetCondition(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        // Split the input string based on the delimiter " - "
        var parts = name.Split(new string[] { " - " }, StringSplitOptions.None);

        if (parts.Length != 2)
        {
            return null;
        }

        var assetName = parts[0];
        if (!int.TryParse(parts[1], out int order))
        {
            return null;
        }

        var assetTypeId = _unitOfWork.Repository<AssetCondition>().Entities()
        .FirstOrDefault(x => x.Name.Equals(assetName.Trim()) && x.Order.Equals(order)).Id;

        return assetTypeId;
    }

    private List<Guid> GetBuildingIds(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new List<Guid>();
        }

        List<string> getBuildingsName = name.Split(',').Select(s => s.Trim()).ToList();
        List<Guid> getCompaniesIds = new();
        foreach (var item in getBuildingsName)
        {

            var buildingId = _unitOfWork.Repository<Flowdesks.Domain.Entities.Buildings.Building>().Entities()
            .FirstOrDefault(x => x.Name.Trim().Equals(item.Trim()));
            if (buildingId == null)
                return new List<Guid>();

            getCompaniesIds.Add(buildingId.Id);
        }

        return getCompaniesIds;
    }

    private List<Guid> GetSiteIds(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new List<Guid>();
        }

        List<string> getSitesName = name.Split(',').Select(s => s.Trim()).ToList();
        List<Guid> getSitesIds = new();
        foreach (var item in getSitesName)
        {

            var comapnyId = _unitOfWork.Repository<Flowdesks.Domain.Entities.Sites.Site>().Entities()
            .FirstOrDefault(x => x.Name.Trim().Equals(item.Trim()));
            if (comapnyId == null)
                return new List<Guid>();

            getSitesIds.Add(comapnyId.Id);
        }

        return getSitesIds;
    }

    private List<Guid> GetTechnicianSkillIds(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new List<Guid>();
        }

        List<string> getSkillsName = name.Split(',').Select(s => s.Trim()).ToList();
        List<Guid> getSkillIds = new();

        foreach (var item in getSkillsName)
        {

            var comapnyId = _unitOfWork.Repository<Skill>().Entities()
            .FirstOrDefault(x => x.Name.Trim().Equals(item.Trim()));
            if (comapnyId == null)
                return new List<Guid>();

            getSkillIds.Add(comapnyId.Id);
        }

        return getSkillIds;
    }

    private bool TryParseDouble(string value, out double result)
    {
        return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
    }

    private string GenerateAssetCode(BulkAssetRequest request)
    {
        string assetTemplateCode = string.Empty;

        var globalSetting = _unitOfWork.Repository<AssetGlobalSetting>().GetAll().FirstOrDefault();
        if (globalSetting != null && globalSetting.IsTemplateUse != null && globalSetting.IsTemplateUse == true)
        {
            var assetCode = _unitOfWork.Repository<AssetTemplateCode>().GetAll().ToList();
            int index = assetCode.FindIndex(t => t.FieldName == Shared.Enums.AssetTemplateCode.Number.ToString());
            if (index >= 0 && index != assetCode.Count - 1)
            {
                var total = assetCode[index];
                assetCode.RemoveAt(index);
                assetCode.Add(total);
            }
            if (assetCode != null)
            {
                var site = _unitOfWork.Repository<Domain.Entities.Sites.Site>().Entities().Include(x => x.Buildings)
                    .ThenInclude(x => x.BuildingLocations).Where(x => x.Id == request.SiteId);
                int firstCount = 0;
                var value = "";
                foreach (var item in assetCode)
                {
                    // Assuming 'item' has a property 'AssetFieldName' representing the asset field name
                    string assetFieldName = item.FieldName;
                    int count = item.NoOfCharacter;
                    string previousValue = "";
                    switch (assetFieldName)
                    {
                        case nameof(Shared.Enums.AssetTemplateCode.Site):
                            value = site.FirstOrDefault()?.Code;
                            break;
                        case nameof(Shared.Enums.AssetTemplateCode.Name):
                            value = request.Name;
                            break;
                        case nameof(Shared.Enums.AssetTemplateCode.Building):
                            value = site.First().Buildings.Where(x => x.Id == request.BuildingId).FirstOrDefault()?.Code;
                            break;
                        case nameof(Shared.Enums.AssetTemplateCode.Location):
                            var building = site.First().Buildings.Where(x => x.Id == request.BuildingId).FirstOrDefault();
                            value = building?.BuildingLocations.Where(x => x.Id == request.LocationId).FirstOrDefault()?.Name;
                            break;
                        case nameof(Shared.Enums.AssetTemplateCode.Type):
                            value = _unitOfWork.Repository<AssetType>().Entities().Where(x => x.Id == request.TypeId).FirstOrDefault()?.Name;
                            break;
                        case nameof(Shared.Enums.AssetTemplateCode.Number):
                            value = GeneratedAutoIncrementNumber();
                            previousValue = assetTemplateCode;
                            value = value.ToString().Substring(value.Length - count);
                            break;
                    }

                    if (value != null)
                    {
                        if (firstCount >= 1)
                        {
                            assetTemplateCode += "-";
                        }
                        assetTemplateCode += value.ToString().Substring(0, Math.Min(count, value.ToString().Length));
                        while (_unitOfWork.Repository<Asset>().Entities().Any(a => a.Code == assetTemplateCode.ToString()))
                        {
                            if (firstCount >= 1)
                            {
                                assetTemplateCode += "-";
                            }
                            var converIntoInt = Convert.ToUInt32(value);
                            converIntoInt++;
                            assetTemplateCode = previousValue;
                            assetTemplateCode += value.ToString().Substring(0, Math.Min(count, value.ToString().Length));
                        }
                        firstCount++;
                    }


                }
                return assetTemplateCode;
            }
            else
            {
                return GeneratedAutoIncrementNumber();
            }
        }
        else
        {
            return GeneratedAutoIncrementNumber();
        }
    }

    private string GeneratedAutoIncrementNumber()
    {
        var lastAsset = _unitOfWork.Repository<Domain.Entities.Assets.Asset>().Entities().OrderByDescending(x => x.CreatedOn).FirstOrDefault();

        int newCode = 0;

        if (lastAsset?.Code != null)
        {
            if ((bool)(lastAsset?.Code.Contains("-")))
            {
                var lastCode = lastAsset?.Code.Substring(lastAsset.Code.LastIndexOf("-") + 1);
                if (lastCode != null)
                {
                    newCode = (int)Convert.ToUInt32(lastCode);
                    newCode++;
                    return "000000" + newCode.ToString();
                }
            }
            else if (int.TryParse(lastAsset.Code, out int lastCode))
            {
                newCode = lastCode + 1;
            }
            else
            {
                newCode = 10001; 
            }
        }
        else
        {
            newCode = 10001; 
        }

        while (_unitOfWork.Repository<Domain.Entities.Assets.Asset>().Entities().Any(a => a.Code == newCode.ToString()))
        {
            newCode++;
        }
        return newCode.ToString();
    }

}
