using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Procedures;
using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Procedures.Command.Update;

public class UpdateProcedureCommand : IRequestHandler<UpdateProcedureRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProcedureCommand(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(UpdateProcedureRequest request, CancellationToken cancellationToken)
    {
        try
        {
            Procedure procedure = await _unitOfWork.Repository<Procedure>().Entities().Include(x => x.Sections).Include(x => x.Questions).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);


            if (procedure != null)
            {
                if (procedure.Category != request.Category)
                {
                    var previousMappings = _unitOfWork.Repository<ProcedureMapping>().Entities()
                    .Where(x => x.ProcedureId.Equals(procedure.Id)).ToList();

                    if (previousMappings != null)
                        _unitOfWork.Repository<ProcedureMapping>().DeleteRange(previousMappings);
                }

                _mapper.Map(request, procedure);

                _unitOfWork.Repository<Procedure>().Update(procedure);
                await _unitOfWork.SaveAsync(cancellationToken);
            }
            return Result<int>.Success("Procedure updated successfully");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}