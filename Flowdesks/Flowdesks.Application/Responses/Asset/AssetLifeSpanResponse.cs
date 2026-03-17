namespace Flowdesks.Application.Responses.Asset
{
    public class AssetLifeSpanResponse
    {
        public Guid BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int WithInLifeSpan { get; set; }
        public int InWarranty { get; set; }
        public int ReplacementDue { get; set; }
    }
}
