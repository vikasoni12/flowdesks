namespace Flowdesks.Application.Responses.Asset
{
    public class AssetReplacementResponse
    {
        public decimal Cost { get; set; }
        public string Year { get; set; }
    }

    public class AssetReplacementDetailResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime ExpectedReplacement { get; set; }
        public decimal Cost { get; set; }
    }
}
