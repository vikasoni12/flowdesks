namespace Flowdesks.Application.Responses.PPMs;

public class SuspendedPPMResponse
{
        public Guid PPMId { get; set; }
        public DateTime SuspendedFrom { get; set; }
        public DateTime? SuspendedTill { get; set; }
        public bool IsVisible { get; set; }
}