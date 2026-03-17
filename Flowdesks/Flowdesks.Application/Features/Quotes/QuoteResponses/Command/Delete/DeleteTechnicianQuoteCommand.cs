using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Domain.Entities.Quotes;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Features.TechnicianQuotes.Command.Delete
{
    public class DeleteTechnicianQuoteCommand : IRequest<Result<int>>
    {
        public List<Guid>? Ids { get; set; }
    }

    public class DeleteTechnicianQuoteCommandHandler : IRequestHandler<DeleteTechnicianQuoteCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTechnicianQuoteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteTechnicianQuoteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var technicianQuotes = _unitOfWork.Repository<TechnicianQuote>().Entities()
                    .WhereIf(request.Ids != null && request.Ids.Count > 0, x => request.Ids.Contains(x.Id)).ToList();

                if (technicianQuotes == null || technicianQuotes.Count <= 0)
                {
                    return await Result<int>.FailAsync($"Not found");
                }

                _unitOfWork.Repository<TechnicianQuote>().DeleteRange(technicianQuotes, true);
                await _unitOfWork.SaveAsync(cancellationToken);

                return await Result<int>.SuccessAsync("Technician Quote deleted successfully");
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(ex.Message);
            }
        }
    }
}
