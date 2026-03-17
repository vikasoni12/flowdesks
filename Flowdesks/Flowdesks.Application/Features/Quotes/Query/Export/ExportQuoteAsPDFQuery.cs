using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Extensions;
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
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Quotes.Query.Export;

public class ExportQuoteAsPDFQuery : QuotePagingRequest, IRequest<Result<string>>
{
}

internal class ExportQuoteAsPDFQueryHandler : IRequestHandler<ExportQuoteAsPDFQuery, Result<string>>
{
    private readonly IPDFGeneratorSerice _pdfService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExportQuoteAsPDFQueryHandler(IPDFGeneratorSerice generatorSerice, IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _pdfService = generatorSerice;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(ExportQuoteAsPDFQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var quoteFilterSpec = new QuoteFilterSpecification(request, _userService);

            var quotes = await _unitOfWork.Repository<Quote>().Entities()
                .Specify(quoteFilterSpec)
                .ProjectTo<ExportQuoteResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var httpContext = _httpContextAccessor.HttpContext;

            foreach (var quote in quotes)
            {
                quote.ConvertUtcDateTimePropertiesToLocalTime(httpContext);
            }

            var byteArray = _pdfService.GeneratePdf("Quotes", quotes);

            return await Result<string>.SuccessAsync(data: Convert.ToBase64String(byteArray));
        }
        catch (Exception ex)
        {
            return Result<string>.Fail(ex.Message);
        }
    }
}
