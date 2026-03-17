namespace Flowdesks.Application.Interfaces.Common
{
    public interface IWorkOrderService
    {
        Task CheckOverDueWorkOrder(DateTime date);
    }
}
