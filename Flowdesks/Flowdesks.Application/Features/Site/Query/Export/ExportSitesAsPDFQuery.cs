using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Extensions;
using Flowdesks.Application.Interfaces.Common;
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

namespace Flowdesks.Application.Features.Assets.Query.Export;

public class ExportSitesAsPDFQuery : SitePagingRequest, IRequest<Result<string>>
{
}

internal class ExportSitesAsPDFQueryHandler : IRequestHandler<ExportSitesAsPDFQuery, Result<string>>
{
    private readonly IPDFGeneratorSerice _pdfService;
    private readonly ICurrentUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ExportSitesAsPDFQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IPDFGeneratorSerice pdfService, ICurrentUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _pdfService = pdfService;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<string>> Handle(ExportSitesAsPDFQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var sitesFilterSpec = new SiteFilterSpecification(request, _userService);

            var sites = await _unitOfWork.Repository<Domain.Entities.Sites.Site>().Entities()
                .Specify(sitesFilterSpec)
                .ProjectTo<ExportSiteResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var httpContext = _httpContextAccessor.HttpContext;

            foreach (var site in sites)
            {
                site.ConvertUtcDateTimePropertiesToLocalTime(httpContext);
            }

            var byteArray = _pdfService.GeneratePdf("Sites", sites);

            return await Result<string>.SuccessAsync(data: Convert.ToBase64String(byteArray));
        }
        catch (Exception ex)
        {
            return Result<string>.Fail(ex.Message);
        }
    }
}