using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Procedures;
using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.ProcedureMappings.Command.Add;

public class AddProcedureMappingCommand : IRequestHandler<ProcedureMappingRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddProcedureMappingCommand(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(ProcedureMappingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            ProcedureMapping procedure = _mapper.Map<ProcedureMapping>(request);

            _unitOfWork.Repository<ProcedureMapping>().Add(procedure);
            await _unitOfWork.SaveAsync(cancellationToken);

            return Result<int>.Success("Procedure saved successfully");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}