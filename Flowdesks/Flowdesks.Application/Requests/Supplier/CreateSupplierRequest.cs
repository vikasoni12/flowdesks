using Flowdesks.Application.Attributes;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System.ComponentModel;

namespace Flowdesks.Application.Requests.Supplier
{
    public class CreateSupplierRequest : CreateEditRequest<Domain.Entities.Suppliers.Supplier>, IRequest<Result<int>>
    {
        [IsRequiredField(true)]
        public string Name { get; set; }
        [IsRequiredField(true)]
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? WebAddress { get; set; }

        [Description("Category")]
        [IsRequiredField(true)]
        public Guid? CategoryId { get; set; }

        public string? Status { get; set; } 

        [Description("Primary Contact")]
        public string? PrimaryContact { get; set; }
        public string? Phone { get; set; }

        [Description("Email")]
        [IsRequiredField(true)]
        public string? Email { get; set; }
        public string? Description { get; set; }

        [Description("Insurance Start Date")]
        public DateTime? InsuranceStartDate { get; set; }

        [Description("Insurance Expiry Date")]
        public DateTime? InsuranceExpiryDate { get; set; }

        [Description("Profile image")]
        public UploadByteArray? SupplierDocument { get; set; } = new();
    }
}
