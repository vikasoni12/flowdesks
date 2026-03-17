using System.ComponentModel;

namespace Flowdesks.Application.Responses.Site
{
    public class ExportSiteResponse
    {
        [Description("Code")]
        public string Code { get; set; }

        [Description("Name")]
        public string Name { get; set; }

        [Description("Address")]
        public string Address { get; set; }

        [Description("Post Code")]
        public string PostCode { get; set; }

        [Description("Country")]
        public string Country { get; set; }
       
        [Description("Telephone Number")]
        public string? TelephoneNumber { get; set; }

        [Description("Contact Number")]
        public string? ContactNumber { get; set; }
    }
}
