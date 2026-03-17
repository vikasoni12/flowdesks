using System.ComponentModel;

namespace Flowdesks.Application.Responses.Supplier
{
    public class ExportSupplierResponse
    {
        [Description("Name")]
        public string Name { get; set; }
        [Description("Code")]
        public string? Code { get; set; }
        [Description("Address")]
        public string? Address { get; set; }
        [Description("Web Address")]
        public string? WebAddress { get; set; }
        [Description("Category")]
        public string? Category { get; set; }
        [Description("Status")]
        public string? Status { get; set; }
        [Description("Correspondence Contact")]
        public string? PrimaryContact { get; set; }
        [Description("Contact Telephone")]
        public string? Phone { get; set; }
        [Description("Email")]
        public string? Email { get; set; }
        [Description("Description")]
        public string? Description { get; set; }
        [Description("Insurance Start Date")]
        public DateTime? InsuranceStartDate { get; set; }
        [Description("Insurance Expiry Date")]
        public DateTime? InsuranceExpiryDate { get; set; }
    }
}
