using Flowdesks.Application.Responses.Supplier;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Supplier
{
    public class UpdateSupplierRequest : CreateEditRequest<Domain.Entities.Suppliers.Supplier>, IRequest<Result<SupplierResponse>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? WebAddress { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Status { get; set; }
        public string? PrimaryContact { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public DateTime? InsuranceStartDate { get; set; }
        public DateTime? InsuranceExpiryDate { get; set; }
        public string DocumentUrl { get; set; }
        public UploadByteArray? SupplierDocument { get; set; } = new();
    }
}
