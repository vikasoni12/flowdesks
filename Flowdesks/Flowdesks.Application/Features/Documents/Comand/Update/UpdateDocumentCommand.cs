using AutoMapper;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Documents;
using Flowdesks.Domain.Entities.Documents;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using Hangfire;
using MediatR;

namespace Flowdesks.Application.Features.Assets.Command;

public class UpdateDocumentCommandhandler : IRequestHandler<UpdateDocumentRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUploadService _uploadService;

    public UpdateDocumentCommandhandler(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _uploadService = uploadService;
    }

    public async Task<Result<int>> Handle(UpdateDocumentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var document = await _unitOfWork.Repository<Document>().GetByIdAsync(request.Id);

            if (document != null)
            {
                if (document.DocumentType.Equals(DocumentType.Document.ToString()))
                {
                    var previousFiles = _unitOfWork.Repository<DocumentFile>().Entities()
                        .Where(x => x.DocumentId.Equals(document.Id)).ToList();

                    var removedFiles = previousFiles.Where(
                        prevFile => !request.Files.Any(reqFile => reqFile.Url == prevFile.Url)).ToList();

                    BackgroundJob.Enqueue(() => _uploadService.DeleteManyAsync(removedFiles.Select(x => x.Url).ToList()));

                    _unitOfWork.Repository<DocumentFile>().DeleteRange(removedFiles);
                }

                if (request.DocumentType.Equals(DocumentType.Document.ToString()))
                {
                    request.ExternalUrl = null;
                }

                _mapper.Map(request, document);

                document.Files = _mapper.Map<List<DocumentFile>>(request.Files);

                _unitOfWork.Repository<Document>().Update(document);

                await _unitOfWork.SaveAsync(cancellationToken);

                return Result<int>.Success("Document updated successfully");
            }
            else
            {
                return Result<int>.Fail($"Document with {request.Id} not found");
            }

        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}
