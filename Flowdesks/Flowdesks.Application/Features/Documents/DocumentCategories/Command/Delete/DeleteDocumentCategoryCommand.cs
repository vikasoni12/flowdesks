using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Domain.Entities.SystemPreferences.Documents;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Documents.DocumentCategories.Command;

public class DeleteDocumentCategoryCommand : IRequest<Result<int>>
{
    public Guid Id { get; set; }
}

public class DeleteDocumentCategoryCommandHandler : IRequestHandler<DeleteDocumentCategoryCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDocumentCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(DeleteDocumentCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var type = await _unitOfWork.Repository<DocumentCategory>().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (type == null)
            {
                return await Result<int>.FailAsync("Invalid building type Id");
            }

            _unitOfWork.Repository<DocumentCategory>().Delete(request.Id);
            await _unitOfWork.SaveAsync();

            return await Result<int>.SuccessAsync("type deleted successfully");
        }
        catch (Exception ex)
        {
            return await Result<int>.FailAsync(ex.Message);
        }
    }
}
