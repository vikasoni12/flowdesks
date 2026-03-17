using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Extensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Site;
using Flowdesks.Application.Responses.Site;
using Flowdesks.Application.Specifications.Site;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Site.Query.Export;

public class ExportSitesAsCSVQuery : SitePagingRequest, IRequest<Result<List<ExportSiteResponse>>>
{
}

internal class ExportSitesAsCSVQueryHandler : IRequestHandler<ExportSitesAsCSVQuery, Result<List<ExportSiteResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExportSitesAsCSVQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<List<ExportSiteResponse>>> Handle(ExportSitesAsCSVQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var technicianFilterSpec = new SiteFilterSpecification(request, _userService);

            var sites = await _unitOfWork.Repository<Domain.Entities.Sites.Site>().Entities()
                .Specify(technicianFilterSpec)
                .ProjectTo<ExportSiteResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var httpContext = _httpContextAccessor.HttpContext;

            foreach (var site in sites)
            {
                site.ConvertUtcDateTimePropertiesToLocalTime(httpContext);
            }

            return await Result<List<ExportSiteResponse>>.SuccessAsync(data: sites);
        }
        catch (Exception ex)
        {
            return Result<List<ExportSiteResponse>>.Fail(ex.Message);
        }
    }
}