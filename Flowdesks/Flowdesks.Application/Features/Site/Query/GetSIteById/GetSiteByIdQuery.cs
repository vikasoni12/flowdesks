using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Site;
using Flowdesks.Application.Responses.Site;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiteEntity = Flowdesks.Domain.Entities.Sites.Site;

namespace Flowdesks.Application.Features.Site.Query.GetSIteById
{
    public class GetSiteByIdQuery : IRequestHandler<GetSiteByIdRequest, Result<SiteResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetSiteByIdQuery(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<SiteResponse>> Handle(GetSiteByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var site = await _unitOfWork.Repository<SiteEntity>().Entities().Include(x=>x.SiteCountry).FirstOrDefaultAsync(x => x.Id == request.Id);

                var siteResponse = _mapper.Map<SiteResponse>(site);

                return Result<SiteResponse>.Success(siteResponse);
            }
            catch (Exception ex)
            {
                return Result<SiteResponse>.Fail(ex.Message);
            }
        }
    }
}
