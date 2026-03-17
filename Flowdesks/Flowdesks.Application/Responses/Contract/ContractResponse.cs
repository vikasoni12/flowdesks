namespace Flowdesks.Application.Responses.Contract
{
    public class ContractResponse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Guid? SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
