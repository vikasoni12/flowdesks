namespace Flowdesks.Application.Interfaces.Common
{
    public interface IMessageService
    {
        Task DeleteOlderMessages(DateTime date);
    }
}
