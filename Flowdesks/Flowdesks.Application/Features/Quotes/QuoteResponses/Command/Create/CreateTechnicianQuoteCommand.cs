using AutoMapper;
using Flowdesks.Application.Interfaces.Notification;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Notification;
using Flowdesks.Application.Requests.Quotes.QuoteResponses;
using Flowdesks.Domain.Entities.Quotes;
using Flowdesks.Shared.Notification;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.QuoteResponses.QuoteResponseResponses.Command.Create;

public class CreateTechnicianQuoteCommand : IRequestHandler<CreateTechnicianQuoteRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTechnicianQuoteCommand(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(CreateTechnicianQuoteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var quoteResponse = _mapper.Map<CreateTechnicianQuoteRequest, TechnicianQuote>(request);

            quoteResponse.Status = "Pending";

            var response = _unitOfWork.Repository<TechnicianQuote>().Add(quoteResponse);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result<int>.Success("Quote Response saved successfully");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}
