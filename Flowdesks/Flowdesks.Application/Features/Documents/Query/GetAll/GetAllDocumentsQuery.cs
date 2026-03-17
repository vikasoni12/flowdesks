using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Documents;
using Flowdesks.Application.Responses.Buldings;
using Flowdesks.Application.Responses.Documents;
using Flowdesks.Application.Specifications.Documents;
using Flowdesks.Domain.Entities.Documents;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Documents.Query;

public class GetAllDocumentsQuery : IRequestHandler<DocumentPagingRequest, Result<PaginatedResult<DocumentResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllDocumentsQuery(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<PaginatedResult<DocumentResponse>>> Handle(DocumentPagingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            DocumentFilterSpecification spec = new(request);

            var query = _unitOfWork.Repository<Document>().Entities().Include(x => x.Files).Specify(spec);

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.ApplySorting(request.SortColumn, request.SortOrder);
            }

            var response = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);

            var documents = _mapper.Map<PaginatedResult<DocumentResponse>>(response);

            return Result<PaginatedResult<DocumentResponse>>.Success(documents);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<DocumentResponse>>.Fail(ex.Message);
        }
    }
}
