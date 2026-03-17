using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Identity;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Site;
using Flowdesks.Application.Responses.Buldings;
using Flowdesks.Application.Responses.Site;
using Flowdesks.Application.Specifications.Site;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Flowdesks.Application.Features.Site.Query.GetAll;

public class GetAllSiteQuery : SitePagingRequest, IRequest<Result<PaginatedResult<SiteResponse>>>
{

}

public class GetAllSiteQueryHandler : IRequestHandler<GetAllSiteQuery, Result<PaginatedResult<SiteResponse>>>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllSiteQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService userService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userService = userService;
    }
    public async Task<Result<PaginatedResult<SiteResponse>>> Handle(GetAllSiteQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var spec = new SiteFilterSpecification(request, _userService);

            var site = _unitOfWork.Repository<Domain.Entities.Sites.Site>().Entities()
                .Include(x => x.SiteCountry)
                .Include(x=>x.Teams)
                .Specify(spec);

            if (request.SortColumn != null && request.SortColumn != "")
            {
                site = site.ApplySorting(request.SortColumn, request.SortOrder);
            }

            var siteResult = await site.ProjectTo<SiteResponse>(_mapper.ConfigurationProvider).ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return Result<PaginatedResult<SiteResponse>>.Success(siteResult);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<SiteResponse>>.Fail(ex.Message);
        }
    }
}
