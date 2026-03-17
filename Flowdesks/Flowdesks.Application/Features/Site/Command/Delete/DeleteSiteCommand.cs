using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Site;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SiteEntity = Flowdesks.Domain.Entities.Sites;

namespace Flowdesks.Application.Features.Site.Command.Delete
{
    public class DeleteSiteCommand : IRequestHandler<DeleteSiteRequest, Result<List<Guid>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<DeleteSiteRequest> _localizer;

        public DeleteSiteCommand(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<DeleteSiteRequest> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<List<Guid>>> Handle(DeleteSiteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = _unitOfWork.Repository<SiteEntity.Site>().Entities().WhereIf(request.Ids != null && request.Ids.Count > 0, x => request.Ids.Contains(x.Id)).ToList();
                if (result != null && result.Count > 0)
                {
                    _unitOfWork.Repository<SiteEntity.Site>().DeleteRange(result);

                    await _unitOfWork.SaveAsync();

                    return await Result<List<Guid>>.SuccessAsync(_localizer["Site Deleted"]);
                }
                else
                {
                    return await Result<List<Guid>>.FailAsync(_localizer["Site Not Found!"]);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
