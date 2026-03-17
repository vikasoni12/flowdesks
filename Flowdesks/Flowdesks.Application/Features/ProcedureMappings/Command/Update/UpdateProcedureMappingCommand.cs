using AutoMapper;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Procedures;
using Flowdesks.Application.Requests.UploadFiles;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;

namespace Flowdesks.Application.Features.ProcedureMappings.Command.Update;

public class UpdateProcedureMappingCommand : IRequestHandler<UpdateProcedureMappingRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUploadService _uploadService;

    public UpdateProcedureMappingCommand(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _uploadService = uploadService;
    }

    public async Task<Result<int>> Handle(UpdateProcedureMappingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var procedure = await _unitOfWork.Repository<ProcedureMapping>().Entities().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (procedure != null)
            {
                var previousResponses = _unitOfWork.Repository<ProcedureResponse>()
                .Entities()
                .Include(x => x.ProcedureQuestion)
                .Where(x => x.ProcedureMappingId.Equals(procedure.Id))
                .ToList();

                if (previousResponses != null && previousResponses.Count > 0)
                {
                    foreach (var response in previousResponses)
                    {

                        bool isResponseNull = request.Responses?.FirstOrDefault(x =>
                           x.ProcedureQuestionId == response.ProcedureQuestionId &&
                           (response.ProcedureQuestion.QuestionType == "Image/File" 
                           ) && x.Response.StartsWith("/Images/Procedure/") &&
                           (x.Image == null || string.IsNullOrEmpty(x.Image.FileName))
                           ) != null;

                        if (!isResponseNull)
                        {
                            if (!string.IsNullOrEmpty(response.Response) && response.Response.StartsWith("/Images/Procedure/"))
                            {
                                await _uploadService.DeleteAsync(response.Response);
                                _unitOfWork.Repository<MessageAttachment>().Delete(response.AttachmentId, true);
                            }

                            _unitOfWork.Repository<ProcedureResponse>().Delete(response.Id);
                        }
                        else
                        {
                            var responseToRemove = request.Responses?.FirstOrDefault(x =>
                            x.ProcedureQuestionId == response.ProcedureQuestionId &&
                            (response.ProcedureQuestion.QuestionType == "Image/File" 
                            ) && x.Response.StartsWith("/Images/Procedure/") &&
                            (x.Image == null || string.IsNullOrEmpty(x.Image.FileName))
                            );

                            if (responseToRemove != null)
                            {
                                request.Responses.Remove(responseToRemove);
                                request.Responses.Add(_mapper.Map<ProcedureResponseRequest>(response));
                                _unitOfWork.Repository<ProcedureResponse>().Delete(response.Id);
                            }

                        }
                    }
                }

                if (request.Responses != null && request.Responses.Any(x => x.Image != null))
                {
                    foreach (var response in request.Responses)
                    {
                        if (response.Image != null && !string.IsNullOrEmpty(response.Image.FileName))
                        {
                            var profilePictureUrl = await _uploadService.UploadAsync(new UploadRequest
                            {
                                Path = FileUploadUrl.Procedure,
                                FileBytes = response.Image.Content,
                                FileName = response.Image.FileName
                            });

                            response.Response = profilePictureUrl.Data;
                            response.Attachment.Url = profilePictureUrl.Data;
                        }
                    }
                }

                _mapper.Map(request, procedure);

                _unitOfWork.Repository<ProcedureMapping>().Update(procedure);
                await _unitOfWork.SaveAsync(cancellationToken);
            }

            return Result<int>.Success("Procedure saved successfully");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}