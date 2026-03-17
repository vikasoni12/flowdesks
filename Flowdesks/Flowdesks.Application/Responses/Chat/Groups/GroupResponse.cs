namespace Flowdesks.Application.Responses.Chat.Groups
{
    public class GroupResponse
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public string CreatedBy { get; set; }
        public int Members { get; set; }
        public int Unread { get; set; }
    }
}
