using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Domain.Entities.Chat;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Infrastructure.Services.Common
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteOlderMessages(DateTime date)
        {
            var olderDirectMessages = _unitOfWork.Repository<DirectMessage>()
                .Entities(false)
                .Include(x => x.Message)
                .Where(x => x.CreatedOn < date)
                .Select(x => x.Message);

            var olderGroupMessages = _unitOfWork.Repository<GroupMessage>()
                .Entities(false)
                .Include(x => x.Message)
                .Where(x => x.CreatedOn < date)
                .Select(x => x.Message);

            var olderMessages = olderDirectMessages.Union(olderGroupMessages);

            if (olderMessages.Any())
            {
                _unitOfWork.Repository<Message>().DeleteRange(olderMessages, true);

                await _unitOfWork.SaveAsync();
            }
        }
    }
}
