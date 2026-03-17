using Flowdesks.Application.Responses;

namespace Flowdesks.Application.Responses.Identity
{
    public class UserGridResponse
    {
        public List<UserResponse> UserList { get; set; }
        public int Total { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }
}
