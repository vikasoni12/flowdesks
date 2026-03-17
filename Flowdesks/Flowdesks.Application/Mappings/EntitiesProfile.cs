using AutoMapper;
using Flowdesks.Application.Extensions;
using Flowdesks.Application.Features.NotificationSettings.Command.Create;
using Flowdesks.Application.Features.PPMs.Command;
using Flowdesks.Application.Features.StockOrders.Command.Add;
using Flowdesks.Application.Features.WorkOrder.Index.Command.Update;
using Flowdesks.Application.Identity;
using Flowdesks.Application.Models.Notification;
using Flowdesks.Application.Requests;
using Flowdesks.Application.Requests.Asset;
using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Requests.BulkUpload;
using Flowdesks.Application.Requests.Chat.DirectMessage;
using Flowdesks.Application.Requests.Chat.Group;
using Flowdesks.Application.Requests.Chat.GroupMessage;
using Flowdesks.Application.Requests.Contacts;
using Flowdesks.Application.Requests.Contract;
using Flowdesks.Application.Requests.Documents;
using Flowdesks.Application.Requests.Finance;
using Flowdesks.Application.Requests.GridStates;
using Flowdesks.Application.Requests.Identity;
using Flowdesks.Application.Requests.Notes;
using Flowdesks.Application.Requests.Notification;
using Flowdesks.Application.Requests.NotificationSettings;
using Flowdesks.Application.Requests.Permit;
using Flowdesks.Application.Requests.PPMs;
using Flowdesks.Application.Requests.Procedures;
using Flowdesks.Application.Requests.PurchaseOrders.Approver;
using Flowdesks.Application.Requests.PurchaseOrders.Invoices;
using Flowdesks.Application.Requests.PurchaseOrders.POAuthorizations;
using Flowdesks.Application.Requests.Quotes;
using Flowdesks.Application.Requests.Quotes.QuoteResponses;
using Flowdesks.Application.Requests.Site;
using Flowdesks.Application.Requests.StockOrders;
using Flowdesks.Application.Requests.Stocks;
using Flowdesks.Application.Requests.Stocks.StockCategory;
using Flowdesks.Application.Requests.Supplier;
using Flowdesks.Application.Requests.Support;
using Flowdesks.Application.Requests.Teams;
using Flowdesks.Application.Requests.Technicians;
using Flowdesks.Application.Requests.Technicians.Qualification;
using Flowdesks.Application.Requests.Technicians.Skill;
using Flowdesks.Application.Requests.Tenant;
using Flowdesks.Application.Requests.WorkOrder;
using Flowdesks.Application.Requests.WorkOrder.Category;
using Flowdesks.Application.Requests.WorkOrder.ComplianceType;
using Flowdesks.Application.Requests.WorkOrder.Priority;
using Flowdesks.Application.Requests.WorkOrder.RequestSource;
using Flowdesks.Application.Requests.WorkOrder.SlaSetting;
using Flowdesks.Application.Requests.WorkOrder.TimeRecord;
using Flowdesks.Application.Requests.WorkRequests;
using Flowdesks.Application.Responses.Asset;
using Flowdesks.Application.Responses.Buldings;
using Flowdesks.Application.Responses.BulkUpload;
using Flowdesks.Application.Responses.Chat.DirectMessage;
using Flowdesks.Application.Responses.Chat.GroupMessage;
using Flowdesks.Application.Responses.Chat.Groups;
using Flowdesks.Application.Responses.Contacts;
using Flowdesks.Application.Responses.Contract;
using Flowdesks.Application.Responses.Documents;
using Flowdesks.Application.Responses.Finance;
using Flowdesks.Application.Responses.GridStates;
using Flowdesks.Application.Responses.Identity;
using Flowdesks.Application.Responses.Master;
using Flowdesks.Application.Responses.Notes;
using Flowdesks.Application.Responses.NotificationSetting;
using Flowdesks.Application.Responses.Permit;
using Flowdesks.Application.Responses.PPMs;
using Flowdesks.Application.Responses.Procedures;
using Flowdesks.Application.Responses.PurchaseOrders;
using Flowdesks.Application.Responses.PurchaseOrders.Approver;
using Flowdesks.Application.Responses.PurchaseOrders.Invoices;
using Flowdesks.Application.Responses.Quotes;
using Flowdesks.Application.Responses.RequiredFields;
using Flowdesks.Application.Responses.Site;
using Flowdesks.Application.Responses.StockOrders;
using Flowdesks.Application.Responses.Stocks;
using Flowdesks.Application.Responses.Supplier;
using Flowdesks.Application.Responses.Support;
using Flowdesks.Application.Responses.Teams;
using Flowdesks.Application.Responses.Technicians;
using Flowdesks.Application.Responses.Technicians.Building;
using Flowdesks.Application.Responses.Technicians.Qualification;
using Flowdesks.Application.Responses.Technicians.Skill;
using Flowdesks.Application.Responses.WorkOrder;
using Flowdesks.Application.Responses.WorkOrder.Category;
using Flowdesks.Application.Responses.WorkOrder.ComplianceType;
using Flowdesks.Application.Responses.WorkOrder.Priority;
using Flowdesks.Application.Responses.WorkOrder.RequestSource;
using Flowdesks.Application.Responses.WorkOrder.SlaSetting;
using Flowdesks.Application.Responses.WorkOrder.TimeRecord;
using Flowdesks.Application.Responses.WorkRequests;
using Flowdesks.Domain.Entities.Asset;
using Flowdesks.Domain.Entities.Assets;
using Flowdesks.Domain.Entities.Buildings;
using Flowdesks.Domain.Entities.BulkUpload;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Domain.Entities.Contacts;
using Flowdesks.Domain.Entities.Contracts;
using Flowdesks.Domain.Entities.Documents;
using Flowdesks.Domain.Entities.GridState;
using Flowdesks.Domain.Entities.Identity;
using Flowdesks.Domain.Entities.Invoices;
using Flowdesks.Domain.Entities.Note;
using Flowdesks.Domain.Entities.Notification;
using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Domain.Entities.Permit;
using Flowdesks.Domain.Entities.PPM;
using Flowdesks.Domain.Entities.PPMs;
using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Domain.Entities.PurchaseOrders;
using Flowdesks.Domain.Entities.Quotes;
using Flowdesks.Domain.Entities.Sites;
using Flowdesks.Domain.Entities.StockOrders;
using Flowdesks.Domain.Entities.Stocks;
using Flowdesks.Domain.Entities.Suppliers;
using Flowdesks.Domain.Entities.Support;
using Flowdesks.Domain.Entities.SystemPreferences;
using Flowdesks.Domain.Entities.SystemPreferences.Asset;
using Flowdesks.Domain.Entities.SystemPreferences.Buildings;
using Flowdesks.Domain.Entities.SystemPreferences.Documents;
using Flowdesks.Domain.Entities.SystemPreferences.Finance;
using Flowdesks.Domain.Entities.SystemPreferences.Stock;
using Flowdesks.Domain.Entities.SystemPreferences.Technician;
using Flowdesks.Domain.Entities.SystemPreferences.WorkOrder;
using Flowdesks.Domain.Entities.Teams;
using Flowdesks.Domain.Entities.Technicians;
using Flowdesks.Domain.Entities.Tenant;
using Flowdesks.Domain.Entities.WorkOrder;
using Flowdesks.Domain.Entities.WorkOrder.TimeRecord;
using Flowdesks.Domain.Entities.WorkRequests;
using Flowdesks.Domain.MasterEntities;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Notification;
using Flowdesks.Shared.Wrapper;

namespace Flowdesks.Application.Mappings;

public class EntitiesProfile : Profile
{
    public EntitiesProfile()
    {
        #region Asset

        CreateMap<Asset, AssetResponse>()
            .ForMember(x => x.SiteName, src => src.MapFrom(r => r.Site.Name))
            .ForMember(x => x.BuildingName, src => src.MapFrom(r => r.Building.Name))
            .ForMember(x => x.SupplierName, src => src.MapFrom(r => r.Supplier.Name))
            .ForMember(x => x.Location, src => src.MapFrom(r => r.Location.Name))
            .ForMember(x => x.Type, src => src.MapFrom(r => r.AssetType.Name))
        .ForPath(dest => dest.AssetHierarchy.ParentAssetId, opt => opt.MapFrom(src => src.ParentAssetId))
        .ForPath(dest => dest.AssetHierarchy.ParentAsset, opt => opt.MapFrom(src => src.ParentAsset))
        .ForPath(dest => dest.AssetHierarchy.ChildAssets, opt => opt.MapFrom(src => src.ChildAssets.AsEnumerable()))
        .ForPath(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
        .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status.ToEnum<AssetStatus>()));

        CreateMap<AssetWrapperResponse, AssetResponse>()
            .IncludeMembers(src => src.Asset)
            .ForMember(dest => dest.TaskCount, opt =>
                opt.MapFrom(r => r.WorkOrders.Count))
            .ForMember(dest => dest.TotalCost, opt =>
                opt.MapFrom(r => r.WorkOrders.Sum(x => x.ActualTotal ?? 0)));

        CreateMap<AssetHealthAndFinanceDetail, AssetHealthAndFinanceDetailResponse>()
            .ForMember(x => x.Condition, src => src.MapFrom(r => r.AssetCondition.Name));

        CreateMap<PaginatedResult<Asset>, PaginatedResult<AssetResponse>>();
        CreateMap<PaginatedResult<AssetWrapperResponse>, PaginatedResult<AssetResponse>>();

        CreateMap<CreateAssetRequest, Asset>()
             .ForMember(dest => dest.Site, opt => opt.Ignore())
             .ForMember(dest => dest.Building, opt => opt.Ignore())
             .ForMember(dest => dest.Location, opt => opt.Ignore())
             .ForMember(dest => dest.AssetType, opt => opt.Ignore())
             .ForMember(dest => dest.Supplier, opt => opt.Ignore())

    .ForMember(dest => dest.HealthAndFinanceDetail, opt => opt.MapFrom(src => new AssetHealthAndFinanceDetail
    {
        PurchaseDate = src.PurchaseDate,
        PurchaseCost = src.PurchaseCost,
        LifeSpan = src.LifeSpan,
        WarrantyExpiresDate = src.WarrantyExpiresDate
    }));
        CreateMap<BulkAssetRequest, Asset>()
             .ForMember(dest => dest.Site, opt => opt.Ignore())
             .ForMember(dest => dest.Building, opt => opt.Ignore())
             .ForMember(dest => dest.Location, opt => opt.Ignore())
             .ForMember(dest => dest.AssetType, opt => opt.Ignore())
             .ForMember(dest => dest.Supplier, opt => opt.Ignore())

    .ForMember(dest => dest.HealthAndFinanceDetail, opt => opt.MapFrom(src => new AssetHealthAndFinanceDetail
    {
        PurchaseDate = src.PurchaseDate,
        PurchaseCost = src.PurchaseCost,
        LifeSpan = src.LifeSpan,
        WarrantyExpiresDate = src.WarrantyExpiresDate
    }));

        CreateMap<UpdateAssetRequest, Asset>()
            .ForMember(dest => dest.HealthAndFinanceDetail, opt => opt.Ignore());

        CreateMap<UpdateAssetRequest, AssetHealthAndFinanceDetail>();
        CreateMap<UpdateHealthAndFinancialRequest, AssetHealthAndFinanceDetail>();
        CreateMap<Asset, ExportAssetResponse>()
            .ForMember(x => x.Site, src => src.MapFrom(r => r.Site.Name))
            .ForMember(x => x.Building, src => src.MapFrom(r => r.Building.Name))
            .ForMember(x => x.Supplier, src => src.MapFrom(r => r.Supplier.Name))
            .ForMember(x => x.Location, src => src.MapFrom(r => r.Location.Name))
            .ForMember(x => x.Type, src => src.MapFrom(r => r.AssetType.Name))
            .ForMember(x => x.PurchaseCost, src => src.MapFrom(r => r.HealthAndFinanceDetail.PurchaseCost))
            .ForMember(x => x.CurrentValue, src => src.MapFrom(r => r.HealthAndFinanceDetail.CurrentValue))
            .ForMember(x => x.DisposalValue, src => src.MapFrom(r => r.HealthAndFinanceDetail.DisposalValue))
            .ForMember(x => x.PurchaseCost, src => src.MapFrom(r => r.HealthAndFinanceDetail.PurchaseCost))
            .ForMember(x => x.ReplacementCost, src => src.MapFrom(r => r.HealthAndFinanceDetail.ReplacementCost))
            .ForMember(x => x.LifeSpan, src => src.MapFrom(r => r.HealthAndFinanceDetail.LifeSpan))
            .ForMember(x => x.Condition, src => src.MapFrom(r => r.HealthAndFinanceDetail.AssetCondition.Name))
            .ForMember(x => x.PurchaseDate, src => src.MapFrom(r => r.HealthAndFinanceDetail.PurchaseDate))
            .ForMember(x => x.WarrantyExpiresDate, src => src.MapFrom(r => r.HealthAndFinanceDetail.WarrantyExpiresDate))
            .ForMember(x => x.LastAssessed, src => src.MapFrom(r => r.HealthAndFinanceDetail.LastAssessed));

        CreateMap<AddUpdateAssetTypeRequest, AssetType>();
        CreateMap<AssetType, AssetTypeResponse>().ForMember(x => x.AssetCount, src => src.MapFrom(x => x.Assets.Count));
        CreateMap<PaginatedResult<AssetType>, PaginatedResult<AssetTypeResponse>>();

        CreateMap<AddAssetConditionRequest, AssetCondition>();
        CreateMap<AssetCondition, AssetConditionResponse>().ForMember(x => x.AssetCount,
            src => src.MapFrom(x => x.AssetHealthAndFinanceDetails.Count));

        CreateMap<PaginatedResult<AssetCondition>, PaginatedResult<AssetConditionResponse>>();

        #endregion

        #region Buildings

        CreateMap<AddUpdateLocationRequest, BuildingLocation>();
        CreateMap<BulkLocationRequest, BuildingLocation>();
        CreateMap<BuildingLocation, LocationResponse>().ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building.Name)).ForMember(dest => dest.SiteId, opt => opt.MapFrom(src => src.Building.Site.Id)).ForMember(dest => dest.SiteName, opt => opt.MapFrom(src => src.Building.Site.Name)); 
        CreateMap<PaginatedResult<BuildingLocation>, PaginatedResult<LocationResponse>>();

        CreateMap<AddUpdateBuildingTypeRequest, BuildingType>();
        CreateMap<BuildingType, BuildingTypeResponse>();
        CreateMap<PaginatedResult<BuildingType>, PaginatedResult<BuildingTypeResponse>>();

        CreateMap<PaginatedResult<Building>, PaginatedResult<BuildingResponse>>();

        CreateMap<AddBuildingRequest, Building>();
        CreateMap<BulkBuildingRequest, Building>();
        CreateMap<BuildingGeneralDetail, BuildingGeneralDetailResponse>();

        CreateMap<Building, BuildingResponse>()
            .ForMember(x => x.WorkOrdersCount, src => src.MapFrom(r => r.WorkOrders != null ? r.WorkOrders.Count : 0))
            .ForMember(x => x.SiteName, src => src.MapFrom(x => x.Site.Name))
            .ForMember(x => x.CostCentre, src => src.MapFrom(r => r.CostCentre.Name))
            .ForMember(x => x.BuildingTypeName, src => src.MapFrom(x => x.BuildingType.Name));

        CreateMap<BuildingDailyScheduleRequest, BuildingDailySchedule>();
        CreateMap<BuildingDailySchedule, BuildingDailyScheduleResponse>();

        CreateMap<UpdateBuildingRequest, Building>().ForMember(x => x.Code, src => src.Ignore());
        CreateMap<UpdateBuildingGeneralDetailRequest, BuildingGeneralDetail>();
        CreateMap<AddBuildingGeneralDetailRequest, BuildingGeneralDetail>();
        CreateMap<Building, ExportBuildingResponse>()
            .ForMember(x => x.Site, src => src.MapFrom(r => r.Site.Name))
            .ForMember(x => x.BuildingType, src => src.MapFrom(r => r.BuildingType.Name));

        #endregion

        #region Country
        CreateMap<Country, CountryDto>().ReverseMap();
        #endregion

        #region Sites 

        CreateMap<SiteResponse, CreateSiteRequest>().ReverseMap();
        CreateMap<Site, CreateSiteRequest>().ReverseMap();
        CreateMap<SiteResponse, BulkSiteRequest>().ReverseMap();
        CreateMap<Site, BulkSiteRequest>().ReverseMap();
        CreateMap<Site, UpdateSiteRequest>().ReverseMap();
        CreateMap<Site, SiteResponse>()
        .ForMember(x => x.CountryName, src => src.MapFrom(r => r.SiteCountry.Name))
        .ForMember(x => x.Teams, src => src.MapFrom(r => r.Teams.Select(team => new SiteTeam() { Id = team.Id, Name = team.Name }).ToList()));
        CreateMap<SitePagingRequest, Site>().ReverseMap();
        CreateMap<Site, ExportSiteResponse>()
        .ForMember(x => x.Country, src => src.MapFrom(r => r.SiteCountry.Name));

        #endregion

        #region Notes

        CreateMap<AddNoteRequest, Note>();
        CreateMap<Note, NoteResponse>()
            .ForMember(x => x.AuthorFullName, opt => opt.MapFrom(x => x.User.FirstName + " " + x.User.LastName))
            .ForMember(x => x.EntityType, opt => opt.MapFrom(x => x.EntityType.ToEnum<EntityType>()));
        CreateMap<PaginatedResult<Note>, PaginatedResult<NoteResponse>>();
        CreateMap<PPMNote, Note>()
            .ForMember(x => x.EntityType, opt => opt.MapFrom(x => EntityType.PPM.ToString()))
            .ForMember(x => x.EntityId, opt => opt.MapFrom(x => x.PPMId))
            .ForMember(x => x.AutoGenerated, opt => opt.Ignore());

        #endregion

        #region Contacts

        CreateMap<AddContactRequest, Contact>();
        CreateMap<UpdateContactRequest, Contact>();
        CreateMap<Contact, ContactResponse>()
            .ForMember(x => x.ContactClass, opt => opt.MapFrom(x => x.ContactClass.Name))
            .ForMember(x => x.EntityType, opt => opt.MapFrom(x => x.EntityType.ToEnum<EntityType>()));
        CreateMap<PaginatedResult<Contact>, PaginatedResult<ContactResponse>>();

        CreateMap<AddUpdateContactClassRequest, ContactClass>();
        CreateMap<ContactClass, ContactClassResponse>();

        #endregion

        #region Documents

        CreateMap<Document, DocumentResponse>()
          .ForMember(x => x.DocumentDate, opt => opt.MapFrom(x => x.DocumentDate ?? x.CreatedOn))
          .ForMember(x => x.EntityType, opt => opt.MapFrom(x => x.EntityType.ToEnum<EntityType>()));
        CreateMap<PaginatedResult<Document>, PaginatedResult<DocumentResponse>>();

        CreateMap<AddDocumentRequest, Document>();
        CreateMap<DocumentFileRequest, DocumentFile>();
        CreateMap<UpdateDocumentRequest, Document>();

        CreateMap<AddUpdateDocumentCategoryRequest, DocumentCategory>();
        CreateMap<DocumentCategory, DocumentCategoryResponse>();
        CreateMap<DocumentFile, DocumentFileResponse>();

        #endregion

        #region Supplier  

        CreateMap<Supplier, CreateSupplierRequest>().ReverseMap();
        CreateMap<Supplier, BulKSupplierRequest>().ReverseMap();

        CreateMap<Supplier, SupplierResponse>()
      .ForMember(x => x.Category, src => src.MapFrom(r => r.SupplierCategory.Name));

        CreateMap<PaginatedResult<Supplier>, PaginatedResult<SupplierResponse>>();

        CreateMap<SupplierPagingRequest, Supplier>().ReverseMap();

        CreateMap<Supplier, CreateSupplierRequest>().ReverseMap();
        CreateMap<Supplier, BulKSupplierRequest>().ReverseMap();

        CreateMap<Supplier, UpdateSupplierRequest>().ReverseMap();
        CreateMap<Supplier, ExportSupplierResponse>()
            .ForMember(x => x.Category, src => src.MapFrom(r => r.SupplierCategory.Name));

        CreateMap<SupplierCategoryRequest, Domain.Entities.Suppliers.SupplierCategory>();
        CreateMap<Domain.Entities.Suppliers.SupplierCategory, SupplierCategoryResponse>();
        CreateMap<PaginatedResult<Domain.Entities.Suppliers.SupplierCategory>, PaginatedResult<SupplierCategoryResponse>>();

        #endregion

        #region Contract

        CreateMap<Contract, CreateContractRequest>().ReverseMap();

        CreateMap<Contract, ContractResponse>()
      .ForMember(x => x.SupplierName, src => src.MapFrom(r => r.Supplier.Name));

        CreateMap<PaginatedResult<Contract>, PaginatedResult<ContractResponse>>();

        CreateMap<ContractPagingRequest, Contract>().ReverseMap();

        CreateMap<Contract, CreateContractRequest>().ReverseMap();
        CreateMap<Contract, UpdateContractRequest>().ReverseMap();
        #endregion

        #region Skill
        CreateMap<SkillResponse, CreateSkillRequest>().ReverseMap();
        CreateMap<Skill, CreateSkillRequest>().ReverseMap();
        CreateMap<Skill, UpdateSkillRequest>().ReverseMap();
        CreateMap<Skill, SkillResponse>();
        CreateMap<SkillPagingRequest, Skill>().ReverseMap();
        #endregion 

        #region Qualification
        CreateMap<QualificationResponse, CreateQualificationRequest>().ReverseMap();
        CreateMap<Qualification, CreateQualificationRequest>().ReverseMap();
        CreateMap<Qualification, UpdateQualificationRequest>().ReverseMap();
        CreateMap<Qualification, QualificationResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        CreateMap<PaginatedResult<Qualification>, PaginatedResult<QualificationResponse>>();
        CreateMap<QualificationPagingRequest, Qualification>().ReverseMap();
        #endregion

        #region Technician
        CreateMap<TechnicianResponse, CreateTechnicianRequest>().ReverseMap();
        CreateMap<TechnicianResponse, BulkTechnicianRequest>().ReverseMap();
        CreateMap<CreateTechnicianRequest, Technician>();
        CreateMap<BulkTechnicianRequest, Technician>();
        CreateMap<TechnicianSkillResponse, TechnicianSkill>().ReverseMap();
        CreateMap<PaginatedResult<TechnicianSkill>, PaginatedResult<TechnicianSkillResponse>>();
        CreateMap<TechnicianQualificationResponse, TechnicianQualification>().ReverseMap();
        CreateMap<PaginatedResult<TechnicianQualification>, PaginatedResult<TechnicianQualificationResponse>>();

        CreateMap<UpdateTechnicianRequest, Technician>().ReverseMap();
        CreateMap<Technician, TechnicianResponse>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null))
            .ForMember(dest => dest.WorkingDays, opt => opt.MapFrom(src =>
                src.WorkingDays != null ?
                src.WorkingDays.Select(days => new TechnicianWorkingDaysResponse
                {
                    DayOfWeek = days.DayOfWeek
                }).Distinct() :
                null))
            .ForMember(dest => dest.TechnicianSites, opt => opt.MapFrom(src => GetCombinedSites(src)))
            .ForMember(dest => dest.TechnicianBuildings, opt => opt.MapFrom(src =>
                src.Buildings != null ?
                src.Buildings.Select(building => new TechnicianBuildingResponse
                {
                    BuildingId = building.Id,
                    BuildingName = building.Name,
                }).Distinct() :
                null))
            .ForMember(dest => dest.TechnicianSkills, opt => opt.MapFrom(src =>
                src.Skills != null ?
                src.Skills.Select(skill => new TechnicianSkillResponse
                {
                    SkillId = skill.Skill.Id,
                    SkillName = skill.Skill.Name,
                }).ToList() :
                null))
            .ForMember(dest => dest.TechnicianQualifications, opt => opt.MapFrom(src =>
                src.Qualifications != null ?
                src.Qualifications.Select(qualification => new TechnicianQualificationResponse
                {
                    QualificationId = qualification.Qualification.Id,
                    QualificationName = qualification.Qualification.Name,
                }).ToList() :
                null)).ReverseMap();

        CreateMap<TechnicianPagingRequest, Technician>().ReverseMap();
        // CreateMap<PaginatedResult<TechnicianResponse>, PaginatedResult<Technician>>();
        CreateMap<PaginatedResult<Technician>, PaginatedResult<TechnicianResponse>>();
        CreateMap<Technician, ExportTechnicianResponse>();

        CreateMap<TechnicianBuilding, BuildingForTechnicianResponse>()
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Building.Code))
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Building.Name))
    .ForMember(dest => dest.SiteId, opt => opt.MapFrom(src => src.Building.SiteId))
    .ForMember(dest => dest.SiteName, opt => opt.MapFrom(src => src.Building.Site != null ? src.Building.Site.Name : null));

        CreateMap<TechnicianBuildingRequest, TechnicianBuilding>()
           .ForMember(dest => dest.TechnicianId, opt => opt.MapFrom(src => src.TechnicianId))
           .ForMember(dest => dest.BuildingId, opt => opt.MapFrom(src => src.BuildingId));


        CreateMap<TechnicianQualification, QualificationResponse>().
            ForMember(dest => dest.TechnicianIdNumber, opt => opt.MapFrom(src => src.Technician.IdNumber))
            .ForMember(dest => dest.TechnicianName, opt => opt.MapFrom(src => src.Technician.Name))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Qualification.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        CreateMap<TechnicianSkill, SkillResponse>().
           ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Skill.Name))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        CreateMap<PaginatedResult<TechnicianSkill>, PaginatedResult<SkillResponse>>();

        CreateMap<UpdateTechnicianQualificationRequest, TechnicianQualification>();
        CreateMap<UpdateTechnicianSkillRequest, TechnicianSkill>();

        CreateMap<PaginatedResult<TechnicianQualification>, PaginatedResult<QualificationResponse>>();

        #endregion

        #region Stock
        CreateMap<StockResponse, CreateStockRequest>().ReverseMap();
        CreateMap<StockResponse, BulkStockRequest>().ReverseMap();
        CreateMap<Stock, CreateStockRequest>().ReverseMap();
        CreateMap<Stock, BulkStockRequest>().ReverseMap();
        CreateMap<UpdateStockRequest, Stock>();
        CreateMap<Stock, StockResponse>()
            .ForMember(x => x.SupplierName, src => src.MapFrom(x => x.Supplier.Name))
            .ForMember(x => x.CategoryName, src => src.MapFrom(x => x.Category.Name))
            .ForMember(x => x.LocationName, src => src.MapFrom(x => x.Location.Name))
            .ForMember(x => x.BuildingName, src => src.MapFrom(x => x.Building.Name));
        CreateMap<StockPagingRequest, Stock>().ReverseMap();
        CreateMap<PaginatedResult<Stock>, PaginatedResult<StockResponse>>();

        CreateMap<AddUpdateStockCategoryRequest, StockCategory>();
        CreateMap<StockCategory, StockCategoryResponse>();
        CreateMap<Stock, ExportStockResponse>()
          .ForMember(x => x.BuildingName, src => src.MapFrom(r => r.Building.Name))
          .ForMember(x => x.SupplierName, src => src.MapFrom(r => r.Supplier.Name))
          .ForMember(x => x.LocationName, src => src.MapFrom(r => r.Location.Name))
          .ForMember(x => x.CategoryName, src => src.MapFrom(r => r.Category.Name));
        #endregion

        #region PurchaseOrder
        CreateMap<AddPurchaseOrderRequest, PurchaseOrder>();
        CreateMap<UpdatePurchaseOrderRequest, PurchaseOrder>();

        CreateMap<PaginatedResult<PurchaseOrder>, PaginatedResult<PurchaseOrderResponse>>();
        CreateMap<PurchaseOrder, PurchaseOrderResponse>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
            .ForMember(dest => dest.PartName, opt => opt.MapFrom(src => src.Stock.PartName))
            .ForMember(dest => dest.Approver, opt => opt.MapFrom(src => src.Approver != null ? src.Approver.Name : ""))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.RaiseDate, opt => opt.MapFrom(src => src.CreatedOn));

        CreateMap<PurchaseOrder, ExportPurchaseOrderResponse>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
            .ForMember(dest => dest.PartName, opt => opt.MapFrom(src => src.Stock.PartName))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
             .ForMember(dest => dest.RaiseDate, opt => opt.MapFrom(src => src.CreatedOn));
        #endregion

        #region Invoice
        CreateMap<CreateInvoiceRequest, Invoice>();
        CreateMap<UpdateInvoiceRequest, Invoice>();
        CreateMap<Invoice, InvoiceResponse>();
        CreateMap<PaginatedResult<Invoice>, PaginatedResult<InvoiceResponse>>();
        #endregion

        #region TimeRecord
        CreateMap<TimeRecordResponse, CreateTimeRecordRequest>().ReverseMap();
        CreateMap<TimeRecord, CreateTimeRecordRequest>().ReverseMap();
        CreateMap<TimeRecord, UpdateTimeRecordRequest>().ReverseMap();
        CreateMap<TimeRecord, TimeRecordResponse>()
        .ForMember(x => x.TechnicianName, src => src.MapFrom(y => y.Technician.Name))
        .ForMember(x => x.IdNumber, src => src.MapFrom(y => y.Technician.IdNumber));
        CreateMap<PaginatedResult<TimeRecord>, PaginatedResult<TimeRecordResponse>>();
        #endregion

        #region Priority
        CreateMap<PriorityResponse, CreatePriorityRequest>().ReverseMap();
        CreateMap<Domain.Entities.SystemPreferences.WorkOrder.Priority, CreatePriorityRequest>().ReverseMap();
        CreateMap<Domain.Entities.SystemPreferences.WorkOrder.Priority, UpdatePriorityRequest>().ReverseMap();
        CreateMap<Domain.Entities.SystemPreferences.WorkOrder.Priority, PriorityResponse>()
            .ForMember(x => x.WorkOrderCount, src => src.MapFrom(x => x.WorkOrders.Count))
            .ForMember(x => x.PPMCount, src => src.MapFrom(x => x.PPMs.Count));
        CreateMap<PaginatedResult<Domain.Entities.SystemPreferences.WorkOrder.Priority>, PaginatedResult<PriorityResponse>>();


        #endregion

        #region SlaSetting
        CreateMap<SlaSettingResponse, CreateSlaSettingRequest>().ReverseMap();
        CreateMap<SLASettings, CreateSlaSettingRequest>().ReverseMap();
        CreateMap<SLASettings, UpdateSlaSettingRequest>().ReverseMap();
        CreateMap<SLASettings, SlaSettingResponse>();
        #endregion

        #region WorkOrder

        CreateMap<WorkOrderResponse, CreateWorkOrderRequest>();
        CreateMap<WorkOrder, CreateWorkOrderRequest>().ReverseMap();
        CreateMap<UpdateWorkOrderRequest, WorkOrder>();
        CreateMap<UpdateWorkOrderCommand, WorkOrder>();
        CreateMap<WorkOrder, ExportWorkOrderResponse>()
            .ForMember(x => x.Priority, src => src.MapFrom(r => r.Priority.Name))
            .ForMember(x => x.Source, src => src.MapFrom(r => r.RequestSource.Name))
             .ForMember(x => x.Category, src => src.MapFrom(r => r.Category.Name))
            .ForMember(x => x.CostCode, src => src.MapFrom(r => r.CostCode.Name))
             .ForMember(x => x.CostCentre, src => src.MapFrom(r => r.CostCentre.Name))
             .ForMember(x => x.BuildingLocation, opt => opt.MapFrom(src => src.BuildingLocation.Name))
           .ForMember(x => x.Category, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<WorkOrder, WorkOrderResponse>()
             .ForMember(x => x.AssetCode, src => src.MapFrom(r => r.Asset.Code))
             .ForMember(x => x.AssetName, src => src.MapFrom(r => r.Asset.Name))
             .ForMember(x => x.BuildingName, src => src.MapFrom(r => r.Building.Name))
             .ForMember(x => x.BuildingLocationName, opt => opt.MapFrom(src => src.BuildingLocation.Name))
             .ForMember(x => x.Priority, src => src.MapFrom(r => r.Priority))
             .ForMember(x => x.Supplier, src => src.MapFrom(r => r.Supplier))
             .ForMember(x => x.Technician, src => src.MapFrom(r => r.Technician))
             .ForMember(x => x.Permit, src => src.MapFrom(r => r.Permit.Name))
             .ForMember(x => x.Source, src => src.MapFrom(r => r.RequestSource.Name))
             .ForMember(x => x.CostCode, src => src.MapFrom(r => r.CostCode.Name))
             .ForMember(x => x.CostCentre, src => src.MapFrom(r => r.CostCentre.Name))
             .ForMember(x => x.Category, src => src.MapFrom(r => r.Category.Name)).ForMember(x => x.PriorityName, src => src.MapFrom(r => r.Priority.Name));

        CreateMap<PaginatedResult<WorkOrder>, PaginatedResult<WorkOrderResponse>>();
        CreateMap<CustomerSatisfactionFormRequest, CustomerSatisfactionForm>();

        CreateMap<PaginatedResult<WorkOrderCategory>, PaginatedResult<WorkOrderCategoryResponse>>();
        CreateMap<AddUpdateCategoryRequest, WorkOrderCategory>();
        CreateMap<WorkOrderCategory, WorkOrderCategoryResponse>();

        CreateMap<WorkOrderProcedureRequest, WorkOrderProcedure>().ReverseMap();

        CreateMap<ComplianceType, ComplianceTypeResponse>().ForMember(x => x.PPMCount, src => src.MapFrom(x => x.PPMs.Count));
        CreateMap<AddUpdateComplianceTypeRequest, ComplianceType>();

        CreateMap<WORequestSource, WORequestSourceResponse>().ForMember(x => x.WorkOrderCount, src =>
        src.MapFrom(x => x.WorkOrders.Count));
        CreateMap<PaginatedResult<WORequestSource>, PaginatedResult<WORequestSourceResponse>>();

        CreateMap<AddUpdateWORequestSourceRequest, WORequestSource>();

        #endregion

        #region PPM

        CreateMap<PaginatedResult<PPM>, PaginatedResult<PPMResponse>>();
        CreateMap<CreateUpdatePPMRequest, PPM>();

        CreateMap<AddUpdateFrequencyColorRequest, PPMFrequencyColor>();
        CreateMap<PPMFrequencyColor, FrequencyColorResponse>();
        CreateMap<PaginatedResult<PPMFrequencyColor>, PaginatedResult<FrequencyColorResponse>>();

        CreateMap<CreatePPMCommand, PPM>().ReverseMap();
        CreateMap<PPM, PPMResponse>()
              .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
              .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building.Name))
              .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Name))
              .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier.Name))
              .ForMember(dest => dest.Technician, opt => opt.MapFrom(src => src.Technician))
              .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
              .ForMember(dest => dest.Instruction, opt => opt.MapFrom(src => src.Instruction.Name))
              .ForMember(dest => dest.SuspendedPPMs, opt => opt.MapFrom(src => src.SuspendedPPMs))
              .ForMember(dest => dest.PPMStatusTracker, opt => opt.MapFrom(src => src.PPMStatusTracker))
              .ForMember(x => x.CostCode, src => src.MapFrom(r => r.CostCode.Name))
              .ForMember(x => x.CostCentre, src => src.MapFrom(r => r.CostCentre.Name))
              .ForMember(x => x.ComplianceType, src => src.MapFrom(r => r.Compliance.Name))
             .ForMember(dest => dest.Skills, opt =>
                           opt.MapFrom(src => src.Skills.Select(skill => new PPMSkillResponse
                           {
                               SkillId = skill.Id,
                               Name = skill.Name,
                           }).ToList()));
        CreateMap<PPM, ExportPPMResponse>()
              .ForMember(dest => dest.Asset, opt => opt.MapFrom(src => src.Asset.Name))
              .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building.Name))
              .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Name))
              .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier.Name))
              .ForMember(dest => dest.Technician, opt => opt.MapFrom(src => src.Technician.Name))
              .ForMember(x => x.CostCode, src => src.MapFrom(r => r.CostCode.Name))
              .ForMember(x => x.CostCentre, src => src.MapFrom(r => r.CostCentre.Name))
              .ForMember(x => x.Compliance, src => src.MapFrom(r => r.Compliance.Name))
              .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.Name));

        CreateMap<SuspendedPPM, SuspendedPPMResponse>();
        CreateMap<PPMStatusTracker, PPMStatusTrackerResponse>();
        CreateMap<PPMStatusTrackerRequest, PPMStatusTracker>();
        CreateMap<PPMSkillRequest, PPMSkill>();

        #endregion

        #region StockOrder
        CreateMap<PaginatedResult<StockOrder>, PaginatedResult<StockOrderResponse>>();
        CreateMap<AddUpdateStockOrderRequest, StockOrder>();

        CreateMap<AddStockOrderCommand, StockOrder>().ReverseMap();
        CreateMap<StockOrder, StockOrderResponse>()
              .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock.PartCode))
              .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier.Name))
              .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Stock.Category.Name))
              .ForMember(dest => dest.UnitCost, opt => opt.MapFrom(src => src.Stock.UnitCost))
            .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => ((src.Stock.UnitCost) * (src.Quantity))));


        #endregion

        #region Chat

        CreateMap<AddUpdateDirectMessageRequest, DirectMessage>()
            .ForPath(x => x.Message.Content, src => src
            .MapFrom(g => g.Content));

        CreateMap<ApplicationUser, UserDirectMessagesResponse>()
            .ForMember(x => x.Name, src => src.MapFrom(x => x.FirstName + " " + x.LastName))
            .ForMember(x => x.Image, src => src.MapFrom(x => x.ProfilePictureDataUrl));

        CreateMap<DirectMessage, DirectMessageResponse>()
             .ForMember(x => x.Content, src => src
            .MapFrom(g => g.Message.Content));

        CreateMap<MessageAttachment, AttachmentDto>().ReverseMap();

        CreateMap<Group, GroupResponse>()
            .ForMember(x => x.Members, src => src
            .MapFrom(g => g.GroupUsers == null ? 0 : g.GroupUsers.Count));
        CreateMap<AddUpdateGroupRequest, Group>();

        CreateMap<GroupMessage, GroupMessageResponse>()
            .ForMember(x => x.Content, src => src
            .MapFrom(g => g.Message.Content));

        CreateMap<AddUpdateGroupMessageRequest, GroupMessage>()
              .ForPath(x => x.Message.Content, src => src
              .MapFrom(g => g.Content));

        CreateMap<Group, GroupDetailResponse>()
            .ForMember(x => x.Users, src => src
            .MapFrom(g => g.GroupUsers.Select(x => x.User)))
            .ForMember(x => x.Members, src => src
            .MapFrom(g => g.GroupUsers == null ? 0 : g.GroupUsers.Count));


        #endregion

        #region Users

        CreateMap<ApplicationUser, UserListResponse>()
       .ForMember(dest => dest.Sites, opt =>
           opt.MapFrom(src => src.Teams != null && src.Teams.Any() ? src.Teams.SelectMany(team => team.Sites).Where(x => x != null).Concat(src.Sites).Distinct() : src.Sites))
       .ForMember(dest => dest.Buildings, opt =>
           opt.MapFrom(src => src.Teams != null && src.Teams.Any() ? src.Teams.SelectMany(team => team.Buildings).Where(x => x != null).Concat(src.Buildings).Distinct() : src.Buildings))
       .ForMember(dest => dest.Teams, opt =>
           opt.MapFrom(src => src.Teams))
        .ForMember(x => x.Roles,
            src => src.MapFrom(x =>
            x.UserRoles.Select(x => x.Role.Name)));

        CreateMap<Team, UserTeamResponse>()
            .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Name));

        CreateMap<Site, UserSiteResponse>()
            .ForMember(dest => dest.SiteId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SiteName, opt => opt.MapFrom(src => src.Name));

        CreateMap<Building, UserBuildingResponse>()
            .ForMember(dest => dest.BuildingId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Name));

        CreateMap<ApplicationUser, UserRolesResponse>();

        CreateMap<UserRole, UserRoleModel>()
            .ForMember(dest => dest.RoleName, opt =>
         opt.MapFrom(src => src.Role.Name))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Role.Id))
            .ForMember(dest => dest.RoleDescription, opt => opt.MapFrom(src => src.Role.Description));




        #endregion

        #region Quote
        CreateMap<PaginatedResult<Quote>, PaginatedResult<QuoteResponse>>();
        CreateMap<UpdateQuoteRequest, Quote>();

        CreateMap<CreateQuoteRequest, Quote>().ReverseMap();
        CreateMap<Quote, QuoteResponse>()
              .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building.Name))
              .ForMember(dest => dest.SiteName, opt => opt.MapFrom(src => src.Site.Name))
              .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
              //.ForMember(dest => dest.JobDate, opt => opt.MapFrom(src => src.CreatedOn))
              .ForMember(dest => dest.SendToQuotes, opt =>
                 opt.MapFrom(src => src.TechnicianQuotes));

        CreateMap<TechnicianQuote, TechnicianQuoteResponse>()
                .ForMember(dest => dest.Name,
    opt => opt.MapFrom(src => src.Technician != null && src.Technician.Name != null
                                ? src.Technician.Name
                                : src.Supplier != null
                                    ? src.Supplier.Name
                                    : null))
            .ForMember(dest => dest.SentDate, opt => opt.MapFrom(src => src.CreatedOn))
            .ForMember(dest => dest.RecievedDate, opt => opt.MapFrom(src => src.LastModifiedOn))
            .ForMember(dest => dest.Approver, opt => opt.MapFrom(src => src.Approver.FirstName));

        CreateMap<Quote, ExportQuoteResponse>()
          .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building.Name))
          .ForMember(dest => dest.SiteName, opt => opt.MapFrom(src => src.Site.Name))
          .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<UpdateTechnicianQuoteRequest, TechnicianQuote>();
        CreateMap<CreateTechnicianQuoteRequest, TechnicianQuote>();
        CreateMap<PaginatedResult<TechnicianQuote>, PaginatedResult<TechnicianQuoteResponse>>();
        #endregion

        #region NotificationSetting
        CreateMap<PaginatedResult<NotificationSetting>, PaginatedResult<NotificationSettingResponse>>();
        CreateMap<CreateUpdateNotificationSettingRequest, NotificationSetting>();

        CreateMap<CreateNotificationSettingCommand, NotificationSetting>().ReverseMap();
        CreateMap<NotificationSetting, NotificationSettingResponse>();
        #endregion

        #region GridState

        CreateMap<GridState, GridStateResponse>();

        #endregion

        #region Notifications

        CreateMap<PaginatedResult<Notification>, PaginatedResult<NotificationDto>>();

        CreateMap<NotificationDto, Notification>().ReverseMap()
            .ForMember(x => x.NotificationType, opt => opt.MapFrom(x => x.NotificationType.ToString()));

        CreateMap<Notification, NotificationDto>()
           .ForMember(x => x.NotificationType, opt => opt.MapFrom(x => x.NotificationType.ToEnum<NotificationType>()));

        CreateMap<CreateUpdateNotificationRequest, Notification>()
            .ForMember(x => x.NotificationType, opt => opt.MapFrom(x => x.NotificationType.ToString()))
            .ReverseMap();

        #endregion

        #region Procedure

        CreateMap<ProcedureRequest, Procedure>();
        CreateMap<UpdateProcedureRequest, Procedure>();
        CreateMap<ProcedureSection, ProcedureSectionResponse>();
        CreateMap<ProcedureQuestion, ProcedureQuestionResponse>();
        CreateMap<ProcedureQuestionOption, ProcedureQuestionOptionResponse>();
        CreateMap<UpdateProcedureQuestionRequest, Procedure>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProcedureId))
            .ReverseMap();

        CreateMap<Procedure, ProcedurePagingResponse>()
            .ForMember(x => x.AssetType, src => src.MapFrom(r => r.AssetType.Name));

        CreateMap<PaginatedResult<Procedure>, PaginatedResult<ProcedurePagingResponse>>();

        CreateMap<ProcedureMappingRequest, ProcedureMapping>();
        CreateMap<UpdateProcedureMappingRequest, ProcedureMapping>();

        CreateMap<ProcedureSection, ProcedureSectionRequest>().ReverseMap();
        CreateMap<ProcedureQuestion, ProcedureQuestionRequest>().ReverseMap();
        CreateMap<ProcedureQuestionOption, ProcedureQuestionOptionRequest>().ReverseMap();

        CreateMap<ProcedureMapping, ProcedureMappingResponse>();
        CreateMap<UpdateProcedureMappingRequest, ProcedureMapping>().ReverseMap();
        CreateMap<ProcedureResponseRequest, Domain.Entities.Procedure.ProcedureResponse>().ReverseMap();
        CreateMap<Domain.Entities.Procedure.ProcedureResponse, Responses.Procedures.ProcedureResponse>().ForMember(x => x.ProcedureMappingResponseId, src => src.MapFrom(r => r.ProcedureMappingId));
        CreateMap<ProcedureMappingResponse, ProcedurePagingResponse>();
        CreateMap<Domain.Entities.Procedure.ProcedureResponse, ProcedureResponseRequest>().ReverseMap();


        #endregion

        #region Team
        CreateMap<CreateUpdateTeamRequest, Team>();
        CreateMap<PaginatedResult<Team>, PaginatedResult<TeamResponse>>();
        CreateMap<Team, TeamResponse>()
        .ForMember(dest => dest.TeamSites, opt =>
        opt.MapFrom(src => src.Sites.Select(site => new TeamSiteResponse
        {
            SiteId = site.Id,
            SiteName = site.Name,
        }).Distinct()))
        .ForMember(dest => dest.TeamBuildings, opt =>
        opt.MapFrom(src => src.Buildings.Select(building => new TeamBuildingResponse
        {
            BuildingId = building.Id,
            BuildingName = building.Name,
        }).Distinct()))

        .ForMember(dest => dest.TeamUsers, opt =>
         opt.MapFrom(src => src.Users.Select(user => new TeamUserResponse
         {
             UserId = user.Id,
             UserName = user.FirstName + " " + user.LastName,
         }).ToList()));

        CreateMap<TeamPagingRequest, Team>().ReverseMap();
        CreateMap<Team, ExportTeamResponse>()
        .ForMember(dest => dest.TotalMember, opt =>
         opt.MapFrom(src => src.Users.Count()));
        #endregion

        #region  UserPermission
        CreateMap<UserPermissionRequest, UserPermission>();
        #endregion

        #region RequiredFields

        CreateMap<EntityRequiredField, RequiredFieldResponse>();
        CreateMap<PaginatedResult<EntityRequiredField>, PaginatedResult<RequiredFieldResponse>>();
        #endregion

        #region AssetCode

        CreateMap<AddUpdateAssetCodeTemplateRequest, Domain.Entities.SystemPreferences.Asset.AssetTemplateCode>();
        CreateMap<Domain.Entities.SystemPreferences.Asset.AssetTemplateCode, AssetCodeTemplateResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FieldName, opt => opt.MapFrom(src => src.FieldName))
            .ForMember(dest => dest.NoOfCharacter, opt => opt.MapFrom(src => src.NoOfCharacter));
        CreateMap<PaginatedResult<CostCode>, PaginatedResult<CostCodeResponse>>();
        CreateMap<PaginatedResult<Domain.Entities.SystemPreferences.Asset.AssetTemplateCode>, PaginatedResult<AssetCodeTemplateResponse>>();
        CreateMap<Domain.Entities.SystemPreferences.Asset.AssetTemplateCode, CreateAssetTemplateCodeRequest>().ReverseMap();

        #endregion

        #region POAuthorization

        CreateMap<POAuthorization, AddUpdatePOAuthorization>().ReverseMap();
        CreateMap<POAuthorization, POAuthorizationResponse>().ReverseMap();

        #endregion

        #region CostCentre

        CreateMap<AddUpdateCostCentreRequest, CostCentre>().ReverseMap();
        CreateMap<CostCentre, CostCentreResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.Budget));
        CreateMap<PaginatedResult<CostCentre>, PaginatedResult<CostCentreResponse>>();

        #endregion

        #region CostCode

        CreateMap<AddUpdateCostCodeRequest, CostCode>().ReverseMap();
        CreateMap<CostCode, CostCodeResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        CreateMap<PaginatedResult<CostCode>, PaginatedResult<CostCodeResponse>>();

        #endregion

        #region Support

        CreateMap<AddUpdateSupportRequest, Support>();
        CreateMap<Support, Responses.Support.SupportResponse>()
           .ForMember(x => x.InitiatedBy, src => src.MapFrom(r => r.User.FirstName + " " + r.User.LastName))
           .ForMember(x => x.ProfilePicture, src => src.MapFrom(r => r.User.ProfilePictureDataUrl))
           .ForMember(x => x.AssignedUser, src => src.MapFrom(r => r.AssignedUser.FirstName + " " + r.AssignedUser.LastName)).ForMember(x => x.PriorityName, src => src.MapFrom(r => r.Priority.Name));
        ;
        CreateMap<PaginatedResult<Support>, PaginatedResult<Responses.Support.SupportResponse>>();

        #endregion

        #region SupportResponse

        CreateMap<AddUpdateSupportResponseRequest, Domain.Entities.Support.SupportResponse>();
        CreateMap<Domain.Entities.Support.SupportResponse, SupportResponsesResponse>();
        CreateMap<PaginatedResult<Domain.Entities.Support.SupportResponse>, PaginatedResult<SupportResponsesResponse>>();

        #endregion

        #region WorkRequest

        CreateMap<CreateWorkRequest, Domain.Entities.WorkRequests.WorkRequest>();
        CreateMap<Domain.Entities.WorkRequests.WorkRequest, WorkRequestResponse>()
             .ForMember(dest => dest.Building, opt => opt.MapFrom(src => src.Building.Name ?? null))
             .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.BuildingLocation.Name ?? null))
             .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.BuildingLocation.Floor ?? null))
            .ForMember(dest => dest.WorkOrderResponse, src => src.MapFrom(workReq => workReq.WorkOrder));
        CreateMap<PaginatedResult<Domain.Entities.WorkRequests.WorkRequest>, PaginatedResult<WorkRequestResponse>>();
        #endregion

        #region SelfServiceUpdate

        CreateMap<AddUpdateSelfServiceUpdateRequest, SelfServiceUpdate>().ReverseMap();
        CreateMap<SelfServiceUpdate, SelfServiceUpdateResponse>()
            .ForMember(x => x.Site, src => src.MapFrom(r => r.Site.Name))
            .ForMember(x => x.Building, src => src.MapFrom(r => r.Building.Name))
            .ForMember(x => x.ModifiedOn, src => src.MapFrom(r => r.LastModifiedOn ?? r.CreatedOn));
        CreateMap<PaginatedResult<SelfServiceUpdate>, PaginatedResult<SelfServiceUpdateResponse>>();
        #endregion

        #region ViewState

        CreateMap<ViewState, ViewStateResponse>()
            .ForMember(x => x.ViewType,
            x => x.MapFrom(s => s.ViewType.ToEnum<ViewType>()))
            .ForMember(x => x.EntityType,
            x => x.MapFrom(s => s.EntityType.ToEnum<EntityType>()));

        CreateMap<CreateUpdateViewStateRequest, ViewState>()
              .ForMember(x => x.ViewType,
            x => x.MapFrom(s => s.ViewType.ToString()))
            .ForMember(x => x.EntityType,
            x => x.MapFrom(s => s.EntityType.ToString()));

        #endregion

        #region PPMNotes

        CreateMap<PPMNote, NoteResponse>()
            .ForMember(x => x.AuthorFullName, opt => opt.MapFrom(x => x.User.FirstName + " " + x.User.LastName))
            .ForMember(x => x.AutoGenerated, opt => opt.Ignore())
            .ForMember(x => x.EntityId, opt => opt.MapFrom(x => x.PPMId))
            .ForMember(x => x.EntityType, opt => opt.MapFrom(x => EntityType.PPM));
        CreateMap<PaginatedResult<PPMNote>, PaginatedResult<NoteResponse>>();

        CreateMap<AddPPMNoteRequest, PPMNote>();

        #endregion

        #region PPMReminderSettings

        CreateMap<AddUpdatePPMReminderSettingsRequest, PPMReminderSettings>();
        CreateMap<PPMReminderSettings, PPMReminderSettingsResponse>();

        #endregion

        #region Permit

        CreateMap<Permit, PermitResponse>();
        CreateMap<PaginatedResult<Permit>, PaginatedResult<PermitResponse>>();
        CreateMap<AddPermitRequest, Permit>();

        #endregion

        #region Approver

        CreateMap<Approver, ApproverResponse>();
        CreateMap<AddApproverRequest, Approver>();

        #endregion

        #region Import

        CreateMap<ImportFileDetail, BulkUploadImportFileResponse>().ReverseMap();
        CreateMap<ImportFileErrorLog, BulkUploadErrorLogResponse>().ReverseMap();
        CreateMap<ImportFileErrorLog, ExportBulkErrorResponse>();
        CreateMap<ExportBulkErrorResponse, ImportFileErrorLog>();
        CreateMap<BulkUploadFileCodeResponse, ImportFileDetail>().ReverseMap();

        #endregion

        #region Tenant

        CreateMap<CreateTenantRequest, Tenant>();

        #endregion

    }

    public static List<TechnicianSiteResponse> GetCombinedSites(Technician technician)
    {
        List<TechnicianSiteResponse> combinedSites = new();
       if(technician.Sites != null && technician.Buildings != null)
       {
            var sitesFromSites = technician.Sites.Select(x => new TechnicianSiteResponse
            {
                SiteId = x.Id,
                SiteName = x.Name
            });

            var sitesFromBuildings = technician.Buildings.Where(x => x.SiteId != null && x.Site != null).Select(x => x.Site).Select(site => new TechnicianSiteResponse
            {
                SiteId = site.Id,
                SiteName = site.Name
            });

             combinedSites = sitesFromSites
                .Concat(sitesFromBuildings)
                .GroupBy(x => new { x.SiteId, x.SiteName })
                .Select(group => group.First())
                .ToList();

       }
       return combinedSites;

    }
}