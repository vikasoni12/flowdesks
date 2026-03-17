using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Requests.PurchaseOrders.Invoices
{
    public class InvoicePagingRequest :PagedRequest
    {
        public Guid PurchaseOrderId { get; set; }
    }
}
