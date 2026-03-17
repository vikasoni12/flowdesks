using AutoMapper;
using Flowdesks.Application.Interfaces.Identity;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Quotes;
using Flowdesks.Application.Responses.Quotes;
using Flowdesks.Domain.Entities.Quotes;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Quotes.Query.GetById
{
    public class GetQuoteByIdQuery : IRequestHandler<GetQuoteByIdRequest, Result<QuoteResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetQuoteByIdQuery(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<QuoteResponse>> Handle(GetQuoteByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                bool isUserLoggedIn = await _userService.IsUserLoggedInAsync();
                var Quote = await _unitOfWork.Repository<Quote>().Entities(isUserLoggedIn)
                    .Include(x => x.Building)
                    .Include(x => x.Site)
                    .Include(x => x.Category)
                    .Include(x => x.TechnicianQuotes)
                    .ThenInclude(x => x.Technician)
                    .Include(x => x.TechnicianQuotes)
                    .ThenInclude(x => x.Supplier)
                    .FirstOrDefaultAsync(x => x.QuoteNumber.Equals(request.QuoteId), cancellationToken: cancellationToken);

                var QuoteResponse = _mapper.Map<QuoteResponse>(Quote);

                return Result<QuoteResponse>.Success(QuoteResponse);
            }
            catch (Exception ex)
            {
                return Result<QuoteResponse>.Fail(ex.Message);
            }
        }
    }
}
