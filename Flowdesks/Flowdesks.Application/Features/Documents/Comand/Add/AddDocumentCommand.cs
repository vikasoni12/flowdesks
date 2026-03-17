using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Documents;
using Flowdesks.Domain.Entities.Documents;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Documents.Comand;

public class AddDocumentCommand : IRequestHandler<AddDocumentRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddDocumentCommand(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(AddDocumentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var document = _mapper.Map<Document>(request);

            _unitOfWork.Repository<Document>().Add(document);
            await _unitOfWork.SaveAsync(cancellationToken);

            return Result<int>.Success();
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}
