using Flowdesks.Application.Attributes;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System.ComponentModel;

namespace Flowdesks.Application.Requests.Contract
{
    public class CreateContractRequest : CreateEditRequest<Domain.Entities.Contracts.Contract>, IRequest<Result<int>>
    {
        public string Name { get; set; }
        [IsRequiredField(true)]
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }

        [IsRequiredField(true)]
        [Description("Start Date")]
        public DateTime? StartDate { get; set; }

        [IsRequiredField(true)]
        [Description("Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        [Description("Supplier")]
        public Guid? SupplierId { get; set; }
    }
}
