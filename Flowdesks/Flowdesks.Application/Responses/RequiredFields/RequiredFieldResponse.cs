namespace Flowdesks.Application.Responses.RequiredFields
{
    public class RequiredFieldResponse
    {
        public Guid Id { get; set; }
        public string EntityType { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
