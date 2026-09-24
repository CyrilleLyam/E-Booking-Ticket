namespace server.src.Dtos;

public class QueueStatusDto
{
    public int EventId { get; set; }
    public string Status { get; set; } = string.Empty;
    public long? Position { get; set; }
    public string Message { get; set; } = string.Empty;
}