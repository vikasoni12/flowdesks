using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Permit;
using Flowdesks.Application.Responses.Buldings;
using Flowdesks.Application.Responses.Permit;
using Flowdesks.Application.Specifications.Permit;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using PermitEntity = Flowdesks.Domain.Entities.Permit.Permit;

namespace Flowdesks.Application.Features.Permit.Query.GetAll;

public class GetAllPermitQuery : IRequestHandler<PermitPagingRequest, Result<PaginatedResult<PermitResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllPermitQuery(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<PermitResponse>>> Handle(PermitPagingRequest request, CancellationToken cancellationToken)
    {
        PermitFilterSpecification spec = new(request);

        var query = _unitOfWork.Repository<PermitEntity>().Entities().Specify(spec);

        if (!string.IsNullOrEmpty(request.SortColumn))
        {
            query = query.ApplySorting(request.SortColumn, request.SortOrder);
        }

        var response = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);

        var permits = _mapper.Map<PaginatedResult<PermitResponse>>(response);

        return Result<PaginatedResult<PermitResponse>>.Success(permits);
    }
}
