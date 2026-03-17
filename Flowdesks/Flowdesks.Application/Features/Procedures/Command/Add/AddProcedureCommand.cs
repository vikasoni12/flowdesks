using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Procedures;
using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Procedures.Command.Add;

public class AddProcedureCommand : IRequestHandler<ProcedureRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddProcedureCommand(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(ProcedureRequest request, CancellationToken cancellationToken)
    {
        try
        {
            Procedure procedure = _mapper.Map<Procedure>(request);

            _unitOfWork.Repository<Procedure>().Add(procedure);
            await _unitOfWork.SaveAsync(cancellationToken);

            return Result<int>.Success("Procedure saved successfully");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}