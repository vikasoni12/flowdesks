using AutoMapper;
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

namespace Flowdesks.Application.Features.Quotes.Query.GetAll
{
    public class GetAllQuoteQuery : QuotePagingRequest, IRequest<Result<PaginatedResult<QuoteResponse>>>
    {
    }

    public class GetAllQuoteQueryHandler : IRequestHandler<GetAllQuoteQuery, Result<PaginatedResult<QuoteResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _userService;

        public GetAllQuoteQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<PaginatedResult<QuoteResponse>>> Handle(GetAllQuoteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                QuoteFilterSpecification spec = new(request, _userService);

                var query = _unitOfWork.Repository<Quote>().Entities()
                    .Include(x => x.Building)
                    .Include(x => x.Site)
                    .Include(x => x.Category)
                    .Include(x => x.TechnicianQuotes).ThenInclude(x => x.Approver)
                    .Include(x => x.TechnicianQuotes).ThenInclude(x => x.Technician)
                    .Include(x => x.TechnicianQuotes).ThenInclude(x => x.Supplier)
                    .OrderByDescending(x => x.QuoteNumber.Equals(request.QuoteNumber))
                    .ThenByDescending(x => x.CreatedOn)
                    .Specify(spec);

                if (!string.IsNullOrEmpty(request.SortColumn))
                {
                    query = query.ApplySorting(request.SortColumn, request.SortOrder);
                }


                var quotesQuery = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);

                var quotes = _mapper.Map<PaginatedResult<QuoteResponse>>(quotesQuery);

                return Result<PaginatedResult<QuoteResponse>>.Success(quotes);
            }
            catch (Exception ex)
            {
                return Result<PaginatedResult<QuoteResponse>>.Fail(ex.Message);
            }
        }
    }
}

