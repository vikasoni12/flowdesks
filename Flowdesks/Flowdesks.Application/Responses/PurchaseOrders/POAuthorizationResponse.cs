namespace Flowdesks.Application.Responses.PurchaseOrders
{
    public class POAuthorizationResponse
    {
        public Guid? Id { get; set; }
        public int AuthorizationLimit { get; set; }
        public bool IsAuthorized { get; set; }
        public bool IsSelfAuthorize { get; set; }
    }
}
