namespace server.src.Dtos;

public class CreateEventDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public int VenueCapacity { get; set; }
}
