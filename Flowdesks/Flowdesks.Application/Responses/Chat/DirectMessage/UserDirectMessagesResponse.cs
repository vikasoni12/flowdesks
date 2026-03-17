namespace Flowdesks.Application.Responses.Chat.DirectMessage
{
    public class UserDirectMessagesResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Unread {  get; set; }
    }
}
