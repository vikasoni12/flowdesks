using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Documents;
using Flowdesks.Domain.Entities.SystemPreferences.Documents;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Assets.Command;

public class UpdateDocumentCategoryCommand : AddUpdateDocumentCategoryRequest, IRequest<Result<int>>
{
    public Guid Id { get; set; }
}

public class UpdateDocumentCategoryCommandHandler : IRequestHandler<UpdateDocumentCategoryCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDocumentCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(UpdateDocumentCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var documentCategory = await _unitOfWork.Repository<DocumentCategory>().GetByIdAsync(request.Id);

            if (documentCategory != null)
            {
                _mapper.Map<AddUpdateDocumentCategoryRequest, DocumentCategory>(request, documentCategory);

                _unitOfWork.Repository<DocumentCategory>().Update(documentCategory);

                await _unitOfWork.SaveAsync();

                return Result<int>.Success("Building type updated successfully");
            }
            else
            {
                return Result<int>.Fail($"Building type with {request.Id} not found");
            }

        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}
