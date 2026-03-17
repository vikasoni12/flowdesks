using Flowdesks.Application.Identity;

namespace Flowdesks.Application.Responses.Chat.Groups
{
    public class GroupDetailResponse
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public string CreatedBy { get; set; }
        public int Members { get; set; }
        public int Unread { get; set; }

        public List<UserListResponse> Users { get; set; }
    }
}
