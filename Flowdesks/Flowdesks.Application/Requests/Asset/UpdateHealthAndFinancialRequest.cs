using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Asset;

public class UpdateHealthAndFinancialRequest : CreateEditRequest<Domain.Entities.Assets.AssetHealthAndFinanceDetail>, IRequest<Result<int>>
{
    public Guid AssetId { get; set; }

    //Health and Finance details
    public DateTime? PurchaseDate { get; set; }
    public decimal? PurchaseCost { get; set; }
    public decimal? LifeSpan { get; set; }
    public DateTime? WarrantyExpiresDate { get; set; }
    public decimal? CurrentValue { get; set; }
    public decimal? DisposalValue { get; set; }
    public decimal? ReplacementCost { get; set; }
    public string? Operational { get; set; }
    public string? HealthOrSafety { get; set; }
    public Guid? ConditionId { get; set; }
    public DateTime? LastAssessed { get; set; }
}
