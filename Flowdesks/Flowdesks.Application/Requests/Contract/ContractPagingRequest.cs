namespace Flowdesks.Application.Requests.Contract
{
    public class ContractPagingRequest : PagedRequest
    {
        public Guid? SupplierId { get; set; }
    }
}
