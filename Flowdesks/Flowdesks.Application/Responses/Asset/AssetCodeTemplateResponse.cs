namespace Flowdesks.Application.Responses.Asset
{
    public class AssetCodeTemplateResponse
    {
        public Guid Id { get; set; }
        public string FieldName { get; set; }
        public int NoOfCharacter { get; set; }
        public bool IsTemplateUse { get; set; }
    }
}
