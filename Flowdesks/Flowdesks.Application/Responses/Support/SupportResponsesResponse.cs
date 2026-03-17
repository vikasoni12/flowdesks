using Flowdesks.Application.Responses.Chat.DirectMessage;

namespace Flowdesks.Application.Responses.Support;

public class SupportResponsesResponse
{
    public Guid Id { get; set; }
    public string Response { get; set; }
    public Guid? AttachmentId { get; set; }
    public AttachmentDto? Attachment { get; set; }
    public Guid SupportId { get; set; }
    public Guid RespondedBy { get; set; }
    public string User {  get; set; }
    public string UserProfile { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool IsEditMode { get; set; } = false;
}