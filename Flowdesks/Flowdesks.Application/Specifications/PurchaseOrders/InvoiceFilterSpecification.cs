using Flowdesks.Application.Requests.PurchaseOrders.Invoices;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Invoices;


namespace Flowdesks.Application.Specifications.PurchaseOrders
{
    public class InvoiceFilterSpecification : Specification<Invoice>
    {
        public InvoiceFilterSpecification(InvoicePagingRequest request)
        {
            And(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                //And(p => p..Contains(request.StringSearch) ||
                //p.Code.Contains(request.StringSearch) ||
                //p.Site.Name.Contains(request.StringSearch) ||
                //p.Building.Name.Contains(request.StringSearch) ||
                //p.Location.Name.Contains(request.StringSearch) ||
                //p.Description.Contains(request.StringSearch));
            }

        }
    }
}
