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
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System.Reflection;

namespace Flowdesks.Application.Features.Master.RequiredFields.Command.Delete;

public class DeleteRequiredFieldsCommand : IRequest<Result<int>>
{
    public List<Guid> Ids { get; set; }
    public EntityType? EntityType { get; set; }
}

public class DeleteRequiredFieldsCommandHandler : IRequestHandler<DeleteRequiredFieldsCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRequiredFieldsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(DeleteRequiredFieldsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var fields = _unitOfWork.Repository<EntityRequiredField>().Entities()
                .WhereIf(request.EntityType != null, x => x.EntityType.Equals(request.EntityType.ToString()))
                .WhereIf(request.Ids != null && request.Ids.Count > 0, x => request.Ids.Contains(x.Id)).ToList();

            if (fields == null || fields.Count == 0)
            {
                return await Result<int>.FailAsync($"Not found");
            }

            if(request.EntityType != null)
            {
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
                        return Result<int>.Fail();
                }

                var entityProperties = entity.GetProperties();

                var fieldResponses = entityProperties
                  .Select(property => new FieldResponse
                  {
                      Value = char.ToLower(property.Name[0]) + property.Name[1..],
                      IsRequired = IsRequired(property)
                  })
                .Where(x => !x.IsRequired);

                var filteredList = fields.ToList().Where(x => fieldResponses.Any(y => y.Value == x.Value));

                _unitOfWork.Repository<EntityRequiredField>().DeleteRange(filteredList, true);
                await _unitOfWork.SaveAsync(cancellationToken);
            }
            else
            {
                _unitOfWork.Repository<EntityRequiredField>().DeleteRange(fields, true);
                await _unitOfWork.SaveAsync(cancellationToken);
            }

            return await Result<int>.SuccessAsync("RequiredFields deleted successfully");
        }
        catch (Exception ex)
        {
            return await Result<int>.FailAsync(ex.Message);
        }
    }

    static bool IsRequired(PropertyInfo property)
    {
        IsRequiredField attribute = (IsRequiredField)Attribute.GetCustomAttribute(property, typeof(IsRequiredField));

        return attribute?.IsDefault ?? false;
    }
}