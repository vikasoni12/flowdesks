using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Documents;
using Flowdesks.Domain.Entities.SystemPreferences.Documents;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Documents.DocumentCategories.Command;

public class AddDocumentCategoryCommand : AddUpdateDocumentCategoryRequest, IRequest<Result<int>>
{
}

public class AddDocumentCategoryCommandHandler : IRequestHandler<AddDocumentCategoryCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddDocumentCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(AddDocumentCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var type = _mapper.Map<DocumentCategory>(request);

            _unitOfWork.Repository<DocumentCategory>().Add(type);
            await _unitOfWork.SaveAsync();

            return Result<int>.Success();
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}
