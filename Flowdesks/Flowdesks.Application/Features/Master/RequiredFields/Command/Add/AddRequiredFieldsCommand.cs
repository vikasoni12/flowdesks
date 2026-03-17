using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.RequiredFields;
using Flowdesks.Domain.Entities.SystemPreferences;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Flowdesks.Application.Features.Master.RequiredFields.Command.Add;

public class AddRequiredFieldsCommand : IRequestHandler<AddRequiredFieldsRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddRequiredFieldsCommand(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<int>> Handle(AddRequiredFieldsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var existingFields = _unitOfWork.Repository<EntityRequiredField>().Entities()
                .Where(x => x.EntityType.Equals(request.EntityType.ToString())).ToList();

            if (existingFields.Any())
            {
                _unitOfWork.Repository<EntityRequiredField>().DeleteRange(existingFields);
            }

            var tenantId = GetTenantId();
            var entities = request.Fields.Select(x => new EntityRequiredField(tenantId.Value, x.Value, x.Title, request.EntityType.ToString()));

            _unitOfWork.Repository<EntityRequiredField>().AddRange(entities);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result<int>.Success();
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }

    private Guid? GetTenantId()
    {
        if (_httpContextAccessor.HttpContext?.Items?.TryGetValue("TenantId", out object tenantIdValue) ?? false)
        {
            if (Guid.TryParse(tenantIdValue?.ToString(), out Guid guidValue))
            {
                return guidValue;
            }
        }

        return null;
    }
}
