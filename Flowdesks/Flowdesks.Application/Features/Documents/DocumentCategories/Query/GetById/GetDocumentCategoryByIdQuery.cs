using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Responses.Documents;
using Flowdesks.Domain.Entities.SystemPreferences.Documents;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Documents.DocumentCategories.Query;

public class GetDocumentCategoryByIdQuery : IRequest<Result<DocumentCategoryResponse>>
{
    public Guid Id { get; set; }
}

public class GetDocumentCategoryByIdQueryHandler : IRequestHandler<GetDocumentCategoryByIdQuery, Result<DocumentCategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDocumentCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<DocumentCategoryResponse>> Handle(GetDocumentCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var type = await _unitOfWork.Repository<DocumentCategory>().Entities().ProjectTo<DocumentCategoryResponse>(_mapper.ConfigurationProvider)
           .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            return await Result<DocumentCategoryResponse>.SuccessAsync(type);
        }
        catch (Exception ex)
        {
            return Result<DocumentCategoryResponse>.Fail(ex.Message);
        }
    }
}
