namespace Flowdesks.Application.Responses;

public class FileDetails
{
    public string Name { get; set; }
    public string FullName { get; set; }
    public long Length { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastWriteTime { get; set; }
    public string Extension { get; set; }
    public string ContentType { get; set; }
}