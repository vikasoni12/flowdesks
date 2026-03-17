using Flowdesks.Application.Interfaces.Common;

namespace Flowdesks.Infrastructure.Services.Common
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}