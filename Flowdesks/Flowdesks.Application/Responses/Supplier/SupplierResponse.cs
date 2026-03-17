namespace Flowdesks.Application.Responses.Supplier
{
    public class SupplierResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? WebAddress { get; set; }
        public Guid? CategoryId { get; set; }
        public string Category { get; set; }
        public string? Status { get; set; }
        public string? PrimaryContact { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public DateTime? InsuranceStartDate { get; set; }
        public DateTime? InsuranceExpiryDate { get; set; }
        public string? DocumentUrl { get; set; }
    }
}
