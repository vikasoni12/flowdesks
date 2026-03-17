using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Quotes;
using Flowdesks.Application.Responses.Quotes;
using Flowdesks.Application.Specifications.Quote;
using Flowdesks.Domain.Entities.Quotes;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Reflection;

namespace Flowdesks.Application.Features.Quotes.Query.Export;

public class ExportQuoteAsExcelQuery :QuotePagingRequest, IRequest<Result<string>>
{
}
internal class ExportQuoteAsExcelQueryHandler : IRequestHandler<ExportQuoteAsExcelQuery, Result<string>>
{
    private readonly IExcelService _excelService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    public ExportQuoteAsExcelQueryHandler(IExcelService excelService,IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService userService)
    {
        _excelService = excelService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<Result<string>> Handle(ExportQuoteAsExcelQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var quoteFilterSpec = new QuoteFilterSpecification(request, _userService);

            var quote = await _unitOfWork.Repository<Quote>().Entities()
                                          .Specify(quoteFilterSpec)
                                          .ProjectTo<ExportQuoteResponse>(_mapper.ConfigurationProvider)
                                          .ToListAsync(cancellationToken);

            var propertyMappers = typeof(ExportQuoteResponse).GetProperties()
             .ToDictionary(
                 prop =>
                 {
                     var descriptionAttribute = prop.GetCustomAttribute<DescriptionAttribute>();
                     return descriptionAttribute?.Description ?? prop.Name ?? "Unknown";
                 },
                 prop => (Func<ExportQuoteResponse, object>)(item => prop.GetValue(item))
             );

            var data = await _excelService.ExportAsync(quote, mappers: propertyMappers, sheetName: $"Quotes_{DateTime.UtcNow.Date}");

            return await Result<string>.SuccessAsync(data: data);
        }
        catch (Exception ex)
        {
            return Result<string>.Fail(ex.Message);
        }
    }
}
