using Flowdesks.Application.Attributes;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests;
using Flowdesks.Application.Requests.Asset;
using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Requests.Contacts;
using Flowdesks.Application.Requests.Contract;
using Flowdesks.Application.Requests.Documents;
using Flowdesks.Application.Requests.PPMs;
using Flowdesks.Application.Requests.Stocks;
using Flowdesks.Application.Requests.Supplier;
using Flowdesks.Application.Requests.Technicians;
using Flowdesks.Application.Requests.WorkOrder;
using Flowdesks.Application.Responses.RequiredFields;
using Flowdesks.Domain.Entities.SystemPreferences;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System.ComponentModel;
using System.Reflection;

namespace Flowdesks.Application.Features.Master.RequiredFields.Query.GetAll;

public class GetFieldsByEntityTypeQuery : IRequest<Result<List<FieldResponse>>>
{
    public EntityType EntityType { get; set; }
}

public class GetFieldsByEntityTypeQueryHandler : IRequestHandler<GetFieldsByEntityTypeQuery, Result<List<FieldResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFieldsByEntityTypeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<FieldResponse>>> Handle(GetFieldsByEntityTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var requiredFields = _unitOfWork.Repository<EntityRequiredField>().Entities()
               .Where(x => x.EntityType.Equals(request.EntityType.ToString()))
               .Select(x => x.Value).ToList();

            Type entity;

            switch (request.EntityType)
            {
                case EntityType.Asset:
                    entity = typeof(AssetRequiredFields);
                    break;
                case EntityType.Supplier:
                    entity = typeof(CreateSupplierRequest);
                    break;
                case EntityType.Technician:
                    entity = typeof(CreateTechnicianRequest);
                    break;
                case EntityType.Stock:
                    entity = typeof(CreateStockRequest);
                    break;
                case EntityType.PurchaseOrder:
                    entity = typeof(AddPurchaseOrderRequest);
                    break;
                case EntityType.WorkOrder:
                    entity = typeof(WorkOrderRequiredFields);
                    break;
                case EntityType.PPM:
                    entity = typeof(PPMRequiredFields);
                    break;
                case EntityType.Building:
                    entity = typeof(BuildingRequiredFiled);
                    break;
                case EntityType.Document:
                    entity = typeof(AddDocumentRequest);
                    break;
                case EntityType.Contracts:
                    entity = typeof(CreateContractRequest);
                    break;
                case EntityType.Contacts:
                    entity = typeof(AddContactRequest);
                    break;
                default:
                    return Result<List<FieldResponse>>.Fail("Invalid entity type");
            }

            var entityProperties = entity.GetProperties();

            var fieldResponses = entityProperties
             .Where(x => !IgnoreProperty(x))
             .Select(property => new FieldResponse
             {
                 Title = GetDescription(property),
                 Value = char.ToLower(property.Name[0]) + property.Name[1..],
                 IsSelected = requiredFields.Any(reqField => reqField.Equals(property.Name, StringComparison.OrdinalIgnoreCase)),
                 IsRequired = IsRequired(property)
             })
            .OrderByDescending(x => x.IsRequired ? 1 : 0)
                .ThenBy(x => x.IsSelected ? 1 : 0)
             .ToList();

            return Result<List<FieldResponse>>.Success(fieldResponses);
        }
        catch (Exception ex)
        {
            return Result<List<FieldResponse>>.Fail(ex.Message);
        }
    }

    static string GetDescription(PropertyInfo property)
    {
        var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute));
        return descriptionAttribute?.Description ?? property.Name;
    }

    static bool IsRequired(PropertyInfo property)
    {
        IsRequiredField attribute = (IsRequiredField)Attribute.GetCustomAttribute(property, typeof(IsRequiredField));

        return attribute?.IsDefault ?? false;
    }

    static bool IgnoreProperty(PropertyInfo property)
    {
        IgnoreField attribute = (IgnoreField)Attribute.GetCustomAttribute(property, typeof(IgnoreField));

        return attribute?.Ignore ?? false;
    }
}