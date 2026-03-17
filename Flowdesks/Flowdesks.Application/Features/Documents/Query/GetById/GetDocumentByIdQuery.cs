using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Responses.Documents;
using Flowdesks.Domain.Entities.Documents;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Documents.Query;

public class GetDocumentByIdQuery : IRequest<Result<DocumentResponse>>
{
    public Guid Id { get; set; }
}

public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, Result<DocumentResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDocumentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<DocumentResponse>> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var document = await _unitOfWork.Repository<Document>().Entities().Include(x => x.Files).ProjectTo<DocumentResponse>(_mapper.ConfigurationProvider)
           .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            return await Result<DocumentResponse>.SuccessAsync(document);
        }
        catch (Exception ex)
        {
            return Result<DocumentResponse>.Fail(ex.Message);
        }
    }
}
