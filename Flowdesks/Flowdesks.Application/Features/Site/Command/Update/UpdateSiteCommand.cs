using AutoMapper;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Site;
using Flowdesks.Application.Requests.UploadFiles;
using Flowdesks.Application.Responses.Site;
using Flowdesks.Domain.Entities.Buildings;
using Flowdesks.Shared.Wrapper;
using MediatR;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;
using SiteEntity = Flowdesks.Domain.Entities.Sites;

namespace Flowdesks.Application.Features.Site.Command.Update
{
    public class UpdateSiteCommand : IRequestHandler<UpdateSiteRequest, Result<SiteResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSiteCommand(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        public async Task<Result<SiteResponse>> Handle(UpdateSiteRequest request, CancellationToken cancellationToken)
        {
            var alreadyExists = _unitOfWork.Repository<SiteEntity.Site>().Entities().Any(x => x.Id != request.Id && x.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (alreadyExists)
            {
                return Result<SiteResponse>.Fail($"Site with code : {request.Code} already exists");
            }
            try
            {
                var site = await _unitOfWork.Repository<SiteEntity.Site>().GetByIdAsync(request.Id);

                if (site != null)
                {
                    if (!string.IsNullOrEmpty(site.ProfilePictureUrl) && (string.IsNullOrEmpty(request.ProfilePictureUrl) || request.ImageName?.FileName != null))
                    {
                        await _uploadService.DeleteAsync(site.ProfilePictureUrl);
                    }

                    _mapper.Map(request, site);

                    if (request.ImageName != null && request.ImageName.FileName != null)
                    {
                        var profilePictureUrl = await _uploadService.UploadAsync(new UploadRequest
                        {
                            Path = FileUploadUrl.Site,
                            FileBytes = request.ImageName.Content,
                            FileName = request.ImageName.FileName
                        });

                        site.ProfilePictureUrl = profilePictureUrl.Data;
                    }

                    _unitOfWork.Repository<SiteEntity.Site>().Update(site);
                    await _unitOfWork.SaveAsync();

                    var siteResponse = _mapper.Map<SiteResponse>(site);

                    return Result<SiteResponse>.Success(siteResponse);
                }
                else
                {
                    return Result<SiteResponse>.Fail($"Site with {request.Id} not found");
                }

            }
            catch (Exception ex)
            {
                return Result<SiteResponse>.Fail(ex.Message);
            }
        }
    }
}
