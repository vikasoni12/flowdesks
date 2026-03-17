namespace Flowdesks.Application.Responses.Asset;

public class AssetHealthAndFinanceDetailResponse
{
    public decimal? PurchaseCost { get; set; }
    public decimal? CurrentValue { get { return CalculateCurrentValue(); } }
    public decimal? DisposalValue { get; set; }
    public decimal? ReplacementCost { get; set; }
    public string? LifeSpan { get; set; }
    public string? Operational { get; set; }
    public string? HealthOrSafety { get; set; }
    public Guid? ConditionId { get; set; }
    public string Condition { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? WarrantyExpiresDate { get; set; }
    public DateTime? LastAssessed { get; set; }

    private decimal? CalculateCurrentValue()
    {
        if (PurchaseDate.HasValue && PurchaseCost.HasValue && !string.IsNullOrEmpty(LifeSpan))
        {

            if (!decimal.TryParse(LifeSpan, out decimal lifeSpan) || lifeSpan <= 0)
                return null;

            decimal? depreciationPerYear = (PurchaseCost - DisposalValue??0) / lifeSpan;

            TimeSpan elapsedLifespan = DateTime.UtcNow - PurchaseDate.Value;
            double elapsedYears = elapsedLifespan.TotalDays / 365;
            decimal? currentValue = PurchaseCost - (decimal)(elapsedYears * (double)depreciationPerYear);

            // // Round the current value to 2 decimal places
            return currentValue.HasValue ? Math.Round(currentValue.Value, 2) : null;
        }
        else
        {
            return null;
        }
    }
}