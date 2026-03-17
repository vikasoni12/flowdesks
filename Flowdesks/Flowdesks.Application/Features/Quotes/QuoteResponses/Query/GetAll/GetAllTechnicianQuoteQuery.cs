using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Quotes;
using Flowdesks.Application.Responses.Quotes;
using Flowdesks.Application.Specifications.TechnicianQuote;
using Flowdesks.Domain.Entities.Quotes;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.TechnicianQuotes.Query.GetAll
{
    public class GetAllTechnicianQuoteQuery : TechnicianQuotePagingRequest, IRequest<Result<PaginatedResult<TechnicianQuoteResponse>>>
    {
    }

    public class GetAllTechnicianQuoteQueryHandler : IRequestHandler<GetAllTechnicianQuoteQuery, Result<PaginatedResult<TechnicianQuoteResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTechnicianQuoteQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedResult<TechnicianQuoteResponse>>> Handle(GetAllTechnicianQuoteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                TechnicianQuoteFilterSpecification spec = new(request);

                var query = _unitOfWork.Repository<TechnicianQuote>().Entities()
                    .Include(x => x.Quote)
                    .Include(x => x.Technician)
                    .Include(s=>s.Supplier)
                    .Specify(spec);

                if (!string.IsNullOrEmpty(request.SortColumn))
                {
                    query = query.ApplySorting(request.SortColumn, request.SortOrder);
                }


                var TechnicianQuotesQuery = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);

                var TechnicianQuotes = _mapper.Map<PaginatedResult<TechnicianQuoteResponse>>(TechnicianQuotesQuery);

                return Result<PaginatedResult<TechnicianQuoteResponse>>.Success(TechnicianQuotes);
            }
            catch (Exception ex)
            {
                return Result<PaginatedResult<TechnicianQuoteResponse>>.Fail(ex.Message);
            }
        }
    }
}

