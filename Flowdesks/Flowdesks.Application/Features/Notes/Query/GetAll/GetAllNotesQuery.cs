using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Notes;
using Flowdesks.Application.Responses.Buldings;
using Flowdesks.Application.Responses.Notes;
using Flowdesks.Application.Specifications.Notes;
using Flowdesks.Domain.Entities.Note;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Notes.Query.GetAll;

public class GetAllNotesQuery : IRequestHandler<NotePagingRequest, Result<PaginatedResult<NoteResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllNotesQuery(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<NoteResponse>>> Handle(NotePagingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            NoteFilterSpecification spec = new(request);

            IQueryable<Note> query = _unitOfWork.Repository<Note>().Entities().Include(x => x.User).Specify(spec).OrderBy(x => x.CreatedOn);

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.ApplySorting(request.SortColumn, request.SortOrder);
            }

            var response = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);

            var notes = _mapper.Map<PaginatedResult<NoteResponse>>(response);

            return Result<PaginatedResult<NoteResponse>>.Success(notes);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<NoteResponse>>.Fail(ex.Message);
        }
    }
}