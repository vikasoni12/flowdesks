using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Domain.Entities.Documents;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Documents.Comand.Delete;

public class DeleteDocumentCommand : IRequest<Result<int>>
{
    public List<Guid>? Ids { get; set; }
    public string? EntityId {  get; set; }
    public string? EntityType {  get; set; }
}

public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUploadService _uploadService;

    public DeleteDocumentCommandHandler(IUnitOfWork unitOfWork, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork;
        _uploadService = uploadService;
    }

    public async Task<Result<int>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var documents = _unitOfWork.Repository<Document>().Entities()
                .Include(x => x.Files).WhereIf(request.Ids != null && request.Ids.Count > 0, x => request.Ids.Contains(x.Id));

            if (documents != null)
            {
                BackgroundJob.Enqueue(() => _uploadService.DeleteManyAsync(documents.SelectMany(x => x.Files).Select(x => x.Url).ToList()));
                _unitOfWork.Repository<Document>().DeleteRange(documents.AsEnumerable());
                await _unitOfWork.SaveAsync(cancellationToken);
            }

            return await Result<int>.SuccessAsync("Document deleted successfully");
        }
        catch (Exception ex)
        {
            return await Result<int>.FailAsync(ex.Message);
        }
    }
}
