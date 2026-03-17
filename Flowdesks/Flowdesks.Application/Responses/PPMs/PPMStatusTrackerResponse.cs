namespace Flowdesks.Application.Responses.PPMs;

public class PPMStatusTrackerResponse
{
    public Guid Id { get; set; }
    public Guid PPMId { get; set; }
    public DateTime RaisedDate { get; set; }
    public string Status { get; set; }
    public bool IsHistorical { get; set; }
}