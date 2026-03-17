using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Documents.Comand.Delete;

public class DeleteDocumentFileCommand : IRequest<Result<int>>
{
    public string Url { get; set; }
}

public class DeleteDocumentFileCommandHandler : IRequestHandler<DeleteDocumentFileCommand, Result<int>>
{
    private readonly IUploadService _uploadService;

    public DeleteDocumentFileCommandHandler(IUploadService uploadService)
    {
        _uploadService = uploadService;
    }

    public async Task<Result<int>> Handle(DeleteDocumentFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await _uploadService.DeleteAsync(request.Url);
        }
        catch (Exception ex)
        {
            return await Result<int>.FailAsync(ex.Message);
        }
    }
}
