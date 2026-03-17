using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Responses.Procedures;
using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Procedures.Query.GetById;

public class GetProcedureByIdQuery : IRequest<Result<ProcedurePagingResponse>>
{
    public Guid Id { get; set; }
    public Guid? ProcedureMappingId { get; set; }
}

public class GetProcedureByIdQueryHandler : IRequestHandler<GetProcedureByIdQuery, Result<ProcedurePagingResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProcedureByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ProcedurePagingResponse>> Handle(GetProcedureByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Procedure>().Entities()
                                .Include(x => x.Sections)
                                .ThenInclude(x => x.Questions)
                                .ThenInclude(x => x.ProcedureQuestionOptions)
                                .Include(x => x.Questions)
                                .ThenInclude(x => x.ProcedureQuestionOptions)
                                .Include(x => x.ProcedureMappings.Where(pm => pm.Id == request.ProcedureMappingId))
                                .ThenInclude(x => x.Responses)
                                .ThenInclude(x => x.Attachment)
                                .Where(x => x.Id == request.Id);

            var procedures = await query.ToListAsync(cancellationToken);
            var procedure = _mapper.Map<List<ProcedurePagingResponse>>(procedures).FirstOrDefault();

            return Result<ProcedurePagingResponse>.Success(procedure);
        }
        catch (Exception ex)
        {
            return Result<ProcedurePagingResponse>.Fail(ex.Message);
        }
    }
}