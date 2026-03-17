using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Responses.Quotes;
using Flowdesks.Domain.Entities.Quotes;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.TechnicianQuotes.Query.GetById
{
    public class GetTechnicianQuoteByIdRequest : IRequest<Result<TechnicianQuoteResponse>>
    {
        public Guid Id { get; set; }
    }

    public class GetTechnicianQuoteByIdQuery : IRequestHandler<GetTechnicianQuoteByIdRequest, Result<TechnicianQuoteResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTechnicianQuoteByIdQuery(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<TechnicianQuoteResponse>> Handle(GetTechnicianQuoteByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var technicianQuote = await _unitOfWork.Repository<TechnicianQuote>().Entities()
                    .Include(x => x.Quote)
                    .Include(x => x.Technician)
                    .Include(x=>x.Supplier)
                    .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken: cancellationToken);

                var TechnicianQuoteResponse = _mapper.Map<TechnicianQuoteResponse>(technicianQuote);

                return Result<TechnicianQuoteResponse>.Success(TechnicianQuoteResponse);
            }
            catch (Exception ex)
            {
                return Result<TechnicianQuoteResponse>.Fail(ex.Message);
            }
        }
    }
}
