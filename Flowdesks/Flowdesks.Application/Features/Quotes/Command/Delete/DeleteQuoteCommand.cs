using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Domain.Entities.Quotes;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.Quotes.Command.Delete
{
    public class DeleteQuoteCommand : IRequest<Result<int>>
    {
        public List<Guid>? QuoteIds { get; set; }
    }

    public class DeleteQuoteCommandHandler : IRequestHandler<DeleteQuoteCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuoteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteQuoteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var quotes = _unitOfWork.Repository<Quote>().Entities()
                    .WhereIf(request.QuoteIds != null && request.QuoteIds.Count > 0, x => request.QuoteIds.Contains(x.Id)).ToList();

                if (quotes == null || quotes.Count <= 0)
                {
                    return await Result<int>.FailAsync($"Not found");
                }

                _unitOfWork.Repository<Quote>().DeleteRange(quotes, true);
                await _unitOfWork.SaveAsync(cancellationToken);

                return await Result<int>.SuccessAsync("Quote deleted successfully");
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(ex.Message);
            }
        }
    }
}
