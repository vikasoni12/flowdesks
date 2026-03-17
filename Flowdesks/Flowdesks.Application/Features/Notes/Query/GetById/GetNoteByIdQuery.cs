using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Features.Documents.Query;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Responses.Documents;
using Flowdesks.Application.Responses.Notes;
using Flowdesks.Domain.Entities.Note;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Notes.Query.GetById;

public class GetNoteByIdQuery : IRequest<Result<NoteResponse>>
{
    public Guid Id { get; set; }
}

public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, Result<NoteResponse>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetNoteByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<NoteResponse>> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            NoteResponse note = await _unitOfWork.Repository<Note>().Entities().ProjectTo<NoteResponse>(_mapper.ConfigurationProvider)
           .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            return await Result<NoteResponse>.SuccessAsync(note);
        }
        catch (Exception ex)
        {
            return Result<NoteResponse>.Fail(ex.Message);
        }
    }
}