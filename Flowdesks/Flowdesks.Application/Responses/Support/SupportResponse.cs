using Flowdesks.Application.Responses.WorkOrder.Priority;

namespace Flowdesks.Application.Responses.Support;

public class SupportResponse
{
    public Guid Id { get; set; }
    public int SupportNumber { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string DocumentUrl { get; set; }
    public string Status { get; set; }
    public Guid? PriorityId { get; set; }
    public PriorityResponse Priority { get; set; }
    public string PriorityName {  get; set; }
    public Guid? AssignedUserId { get; set; }
    public string AssignedUser { get; set; }
    public Guid? UserId { get; set; }
    public string InitiatedBy { get; set; }
    public string ProfilePicture { get; set; }
    public string CreatedOn { get; set; }
}