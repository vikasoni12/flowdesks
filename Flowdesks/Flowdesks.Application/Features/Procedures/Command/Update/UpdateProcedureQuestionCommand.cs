using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Procedures;
using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Procedures.Command.Update;

public class UpdateProcedureQuestionCommand : IRequestHandler<UpdateProcedureQuestionRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProcedureQuestionCommand(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(UpdateProcedureQuestionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            Procedure procedure = await _unitOfWork.Repository<Procedure>().Entities().Include(x => x.Sections).Include(x => x.Questions).FirstOrDefaultAsync(x => x.Id == request.ProcedureId, cancellationToken: cancellationToken);

            if(procedure != null)
            {
                if (request.Questions != null || request.Sections != null)
                {
                    var previousQuestions = _unitOfWork.Repository<ProcedureQuestion>().Entities()
                    .Where(x => x.ProcedureId.Equals(procedure.Id)).ToList();

                    if (previousQuestions != null)
                        _unitOfWork.Repository<ProcedureQuestion>().DeleteRange(previousQuestions);

                    var previousSections = _unitOfWork.Repository<ProcedureSection>().Entities()
                    .Where(x => x.ProcedureId.Equals(procedure.Id)).ToList();

                    if (previousSections != null)
                        _unitOfWork.Repository<ProcedureSection>().DeleteRange(previousSections);

                    _mapper.Map(request, procedure);

                    _unitOfWork.Repository<Procedure>().Update(procedure);
                    await _unitOfWork.SaveAsync(cancellationToken);
                }
            }

            return Result<int>.Success("Procedure Questions updated successfully");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}

