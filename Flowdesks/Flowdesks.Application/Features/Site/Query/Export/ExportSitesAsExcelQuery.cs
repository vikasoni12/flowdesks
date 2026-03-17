using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Identity;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Site;
using Flowdesks.Application.Responses.Site;
using Flowdesks.Application.Specifications.Site;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Site.Query.Export;

public class ExportSitesAsExcelQuery : SitePagingRequest, IRequest<Result<string>>
{
}

internal class ExportSitesAsExcelQueryHandler : IRequestHandler<ExportSitesAsExcelQuery, Result<string>>
{
    private readonly IExcelService _excelService;
    private readonly ICurrentUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ExportSitesAsExcelQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IExcelService excelService, ICurrentUserService userService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _excelService = excelService;
        _userService = userService;
    }

    public async Task<Result<string>> Handle(ExportSitesAsExcelQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var sitesFilterSpec = new SiteFilterSpecification(request, _userService);

            var sites = await _unitOfWork.Repository<Domain.Entities.Sites.Site>().Entities()
                .Specify(sitesFilterSpec)
                .ProjectTo<ExportSiteResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var data = await _excelService.ExportAsync(sites, mappers: new Dictionary<string, Func<ExportSiteResponse, object>>
            {
                { "Code", item => item.Code },
                { "Name", item => item.Name },
                { "Address", item => item.Address },
                { "PostCode", item => item.PostCode },
                { "Country", item => item.Country },               
                { "TelephoneNumber", item => item.TelephoneNumber },
                { "ContactNumber", item => item.ContactNumber },
            }, sheetName: $"Sites_{DateTime.UtcNow.Date}");

            return await Result<string>.SuccessAsync(data: data);
        }
        catch (Exception ex)
        {
            return Result<string>.Fail(ex.Message);
        }
    }
}
