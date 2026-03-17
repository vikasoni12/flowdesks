using Flowdesks.Application.Responses.PurchaseOrders;
using MediatR;

namespace Flowdesks.Application.Requests.PurchaseOrders.POAuthorizations
{
    public class GetAllPOAuthorizationRequest : IRequest<List<POAuthorizationResponse>>
    {
    }
}
