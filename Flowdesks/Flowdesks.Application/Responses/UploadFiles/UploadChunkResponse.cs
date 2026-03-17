namespace Flowdesks.Application.Responses.UploadFiles
{
    public class UploadChunkResponse
    {
        public string UploadId { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string UploadSequence { get; set; }
    }
}
