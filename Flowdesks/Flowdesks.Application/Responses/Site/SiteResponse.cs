namespace Flowdesks.Application.Responses.Site
{
    public class SiteResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public Guid? CountryId { get; set; }
        public string CountryName { get; set; }
        public string TelephoneNumber { get; set; }
        public string ContactNumber { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ProfilePictureId { get; set; }
        public List<SiteTeam>? Teams { get; set; }
    }

    public class SiteTeam
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
