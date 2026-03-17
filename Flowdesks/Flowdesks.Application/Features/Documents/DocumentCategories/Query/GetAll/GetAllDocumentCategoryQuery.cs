using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Documents;
using Flowdesks.Application.Responses.Buldings;
using Flowdesks.Application.Responses.Documents;
using Flowdesks.Application.Specifications.Documents;
using Flowdesks.Domain.Entities.SystemPreferences.Documents;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Documents.DocumentCategories.Query;

public class GetAllDocumentCategoryQuery : IRequestHandler<DocumentCategoryPagingRequest, Result<PaginatedResult<DocumentCategoryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllDocumentCategoryQuery(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<DocumentCategoryResponse>>> Handle(DocumentCategoryPagingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            DocumentCategoryFilterSpecification spec = new(request);

            var query = _unitOfWork.Repository<DocumentCategory>().Entities().Specify(spec);

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.ApplySorting(request.SortColumn, request.SortOrder);
            }

            var response = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);

            var types = _mapper.Map<PaginatedResult<DocumentCategoryResponse>>(response);

            return Result<PaginatedResult<DocumentCategoryResponse>>.Success(types);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<DocumentCategoryResponse>>.Fail(ex.Message);
        }
    }
}
