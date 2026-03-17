using AutoMapper;
using Flowdesks.Application.Attributes;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests;
using Flowdesks.Application.Requests.Asset;
using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Application.Requests.Contacts;
using Flowdesks.Application.Requests.Contract;
using Flowdesks.Application.Requests.Documents;
using Flowdesks.Application.Requests.PPMs;
using Flowdesks.Application.Requests.RequiredFields;
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

namespace Flowdesks.Application.Features.Master.RequiredFields.Query.GetAll;

public class GetAllRequiredFieldsQuery : RequiredFieldsPagingRequest, IRequest<Result<PaginatedResult<RequiredFieldResponse>>>
{
}

public class GetAllRequiredFieldsQueryHandler : IRequestHandler<GetAllRequiredFieldsQuery, Result<PaginatedResult<RequiredFieldResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllRequiredFieldsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<RequiredFieldResponse>>> Handle(GetAllRequiredFieldsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<EntityRequiredField>().Entities()
                .Where(x => x.EntityType.Equals(request.EntityType.ToString()));

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.ApplySorting(request.SortColumn, request.SortOrder);
            }

            if (request.IsFromGrid)
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
                        return Result<PaginatedResult<RequiredFieldResponse>>.Fail("Invalid entity type");
                }

                var entityProperties = entity.GetProperties();

                var fieldResponses = entityProperties
              .Select(property => new FieldResponse
              {
                  Value = char.ToLower(property.Name[0]) + property.Name[1..],
                  IsRequired = IsRequired(property)
              })
              .Where(x => !x.IsRequired);

                var filteredList = query.ToList().Where(x => fieldResponses.Any(y => y.Value == x.Value));

                var res = filteredList.ToPaginatedEnumerableList(request.PageNumber, request.PageSize);

                var requiredFieldsResponse = _mapper.Map<PaginatedResult<RequiredFieldResponse>>(res);

                return Result<PaginatedResult<RequiredFieldResponse>>.Success(requiredFieldsResponse);
            }
            else
            {
                var res = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);

                var requiredFieldsResponse = _mapper.Map<PaginatedResult<RequiredFieldResponse>>(res);

                return Result<PaginatedResult<RequiredFieldResponse>>.Success(requiredFieldsResponse);
            }

        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<RequiredFieldResponse>>.Fail(ex.Message);
        }
    }

    static bool IsRequired(PropertyInfo property)
    {
        IsRequiredField attribute = (IsRequiredField)Attribute.GetCustomAttribute(property, typeof(IsRequiredField));

        return attribute?.IsDefault ?? false;
    }
}