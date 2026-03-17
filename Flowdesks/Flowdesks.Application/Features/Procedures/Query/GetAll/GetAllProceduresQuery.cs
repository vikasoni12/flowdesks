using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Procedures;
using Flowdesks.Application.Responses.Procedures;
using Flowdesks.Application.Specifications.Procedures;
using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAllProceduresQuery : IRequestHandler<ProcedurePagingRequest, Result<PaginatedResult<ProcedurePagingResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllProceduresQuery(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<ProcedurePagingResponse>>> Handle(ProcedurePagingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            ProcedureFilterSpecification spec = new(request);

            var query = _unitOfWork.Repository<Procedure>().Entities().Include(x => x.AssetType).Include(x => x.Questions).ThenInclude(x => x.ProcedureQuestionOptions).Include(x => x.Sections).ThenInclude(x => x.Questions)
                                .ThenInclude(x => x.ProcedureQuestionOptions).Include(x => x.ProcedureMappings).ThenInclude(x => x.Responses).OrderByDescending(x => x.CreatedOn).Specify(spec);

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.ApplySorting(request.SortColumn, request.SortOrder);
            }

            var procedureData = await _mapper.ProjectTo<ProcedurePagingResponse>(query).ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return Result<PaginatedResult<ProcedurePagingResponse>>.Success(procedureData);

        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<ProcedurePagingResponse>>.Fail(ex.Message);
        }
    }
}