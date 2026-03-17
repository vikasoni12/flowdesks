using AutoMapper;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Site;
using Flowdesks.Application.Requests.UploadFiles;
using Flowdesks.Shared.Wrapper;
using MediatR;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;
using SiteEntity = Flowdesks.Domain.Entities.Sites;

namespace Flowdesks.Application.Features.Site.Command.Create
{
    public class CreateSiteCommand : IRequestHandler<CreateSiteRequest, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSiteCommand(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _uploadService = uploadService;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateSiteRequest request, CancellationToken cancellationToken)
        {
            var alreadyExists = _unitOfWork.Repository<SiteEntity.Site>()
                .Entities().Any(x => x.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (alreadyExists)
            {
                return Result<int>.Fail($"Site with code : {request.Code} already exists");
            }
            try
            {
                var site = _mapper.Map<CreateSiteRequest, SiteEntity.Site>(request);

                if (request.ProfilePicture?.FileName != null)
                {
                    var profilePictureUrl = await _uploadService.UploadAsync(new UploadRequest
                    {
                        Path = FileUploadUrl.Site,
                        FileBytes = request.ProfilePicture.Content,
                        FileName = request.ProfilePicture.FileName
                    });

                    site.ProfilePictureUrl = profilePictureUrl.Data;
                }

                _unitOfWork.Repository<SiteEntity.Site>().Add(site);

                await _unitOfWork.SaveAsync(cancellationToken);

                return Result<int>.Success("Site saved successfully");
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex.Message);
            }
        }
    }
}